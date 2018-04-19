using PicSimulator.Helper;
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
        private Dictionary<byte, Register> _registerAdressTable;
        private Register _workingRegister;
        private Register _programCounter;
        private Register _statusRegister;
        private Register _pclathRegister;
        private ulong _cycle = 0;
        private bool _stopExecution;
        private SynchronizationContext _syncContext;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties

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
                    _registerAdressTable[2].Content = lower8Bit;
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
        }
        #endregion

        #region Methods

        public void InvokePropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, e);
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
            ExecuteOperationInt(operationEnum, destinationSelect, literal8Bit, literal11Bit);
        }

        private void ExecuteOperationInt(OperationsEnum operationToExecute, int destinationSelect, int literal8Bit, int literal11Bit)
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
        }

        private void ExecuteMOVLW(int literal8Bit)
        {
            WorkingRegisterContent = literal8Bit;
            CheckWorkingRegisterForZero();
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteANDLW(int literal8Bit)
        {
            WorkingRegisterContent = WorkingRegisterContent & literal8Bit;
            CheckWorkingRegisterForZero();
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteIORLW(int literal8Bit)
        {
            WorkingRegisterContent = WorkingRegisterContent | literal8Bit;
            CheckWorkingRegisterForZero();
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteSUBLW(int literal8Bit)
        {

            var complement2OfWorkingReg = BinaryCalculations.Build2ndComplement(WorkingRegisterContent);
            var setC = false;
            var setDC = false;
            WorkingRegisterContent = BinaryCalculations.BinaryAddition(literal8Bit, complement2OfWorkingReg, ref setDC, ref setC);
            CheckWorkingRegisterForZero();
            SetCarryFlags(setDC, setC);

            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteXORLW(int literal8Bit){

            WorkingRegisterContent = WorkingRegisterContent ^ literal8Bit;
            CheckWorkingRegisterForZero();
            Cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteADDLW(int literal8Bit)
        {
            var setDC = false;
            var setC = false;
            WorkingRegisterContent = BinaryCalculations.BinaryAddition(literal8Bit, WorkingRegisterContent, ref setDC, ref setC);
            CheckWorkingRegisterForZero();
            SetCarryFlags(setDC, setC);
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

        private void CheckWorkingRegisterForZero()
        {
            if (WorkingRegisterContent == 0)
            {
                SetZeroBitTo1();
            }
            else
            {
                SetZeroBitTo0();
            }
        }

        private void SetCarryFlags(bool setDC, bool setC)
        {
            if (setC)
            {
                SetCBitTo1();
            }
            else
            {
                SetCBitTo0();
            }

            if (setDC)
            {
                SetDCBitTo1();
            }
            else
            {
                SetDCBitTo0();
            }
        }

        private void InitRegisters()
        {
            _programCounter = new Register(0, "PC");
            _statusRegister = new Register(24, "STATUS");
            _workingRegister = new Register(0, "W");
            _pclathRegister = new Register(0, "PCLATH");

            _registerAdressTable = new Dictionary<byte, Register>();
            _registerAdressTable.Add(0, new Register(0,"INDF"));
            _registerAdressTable.Add(1, new Register(0, "TMR0"));
            _registerAdressTable.Add(2, new Register(0, "PCL"));
            _registerAdressTable.Add(3, _statusRegister);
            _registerAdressTable.Add(4, new Register(0, "FSR"));
            _registerAdressTable.Add(5, new Register(0, "PORTA"));
            _registerAdressTable.Add(6, new Register(0, "PORTB"));
            _registerAdressTable.Add(8, new Register(0, "EEDATA"));
            _registerAdressTable.Add(9, new Register(0, "EEADR"));
            _registerAdressTable.Add(10, _pclathRegister);
            _registerAdressTable.Add(11, new Register(0, "INTCON"));
            _registerAdressTable.Add(129, new Register(255, "OPTION_REG"));
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

        private void SetZeroBitTo1()
        {
            StatusRegisterContent = StatusRegisterContent | 4;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(ZeroBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetZeroBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 251;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(ZeroBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetCBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 254;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(CBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetCBitTo1()
        {
            StatusRegisterContent = StatusRegisterContent | 1;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(CBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetDCBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 253;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(DCBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }

        private void SetDCBitTo1()
        {
            StatusRegisterContent = StatusRegisterContent | 2;
            var propChangedEventArgs = new PropertyChangedEventArgs(nameof(DCBit));
            _syncContext.Post(new SendOrPostCallback((o) => InvokePropertyChanged(propChangedEventArgs)), null);
        }
        #endregion
    }
}
