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
        private ulong _cycle = 0;
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Properties
        public int ProgramCounter
        {
            get { return _registerAdressTable[2].Content; }
        }

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
            var operationToExeute = _operationStack[ProgramCounter];
            var operationEnum = OperationDecoder.DecodeOperation(operationToExeute);
            var destinationSelect = OperationDecoder.DecodeDestinationSelect(operationToExeute);
            var literal8Bit = OperationDecoder.Decode8BitLiteral(operationToExeute);
            ExecuteOperationInt(operationEnum, destinationSelect, literal8Bit);
            _cycle++;
        }

        private void ExecuteOperationInt(OperationsEnum operationToExecute, int destinationSelect, int literal8Bit)
        {
            if(operationToExecute == OperationsEnum.MOVLW)
            {
                ExecuteMOVLW(literal8Bit);
            }
        }

        private void ExecuteMOVLW(int literal8Bit)
        {
            WorkingRegisterContent = literal8Bit;
        }

        private void InitRegisters()
        {
            _registerAdressTable = new Dictionary<byte, Register>();
            _registerAdressTable.Add(0, new Register(0,"INDF"));
            _registerAdressTable.Add(1, new Register(0, "TMR0"));
            _registerAdressTable.Add(2, new Register(0, "PCL"));
            _registerAdressTable.Add(3, new Register(24, "STATUS"));
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

            _workingRegister = new Register(0, "W");
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
        #endregion
    }
}
