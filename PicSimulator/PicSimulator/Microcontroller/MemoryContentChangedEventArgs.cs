using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class MemoryContentChangedEventArgs: EventArgs
    {
        public int MemoryAddress { get; set; }
        public int MemoryContent { get; set; }
    }
}
