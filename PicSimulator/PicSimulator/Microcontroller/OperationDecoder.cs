using System;
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
            if(operationCode6bit == 1792)
            {
                return OperationsEnum.ADDWF;
            }

            //bit oriented operations

            //literal and control operations
            if(operationCode4bit == 12288)
            {
                return OperationsEnum.MOVLW;
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
