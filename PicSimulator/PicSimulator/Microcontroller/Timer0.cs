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
        private int _skipCycles = 0;
        private int _edgesSinceLastIncrement = 0;

        public int SkipCycles
        {
            get { return _skipCycles; }
            set { _skipCycles = value; }
        }

        //method is called when timer mode is active to get the new timer value
        public int IncreaseTimer(bool prescalerActive, int prescalerValue)
        {
            if(_skipCycles > 0)
            {
                _skipCycles--;
                return 0;
            }
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

        //method is called to get the prescaler rate 
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

        //method is called when counter mode is active to get the new timer value
        internal int IncreaseCounter(int numberOfRisingEdgesInExtFuncGen, int numberOfFallingEdgesInExtFuncGen, bool countOnRisingEdge,
            bool prescalerActive, int prescalerValue)
        {
            int edgesToCount = 0;

            if (countOnRisingEdge)
            {
                edgesToCount = numberOfRisingEdgesInExtFuncGen;
            }
            else
            {
                edgesToCount = numberOfFallingEdgesInExtFuncGen;
            }

            if (!prescalerActive)
            {
                _edgesSinceLastIncrement = 0;
                return edgesToCount;
            }
            else
            {
                var returnValue = 0;
                var prescalerRate = DecodePrescalerRate(prescalerValue);
                while(edgesToCount > 0)
                {
                    edgesToCount--;
                    _edgesSinceLastIncrement++;
                    if(_edgesSinceLastIncrement == prescalerRate)
                    {
                        _edgesSinceLastIncrement = 0;
                        returnValue++;
                    }
                }

                return returnValue;
            }
        }
    }
}
