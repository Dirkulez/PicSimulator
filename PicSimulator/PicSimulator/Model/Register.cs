using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PicSimulator.Model
{
    public class Register
    {
        private int _content = 0;
        private string _name = string.Empty;

        public int Content
        {
            get { return _content; }
            set
            {
                if (value != _content)
                {
                    _content = value;
                }
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                if(value != _name)
                {
                    _name = value;
                }
            }
        }

        public Register(int content, string name)
        {
            this.Name = name;
            this.Content = content;
        }


    }
}
