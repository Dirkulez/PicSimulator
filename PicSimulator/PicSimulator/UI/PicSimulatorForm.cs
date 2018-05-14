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
        private FrequencyInputDialog _frequencyInputDialog;
        private RegisterContentChangeDialog _registerContentChangeDialog;
        private int _listViewDoubleClickCount = 0; //neccessary to avoid calling the event twice !?

        public event EventHandler<EventArgs> LstLoaded;

        public PicSimulatorForm()
        {
            //Init
            InitializeComponent();
            InitBackgroundWorker();
            InitRegisterMemoryListView();
            InitFuncGenPortDropDown();
            WindowState = FormWindowState.Maximized;

            //prepare some items
            _dataBindingInitialized = false;
            executeToolStripMenuItem.Enabled = false;
            frequenzToolStripMenuItem.Enabled = false;
            stopToolStripMenuItem.Enabled = false;
            runningTextBox.Visible = false;
            stopedTextBox.Visible = false;
            execButton.Enabled = false;
            stopButton.Enabled = false;
            singleStepButton.Enabled = false;
            resetButton.Enabled = false;

            //subscribe events
            wregTextBox.DoubleClick += WregTextBox_DoubleClick;
            LstContentBox.SelectedIndexChanged += LstContentBox_SelectedIndexChanged;
            LstLoaded += LstLoaded_Executed;
            funcActive1.CheckedChanged += FuncActive1_CheckedChanged;
        }

        private void InitFuncGenPortDropDown()
        {
            funcGenPinComboBox.Items.Add("RA0");
            funcGenPinComboBox.Items.Add("RA1");
            funcGenPinComboBox.Items.Add("RA2");
            funcGenPinComboBox.Items.Add("RA3");
            funcGenPinComboBox.Items.Add("RA4");
            funcGenPinComboBox.Items.Add("RB0");
            funcGenPinComboBox.Items.Add("RB1");
            funcGenPinComboBox.Items.Add("RB2");
            funcGenPinComboBox.Items.Add("RB3");
            funcGenPinComboBox.Items.Add("RB4");
            funcGenPinComboBox.Items.Add("RB5");
            funcGenPinComboBox.Items.Add("RB6");
            funcGenPinComboBox.Items.Add("RB7");

            funcGenPinComboBox.SelectedIndex = 4;
        }

        private void FuncActive1_CheckedChanged(object sender, EventArgs e)
        {
            if (funcActive1.Checked)
            {
                funcGenFreqTextBox.Text = _microController.Frequency.ToString();
                _microController.FuncGen = new FunctionGenerator(double.Parse(funcGenFreqTextBox.Text), funcGenPinComboBox.Text);
            }
            else
            {
                funcGenFreqTextBox.Text = string.Empty;
                _microController.FuncGen = null;
            }
        }

        private void LstContentBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(LstContentBox.SelectedItem != null)
            {
                if (LstContentBox.SelectedItem.ToString().StartsWith(" "))
                {
                    SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
                }
            }
        }

        private void WregTextBox_DoubleClick(object sender, EventArgs e)
        {
            if(_microController == null)
            {
                return;
            }

            _registerContentChangeDialog = new RegisterContentChangeDialog("W-Reg",
                wregTextBox.Text);
            var dialogResult = _registerContentChangeDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _microController.ChangeWorkingRegisterContent((int)_registerContentChangeDialog.NewRegisterContent);
            }
        }

        private void LstLoaded_Executed(object sender, EventArgs e)
        {
            InitMicrocontroller();
            InitDataBindings();
            executeToolStripMenuItem.Enabled = true;
            frequenzToolStripMenuItem.Enabled = true;
            SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
            FillRegisterMemoryListView();
            stopedTextBox.Visible = true;
            stopButton.Enabled = false;
            execButton.Enabled = true;
            singleStepButton.Enabled = true;
            resetButton.Enabled = true;
            funcActive1.Enabled = true;
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
            ExecuteInt();
        }

        private void ExecuteInt()
        {
            _backgroundWorker.RunWorkerAsync();
            stopToolStripMenuItem.Enabled = true;
            ausführenToolStripMenuItem.Enabled = false;
            singleStepToolStripMenuItem.Enabled = false;
            execButton.Enabled = false;
            singleStepButton.Enabled = false;
            stopButton.Enabled = true;
            stopedTextBox.Visible = false;
            runningTextBox.Visible = true;
            funcActive1.Enabled = false;
            funcGenPinComboBox.Enabled = false;
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
                Thread.Sleep(10);
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
                CheckForBreakpoint(_microController.ProgramCounterContent);
            }
        }

        private void CheckForBreakpoint(int currentProgramCounter)
        {
            foreach(var item in LstContentBox.CheckedItems)
            {
                if (item.ToString().StartsWith(currentProgramCounter.ToString("X4").ToUpper()))
                {
                    _backgroundWorker.CancelAsync();
                    StopExecutionInt();
                    break;
                }
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

            var statusDataBinding = new Binding(nameof(statusRegContentTextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            statusDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            statusRegContentTextBox.DataBindings.Add(statusDataBinding);

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

            var frequencyDataBinding = new Binding(nameof(frequencyTextBox.Text), _microController, nameof(_microController.Frequency),
                true, DataSourceUpdateMode.OnPropertyChanged);
            frequencyDataBinding.Format += new ConvertEventHandler(ConvertDouble);
            frequencyTextBox.DataBindings.Add(frequencyDataBinding);

            var cycleDurationDataBinding = new Binding(nameof(cycleDurationTextBox.Text), _microController, nameof(_microController.CycleDuration),
                true, DataSourceUpdateMode.OnPropertyChanged);
            cycleDurationDataBinding.Format += new ConvertEventHandler(ConvertDouble);
            cycleDurationTextBox.DataBindings.Add(cycleDurationDataBinding);

            var runtimeDataBinding = new Binding(nameof(runtimeTextBox.Text), _microController, nameof(_microController.RuntimeDuration),
                true, DataSourceUpdateMode.OnPropertyChanged);
            runtimeDataBinding.Format += new ConvertEventHandler(ConvertDouble);
            runtimeTextBox.DataBindings.Add(runtimeDataBinding);

            zeroBitTextBox.DataBindings.Add(nameof(zeroBitTextBox.Text), _microController, nameof(_microController.ZeroBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            cBitTextBox.DataBindings.Add(nameof(cBitTextBox.Text), _microController, nameof(_microController.CBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            dcBitTextBox.DataBindings.Add(nameof(dcBitTextBox.Text), _microController, nameof(_microController.DCBit),
                false, DataSourceUpdateMode.OnPropertyChanged);
            _dataBindingInitialized = true;
        }

        private void ConvertDouble(object sender, ConvertEventArgs e)
        {
            var givenNumber = (double)e.Value;
            givenNumber = Math.Round(givenNumber, 4);
            e.Value = givenNumber.ToString();
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
            frequencyTextBox.DataBindings.RemoveAt(0);
            cycleDurationTextBox.DataBindings.RemoveAt(0);
            runtimeTextBox.DataBindings.RemoveAt(0);
            statusRegContentTextBox.DataBindings.RemoveAt(0);
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopExecutionInt();
        }

        private void StopExecutionInt()
        {
            _backgroundWorker.CancelAsync();
            stopToolStripMenuItem.Enabled = false;
            ausführenToolStripMenuItem.Enabled = true;
            singleStepToolStripMenuItem.Enabled = true;
            execButton.Enabled = true;
            stopButton.Enabled = false;
            singleStepButton.Enabled = true;
            runningTextBox.Visible = false;
            stopedTextBox.Visible = true;
            funcActive1.Enabled = true;
            funcGenPinComboBox.Enabled = true;
        }

        private void singleStepToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteSingleStepInt();
        }

        private void ExecuteSingleStepInt()
        {
            runningTextBox.Visible = true;
            stopedTextBox.Visible = false;
            ausführenToolStripMenuItem.Enabled = false;
            execButton.Enabled = false;
            _microController.ExecuteOperation();
            runningTextBox.Visible = false;
            stopedTextBox.Visible = true;
            ausführenToolStripMenuItem.Enabled = true;
            execButton.Enabled = true;
        }

        private void InitRegisterMemoryListView()
        {
            registerMemoryListView1.Columns.Add("Adr.", 35);
            registerMemoryListView1.Columns.Add("Inh.", 35);

            registerMemoryListView1.View = View.Details;
            registerMemoryListView1.DoubleClick += RegisterMemoryListView1_DoubleClick;

            registerMemoryListView2.Columns.Add("Adr.", 35);
            registerMemoryListView2.Columns.Add("Inh.", 35);

            registerMemoryListView2.View = View.Details;
            registerMemoryListView2.DoubleClick += RegisterMemoryListView2_DoubleClick;

            registerMemoryListView3.Columns.Add("Adr.", 35);
            registerMemoryListView3.Columns.Add("Inh.", 35);

            registerMemoryListView3.View = View.Details;
            registerMemoryListView3.DoubleClick += RegisterMemoryListView3_DoubleClick;

            registerMemoryListView4.Columns.Add("Adr.", 35);
            registerMemoryListView4.Columns.Add("Inh.", 35);

            registerMemoryListView4.View = View.Details;
            registerMemoryListView4.DoubleClick += RegisterMemoryListView4_DoubleClick;
        }

        private void RegisterMemoryListView4_DoubleClick(object sender, EventArgs e)
        {
            _listViewDoubleClickCount++;
            if (_listViewDoubleClickCount > 1)
            {
                _listViewDoubleClickCount = 0;
                return;
            }

            if (registerMemoryListView4.SelectedItems.Count != 1)
            {
                return;
            }

            _registerContentChangeDialog = new RegisterContentChangeDialog(registerMemoryListView4.SelectedItems[0].Text,
                registerMemoryListView4.SelectedItems[0].SubItems[1].Text);
            var dialogResult = _registerContentChangeDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _microController.WriteResultToRegisterWithGivenAddress((int)_registerContentChangeDialog.NewRegisterContent,
                    Convert.ToInt32(registerMemoryListView4.SelectedItems[0].Text, 16));
            }
        }

        private void RegisterMemoryListView3_DoubleClick(object sender, EventArgs e)
        {
            _listViewDoubleClickCount++;
            if (_listViewDoubleClickCount > 1)
            {
                _listViewDoubleClickCount = 0;
                return;
            }

            if (registerMemoryListView3.SelectedItems.Count != 1)
            {
                return;
            }

            if (registerMemoryListView3.SelectedItems[0].Text == "87" || registerMemoryListView3.SelectedItems[0].Text == "8A")
            {
                return;
            }

            _registerContentChangeDialog = new RegisterContentChangeDialog(registerMemoryListView3.SelectedItems[0].Text,
                registerMemoryListView3.SelectedItems[0].SubItems[1].Text);
            var dialogResult = _registerContentChangeDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _microController.WriteResultToRegisterWithGivenAddress((int)_registerContentChangeDialog.NewRegisterContent,
                    Convert.ToInt32(registerMemoryListView3.SelectedItems[0].Text, 16));
            }
        }

        private void RegisterMemoryListView2_DoubleClick(object sender, EventArgs e)
        {
            _listViewDoubleClickCount++;
            if (_listViewDoubleClickCount > 1)
            {
                _listViewDoubleClickCount = 0;
                return;
            }

            if (registerMemoryListView2.SelectedItems.Count != 1)
            {
                return;
            }

            _registerContentChangeDialog = new RegisterContentChangeDialog(registerMemoryListView2.SelectedItems[0].Text,
                registerMemoryListView2.SelectedItems[0].SubItems[1].Text);
            var dialogResult = _registerContentChangeDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _microController.WriteResultToRegisterWithGivenAddress((int)_registerContentChangeDialog.NewRegisterContent,
                    Convert.ToInt32(registerMemoryListView2.SelectedItems[0].Text, 16));
            }
        }

        private void RegisterMemoryListView1_DoubleClick(object sender, EventArgs e)
        {
            _listViewDoubleClickCount++;
            if (_listViewDoubleClickCount > 1)
            {
                _listViewDoubleClickCount = 0;
                return;
            }

            if(registerMemoryListView1.SelectedItems.Count != 1)
            {
                return;
            }

            if(registerMemoryListView1.SelectedItems[0].Text == "07")
            {
                return;
            }

            _registerContentChangeDialog = new RegisterContentChangeDialog(registerMemoryListView1.SelectedItems[0].Text,
                registerMemoryListView1.SelectedItems[0].SubItems[1].Text);
            var dialogResult = _registerContentChangeDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                _microController.WriteResultToRegisterWithGivenAddress((int)_registerContentChangeDialog.NewRegisterContent,
                    Convert.ToInt32(registerMemoryListView1.SelectedItems[0].Text, 16));
            }
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

        private void frequenzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frequencyInputDialog = new FrequencyInputDialog(_microController.Frequency.ToString());
            var dialogResult = _frequencyInputDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                _microController.Frequency = double.Parse(_frequencyInputDialog.Frequency);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void execButton_Click(object sender, EventArgs e)
        {
            ExecuteInt();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopExecutionInt();
        }

        private void singleStepButton_Click(object sender, EventArgs e)
        {
            ExecuteSingleStepInt();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetInt();
        }

        private void ResetInt()
        {
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
            }
            StopExecutionInt();
            _microController.PowerOnReset();
            FillRegisterMemoryListView();
            SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetInt();
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (funcActive1.Enabled && funcActive1.Checked)
            {
                _frequencyInputDialog = new FrequencyInputDialog();
                var dialogResult = _frequencyInputDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    _microController.FuncGen.Frequency = double.Parse(_frequencyInputDialog.Frequency);
                    funcGenFreqTextBox.Text = _frequencyInputDialog.Frequency;
                }
            }
        }

    }
}
