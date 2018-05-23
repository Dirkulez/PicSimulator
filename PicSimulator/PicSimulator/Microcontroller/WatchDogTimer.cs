using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Microcontroller
{
    public class WatchDogTimer
    {

        private int _defaultTimeOutPeriod = 18; // in ms 
        private bool _enabled = false;
        private Microcontroller16F84 _microController;
        private double _runtimeDuration = 0;

        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public WatchDogTimer(Microcontroller16F84 microController)
        {
            _microController = microController;
        }

        public bool ProcessWDT(double elapsedMiliseconds, bool prescalerAssigned, int prescalerValue) 
        {
            _runtimeDuration += elapsedMiliseconds;
            var msToTimeOut = _defaultTimeOutPeriod - _runtimeDuration;

            if (prescalerAssigned)
            {
                var prescalerRate = DecodePrescalerRate(prescalerValue);
                msToTimeOut = (_defaultTimeOutPeriod * prescalerRate) - _runtimeDuration;
            }

            if(msToTimeOut <= 0)
            {
                //time out reached
                _runtimeDuration = 0;
                return true;
            }

            return false;
        }

        private int DecodePrescalerRate(int prescalerValue)
        {
            if (prescalerValue == 0)
            {
                return 1;
            }
            else if (prescalerValue == 1)
            {
                return 2;
            }
            else if (prescalerValue == 2)
            {
                return 4;
            }
            else if (prescalerValue == 3)
            {
                return 8;
            }
            else if (prescalerValue == 4)
            {
                return 16;
            }
            else if (prescalerValue == 5)
            {
                return 32;
            }
            else if (prescalerValue == 6)
            {
                return 64;
            }
            else if (prescalerValue == 7)
            {
                return 128;
            }

            return 0;
        }

        public void Reset()
        {
            _runtimeDuration = 0;
        }

    }
}
