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
        #region Fields
        private List<string> _lstContent;
        private List<string> _operationCodes;
        #endregion

        #region Properties
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

        public List<string> OperationCodes
        {
            get { return _operationCodes; }
            set
            {
                if(value != _operationCodes)
                {
                    _operationCodes = value;
                }
            }
        }
        #endregion

        #region Constructor
        public LstParser()
        {
            //nop
        }

        public LstParser(StreamReader lstStream)
        {
            InitLstContent(lstStream);
        }
        #endregion

        #region Methods
        public void InitLstContent(StreamReader lstStream)
        {
            LstContent = new List<string>();
            while (!lstStream.EndOfStream)
            {
                LstContent.Add(lstStream.ReadLine());
            }

            ParseLstContentToOperationCodes();
        }

        private void ParseLstContentToOperationCodes()
        {
            OperationCodes = new List<string>();
            foreach(var line in LstContent)
            {
                if(!line.StartsWith(" "))
                {
                    OperationCodes.Add(line.Substring(0, 9));
                }
            }
        }
        #endregion
    }
}
