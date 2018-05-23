using PicSimulator.Microcontroller;
using PicSimulator.Model;
using PicSimulator.Parser;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
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
        private SerialPort _comPort;
        private int _executionDelay = 100; // delay after each instruction in ms

        public event EventHandler<EventArgs> LstLoaded;

        public PicSimulatorForm()
        {
            //Init
            InitializeComponent();
            InitBackgroundWorker();
            InitRegisterMemoryListView();
            InitFuncGenPortDropDown();
            InitHardwareConnection();
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
            mclrButton.Enabled = false;
            wdtEnabledCheckBox.Enabled = false;

            //subscribe events
            wregTextBox.DoubleClick += WregTextBox_DoubleClick;
            LstContentBox.SelectedIndexChanged += LstContentBox_SelectedIndexChanged;
            LstLoaded += LstLoaded_Executed;
            funcActive1.CheckedChanged += FuncActive1_CheckedChanged;
            hardwareCheckBox.CheckedChanged += HardwareCheckBox_CheckedChanged;
            funcGenPinComboBox.SelectedIndexChanged += FuncGenPinComboBox_SelectedIndexChanged;
            wdtEnabledCheckBox.CheckedChanged += WdtEnabledCheckBox_CheckedChanged;
        }

        private void WdtEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (wdtEnabledCheckBox.Checked)
            {
                _microController.EnableWatchDogTimer();
            }
            else
            {
                _microController.DisableWatchDogTimer();
            }
        }

        private void FuncGenPinComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_microController.FuncGen != null)
            {
                _microController.FuncGen.Pin = funcGenPinComboBox.Text;
            }
        }

        private void HardwareCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (hardwareCheckBox.Checked)
            {
                try
                {
                    _comPort = new SerialPort(hardwareComboBox.Text, 4800, Parity.None, 8, StopBits.One);
                    _comPort.Open();
                    executeHardwareButton.Enabled = true;
                    hardwareComboBox.Enabled = false;
                }catch(Exception)
                {
                    MessageBox.Show("Beim Verbinden mit dem angegebenen COM-Port ist ein Fehler aufgetreten." +
                        " Bitte stellen Sie sicher, dass der ausgewählte Port aktiv und bereit zur Verwendung ist.",
                        "Es ist ein Fehler aufgetreten", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _comPort = null;
                    hardwareCheckBox.Checked = false;
                }
            }
            else
            {
                if (_comPort != null && _comPort.IsOpen)
                {
                    _comPort.Close();
                    _comPort = null;
                }
                executeHardwareButton.Enabled = false;
                hardwareComboBox.Enabled = true;
            }
        }

        private void InitHardwareConnection()
        {
            refreshComPortsButton.Enabled = false;
            var availableComPortNames = SerialPort.GetPortNames();

            foreach(var port in availableComPortNames)
            {
                hardwareComboBox.Items.Add(port);
            }

            if (hardwareComboBox.Items.Count > 0)
            {
                hardwareComboBox.SelectedIndex = 0;
            }

            hardwareCheckBox.Enabled = false;
            executeHardwareButton.Enabled = false;
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
            mclrButton.Enabled = true;
            funcActive1.Enabled = true;
            wdtEnabledCheckBox.Enabled = true;
            if(hardwareComboBox.Items.Count > 0)
            {
                hardwareCheckBox.Enabled = true;
            }
            refreshComPortsButton.Enabled = true;
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
            settingToolStripMenuItem.Enabled = false;
            wdtEnabledCheckBox.Enabled = false;
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
                Thread.Sleep(_executionDelay);
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

            var pdBitDataBinding = new Binding(nameof(pdTextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pdBitDataBinding.Format += new ConvertEventHandler(ExtractBit3FromGivenRegisterContent);
            pdTextBox.DataBindings.Add(pdBitDataBinding);

            var toBitDataBinding = new Binding(nameof(toTextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            toBitDataBinding.Format += new ConvertEventHandler(ExtractBit4FromGivenRegisterContent);
            toTextBox.DataBindings.Add(toBitDataBinding);

            var rp0BitDataBinding = new Binding(nameof(rp0TextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            rp0BitDataBinding.Format += new ConvertEventHandler(ExtractBit5FromGivenRegisterContent);
            rp0TextBox.DataBindings.Add(rp0BitDataBinding);

            var rp1BitDataBinding = new Binding(nameof(rp1TextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            rp1BitDataBinding.Format += new ConvertEventHandler(ExtractBit6FromGivenRegisterContent);
            rp1TextBox.DataBindings.Add(rp1BitDataBinding);

            var irpBitDataBinding = new Binding(nameof(irpTextBox.Text), _microController, nameof(_microController.StatusRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            irpBitDataBinding.Format += new ConvertEventHandler(ExtractBit7FromGivenRegisterContent);
            irpTextBox.DataBindings.Add(irpBitDataBinding);

            var fsrDataBinding = new Binding(nameof(fsrTextBox.Text), _microController, nameof(_microController.FsrRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            fsrDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            fsrTextBox.DataBindings.Add(fsrDataBinding);

            var pcDataBinding = new Binding(nameof(pcTextBox.Text), _microController, nameof(_microController.ProgramCounterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pcDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith4Digits);
            pcTextBox.DataBindings.Add(pcDataBinding);

            var pclathDataBinding = new Binding(nameof(pclathTextBox.Text), _microController, nameof(_microController.PclathRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pclathDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            pclathTextBox.DataBindings.Add(pclathDataBinding);

            var pclDataBinding = new Binding(nameof(pclContentTextBox.Text), _microController, nameof(_microController.PclRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            pclDataBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            pclContentTextBox.DataBindings.Add(pclDataBinding);

            var optionRegisterBinding = new Binding(nameof(optionTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            optionRegisterBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            optionTextBox.DataBindings.Add(optionRegisterBinding);

            var ps0BitDataBinding = new Binding(nameof(ps0TextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            ps0BitDataBinding.Format += new ConvertEventHandler(ExtractBit0FromGivenRegisterContent);
            ps0TextBox.DataBindings.Add(ps0BitDataBinding);

            var ps1BitDataBinding = new Binding(nameof(ps1TextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            ps1BitDataBinding.Format += new ConvertEventHandler(ExtractBit1FromGivenRegisterContent);
            ps1TextBox.DataBindings.Add(ps1BitDataBinding);

            var ps2BitDataBinding = new Binding(nameof(ps2TextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            ps2BitDataBinding.Format += new ConvertEventHandler(ExtractBit2FromGivenRegisterContent);
            ps2TextBox.DataBindings.Add(ps2BitDataBinding);

            var psaBitDataBinding = new Binding(nameof(psaTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            psaBitDataBinding.Format += new ConvertEventHandler(ExtractBit3FromGivenRegisterContent);
            psaTextBox.DataBindings.Add(psaBitDataBinding);

            var toseBitDataBinding = new Binding(nameof(toseTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            toseBitDataBinding.Format += new ConvertEventHandler(ExtractBit4FromGivenRegisterContent);
            toseTextBox.DataBindings.Add(toseBitDataBinding);

            var tocsBitDataBinding = new Binding(nameof(tocsTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            tocsBitDataBinding.Format += new ConvertEventHandler(ExtractBit5FromGivenRegisterContent);
            tocsTextBox.DataBindings.Add(tocsBitDataBinding);

            var intedgBitDataBinding = new Binding(nameof(intedgTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            intedgBitDataBinding.Format += new ConvertEventHandler(ExtractBit6FromGivenRegisterContent);
            intedgTextBox.DataBindings.Add(intedgBitDataBinding);

            var rbpuBitDataBinding = new Binding(nameof(rbpuTextBox.Text), _microController, nameof(_microController.OptionRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            rbpuBitDataBinding.Format += new ConvertEventHandler(ExtractBit7FromGivenRegisterContent);
            rbpuTextBox.DataBindings.Add(rbpuBitDataBinding);

            var intconRegisterBinding = new Binding(nameof(intconTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            intconRegisterBinding.Format += new ConvertEventHandler(ConvertRegisterContentToHexWith2Digits);
            intconTextBox.DataBindings.Add(intconRegisterBinding);

            var rbifBitDataBinding = new Binding(nameof(rbifTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            rbifBitDataBinding.Format += new ConvertEventHandler(ExtractBit0FromGivenRegisterContent);
            rbifTextBox.DataBindings.Add(rbifBitDataBinding);

            var intfBitDataBinding = new Binding(nameof(intfTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            intfBitDataBinding.Format += new ConvertEventHandler(ExtractBit1FromGivenRegisterContent);
            intfTextBox.DataBindings.Add(intfBitDataBinding);

            var t0ifBitDataBinding = new Binding(nameof(t0ifTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            t0ifBitDataBinding.Format += new ConvertEventHandler(ExtractBit2FromGivenRegisterContent);
            t0ifTextBox.DataBindings.Add(t0ifBitDataBinding);

            var rbieBitDataBinding = new Binding(nameof(rbieTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            rbieBitDataBinding.Format += new ConvertEventHandler(ExtractBit3FromGivenRegisterContent);
            rbieTextBox.DataBindings.Add(rbieBitDataBinding);

            var inteBitDataBinding = new Binding(nameof(inteTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            inteBitDataBinding.Format += new ConvertEventHandler(ExtractBit4FromGivenRegisterContent);
            inteTextBox.DataBindings.Add(inteBitDataBinding);

            var t0ieBitDataBinding = new Binding(nameof(t0ieTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            t0ieBitDataBinding.Format += new ConvertEventHandler(ExtractBit5FromGivenRegisterContent);
            t0ieTextBox.DataBindings.Add(t0ieBitDataBinding);

            var eeieBitDataBinding = new Binding(nameof(eeieTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            eeieBitDataBinding.Format += new ConvertEventHandler(ExtractBit6FromGivenRegisterContent);
            eeieTextBox.DataBindings.Add(eeieBitDataBinding);

            var gieBitDataBinding = new Binding(nameof(gieTextBox.Text), _microController, nameof(_microController.IntconRegisterContent),
                true, DataSourceUpdateMode.OnPropertyChanged);
            gieBitDataBinding.Format += new ConvertEventHandler(ExtractBit7FromGivenRegisterContent);
            gieTextBox.DataBindings.Add(gieBitDataBinding);

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

        private void ExtractBit0FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 1) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit1FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 2) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit2FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 4) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }


        private void ExtractBit3FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if((givenRegisterContent & 8) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit4FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 16) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit5FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 32) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit6FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 64) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
            }
        }

        private void ExtractBit7FromGivenRegisterContent(object sender, ConvertEventArgs e)
        {
            var givenRegisterContent = (int)e.Value;
            if ((givenRegisterContent & 128) != 0)
            {
                e.Value = "1";
            }
            else
            {
                e.Value = "0";
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
            fsrTextBox.DataBindings.RemoveAt(0);
            pclContentTextBox.DataBindings.RemoveAt(0);
            pdTextBox.DataBindings.RemoveAt(0);
            toTextBox.DataBindings.RemoveAt(0);
            rp0TextBox.DataBindings.RemoveAt(0);
            rp1TextBox.DataBindings.RemoveAt(0);
            irpTextBox.DataBindings.RemoveAt(0);
            optionTextBox.DataBindings.RemoveAt(0);
            ps0TextBox.DataBindings.RemoveAt(0);
            ps1TextBox.DataBindings.RemoveAt(0);
            ps2TextBox.DataBindings.RemoveAt(0);
            psaTextBox.DataBindings.RemoveAt(0);
            toseTextBox.DataBindings.RemoveAt(0);
            tocsTextBox.DataBindings.RemoveAt(0);
            intedgTextBox.DataBindings.RemoveAt(0);
            rbpuTextBox.DataBindings.RemoveAt(0);
            intconTextBox.DataBindings.RemoveAt(0);
            rbifTextBox.DataBindings.RemoveAt(0);
            intfTextBox.DataBindings.RemoveAt(0);
            t0ifTextBox.DataBindings.RemoveAt(0);
            rbieTextBox.DataBindings.RemoveAt(0);
            inteTextBox.DataBindings.RemoveAt(0);
            t0ieTextBox.DataBindings.RemoveAt(0);
            eeieTextBox.DataBindings.RemoveAt(0);
            gieTextBox.DataBindings.RemoveAt(0);
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
            settingToolStripMenuItem.Enabled = true;
            wdtEnabledCheckBox.Enabled = true;
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
            PowerOnResetInt();
        }

        private void PowerOnResetInt()
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

        private void MCLRResetInt()
        {
            if (_backgroundWorker.IsBusy)
            {
                _backgroundWorker.CancelAsync();
            }
            StopExecutionInt();
            _microController.MCLRReset();
            FillRegisterMemoryListView();
            SelectCurrentLineOfLstContentBox(_microController.ProgramCounterContent);
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            PowerOnResetInt();
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

        private void refreshComPortsButton_Click(object sender, EventArgs e)
        {
            hardwareComboBox.Items.Clear();
            InitHardwareConnection();
            if(hardwareComboBox.Items.Count > 0)
            {
                hardwareCheckBox.Enabled = true;
            }
            else
            {
                hardwareComboBox.Enabled = false;
            }
        }

        private void ausfuehrungToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var executionSettingsDialog = new ExecutionSettingsDialog(_executionDelay);
            var dialogResult = executionSettingsDialog.ShowDialog();
            if(dialogResult == DialogResult.OK)
            {
                _executionDelay = executionSettingsDialog.ExecutionDelay;
            }
        }

        private void executeHardwareButton_Click(object sender, EventArgs e)
        {
            PicViewHardwareConnector.SendData(_microController.TrisA, _microController.PortA, _microController.TrisB, _microController.PortB, _comPort);
            var newPortAContent = 0;
            var newPortBContent = 0;
            var readDataSuccessful = PicViewHardwareConnector.ReadData(ref newPortAContent, ref newPortBContent, _comPort);
            if (readDataSuccessful)
            {
                _microController.PortA = newPortAContent;
                _microController.PortB = newPortBContent;
            }
        }

        private void mclrButton_Click(object sender, EventArgs e)
        {
            MCLRResetInt();
        }

        private void mCLRToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MCLRResetInt();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("PicSimulator für die Vorlesung von Herr Lehmann.\nVon Dirk Watteroth und Luke Adam.\n",
                "Über PicSimulator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pICDokumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("PIC16F8x.pdf"))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.EnableRaisingEvents = false;
                process.StartInfo.FileName = "PIC16F8x.pdf";
                process.Start();
            }
            else
            {
                MessageBox.Show("Die PIC Dokumentation PIC16F8x.pdf konnte nicht geöffnet werden. Bitte stellen Sie sicher, dass die entsprechende Datei vorhanden ist und im gleichen Verzeichnis wie die Anwendung liegt.",
                    "PIC Dokumentation konnte nicht gefunden werden", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void projektDokumentationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists("Doku.pdf"))
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.EnableRaisingEvents = false;
                process.StartInfo.FileName = "Doku.pdf";
                process.Start();
            }
            else
            {
                MessageBox.Show("Die Projekt Dokumentation Doku.pdf konnte nicht geöffnet werden. Bitte stellen Sie sicher, dass die entsprechende Datei vorhanden ist und im gleichen Verzeichnis wie die Anwendung liegt.",
                    "Projekt Dokumentation konnte nicht gefunden werden", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
