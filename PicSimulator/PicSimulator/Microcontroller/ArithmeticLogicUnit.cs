using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class ArithmeticLogicUnit
    {
        #region Events
        public event EventHandler<EventArgs> ResultZero;
        public event EventHandler<EventArgs> ResultNotZero;
        public event EventHandler<EventArgs> DCset;
        public event EventHandler<EventArgs> DCunset;
        public event EventHandler<EventArgs> Cset;
        public event EventHandler<EventArgs> Cunset;
        #endregion

        #region Methods

        public int BuildComplement(int originFigure)
        {
            var bit0 = (originFigure & 1) == 1 ? 0 : 1;
            var bit1 = (originFigure & 2) == 2 ? 0 : 2;
            var bit2 = (originFigure & 4) == 4 ? 0 : 4;
            var bit3 = (originFigure & 8) == 8 ? 0 : 8;
            var bit4 = (originFigure & 16) == 16 ? 0 : 16;
            var bit5 = (originFigure & 32) == 32 ? 0 : 32;
            var bit6 = (originFigure & 64) == 64 ? 0 : 64;
            var bit7 = (originFigure & 128) == 128 ? 0 : 128;

            var result = bit0 + bit1 + bit2 + bit3 + bit4 + bit5 + bit6 + bit7;

            return result;
        }

        public int Build2ndComplement(int originFigure)
        {
            if (originFigure > 255)
            {
                return -1;
            }

            return BuildComplement(originFigure) + 1;
        }

        public int BinaryAddition(int add1, int add2)
        {
            bool carry = false;
            var result = 0;

            for (int i = 1; i < 256; i = i * 2)
            {
                var currentBitAdd1 = (add1 & i) == i ? i : 0;
                var currentBitAdd2 = (add2 & i) == i ? i : 0;

                if (currentBitAdd1 == i && currentBitAdd2 == i)
                {
                    if (carry)
                    {
                        result += i;
                    }

                    carry = true;

                }
                else if (currentBitAdd1 == 0 && currentBitAdd2 == 0)
                {
                    if (carry)
                    {
                        result += i;
                    }

                    carry = false;
                }
                else
                {
                    if (carry)
                    {
                        carry = true;
                    }
                    else
                    {
                        result += i;
                        carry = false;
                    }
                }

                if (i == 8)
                {
                    if (carry)
                    {
                        OnDCSet();
                    }
                    else
                    {
                        OnDCunset();
                    }
                }
                else if (i == 128)
                {

                    if (carry)
                    {
                        OnCSet();
                    }
                    else
                    {
                        OnCunset();
                    }
                }
            }
            if(result == 0)
            {
                OnResultZero();
            }
            else
            {
                OnResultNotZero();
            }

            return result;
        }

        public int LogicalAND(int and1, int and2)
        {
            var result = and1 & and2;

            CheckResultForZero(result);

            return result;
        }

        public int LogicalInclusiveOR(int or1, int or2)
        {
            var result = or1 | or2;

            CheckResultForZero(result);

            return result;
        }

        public int LogicalExclusiveOR(int or1, int or2)
        {
            var result = or1 ^ or2;

            CheckResultForZero(result);

            return result;
        }

        private void CheckResultForZero(int result)
        {
            if (result == 0)
            {
                OnResultZero();
            }
            else
            {
                OnResultNotZero();
            }
        }

        public void OnResultZero()
        {
            ResultZero?.Invoke(this, new EventArgs());
        }

        public void OnResultNotZero()
        {
            ResultNotZero?.Invoke(this, new EventArgs());
        }

        public void OnDCSet()
        {
            DCset?.Invoke(this, new EventArgs());
        }

        public void OnDCunset()
        {
            DCunset?.Invoke(this, new EventArgs());
        }

        public void OnCSet()
        {
            Cset?.Invoke(this, new EventArgs());
        }

        public void OnCunset()
        {
            Cunset?.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
