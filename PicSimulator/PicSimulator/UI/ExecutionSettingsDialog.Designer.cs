namespace PicSimulator.UI
{
    partial class ExecutionSettingsDialog
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
            this.executionDelayTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.executionDelayInvalid = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(422, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Verzögerung nach jeder Befehlsausführung (positive Ganzzahl) : ";
            // 
            // executionDelayTextBox
            // 
            this.executionDelayTextBox.Location = new System.Drawing.Point(427, 30);
            this.executionDelayTextBox.Name = "executionDelayTextBox";
            this.executionDelayTextBox.Size = new System.Drawing.Size(100, 22);
            this.executionDelayTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(533, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "ms";
            // 
            // executionDelayInvalid
            // 
            this.executionDelayInvalid.AutoSize = true;
            this.executionDelayInvalid.ForeColor = System.Drawing.Color.Red;
            this.executionDelayInvalid.Location = new System.Drawing.Point(565, 33);
            this.executionDelayInvalid.Name = "executionDelayInvalid";
            this.executionDelayInvalid.Size = new System.Drawing.Size(110, 17);
            this.executionDelayInvalid.TabIndex = 3;
            this.executionDelayInvalid.Text = "Ungültiger Wert!";
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(296, 118);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(89, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Abbrechen";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(427, 118);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(86, 23);
            this.okButton.TabIndex = 5;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // ExecutionSettingsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 184);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.executionDelayInvalid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.executionDelayTextBox);
            this.Controls.Add(this.label1);
            this.Name = "ExecutionSettingsDialog";
            this.Text = "Ausführungseinstellungen";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox executionDelayTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label executionDelayInvalid;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
    }
}