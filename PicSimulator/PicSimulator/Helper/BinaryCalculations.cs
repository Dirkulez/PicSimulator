using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Helper
{
    public static class BinaryCalculations
    {

        public static int Build2ndComplement(int originFigure)
        {
            if(originFigure > 255)
            {
                return -1;
            }

            var bit0 = (originFigure & 1) == 1 ? 0 : 1;
            var bit1 = (originFigure & 2) == 2 ? 0 : 2;
            var bit2 = (originFigure & 4) == 4 ? 0 : 4;
            var bit3 = (originFigure & 8) == 8 ? 0 : 8;
            var bit4 = (originFigure & 16) == 16 ? 0 : 16;
            var bit5 = (originFigure & 32) == 32 ? 0 : 32;
            var bit6 = (originFigure & 64) == 64 ? 0 : 64;
            var bit7 = (originFigure & 128) == 128 ? 0 : 128;

            return bit0 + bit1 + bit2 + bit3 + bit4 + bit5 + bit6 + bit7 + 1;
        }

        public static int BinaryAddition(int add1, int add2, ref bool setDC, ref bool setC)
        {
            bool carry = false;
            var result = 0;
            setC = false;
            setDC = false;

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

                if(i == 8 && carry)
                {
                    setDC = true;
                }

                if(i == 128 && carry)
                {
                    setC = true;
                }
            }
            return result;
        }
    }
}
