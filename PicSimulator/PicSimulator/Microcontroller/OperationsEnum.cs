using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public enum OperationsEnum
    {
        //BYTE-ORIENTED FILE REGISTER OPERATIONS
        ADDWF = 1,
        ANDWF = 2,
        CLRF = 3,
        CLRW = 4,
        COMF = 5,
        DECF = 6,
        DECFSZ = 7,
        INCF = 8,
        INCFSZ = 9,
        IORWF = 10,
        MOVF = 11,
        MOVWF = 12, 
        NOP = 13,
        RLF = 14,
        RRF = 15,
        SUBWF = 16,
        SWAPF = 17,
        XORWF = 18,

        //BIT-ORIENTED FILE REGISTER OPERATIONS
        BCF = 19,
        BSF = 20,
        BTFSC = 21,
        BTFSS = 22,

        //LITERAL AND CONTROL OPERATIONS
        ADDLW = 23,
        ANDLW = 24,
        CALL = 25,
        CLRWDT = 26,
        GOTO = 27,
        IORLW = 28,
        MOVLW = 29,
        RETFIE = 30,
        RETLW = 31,
        RETURN = 32,
        SLEEP = 33,
        SUBLW = 34,
        XORLW = 35

    }
}
