using PicSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class Timer0
    {
        private int _cyclesSinceLastIncrement = 0;

        public int IncreaseTimer(bool prescalerActive, int prescalerValue)
        {
            if (!prescalerActive)
            {
                _cyclesSinceLastIncrement = 0;
                return 1;
            }
            else
            {
                //prescaler is active
                var prescalerRate = DecodePrescalerRate(prescalerValue);
                if(_cyclesSinceLastIncrement == prescalerRate-1)
                {
                    _cyclesSinceLastIncrement = 0;
                    return 1;
                }
                else
                {
                    _cyclesSinceLastIncrement++;
                    return 0;
                }
            }
        }

        private int DecodePrescalerRate(int prescalerValue)
        {
            if(prescalerValue == 0)
            {
                return 2;
            }
            else if(prescalerValue == 1)
            {
                return 4;
            }
            else if (prescalerValue == 2)
            {
                return 8;
            }
            else if (prescalerValue == 3)
            {
                return 16;
            }
            else if (prescalerValue == 4)
            {
                return 32;
            }
            else if (prescalerValue == 5)
            {
                return 64;
            }
            else if (prescalerValue == 6)
            {
                return 128;
            }
            else if (prescalerValue == 7)
            {
                return 256;
            }

            return 0;
        }
    }
}
