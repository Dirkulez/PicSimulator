using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Model
{
    public class FunctionGenerator
    {
        #region Fields

        private string _pin;
        private double _frequency;
        private int _currentState; // 0 or 1 
        private double _timeSinceLastEdgeChange; // microseconds

        #endregion

        #region Properties

        public String Pin
        {
            get { return _pin; }
            set
            {
                _pin = value;
            }
        }

        public Double Frequency
        {
            get { return _frequency; }
            set
            {
                _frequency = value;
            }
        }

        #endregion

        #region Constructor

        public FunctionGenerator(double frequency)
        {
            Frequency = frequency;
            Pin = "RA4";
            _currentState = 0;
            _timeSinceLastEdgeChange = 0.25;
        }

        #endregion
    }
}
