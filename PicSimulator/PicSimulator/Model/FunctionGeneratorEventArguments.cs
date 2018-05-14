using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Model
{
    public class FunctionGeneratorEventArguments: EventArgs
    {
        public string AffectedPin { get; set; }
        public int Value { get; set; }

        public FunctionGeneratorEventArguments(string affectedPin, int value)
        {
            AffectedPin = affectedPin;
            Value = value;
        }
    }
}
