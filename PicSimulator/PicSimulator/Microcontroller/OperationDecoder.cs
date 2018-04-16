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
    }
}
