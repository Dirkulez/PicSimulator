using PicSimulator.Microcontroller;
using PicSimulator.Model;
using PicSimulator.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PicSimulator.UI
{
    public partial class PicSimulatorForm : Form
    {
        private LstParser _lstParser = new LstParser();
        private Microcontroller16F84 _microController;
        private bool _dataBindingInitialized;
        private BackgroundWorker _backgroundWorker;

        public event EventHandler<EventArgs> LstLoaded;

        public PicSimulatorForm()
        {
            InitializeComponent();
            LstLoaded += LstLoaded_Executed;
            _dataBindingInitialized = false;
            executeToolStripMenuItem.Enabled = false;
            InitBackgroundWorker();
            InitRegisterMemoryListView();
            WindowState = FormWindowState.Maximized;
         }

        private void LstLoaded_Executed(object sender, EventArgs e)
        {
            InitMicrocontroller();
            InitDataBindings();
            executeToolStripMenuItem.Enabled = true;
            debugToolStripMenuItem.Enabled = false;
            SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
            FillRegisterMemoryListView();
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
            ClearLstContentBox();
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

        private void ClearLstContentBox()
        {
            while (LstContentBox.Items.Count > 0)
            {
                LstContentBox.Items.RemoveAt(0);
            }
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundWorker.RunWorkerAsync();
        }

        private void InitBackgroundWorker()
        {
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += new DoWorkEventHandler(worker_DoWork);
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_backgroundWorker.CancellationPending)
            {
                _microController.ExecuteOperation();
                Thread.Sleep(1000);
            }
        }

        private void InitMicrocontroller()
        {
            _microController = new Microcontroller16F84(_lstParser.OperationCodes, SynchronizationContext.Current);
            _microController.PropertyChanged += MicroController_PropertyChanged;
            _microController.MemoryContentChanged += MicroController_MemoryContentChanged;
        }

        private void MicroController_MemoryContentChanged(object sender, MemoryContentChangedEventArgs e)
        {
            UpdateRegisterMemoryListView(e.MemoryAddress, e.MemoryContent);
        }

        private void MicroController_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(_microController.ProgramCounterContent))
            {
                SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
            }
        }

        private void SelectCurrentLineOfLstContentBox(int currentProgramCounter)
        {
            object itemToSelect = null;
            LstContentBox.ClearSelected();
            foreach (var item in LstContentBox.Items)
            {
                var itemContent = item.ToString();
                if (itemContent.StartsWith(currentProgramCounter.ToString("X4").ToUpper()))
                {
                    itemToSelect = item;
                    break;
                }
            }

            if (itemToSelect != null)
            {
                LstContentBox.SelectedItems.Add(itemToSelect);
            }
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

            var pclathDataBinding = new Binding(nameof(pclathTextBox.Text), _microController, nameof(_microController.PclathRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pclathDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            pclathTextBox.DataBindings.Add(pclathDataBinding);

            var cycleDataBinding = new Binding(nameof(cycleTextBox.Text), _microController, nameof(_microController.Cycle),
                false, DataSourceUpdateMode.OnPropertyChanged);
            cycleTextBox.DataBindings.Add(cycleDataBinding);

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
            pclathTextBox.DataBindings.RemoveAt(0);
            cycleTextBox.DataBindings.RemoveAt(0);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _backgroundWorker.CancelAsync();
        }

        private void singleStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _microController.ExecuteOperation();
        }

        private void InitRegisterMemoryListView()
        {
            registerMemoryListView1.Columns.Add("Adr.", 35);
            registerMemoryListView1.Columns.Add("Inh.", 35);

            registerMemoryListView1.View = View.Details;

            registerMemoryListView2.Columns.Add("Adr.", 35);
            registerMemoryListView2.Columns.Add("Inh.", 35);

            registerMemoryListView2.View = View.Details;

            registerMemoryListView3.Columns.Add("Adr.", 35);
            registerMemoryListView3.Columns.Add("Inh.", 35);

            registerMemoryListView3.View = View.Details;

            registerMemoryListView4.Columns.Add("Adr.", 35);
            registerMemoryListView4.Columns.Add("Inh.", 35);

            registerMemoryListView4.View = View.Details;
        }

        private void FillRegisterMemoryListView()
        {
            registerMemoryListView1.Clear();
            registerMemoryListView2.Clear();
            registerMemoryListView3.Clear();
            registerMemoryListView4.Clear();

            InitRegisterMemoryListView();

            for (int i = 0; i < 40; i++)
            {

                if (_microController.RegisterAdressTable.ContainsKey(i))
                {
                    var item = new ListViewItem(new[] { i.ToString("X2"), _microController.RegisterAdressTable[i].Content.ToString("X2") });

                    registerMemoryListView1.Items.Add(item);
                }
                else
                {
                    registerMemoryListView1.Items.Add(new ListViewItem(new[] { i.ToString("X2"), "ND" }));
                }

            }

            for (int i = 40; i < 80; i++)
            {

                if (_microController.RegisterAdressTable.ContainsKey(i))
                {
                    var item = new ListViewItem(new[] { i.ToString("X2"), _microController.RegisterAdressTable[i].Content.ToString("X2") });

                    registerMemoryListView2.Items.Add(item);
                }
                else
                {
                    registerMemoryListView2.Items.Add(new ListViewItem(new[] { i.ToString("X2"), "ND" }));
                }

            }

            for(int i = 128; i<168; i++)
            {
                if (_microController.RegisterAdressTable.ContainsKey(i))
                {
                    var item = new ListViewItem(new[] { i.ToString("X2"), _microController.RegisterAdressTable[i].Content.ToString("X2") });

                    registerMemoryListView3.Items.Add(item);
                }
                else
                {
                    registerMemoryListView3.Items.Add(new ListViewItem(new[] { i.ToString("X2"), "ND" }));
                }
            }

            for (int i = 168; i < 208; i++)
            {
                if (_microController.RegisterAdressTable.ContainsKey(i))
                {
                    var item = new ListViewItem(new[] { i.ToString("X2"), _microController.RegisterAdressTable[i].Content.ToString("X2") });

                    registerMemoryListView4.Items.Add(item);
                }
                else
                {
                    registerMemoryListView4.Items.Add(new ListViewItem(new[] { i.ToString("X2"), "ND" }));
                }
            }
        }

        private void UpdateRegisterMemoryListView(IEnumerable<int> memoryAddressList, int memoryContent)
        {
            foreach (var memoryAddress in memoryAddressList)
            {
                if (memoryAddress >= 0 && memoryAddress < 40)
                {
                    var itemToUpdate = registerMemoryListView1.Items[memoryAddress];
                    itemToUpdate.SubItems[1].Text = memoryContent.ToString("X2");
                }
                else if (memoryAddress >= 40 && memoryAddress < 80)
                {
                    var itemToUpdate = registerMemoryListView2.Items[memoryAddress % 40];
                    itemToUpdate.SubItems[1].Text = memoryContent.ToString("X2");
                }
                else if (memoryAddress >= 128 && memoryAddress < 168)
                {
                    var itemToUpdate = registerMemoryListView3.Items[memoryAddress % 128];
                    itemToUpdate.SubItems[1].Text = memoryContent.ToString("X2");
                }
                else if (memoryAddress >= 168 && memoryAddress < 208)
                {
                    var itemToUpdate = registerMemoryListView4.Items[memoryAddress % 168];
                    itemToUpdate.SubItems[1].Text = memoryContent.ToString("X2");
                }
            }
        }
    }
}
