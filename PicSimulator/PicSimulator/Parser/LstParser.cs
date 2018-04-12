using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Parser
{
    public class LstParser
    {
        private List<string> _lstContent;
        private bool _initialized;

        public List<string> LstContent
        {
            get { return _lstContent; }
            set
            {
                if (value != _lstContent)
                {
                    _lstContent = value;
                }
            }
        }

        public LstParser()
        {
            //nop
        }

        public LstParser(StreamReader lstStream)
        {
            InitLstContent(lstStream);
        }

        public void InitLstContent(StreamReader lstStream)
        {
            _lstContent = new List<string>();
            while (!lstStream.EndOfStream)
            {
                _lstContent.Add(lstStream.ReadLine());
            }
            _initialized = true;
        }
    }
}
