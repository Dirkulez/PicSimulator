namespace PicSimulator.UI
{
    partial class RegisterContentChangeDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.registerAddressTextBox = new System.Windows.Forms.TextBox();
            this.registerOldContentTextBox = new System.Windows.Forms.TextBox();
            this.registerContentNewValueTextBox = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.invalidInputLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(490, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Zum Ändern des Registerinhalts den gewünschten Wert (0 - 255) eingeben: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Adresse (hex)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Alter Wert (hex)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Neuer Wert (dez)";
            // 
            // registerAddressTextBox
            // 
            this.registerAddressTextBox.Location = new System.Drawing.Point(148, 66);
            this.registerAddressTextBox.Name = "registerAddressTextBox";
            this.registerAddressTextBox.ReadOnly = true;
            this.registerAddressTextBox.Size = new System.Drawing.Size(53, 22);
            this.registerAddressTextBox.TabIndex = 4;
            // 
            // registerOldContentTextBox
            // 
            this.registerOldContentTextBox.Location = new System.Drawing.Point(148, 99);
            this.registerOldContentTextBox.Name = "registerOldContentTextBox";
            this.registerOldContentTextBox.ReadOnly = true;
            this.registerOldContentTextBox.Size = new System.Drawing.Size(55, 22);
            this.registerOldContentTextBox.TabIndex = 5;
            // 
            // registerContentNewValueTextBox
            // 
            this.registerContentNewValueTextBox.Location = new System.Drawing.Point(148, 133);
            this.registerContentNewValueTextBox.Name = "registerContentNewValueTextBox";
            this.registerContentNewValueTextBox.Size = new System.Drawing.Size(54, 22);
            this.registerContentNewValueTextBox.TabIndex = 6;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(255, 191);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(103, 36);
            this.okButton.TabIndex = 7;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(120, 191);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(103, 34);
            this.cancelButton.TabIndex = 8;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // invalidInputLabel
            // 
            this.invalidInputLabel.AutoSize = true;
            this.invalidInputLabel.ForeColor = System.Drawing.Color.Red;
            this.invalidInputLabel.Location = new System.Drawing.Point(208, 136);
            this.invalidInputLabel.Name = "invalidInputLabel";
            this.invalidInputLabel.Size = new System.Drawing.Size(110, 17);
            this.invalidInputLabel.TabIndex = 9;
            this.invalidInputLabel.Text = "Ungültiger Wert!";
            // 
            // RegisterContentChangeDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 255);
            this.Controls.Add(this.invalidInputLabel);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.registerContentNewValueTextBox);
            this.Controls.Add(this.registerOldContentTextBox);
            this.Controls.Add(this.registerAddressTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "RegisterContentChangeDialog";
            this.Text = "Registerinhalt ändern";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox registerAddressTextBox;
        private System.Windows.Forms.TextBox registerOldContentTextBox;
        private System.Windows.Forms.TextBox registerContentNewValueTextBox;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label invalidInputLabel;
    }
}