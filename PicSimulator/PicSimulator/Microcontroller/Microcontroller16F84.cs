using PicSimulator.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class Microcontroller16F84: INotifyPropertyChanged
    {
        #region Fields
        private Dictionary<int, int> _operationStack;
        private Dictionary<int, Register> _registerAdressTable;
        private Register _workingRegister;
        private Register _programCounter;
        private Register _statusRegister;
        private Register _pclathRegister;
        private Register _pclRegister;
        private ulong _cycle = 0;
        private bool _stopExecution;
        private SynchronizationContext _syncContext;
        private ProgramCounterStack _programCounterStack;
        private ArithmeticLogicUnit _alu;
        private SRAMRegisters _sram;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<MemoryContentChangedEventArgs> MemoryContentChanged;
        #endregion

        #region Properties

        public Dictionary<int, Register> RegisterAdressTable
        {
            get { return _registerAdressTable; }
        }

        public bool StopExecution
        {
            get { return _stopExecution; }
            set { _stopExecution = value; }
        }

        public ulong Cycle
        {
            get { return _cycle; }
            set
            {
                if(value != _cycle)
                {
                    _cycle = value;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(Cycle));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
                }
            }
        }

        public int WorkingRegisterContent
        {
            get { return _workingRegister.Content; }
            private set
            {
                if(value != _workingRegister.Content)
                {
                    _workingRegister.Content = value;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(WorkingRegisterContent));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
                }
            }
        }

        public int ProgramCounterContent
        {
            get { return _programCounter.Content; }
            private set
            {
                if(value != _programCounter.Content)
                {
                    _programCounter.Content = value;
                    var mask = 0xff;
                    var lower8Bit = mask & value;
                    PclRegisterContent = lower8Bit;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(ProgramCounterContent));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
                }
            }
        }

        public int StatusRegisterContent
        {
            get { return _statusRegister.Content; }
            private set
            {
                if(value != _statusRegister.Content)
                {
                    _statusRegister.Content = value;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(StatusRegisterContent));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);

                    InvokeMemoryChanged(3, value);
                    InvokeMemoryChanged(131, value);

                }
            }
        }

        public int PclathRegisterContent
        {
            get { return _pclathRegister.Content; }
            set
            {
                if(value != _pclathRegister.Content)
                {
                    _pclathRegister.Content = value;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(PclathRegisterContent));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);

                    InvokeMemoryChanged(10, value);
                    InvokeMemoryChanged(138, value);

                }
            }
        }

        public int PclRegisterContent
        {
            get { return _pclRegister.Content; }
            set
            {
                if(value != _pclRegister.Content)
                {
                    _pclRegister.Content = value;
                    var propChangedEventArgs = new PropertyChangedEventArgs(nameof(PclRegisterContent));
                    _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);

                    InvokeMemoryChanged(2, value);
                    InvokeMemoryChanged(130, value);

                }
            }
        }

        public int ZeroBit
        {
            get { return (StatusRegisterContent & 4) == 4 ? 1 : 0; }
        }

        public int CBit
        {
            get { return (StatusRegisterContent & 1) == 1 ? 1 : 0; }
        }

        public int DCBit
        {
            get { return (StatusRegisterContent & 2) == 2 ? 1 : 0; }
        }

        #endregion

        #region Constructor
        public Microcontroller16F84(IEnumerable<string> operations, SynchronizationContext synchronzationContext)
        {
            _syncContext = synchronzationContext ?? new SynchronizationContext();
            InitRegisters();
            InitOperations(operations);
            _programCounterStack = new ProgramCounterStack();
            InitAlu();
        }
        #endregion

        #region Methods

        private void InitAlu()
        {
            _alu = new ArithmeticLogicUnit();
            _alu.Cset += SetCBitTo1;
            _alu.Cunset += SetCBitTo0;
            _alu.DCset += SetDCBitTo1;
            _alu.DCunset += SetDCBitTo0;
            _alu.ResultZero += SetZeroBitTo1;
            _alu.ResultNotZero += SetZeroBitTo0;
        }

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
        }

        private void InvokeMemoryChanged(int memoryAddress, int memoryContent)
        {
            var memoryContentChangedEventArgs = new MemoryContentChangedEventArgs();
            memoryContentChangedEventArgs.MemoryAddress = memoryAddress;
            memoryContentChangedEventArgs.MemoryContent = memoryContent;
            _syncContext.Post(new SendOrPostCallback((o) => OnMemoryContentChanged(memoryContentChangedEventArgs)), null);
        }

        public void OnMemoryContentChanged(MemoryContentChangedEventArgs e)
        {
            MemoryContentChanged?.Invoke(this, e);
        }

        public void Execute()
        {
            while (!_stopExecution)
            {
                ExecuteOperation();
                Thread.Sleep(2000);
            }
        }

        public void ExecuteOperation()
        {
            var operationToExeute = _operationStack[ProgramCounterContent];
            var operationEnum = OperationDecoder.DecodeOperation(operationToExeute);
            var destinationSelect = OperationDecoder.DecodeDestinationSelect(operationToExeute);
            var literal8Bit = OperationDecoder.Decode8BitLiteral(operationToExeute);
            var literal11Bit = OperationDecoder.Decode11BitLiteral(operationToExeute);
            var fileRegisterAdress7Bit = OperationDecoder.DecodeFileRegisterAdress7Bit(operationToExeute);
            ExecuteOperationInt(operationEnum, destinationSelect, literal8Bit, literal11Bit, fileRegisterAdress7Bit);
        }

        private void ExecuteOperationInt(OperationsEnum operationToExecute, int destinationSelect, int literal8Bit, int literal11Bit,
            int fileRegisterAdress7Bit)
        {
            if(operationToExecute == OperationsEnum.MOVLW)
            {
                ExecuteMOVLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.ANDLW)
            {
                ExecuteANDLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.IORLW)
            {
                ExecuteIORLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.SUBLW)
            {
                ExecuteSUBLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.XORLW)
            {
                ExecuteXORLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.ADDLW)
            {
                ExecuteADDLW(literal8Bit);
            }
            else if(operationToExecute == OperationsEnum.GOTO)
            {
                ExecuteGOTO(literal11Bit);
            }
            else if (operationToExecute == OperationsEnum.CALL)
            {
                ExecuteCALL(literal11Bit);
            }
            else if (operationToExecute == OperationsEnum.NOP)
            {
                ExecuteNOP();
            }
          
            else if (operationToExecute == OperationsEnum.RETLW)
            {
                ExecuteRETLW(literal8Bit);
            }

            else if (operationToExecute == OperationsEnum.RETURN)
            {
                ExecuteRETURN();
            }

            else if (operationToExecute == OperationsEnum.MOVWF)
            {
                ExecuteMOVWF(fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.ADDWF)
            {
                ExecuteADDWF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.ANDWF)
            {
                ExecuteANDWF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.CLRF)
            {
                ExecuteCLRF(fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.COMF)
            {
                ExecuteCOMF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.DECF)
            {
                ExecuteDECF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.INCF)
            {
                ExecuteINCF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.MOVF)
            {
                ExecuteMOVF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.IORWF)
            {
                ExecuteIORWF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.SUBWF)
            {
                ExecuteSUBWF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.SWAPF)
            {
                ExecuteSWAPF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.XORWF)
            {
                ExecuteXORWF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.CLRW)
            {
                ExecuteCLRW();
            }

            else if (operationToExecute == OperationsEnum.RLF)
            {
                ExecuteRLF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if(operationToExecute == OperationsEnum.RRF)
            {
                ExecuteRRF(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.DECFSZ)
            {
                ExecuteDECFSZ(destinationSelect, fileRegisterAdress7Bit);
            }

            else if (operationToExecute == OperationsEnum.INCFSZ)
            {
                ExecuteINCFSZ(destinationSelect, fileRegisterAdress7Bit);
            }
        }

        private void ExecuteINCFSZ(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.Increment(_registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;

            if(result == 0)
            {
                ExecuteNOP();
            }
        }

        private void ExecuteDECFSZ(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.Decrement(_registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;

            if(result == 0)
            {
                ExecuteNOP();
            }
        }

        private void ExecuteRRF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var carry = CBit;

            var result = _alu.RotateRight(carry, _registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteRLF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var carry = CBit;

            var result = _alu.RotateLeft(carry, _registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteCLRW()
        {
            WorkingRegisterContent = 0;
            SetZeroBitTo1(this, new EventArgs());

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteXORWF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.LogicalExclusiveOR(WorkingRegisterContent, _registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteSWAPF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var registerContent = _registerAdressTable[fileRegisterAdress7Bit].Content;
            var lowerNibble = registerContent & 15;
            var higherNibble = registerContent & 240;
            var lowerNibbleShifted = lowerNibble << 4;
            var higherNibbleShifted = higherNibble >> 4;
            var result = lowerNibbleShifted + higherNibbleShifted;

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteSUBWF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var complement2OfWorkingRegister = _alu.Build2ndComplement(WorkingRegisterContent);
            var result = _alu.BinaryAddition(complement2OfWorkingRegister, _registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteIORWF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.LogicalInclusiveOR(WorkingRegisterContent, _registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteMOVF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _registerAdressTable[fileRegisterAdress7Bit].Content;
            if(result == 0)
            {
                SetZeroBitTo1(this, new EventArgs());
            }
            else
            {
                SetZeroBitTo0(this, new EventArgs());
            }

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteINCF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.Increment(_registerAdressTable[fileRegisterAdress7Bit].Content);

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteDECF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.Decrement(_registerAdressTable[fileRegisterAdress7Bit].Content);
            
            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;

        }

        private void ExecuteCOMF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var complement = _alu.BuildComplement(_registerAdressTable[fileRegisterAdress7Bit].Content);
            if(complement == 0)
            {
                SetZeroBitTo1(this, new EventArgs());
            }
            else
            {
                SetZeroBitTo0(this, new EventArgs());
            }

            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = complement;
                InvokeMemoryChanged(fileRegisterAdress7Bit, complement);
            }
            else
            {
                WorkingRegisterContent = complement;
            }

            Cycle++;
            ProgramCounterContent++;
                
        }

        private void ExecuteCLRF(int fileRegisterAdress7Bit)
        {
            _registerAdressTable[fileRegisterAdress7Bit].Content = 0;
            InvokeMemoryChanged(fileRegisterAdress7Bit, 0);
            SetZeroBitTo1(this, new EventArgs());
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteANDWF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.LogicalAND(WorkingRegisterContent, _registerAdressTable[fileRegisterAdress7Bit].Content);
            if (destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteADDWF(int destinationSelect, int fileRegisterAdress7Bit)
        {
            var result = _alu.BinaryAddition(WorkingRegisterContent, _registerAdressTable[fileRegisterAdress7Bit].Content);
            if(destinationSelect == 1)
            {
                _registerAdressTable[fileRegisterAdress7Bit].Content = result;
                InvokeMemoryChanged(fileRegisterAdress7Bit, result);
            }
            else
            {
                WorkingRegisterContent = result;
            }

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteMOVWF(int fileRegisterAdress7Bit)
        {
            _registerAdressTable[fileRegisterAdress7Bit].Content = WorkingRegisterContent;
            InvokeMemoryChanged(fileRegisterAdress7Bit, WorkingRegisterContent);
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteCALL(int literal11Bit)
        {
            /*Call Subroutine. First, return address
            (PC + 1) is pushed onto the stack. The
              eleven bit immediate address is loaded
              into PC bits<10:0 >.The upper bits of
             the PC are loaded from PCLATH.*/
            _programCounterStack.PushToStack(ProgramCounterContent+1);
            var pclathValue = PclathRegisterContent & 24;
            pclathValue = pclathValue << 8;
            ProgramCounterContent = literal11Bit + pclathValue;
            Cycle += 2;

        }

        private void ExecuteNOP()
        {
            WorkingRegisterContent = WorkingRegisterContent;
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteRETLW(int literal8Bit)
        {
            //program counter is loaded from the top of the stack
            //8bitliteral is stored in w_reg
            WorkingRegisterContent = literal8Bit;
            Cycle+= 2;
            ProgramCounterContent = _programCounterStack.PopFromStack();
        }

        private void ExecuteRETURN()
        {
            Cycle += 2;
            ProgramCounterContent = _programCounterStack.PopFromStack();
        }

        private void ExecuteMOVLW(int literal8Bit)
        {
            WorkingRegisterContent = literal8Bit;
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteANDLW(int literal8Bit)
        {
            WorkingRegisterContent = _alu.LogicalAND(WorkingRegisterContent, literal8Bit);
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteIORLW(int literal8Bit)
        {
            WorkingRegisterContent = _alu.LogicalInclusiveOR(WorkingRegisterContent, literal8Bit);
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteSUBLW(int literal8Bit)
        {

            var complement2OfWorkingReg = _alu.Build2ndComplement(WorkingRegisterContent);
            WorkingRegisterContent = _alu.BinaryAddition(literal8Bit, complement2OfWorkingReg);

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteXORLW(int literal8Bit){

            WorkingRegisterContent = _alu.LogicalExclusiveOR(WorkingRegisterContent, literal8Bit);

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteADDLW(int literal8Bit)
        {
            WorkingRegisterContent = _alu.BinaryAddition(literal8Bit, WorkingRegisterContent);

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteGOTO(int literal11Bit)
        {
            var pclathValue = PclathRegisterContent & 24;
            pclathValue = pclathValue << 8;
            ProgramCounterContent = literal11Bit + pclathValue;
            Cycle += 2;
        }

        private void InitRegisters()
        {
            _programCounter = new Register(0, "PC");
            _statusRegister = new Register(24, "STATUS");
            _workingRegister = new Register(0, "W");
            _pclathRegister = new Register(0, "PCLATH");
            _pclRegister = new Register(0, "PCL");

            _sram = new SRAMRegisters();

            _registerAdressTable = new Dictionary<int, Register>();
            FillRegisterAdressTable();
           
            
        }

        private void FillRegisterAdressTable()
        {
            _registerAdressTable.Add(0, new Register(0, "INDF"));
            _registerAdressTable.Add(1, new Register(0, "TMR0"));
            _registerAdressTable.Add(2, _pclRegister);
            _registerAdressTable.Add(3, _statusRegister);
            _registerAdressTable.Add(4, new Register(0, "FSR"));
            _registerAdressTable.Add(5, new Register(0, "PORTA"));
            _registerAdressTable.Add(6, new Register(0, "PORTB"));
            _registerAdressTable.Add(8, new Register(0, "EEDATA"));
            _registerAdressTable.Add(9, new Register(0, "EEADR"));
            _registerAdressTable.Add(10, _pclathRegister);
            _registerAdressTable.Add(11, new Register(0, "INTCON"));

            //map SRAM registers
            _registerAdressTable.Add(12, _sram.Sram0);
            _registerAdressTable.Add(13, _sram.Sram1);
            _registerAdressTable.Add(14, _sram.Sram2);
            _registerAdressTable.Add(15, _sram.Sram3);
            _registerAdressTable.Add(16, _sram.Sram4);
            _registerAdressTable.Add(17, _sram.Sram5);
            _registerAdressTable.Add(18, _sram.Sram6);
            _registerAdressTable.Add(19, _sram.Sram7);
            _registerAdressTable.Add(20, _sram.Sram8);
            _registerAdressTable.Add(21, _sram.Sram9);
            _registerAdressTable.Add(22, _sram.Sram10);
            _registerAdressTable.Add(23, _sram.Sram11);
            _registerAdressTable.Add(24, _sram.Sram12);
            _registerAdressTable.Add(25, _sram.Sram13);
            _registerAdressTable.Add(26, _sram.Sram14);
            _registerAdressTable.Add(27, _sram.Sram15);
            _registerAdressTable.Add(28, _sram.Sram16);
            _registerAdressTable.Add(29, _sram.Sram17);
            _registerAdressTable.Add(30, _sram.Sram18);
            _registerAdressTable.Add(31, _sram.Sram19);
            _registerAdressTable.Add(32, _sram.Sram20);
            _registerAdressTable.Add(33, _sram.Sram21);
            _registerAdressTable.Add(34, _sram.Sram22);
            _registerAdressTable.Add(35, _sram.Sram23);
            _registerAdressTable.Add(36, _sram.Sram24);
            _registerAdressTable.Add(37, _sram.Sram25);
            _registerAdressTable.Add(38, _sram.Sram26);
            _registerAdressTable.Add(39, _sram.Sram27);
            _registerAdressTable.Add(40, _sram.Sram28);
            _registerAdressTable.Add(41, _sram.Sram29);
            _registerAdressTable.Add(42, _sram.Sram30);
            _registerAdressTable.Add(43, _sram.Sram31);
            _registerAdressTable.Add(44, _sram.Sram32);
            _registerAdressTable.Add(45, _sram.Sram33);
            _registerAdressTable.Add(46, _sram.Sram34);
            _registerAdressTable.Add(47, _sram.Sram35);
            _registerAdressTable.Add(48, _sram.Sram36);
            _registerAdressTable.Add(49, _sram.Sram37);
            _registerAdressTable.Add(50, _sram.Sram38);
            _registerAdressTable.Add(51, _sram.Sram39);
            _registerAdressTable.Add(52, _sram.Sram40);
            _registerAdressTable.Add(53, _sram.Sram41);
            _registerAdressTable.Add(54, _sram.Sram42);
            _registerAdressTable.Add(55, _sram.Sram43);
            _registerAdressTable.Add(56, _sram.Sram44);
            _registerAdressTable.Add(57, _sram.Sram45);
            _registerAdressTable.Add(58, _sram.Sram46);
            _registerAdressTable.Add(59, _sram.Sram47);
            _registerAdressTable.Add(60, _sram.Sram48);
            _registerAdressTable.Add(61, _sram.Sram49);
            _registerAdressTable.Add(62, _sram.Sram50);
            _registerAdressTable.Add(63, _sram.Sram51);
            _registerAdressTable.Add(64, _sram.Sram52);
            _registerAdressTable.Add(65, _sram.Sram53);
            _registerAdressTable.Add(66, _sram.Sram54);
            _registerAdressTable.Add(67, _sram.Sram55);
            _registerAdressTable.Add(68, _sram.Sram56);
            _registerAdressTable.Add(69, _sram.Sram57);
            _registerAdressTable.Add(70, _sram.Sram58);
            _registerAdressTable.Add(71, _sram.Sram59);
            _registerAdressTable.Add(72, _sram.Sram60);
            _registerAdressTable.Add(73, _sram.Sram61);
            _registerAdressTable.Add(74, _sram.Sram62);
            _registerAdressTable.Add(75, _sram.Sram63);
            _registerAdressTable.Add(76, _sram.Sram64);
            _registerAdressTable.Add(77, _sram.Sram65);
            _registerAdressTable.Add(78, _sram.Sram66);
            _registerAdressTable.Add(79, _sram.Sram67);

            _registerAdressTable.Add(140, _sram.Sram0);
            _registerAdressTable.Add(141, _sram.Sram1);
            _registerAdressTable.Add(142, _sram.Sram2);
            _registerAdressTable.Add(143, _sram.Sram3);
            _registerAdressTable.Add(144, _sram.Sram4);
            _registerAdressTable.Add(145, _sram.Sram5);
            _registerAdressTable.Add(146, _sram.Sram6);
            _registerAdressTable.Add(147, _sram.Sram7);
            _registerAdressTable.Add(148, _sram.Sram8);
            _registerAdressTable.Add(149, _sram.Sram9);
            _registerAdressTable.Add(150, _sram.Sram10);
            _registerAdressTable.Add(151, _sram.Sram11);
            _registerAdressTable.Add(152, _sram.Sram12);
            _registerAdressTable.Add(153, _sram.Sram13);
            _registerAdressTable.Add(154, _sram.Sram14);
            _registerAdressTable.Add(155, _sram.Sram15);
            _registerAdressTable.Add(156, _sram.Sram16);
            _registerAdressTable.Add(157, _sram.Sram17);
            _registerAdressTable.Add(158, _sram.Sram18);
            _registerAdressTable.Add(159, _sram.Sram19);
            _registerAdressTable.Add(160, _sram.Sram20);
            _registerAdressTable.Add(161, _sram.Sram21);
            _registerAdressTable.Add(162, _sram.Sram22);
            _registerAdressTable.Add(163, _sram.Sram23);
            _registerAdressTable.Add(164, _sram.Sram24);
            _registerAdressTable.Add(165, _sram.Sram25);
            _registerAdressTable.Add(166, _sram.Sram26);
            _registerAdressTable.Add(167, _sram.Sram27);
            _registerAdressTable.Add(168, _sram.Sram28);
            _registerAdressTable.Add(169, _sram.Sram29);
            _registerAdressTable.Add(170, _sram.Sram30);
            _registerAdressTable.Add(171, _sram.Sram31);
            _registerAdressTable.Add(172, _sram.Sram32);
            _registerAdressTable.Add(173, _sram.Sram33);
            _registerAdressTable.Add(174, _sram.Sram34);
            _registerAdressTable.Add(175, _sram.Sram35);
            _registerAdressTable.Add(176, _sram.Sram36);
            _registerAdressTable.Add(177, _sram.Sram37);
            _registerAdressTable.Add(178, _sram.Sram38);
            _registerAdressTable.Add(179, _sram.Sram39);
            _registerAdressTable.Add(180, _sram.Sram40);
            _registerAdressTable.Add(181, _sram.Sram41);
            _registerAdressTable.Add(182, _sram.Sram42);
            _registerAdressTable.Add(183, _sram.Sram43);
            _registerAdressTable.Add(184, _sram.Sram44);
            _registerAdressTable.Add(185, _sram.Sram45);
            _registerAdressTable.Add(186, _sram.Sram46);
            _registerAdressTable.Add(187, _sram.Sram47);
            _registerAdressTable.Add(188, _sram.Sram48);
            _registerAdressTable.Add(189, _sram.Sram49);
            _registerAdressTable.Add(190, _sram.Sram50);
            _registerAdressTable.Add(191, _sram.Sram51);
            _registerAdressTable.Add(192, _sram.Sram52);
            _registerAdressTable.Add(193, _sram.Sram53);
            _registerAdressTable.Add(194, _sram.Sram54);
            _registerAdressTable.Add(195, _sram.Sram55);
            _registerAdressTable.Add(196, _sram.Sram56);
            _registerAdressTable.Add(197, _sram.Sram57);
            _registerAdressTable.Add(198, _sram.Sram58);
            _registerAdressTable.Add(199, _sram.Sram59);
            _registerAdressTable.Add(200, _sram.Sram60);
            _registerAdressTable.Add(201, _sram.Sram61);
            _registerAdressTable.Add(202, _sram.Sram62);
            _registerAdressTable.Add(203, _sram.Sram63);
            _registerAdressTable.Add(204, _sram.Sram64);
            _registerAdressTable.Add(205, _sram.Sram65);
            _registerAdressTable.Add(206, _sram.Sram66);
            _registerAdressTable.Add(207, _sram.Sram67);

            _registerAdressTable.Add(129, new Register(255, "OPTION_REG"));
            _registerAdressTable.Add(130, _pclRegister);
            _registerAdressTable.Add(131, _statusRegister);
            _registerAdressTable.Add(133, new Register(31, "TRISA"));
            _registerAdressTable.Add(134, new Register(255, "TRISB"));
            _registerAdressTable.Add(136, new Register(0, "EECON1"));
            _registerAdressTable.Add(137, new Register(0, "EECON2"));
        }

        private void InitOperations(IEnumerable<string> operations)
        {
            _operationStack = new Dictionary<int, int>(operations.Count());
            foreach(var operation in operations)
            {
                _operationStack.Add(
                    Int32.Parse(operation.Substring(0, 4), System.Globalization.NumberStyles.HexNumber),
                    Int32.Parse(operation.Substring(5, 4), System.Globalization.NumberStyles.HexNumber));
            }
        }

        private void SetZeroBitTo1(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent | 4;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(ZeroBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetZeroBitTo0(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent & 251;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(ZeroBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetCBitTo0(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent & 254;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(CBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetCBitTo1(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent | 1;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(CBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetDCBitTo0(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent & 253;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(DCBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetDCBitTo1(Object sender, EventArgs e)
        {
            StatusRegisterContent = StatusRegisterContent | 2;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(DCBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }
       
        #endregion
    }
}
