using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicSimulator.UI
{
    public partial class FrequencyInputDialog : Form
    {


        public string Frequency
        {
            get { return maskedTextBox1.Text; }
            set { maskedTextBox1.Text = value; }
        }

        public FrequencyInputDialog()
        {
            InitializeComponent();
        }

        public FrequencyInputDialog(string currentFrequency): base()
        {
            InitializeComponent();
            Frequency = currentFrequency;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
