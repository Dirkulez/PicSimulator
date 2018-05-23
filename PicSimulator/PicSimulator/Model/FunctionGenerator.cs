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
        private double _edgeDuration; // microseconds

        #endregion

        #region Events

        public event EventHandler<FunctionGeneratorEventArguments> EdgeChanged;

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
                _edgeDuration = (1 / Frequency) * 2;
            }
        }

        #endregion

        #region Constructor

        public FunctionGenerator(double frequency, string pin)
        {
            Frequency = frequency;
            Pin = pin;
            _currentState = 0;
            _timeSinceLastEdgeChange = 0;
        }

        #endregion

        #region Methods

        public void ProcessFunction(double microCycleDuration, ref int numberOfRisingEdges, ref int numberOfFallingEdges)
        {
            numberOfRisingEdges = 0;
            numberOfFallingEdges = 0;
            var timeNew = _timeSinceLastEdgeChange + microCycleDuration;

            while(timeNew >= _edgeDuration)
            {
                if(_currentState == 0)
                {
                    _currentState = 1;
                    timeNew = timeNew - _edgeDuration;
                    OnEdgeChanged(new FunctionGeneratorEventArguments(Pin, 1));
                    numberOfRisingEdges++;
                }
                else
                {
                    _currentState = 0;
                    timeNew = timeNew - _edgeDuration;
                    OnEdgeChanged(new FunctionGeneratorEventArguments(Pin, 0));
                    numberOfFallingEdges++;
                }
            }

            _timeSinceLastEdgeChange = timeNew;
        }

        public void OnEdgeChanged(FunctionGeneratorEventArguments e)
        {
            EdgeChanged?.Invoke(this, e);
        }

        #endregion
    }
}
