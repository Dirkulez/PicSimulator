using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class Microcontroller16F84
    {
        private Dictionary<byte, Register> _registerAdressTable;
        private Register _workingRegister;

        public Microcontroller16F84()
        {
            InitRegisters();
        }

        public void ExecuteOperation(string binaryOperationCode)
        {
            var operationToExecute = OperationDecoder.DecodeOperation(binaryOperationCode);

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
    }
}
