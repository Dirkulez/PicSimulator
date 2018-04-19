using PicSimulator.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
        private ulong _cycle = 0;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties

        public int WorkingRegisterContent
        {
            get { return _workingRegister.Content; }
            private set
            {
                if(value != _workingRegister.Content)
                {
                    _workingRegister.Content = value;
                    InvokePropertyChanged(new PropertyChangedEventArgs(nameof(WorkingRegisterContent)));
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
                    InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ProgramCounterContent)));
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
                    InvokePropertyChanged(new PropertyChangedEventArgs(nameof(StatusRegisterContent)));
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
        public Microcontroller16F84(IEnumerable<string> operations)
        {
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

        public void ExecuteOperation(int binaryOperationCode)
        {
            var operationToExeute = _operationStack[ProgramCounterContent];
            var operationEnum = OperationDecoder.DecodeOperation(operationToExeute);
            var destinationSelect = OperationDecoder.DecodeDestinationSelect(operationToExeute);
            var literal8Bit = OperationDecoder.Decode8BitLiteral(operationToExeute);
            ExecuteOperationInt(operationEnum, destinationSelect, literal8Bit);
            
        }

        private void ExecuteOperationInt(OperationsEnum operationToExecute, int destinationSelect, int literal8Bit)
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
        }

        private void ExecuteMOVLW(int literal8Bit)
        {
            WorkingRegisterContent = literal8Bit;
            CheckWorkingRegisterForZero();
            _cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteANDLW(int literal8Bit)
        {
            WorkingRegisterContent = WorkingRegisterContent & literal8Bit;
            CheckWorkingRegisterForZero();
            _cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteIORLW(int literal8Bit)
        {
            WorkingRegisterContent = WorkingRegisterContent | literal8Bit;
            CheckWorkingRegisterForZero();
            _cycle++;
            ProgramCounterContent++;
        }

        private void ExecuteSUBLW(int literal8Bit)
        {
            //TODO: implement subtraction: WorkingRegister = literal8Bit - WorkingRegister : think of negative result and zero flag
            var complement2OfWorkingReg = BinaryCalculations.Build2ndComplement(WorkingRegisterContent);
            var setC = false;
            var setDC = false;
            WorkingRegisterContent = BinaryCalculations.BinaryAddition(literal8Bit, complement2OfWorkingReg, ref setDC, ref setC);
            CheckWorkingRegisterForZero();

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

            _cycle++;
            ProgramCounterContent++;
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

        private void InitRegisters()
        {
            _programCounter = new Register(0, "PC");
            _statusRegister = new Register(24, "STATUS");
            _workingRegister = new Register(0, "W");

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
            _registerAdressTable.Add(10, new Register(0, "PCLATH"));
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
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ZeroBit)));
        }

        private void SetZeroBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 251;
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ZeroBit)));
        }

        private void SetCBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 254;
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(CBit)));
        }

        private void SetCBitTo1()
        {
            StatusRegisterContent = StatusRegisterContent | 1;
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(CBit)));
        }

        private void SetDCBitTo0()
        {
            StatusRegisterContent = StatusRegisterContent & 253;
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ZeroBit)));
        }

        private void SetDCBitTo1()
        {
            StatusRegisterContent = StatusRegisterContent | 2;
            InvokePropertyChanged(new PropertyChangedEventArgs(nameof(ZeroBit)));
        }
        #endregion
    }
}
