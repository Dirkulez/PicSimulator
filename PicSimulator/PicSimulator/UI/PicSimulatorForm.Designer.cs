namespace PicSimulator.UI
{
    partial class PicSimulatorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PicSimulatorForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.executeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ausführenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LstContentBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cycleTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.pclathTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dcBitTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cBitTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.zeroBitTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pcTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.wregTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.registerMemoryListView1 = new System.Windows.Forms.ListView();
            this.registerMemoryListView2 = new System.Windows.Forms.ListView();
            this.registerMemoryListView3 = new System.Windows.Forms.ListView();
            this.registerMemoryListView4 = new System.Windows.Forms.ListView();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.executeToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1602, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(57, 24);
            this.fileToolStripMenuItem.Text = "Datei";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.openToolStripMenuItem.Text = "Öffnen";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(182, 26);
            this.exitToolStripMenuItem.Text = "Ende";
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ausführenToolStripMenuItem,
            this.singleStepToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.stopToolStripMenuItem});
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new System.Drawing.Size(87, 24);
            this.executeToolStripMenuItem.Text = "Ausführen";
            // 
            // ausführenToolStripMenuItem
            // 
            this.ausführenToolStripMenuItem.Name = "ausführenToolStripMenuItem";
            this.ausführenToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.ausführenToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.ausführenToolStripMenuItem.Text = "Ausführen";
            this.ausführenToolStripMenuItem.Click += new System.EventHandler(this.executeToolStripMenuItem_Click);
            // 
            // singleStepToolStripMenuItem
            // 
            this.singleStepToolStripMenuItem.Name = "singleStepToolStripMenuItem";
            this.singleStepToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.singleStepToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.singleStepToolStripMenuItem.Text = "Single Step";
            this.singleStepToolStripMenuItem.Click += new System.EventHandler(this.singleStepToolStripMenuItem_Click);
            // 
            // debugToolStripMenuItem
            // 
            this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
            this.debugToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F6;
            this.debugToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.debugToolStripMenuItem.Text = "Debug";
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.helpToolStripMenuItem.Text = "Hilfe";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.aboutToolStripMenuItem.Text = "Über";
            // 
            // LstContentBox
            // 
            this.LstContentBox.FormattingEnabled = true;
            this.LstContentBox.Location = new System.Drawing.Point(12, 40);
            this.LstContentBox.Name = "LstContentBox";
            this.LstContentBox.Size = new System.Drawing.Size(831, 888);
            this.LstContentBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cycleTextBox);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.pclathTextBox);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dcBitTextBox);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.cBitTextBox);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.zeroBitTextBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.pcTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.wregTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(877, 47);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 521);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spezialfunktionsregister";
            // 
            // cycleTextBox
            // 
            this.cycleTextBox.Location = new System.Drawing.Point(88, 250);
            this.cycleTextBox.Name = "cycleTextBox";
            this.cycleTextBox.ReadOnly = true;
            this.cycleTextBox.Size = new System.Drawing.Size(181, 22);
            this.cycleTextBox.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 255);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 17);
            this.label8.TabIndex = 13;
            this.label8.Text = "Cycle";
            // 
            // pclathTextBox
            // 
            this.pclathTextBox.Location = new System.Drawing.Point(87, 104);
            this.pclathTextBox.Name = "pclathTextBox";
            this.pclathTextBox.ReadOnly = true;
            this.pclathTextBox.Size = new System.Drawing.Size(181, 22);
            this.pclathTextBox.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(62, 17);
            this.label7.TabIndex = 11;
            this.label7.Text = "PCLATH";
            // 
            // dcBitTextBox
            // 
            this.dcBitTextBox.Location = new System.Drawing.Point(241, 174);
            this.dcBitTextBox.Name = "dcBitTextBox";
            this.dcBitTextBox.ReadOnly = true;
            this.dcBitTextBox.Size = new System.Drawing.Size(28, 22);
            this.dcBitTextBox.TabIndex = 10;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(204, 178);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(27, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "DC";
            // 
            // cBitTextBox
            // 
            this.cBitTextBox.Location = new System.Drawing.Point(162, 173);
            this.cBitTextBox.Name = "cBitTextBox";
            this.cBitTextBox.ReadOnly = true;
            this.cBitTextBox.Size = new System.Drawing.Size(36, 22);
            this.cBitTextBox.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(139, 178);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 17);
            this.label5.TabIndex = 7;
            this.label5.Text = "C";
            // 
            // zeroBitTextBox
            // 
            this.zeroBitTextBox.Location = new System.Drawing.Point(99, 173);
            this.zeroBitTextBox.Name = "zeroBitTextBox";
            this.zeroBitTextBox.ReadOnly = true;
            this.zeroBitTextBox.Size = new System.Drawing.Size(34, 22);
            this.zeroBitTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(55, 178);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "Zero";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(48, 17);
            this.label3.TabIndex = 4;
            this.label3.Text = "Status";
            // 
            // pcTextBox
            // 
            this.pcTextBox.Location = new System.Drawing.Point(88, 69);
            this.pcTextBox.Name = "pcTextBox";
            this.pcTextBox.ReadOnly = true;
            this.pcTextBox.Size = new System.Drawing.Size(181, 22);
            this.pcTextBox.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "PC";
            // 
            // wregTextBox
            // 
            this.wregTextBox.Location = new System.Drawing.Point(88, 30);
            this.wregTextBox.Name = "wregTextBox";
            this.wregTextBox.ReadOnly = true;
            this.wregTextBox.Size = new System.Drawing.Size(181, 22);
            this.wregTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "W-Reg";
            // 
            // registerMemoryListView1
            // 
            this.registerMemoryListView1.Location = new System.Drawing.Point(1177, 47);
            this.registerMemoryListView1.Name = "registerMemoryListView1";
            this.registerMemoryListView1.Size = new System.Drawing.Size(98, 891);
            this.registerMemoryListView1.TabIndex = 3;
            this.registerMemoryListView1.UseCompatibleStateImageBehavior = false;
            // 
            // registerMemoryListView2
            // 
            this.registerMemoryListView2.Location = new System.Drawing.Point(1281, 47);
            this.registerMemoryListView2.Name = "registerMemoryListView2";
            this.registerMemoryListView2.Size = new System.Drawing.Size(98, 891);
            this.registerMemoryListView2.TabIndex = 4;
            this.registerMemoryListView2.UseCompatibleStateImageBehavior = false;
            // 
            // registerMemoryListView3
            // 
            this.registerMemoryListView3.Location = new System.Drawing.Point(1385, 47);
            this.registerMemoryListView3.Name = "registerMemoryListView3";
            this.registerMemoryListView3.Size = new System.Drawing.Size(98, 891);
            this.registerMemoryListView3.TabIndex = 5;
            this.registerMemoryListView3.UseCompatibleStateImageBehavior = false;
            // 
            // registerMemoryListView4
            // 
            this.registerMemoryListView4.Location = new System.Drawing.Point(1489, 47);
            this.registerMemoryListView4.Name = "registerMemoryListView4";
            this.registerMemoryListView4.Size = new System.Drawing.Size(98, 891);
            this.registerMemoryListView4.TabIndex = 6;
            this.registerMemoryListView4.UseCompatibleStateImageBehavior = false;
            // 
            // PicSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1602, 954);
            this.Controls.Add(this.registerMemoryListView4);
            this.Controls.Add(this.registerMemoryListView3);
            this.Controls.Add(this.registerMemoryListView2);
            this.Controls.Add(this.registerMemoryListView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LstContentBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PicSimulatorForm";
            this.Text = "PicSimulator";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.CheckedListBox LstContentBox;
        private System.Windows.Forms.ToolStripMenuItem executeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ausführenToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox wregTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox pcTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox zeroBitTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox dcBitTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox cBitTextBox;
        private System.Windows.Forms.TextBox pclathTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox cycleTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ToolStripMenuItem singleStepToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ListView registerMemoryListView1;
        private System.Windows.Forms.ListView registerMemoryListView2;
        private System.Windows.Forms.ListView registerMemoryListView3;
        private System.Windows.Forms.ListView registerMemoryListView4;
    }
}

