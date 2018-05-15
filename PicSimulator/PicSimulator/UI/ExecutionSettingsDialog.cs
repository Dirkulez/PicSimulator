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
    public partial class ExecutionSettingsDialog : Form
    {
        private int _executionDelay;

        public int ExecutionDelay
        {
            get { return _executionDelay; }
            private set { _executionDelay = value;}
        }
        public ExecutionSettingsDialog(int currentExecutionDelay)
        {
            InitializeComponent();
            _executionDelay = currentExecutionDelay;
            executionDelayTextBox.Text = _executionDelay.ToString();
            executionDelayTextBox.TextChanged += ExecutionDelayTextBox_TextChanged;
            executionDelayInvalid.Visible = false;
        }

        private void ExecutionDelayTextBox_TextChanged(object sender, EventArgs e)
        {
            var enteredExecutionDelay = executionDelayTextBox.Text;
            int enteredExecutionDelayAsInt = 0;
            var result = int.TryParse(enteredExecutionDelay, out enteredExecutionDelayAsInt);

            if (result && (enteredExecutionDelayAsInt > 0))
            {
                executionDelayInvalid.Visible = false;
                okButton.Enabled = true;
                _executionDelay = enteredExecutionDelayAsInt;
            }
            else
            {
                executionDelayInvalid.Visible = true;
                okButton.Enabled = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
