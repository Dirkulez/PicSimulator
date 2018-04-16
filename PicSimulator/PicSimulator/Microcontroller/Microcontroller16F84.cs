using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class Microcontroller16F84
    {
        public void ExecuteOperation(string binaryOperationCode)
        {
            var operationToExecute = OperationDecoder.DecodeOperation(binaryOperationCode);

        }
    }
}
