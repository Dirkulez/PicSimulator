﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public static class OperationDecoder
    {
        public static OperationsEnum DecodeOperation(int binaryOperationCode)
        {
            if(binaryOperationCode == 0)
            {
                return OperationsEnum.NOP;
            }

            //byte oriented operations
            var operationCode6bit = binaryOperationCode & 16128;
            var operationCode4bit = binaryOperationCode & 15360;
            var operationCode5bit = binaryOperationCode & 15872;
            var operationCode3bit = binaryOperationCode & 14336;
            if (operationCode6bit == 1792)
            {
                return OperationsEnum.ADDWF;
            }
            if (operationCode6bit == 1280)
            {
                return OperationsEnum.ANDWF;
            }
            if (operationCode6bit == 256)
            {
                return OperationsEnum.CLRF;
            }
            /*Sonderfall
             * if (operationCode6bit ==)
            {
                return OperationsEnum.CLRW;
            }*/
            if (operationCode6bit == 2304)
            {
                return OperationsEnum.COMF;
            }
            if (operationCode6bit == 768)
            {
                return OperationsEnum.DECF;
            }
            if (operationCode6bit == 2816)
            {
                return OperationsEnum.DECFSZ;
            }
            if (operationCode6bit == 2560)
            {
                return OperationsEnum.INCF;
            }
            if (operationCode6bit == 3840)
            {
                return OperationsEnum.INCFSZ;
            }
            if (operationCode6bit == 1024)
            {
                return OperationsEnum.IORWF;
            }
            if (operationCode6bit == 2048)
            {
                return OperationsEnum.MOVF;
            }
            if (operationCode6bit == 0)
            {
                return OperationsEnum.MOVWF;
            }
            if (operationCode6bit == 3328)
            {
                return OperationsEnum.RLF;
            }
            if (operationCode6bit == 3072)
            {
                return OperationsEnum.RRF;
            }
            if (operationCode6bit == 512)
            {
                return OperationsEnum.SUBWF;
            }
            if (operationCode6bit == 3584)
            {
                return OperationsEnum.SWAPF;
            }
            if (operationCode6bit == 1536)
            {
                return OperationsEnum.XORWF;
            }
            //bit oriented operations
            if (operationCode4bit == 4096)
            {
                return OperationsEnum.BCF;
            }
            if (operationCode4bit == 5120)
            {
                return OperationsEnum.BSF;
            }
            if (operationCode4bit == 6144)
            {
                return OperationsEnum.BTFSC;
            }
            if (operationCode4bit == 7168)
            {
                return OperationsEnum.BTFSS;
            }
            //literal and control operations
            if (operationCode5bit == 15872)
            {
                return OperationsEnum.ADDLW;
            }
            if (operationCode6bit == 14592)
            {
                return OperationsEnum.ANDLW;
            }
            if (operationCode3bit == 8192)
            {
                return OperationsEnum.CALL;
            }
            if (binaryOperationCode == 100)
            {
                return OperationsEnum.CLRWDT;
            }
            if (operationCode3bit == 10240)
            {
                return OperationsEnum.GOTO;
            }
            if (operationCode6bit == 14336)
            {
                return OperationsEnum.IORLW;
            }
            if (operationCode4bit == 12288)
            {
                return OperationsEnum.MOVLW;
            }
            if (binaryOperationCode == 9)
            {
                return OperationsEnum.RETFIE;
            }
            if (operationCode4bit == 13312)
            {
                return OperationsEnum.RETLW;
            }
            if (binaryOperationCode == 8)
            {
                return OperationsEnum.RETURN;
            }
            if (binaryOperationCode == 99)
            {
                return OperationsEnum.SLEEP;
            }
            if (operationCode5bit == 15360)
            {
                return OperationsEnum.SUBLW;
            }
            if (operationCode6bit == 14848)
            {
                return OperationsEnum.XORLW;
            }




            return OperationsEnum.UNKNOWN;
        }

        public static int DecodeDestinationSelect(int binaryOperationCode)
        {
            var mask = 128;
            var maskedCode = mask & binaryOperationCode;
            if (maskedCode != 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static int Decode8BitLiteral(int binaryOperationCode)
        {
            var mask = 255;
            var maskedCode = mask & binaryOperationCode;
            return maskedCode;
        }
    }
}