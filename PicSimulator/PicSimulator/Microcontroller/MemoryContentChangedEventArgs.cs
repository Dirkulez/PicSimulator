using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class MemoryContentChangedEventArgs: EventArgs
    {
        private List<int> _memoryAddressList = new List<int>();
        public List<int> MemoryAddress
        {
            get
            {
                return _memoryAddressList;
            }
            set
            {
                _memoryAddressList = value;
            }
        }
        public int MemoryContent { get; set; }

    }
}
