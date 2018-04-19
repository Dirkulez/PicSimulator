using PicSimulator.Microcontroller;
using PicSimulator.Parser;
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
    public partial class PicSimulatorForm : Form
    {
        private LstParser _lstParser = new LstParser();
        private Microcontroller16F84 _microController;
        private bool _dataBindingInitialized;

        public event EventHandler<EventArgs> LstLoaded;

        public PicSimulatorForm()
        {
            InitializeComponent();
            LstLoaded += LstLoaded_Executed;
            _dataBindingInitialized = false;
        }

        private void LstLoaded_Executed(object sender, EventArgs e)
        {
            InitMicrocontroller();
            InitDataBindings();
        }

        private void ZeroBitChanged_Executed(object sender, EventArgs e)
        {
            zeroBitTextBox.Text = _microController.ZeroBit.ToString();
        }

        public void OnLstLoaded(object sender, EventArgs e)
        {
            EventHandler<EventArgs> handler = LstLoaded;
            if(handler != null)
            {
                handler(sender, e);
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "D:\\DHBW\\Theorie1\\Rechnertechnik2\\TPicSim";
            openFileDialog.Filter = "LST files (*.LST)|*.LST|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                   System.IO.StreamReader(openFileDialog.FileName);
                _lstParser.InitLstContent(sr);
                sr.Close();
                foreach (var line in _lstParser.LstContent)
                {
                    LstContentBox.Items.Add(line);
                }
            }

            OnLstLoaded(this, new EventArgs());
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _microController.ExecuteOperation(0);
        }

        private void InitMicrocontroller()
        {
            _microController = new Microcontroller16F84(_lstParser.OperationCodes);
        }

        private void InitDataBindings()
        {
            if (_dataBindingInitialized)
            {
                RemoveDataBindings();
            }

            var wregDataBinding = new Binding(nameof(wregTextBox.Text), _microController, nameof(_microController.WorkingRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            wregDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            wregTextBox.DataBindings.Add(wregDataBinding);

            var pcDataBinding = new Binding(nameof(pcTextBox.Text), _microController, nameof(_microController.ProgramCounterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pcDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith4Digits);
            pcTextBox.DataBindings.Add(pcDataBinding);

            zeroBitTextBox.DataBindings.Add(nameof(zeroBitTextBox.Text), _microController, nameof(_microController.ZeroBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            cBitTextBox.DataBindings.Add(nameof(cBitTextBox.Text), _microController, nameof(_microController.CBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            dcBitTextBox.DataBindings.Add(nameof(dcBitTextBox.Text), _microController, nameof(_microController.DCBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            _dataBindingInitialized = true;
        }

        private void ConvertRegisterContentToHexWith2Digits(object sender, ConvertEventArgs e)
        {
            var givenNumber = (int)e.Value;
            if(givenNumber == 0)
            {
                e.Value = "00";
            }
            else
            {
                e.Value = givenNumber.ToString("X2").ToUpper();
            }
        }

        private void ConvertRegisterContentToHexWith4Digits(object sender, ConvertEventArgs e)
        {
            var givenNumber = (int)e.Value;
            if (givenNumber == 0)
            {
                e.Value = "0000";
            }
            else
            {
                e.Value = givenNumber.ToString("X4").ToUpper();
            }
        }

        private void RemoveDataBindings()
        {
            wregTextBox.DataBindings.RemoveAt(0);
            pcTextBox.DataBindings.RemoveAt(0);
            zeroBitTextBox.DataBindings.RemoveAt(0);
            cBitTextBox.DataBindings.RemoveAt(0);
            dcBitTextBox.DataBindings.RemoveAt(0);
        }

    }
}
