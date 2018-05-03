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
    public partial class RegisterContentChangeDialog : Form
    {
        private byte _newRegisterContent;
        public byte NewRegisterContent
        {
            get { return _newRegisterContent; }
        }
        public RegisterContentChangeDialog(string registerAddress, string registerOldContent)
        {
            InitializeComponent();
            registerAddressTextBox.Text = registerAddress;
            registerOldContentTextBox.Text = registerOldContent;
            registerContentNewValueTextBox.TextChanged += RegisterContentNewValueTextBox_TextChanged;
            invalidInputLabel.Visible = false;
            okButton.Enabled = false;
        }

        private void RegisterContentNewValueTextBox_TextChanged(object sender, EventArgs e)
        {
            var content = registerContentNewValueTextBox.Text;
            byte contentAsByte = 0;
            var result = byte.TryParse(content, out contentAsByte);

            if (result)
            {
                invalidInputLabel.Visible = false;
                okButton.Enabled = true;
                _newRegisterContent = contentAsByte;
            }
            else
            {
                invalidInputLabel.Visible = true;
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
