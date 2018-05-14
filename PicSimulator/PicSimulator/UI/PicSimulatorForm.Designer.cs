﻿namespace PicSimulator.UI
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
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleStepToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frequenzToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LstContentBox = new System.Windows.Forms.CheckedListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.cycleTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.registerMemoryListView1 = new System.Windows.Forms.ListView();
            this.registerMemoryListView2 = new System.Windows.Forms.ListView();
            this.registerMemoryListView3 = new System.Windows.Forms.ListView();
            this.registerMemoryListView4 = new System.Windows.Forms.ListView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cycleDurationTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.frequencyTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.runtimeTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.controlGroupBox = new System.Windows.Forms.GroupBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.stopedTextBox = new System.Windows.Forms.TextBox();
            this.runningTextBox = new System.Windows.Forms.TextBox();
            this.singleStepButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.execButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.funcGenFreqTextBox = new System.Windows.Forms.TextBox();
            this.funcActive1 = new System.Windows.Forms.CheckBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.statusRegContentTextBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.controlGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.executeToolStripMenuItem,
            this.settingToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1924, 28);
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
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // executeToolStripMenuItem
            // 
            this.executeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ausführenToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.singleStepToolStripMenuItem,
            this.resetToolStripMenuItem});
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
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F8;
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.stopToolStripMenuItem.Text = "Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // singleStepToolStripMenuItem
            // 
            this.singleStepToolStripMenuItem.Name = "singleStepToolStripMenuItem";
            this.singleStepToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F10;
            this.singleStepToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.singleStepToolStripMenuItem.Text = "Single Step";
            this.singleStepToolStripMenuItem.Click += new System.EventHandler(this.singleStepToolStripMenuItem_Click);
            // 
            // resetToolStripMenuItem
            // 
            this.resetToolStripMenuItem.Name = "resetToolStripMenuItem";
            this.resetToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F12;
            this.resetToolStripMenuItem.Size = new System.Drawing.Size(191, 26);
            this.resetToolStripMenuItem.Text = "Reset";
            this.resetToolStripMenuItem.Click += new System.EventHandler(this.resetToolStripMenuItem_Click);
            // 
            // settingToolStripMenuItem
            // 
            this.settingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.frequenzToolStripMenuItem});
            this.settingToolStripMenuItem.Name = "settingToolStripMenuItem";
            this.settingToolStripMenuItem.Size = new System.Drawing.Size(109, 24);
            this.settingToolStripMenuItem.Text = "Einstellungen";
            // 
            // frequenzToolStripMenuItem
            // 
            this.frequenzToolStripMenuItem.Name = "frequenzToolStripMenuItem";
            this.frequenzToolStripMenuItem.Size = new System.Drawing.Size(144, 26);
            this.frequenzToolStripMenuItem.Text = "Frequenz";
            this.frequenzToolStripMenuItem.Click += new System.EventHandler(this.frequenzToolStripMenuItem_Click);
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
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(116, 26);
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
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.statusRegContentTextBox);
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
            this.groupBox1.Size = new System.Drawing.Size(284, 345);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Spezialfunktionsregister";
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
            this.label3.Location = new System.Drawing.Point(23, 144);
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
            // cycleTextBox
            // 
            this.cycleTextBox.Location = new System.Drawing.Point(98, 32);
            this.cycleTextBox.Name = "cycleTextBox";
            this.cycleTextBox.ReadOnly = true;
            this.cycleTextBox.Size = new System.Drawing.Size(135, 22);
            this.cycleTextBox.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 35);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 17);
            this.label8.TabIndex = 13;
            this.label8.Text = "Zyklen";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cycleDurationTextBox);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.frequencyTextBox);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.runtimeTextBox);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.cycleTextBox);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(875, 413);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(286, 175);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Frequenz / Laufzeit";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(241, 101);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 17);
            this.label14.TabIndex = 23;
            this.label14.Text = "Mhz";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(241, 140);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(23, 17);
            this.label13.TabIndex = 22;
            this.label13.Text = "µs";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(240, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(23, 17);
            this.label12.TabIndex = 21;
            this.label12.Text = "µs";
            // 
            // cycleDurationTextBox
            // 
            this.cycleDurationTextBox.Location = new System.Drawing.Point(101, 135);
            this.cycleDurationTextBox.Name = "cycleDurationTextBox";
            this.cycleDurationTextBox.ReadOnly = true;
            this.cycleDurationTextBox.Size = new System.Drawing.Size(132, 22);
            this.cycleDurationTextBox.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 140);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(86, 17);
            this.label11.TabIndex = 19;
            this.label11.Text = "Zyklusdauer";
            // 
            // frequencyTextBox
            // 
            this.frequencyTextBox.Location = new System.Drawing.Point(100, 98);
            this.frequencyTextBox.Name = "frequencyTextBox";
            this.frequencyTextBox.ReadOnly = true;
            this.frequencyTextBox.Size = new System.Drawing.Size(133, 22);
            this.frequencyTextBox.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 103);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(68, 17);
            this.label10.TabIndex = 17;
            this.label10.Text = "Frequenz";
            // 
            // runtimeTextBox
            // 
            this.runtimeTextBox.Location = new System.Drawing.Point(98, 68);
            this.runtimeTextBox.Name = "runtimeTextBox";
            this.runtimeTextBox.ReadOnly = true;
            this.runtimeTextBox.Size = new System.Drawing.Size(135, 22);
            this.runtimeTextBox.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(58, 17);
            this.label9.TabIndex = 15;
            this.label9.Text = "Laufzeit";
            // 
            // controlGroupBox
            // 
            this.controlGroupBox.Controls.Add(this.resetButton);
            this.controlGroupBox.Controls.Add(this.stopedTextBox);
            this.controlGroupBox.Controls.Add(this.runningTextBox);
            this.controlGroupBox.Controls.Add(this.singleStepButton);
            this.controlGroupBox.Controls.Add(this.stopButton);
            this.controlGroupBox.Controls.Add(this.execButton);
            this.controlGroupBox.Location = new System.Drawing.Point(881, 637);
            this.controlGroupBox.Name = "controlGroupBox";
            this.controlGroupBox.Size = new System.Drawing.Size(280, 163);
            this.controlGroupBox.TabIndex = 8;
            this.controlGroupBox.TabStop = false;
            this.controlGroupBox.Text = "Steuerung";
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(148, 80);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(117, 39);
            this.resetButton.TabIndex = 5;
            this.resetButton.Text = "Reset (F12)";
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            // 
            // stopedTextBox
            // 
            this.stopedTextBox.BackColor = System.Drawing.Color.Red;
            this.stopedTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stopedTextBox.Location = new System.Drawing.Point(148, 135);
            this.stopedTextBox.Name = "stopedTextBox";
            this.stopedTextBox.Size = new System.Drawing.Size(116, 22);
            this.stopedTextBox.TabIndex = 4;
            this.stopedTextBox.Text = "Gestoppt !";
            this.stopedTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // runningTextBox
            // 
            this.runningTextBox.BackColor = System.Drawing.Color.Lime;
            this.runningTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runningTextBox.Location = new System.Drawing.Point(19, 135);
            this.runningTextBox.Name = "runningTextBox";
            this.runningTextBox.ReadOnly = true;
            this.runningTextBox.Size = new System.Drawing.Size(116, 22);
            this.runningTextBox.TabIndex = 3;
            this.runningTextBox.Text = "Läuft ... ";
            this.runningTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // singleStepButton
            // 
            this.singleStepButton.Location = new System.Drawing.Point(18, 80);
            this.singleStepButton.Name = "singleStepButton";
            this.singleStepButton.Size = new System.Drawing.Size(117, 39);
            this.singleStepButton.TabIndex = 2;
            this.singleStepButton.Text = "Step (F10)";
            this.singleStepButton.UseVisualStyleBackColor = true;
            this.singleStepButton.Click += new System.EventHandler(this.singleStepButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(148, 35);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(117, 39);
            this.stopButton.TabIndex = 1;
            this.stopButton.Text = "Stop (F8)";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // execButton
            // 
            this.execButton.Location = new System.Drawing.Point(18, 35);
            this.execButton.Name = "execButton";
            this.execButton.Size = new System.Drawing.Size(117, 39);
            this.execButton.TabIndex = 0;
            this.execButton.Text = "Ausführen (F5)";
            this.execButton.UseVisualStyleBackColor = true;
            this.execButton.Click += new System.EventHandler(this.execButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Location = new System.Drawing.Point(1603, 51);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(232, 163);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Funktionsgenerator";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label17);
            this.groupBox4.Controls.Add(this.funcGenFreqTextBox);
            this.groupBox4.Controls.Add(this.funcActive1);
            this.groupBox4.Controls.Add(this.label16);
            this.groupBox4.Controls.Add(this.label15);
            this.groupBox4.Location = new System.Drawing.Point(6, 26);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(220, 131);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "#1";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(180, 28);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(34, 17);
            this.label17.TabIndex = 5;
            this.label17.Text = "Mhz";
            // 
            // funcGenFreqTextBox
            // 
            this.funcGenFreqTextBox.Location = new System.Drawing.Point(80, 28);
            this.funcGenFreqTextBox.Name = "funcGenFreqTextBox";
            this.funcGenFreqTextBox.ReadOnly = true;
            this.funcGenFreqTextBox.Size = new System.Drawing.Size(94, 22);
            this.funcGenFreqTextBox.TabIndex = 4;
            this.funcGenFreqTextBox.Click += new System.EventHandler(this.textBox1_Click);
            // 
            // funcActive1
            // 
            this.funcActive1.AutoSize = true;
            this.funcActive1.Enabled = false;
            this.funcActive1.Location = new System.Drawing.Point(6, 88);
            this.funcActive1.Name = "funcActive1";
            this.funcActive1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.funcActive1.Size = new System.Drawing.Size(60, 21);
            this.funcActive1.TabIndex = 3;
            this.funcActive1.Text = "Aktiv";
            this.funcActive1.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 58);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(28, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "Pin";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 28);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(68, 17);
            this.label15.TabIndex = 0;
            this.label15.Text = "Frequenz";
            // 
            // statusRegContentTextBox
            // 
            this.statusRegContentTextBox.Enabled = false;
            this.statusRegContentTextBox.Location = new System.Drawing.Point(87, 141);
            this.statusRegContentTextBox.Name = "statusRegContentTextBox";
            this.statusRegContentTextBox.Size = new System.Drawing.Size(180, 22);
            this.statusRegContentTextBox.TabIndex = 13;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(27, 219);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(50, 17);
            this.label18.TabIndex = 14;
            this.label18.Text = "Option";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(34, 279);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 17);
            this.label19.TabIndex = 15;
            this.label19.Text = "Intcon";
            // 
            // PicSimulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 954);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.controlGroupBox);
            this.Controls.Add(this.groupBox2);
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
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.controlGroupBox.ResumeLayout(false);
            this.controlGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ListView registerMemoryListView1;
        private System.Windows.Forms.ListView registerMemoryListView2;
        private System.Windows.Forms.ListView registerMemoryListView3;
        private System.Windows.Forms.ListView registerMemoryListView4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox runtimeTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox frequencyTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox cycleDurationTextBox;
        private System.Windows.Forms.ToolStripMenuItem settingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frequenzToolStripMenuItem;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox controlGroupBox;
        private System.Windows.Forms.TextBox stopedTextBox;
        private System.Windows.Forms.TextBox runningTextBox;
        private System.Windows.Forms.Button singleStepButton;
        private System.Windows.Forms.Button stopButton;
        private System.Windows.Forms.Button execButton;
        private System.Windows.Forms.ToolStripMenuItem resetToolStripMenuItem;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox funcGenFreqTextBox;
        private System.Windows.Forms.CheckBox funcActive1;
        private System.Windows.Forms.TextBox statusRegContentTextBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
    }
}

