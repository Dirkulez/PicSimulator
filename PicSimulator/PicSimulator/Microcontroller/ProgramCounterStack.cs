using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Model
{
    public class ProgramCounterStack
    {

        #region Fields
        private Dictionary<byte, int> _stackAddressTable;
        private byte _stackPointer = 0;
        #endregion

        #region Constructor
        public ProgramCounterStack()
        {
            InitStack();
        }
        #endregion

        #region Methods

        public void PushToStack(int contentToPush)
        {
            if (_stackPointer == 7)
            {
                _stackPointer = 0;
            }
            else
            {
                _stackPointer++;
            }

            _stackAddressTable[_stackPointer] = contentToPush;
        }

        public int PopFromStack()
        {
            var topOfStack = _stackAddressTable[_stackPointer];

            if (_stackPointer == 0)
            {
                _stackPointer = 7;
            }
            else
            {
                _stackPointer--;
            }

            return topOfStack;
        }

        private void InitStack()
        {
            _stackAddressTable = new Dictionary<byte, int>();
            _stackAddressTable.Add(0, 0);
            _stackAddressTable.Add(1, 0);
            _stackAddressTable.Add(2, 0);
            _stackAddressTable.Add(3, 0);
            _stackAddressTable.Add(4, 0);
            _stackAddressTable.Add(5, 0);
            _stackAddressTable.Add(6, 0);
            _stackAddressTable.Add(7, 0);
        }

        #endregion
    }
}
