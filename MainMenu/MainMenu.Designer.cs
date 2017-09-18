namespace MainMenu {
    partial class mainMenu {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainMenu));
            this.run_exe = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exco = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.loadConfig = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.configName = new System.Windows.Forms.TextBox();
            this.saveConfig = new System.Windows.Forms.Button();
            this.programs = new System.Windows.Forms.GroupBox();
            this.prog3 = new System.Windows.Forms.RadioButton();
            this.prog2 = new System.Windows.Forms.RadioButton();
            this.prog1 = new System.Windows.Forms.RadioButton();
            this.newConfig = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.sumary = new System.Windows.Forms.TextBox();
            this.simControls = new System.Windows.Forms.GroupBox();
            this.remcontrols = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.changecmd = new System.Windows.Forms.Button();
            this.cmdfolder = new System.Windows.Forms.TextBox();
            this.logSum = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.logTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.logProgressBar = new System.Windows.Forms.ProgressBar();
            this.LogAccept = new System.Windows.Forms.Button();
            this.ownmark = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.logConfig = new System.Windows.Forms.Button();
            this.posfolder = new System.Windows.Forms.TextBox();
            this.changepos = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fileMarkName = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.imageWorkProgress = new System.Windows.Forms.ProgressBar();
            this.vidoptions = new System.Windows.Forms.Button();
            this.processVideo = new System.Windows.Forms.Button();
            this.imageShower = new System.Windows.Forms.PictureBox();
            this.goToFiLoc = new System.Windows.Forms.Button();
            this.fileLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.logControls = new System.Windows.Forms.GroupBox();
            this.notes = new System.Windows.Forms.Button();
            this.userPersID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.usSet = new System.Windows.Forms.CheckBox();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.programs.SuspendLayout();
            this.simControls.SuspendLayout();
            this.logTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageShower)).BeginInit();
            this.logControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // run_exe
            // 
            this.run_exe.Location = new System.Drawing.Point(337, 159);
            this.run_exe.Name = "run_exe";
            this.run_exe.Size = new System.Drawing.Size(75, 23);
            this.run_exe.TabIndex = 7;
            this.run_exe.Text = "Accept";
            this.run_exe.UseVisualStyleBackColor = true;
            this.run_exe.Click += new System.EventHandler(this.simulator_exe_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 508);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(926, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(926, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.propertiesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.propertiesToolStripMenuItem.Text = "Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // exco
            // 
            this.exco.AutoSize = true;
            this.exco.Location = new System.Drawing.Point(98, 164);
            this.exco.Name = "exco";
            this.exco.Size = new System.Drawing.Size(52, 13);
            this.exco.TabIndex = 6;
            this.exco.Text = "ExitCode:";
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(12, 159);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Excel Test";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // loadConfig
            // 
            this.loadConfig.Location = new System.Drawing.Point(186, 45);
            this.loadConfig.Name = "loadConfig";
            this.loadConfig.Size = new System.Drawing.Size(84, 23);
            this.loadConfig.TabIndex = 6;
            this.loadConfig.Text = "Load Config.";
            this.loadConfig.UseVisualStyleBackColor = true;
            this.loadConfig.Click += new System.EventHandler(this.loadConfig_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // configName
            // 
            this.configName.BackColor = System.Drawing.SystemColors.Control;
            this.configName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.configName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configName.Location = new System.Drawing.Point(6, 19);
            this.configName.Name = "configName";
            this.configName.ReadOnly = true;
            this.configName.Size = new System.Drawing.Size(264, 20);
            this.configName.TabIndex = 9;
            this.configName.TextChanged += new System.EventHandler(this.configName_TextChanged);
            // 
            // saveConfig
            // 
            this.saveConfig.Enabled = false;
            this.saveConfig.Location = new System.Drawing.Point(96, 45);
            this.saveConfig.Name = "saveConfig";
            this.saveConfig.Size = new System.Drawing.Size(84, 23);
            this.saveConfig.TabIndex = 5;
            this.saveConfig.Text = "Save Config.";
            this.saveConfig.UseVisualStyleBackColor = true;
            this.saveConfig.Click += new System.EventHandler(this.saveConfig_Click);
            // 
            // programs
            // 
            this.programs.Controls.Add(this.prog3);
            this.programs.Controls.Add(this.prog2);
            this.programs.Controls.Add(this.prog1);
            this.programs.Location = new System.Drawing.Point(12, 30);
            this.programs.Name = "programs";
            this.programs.Size = new System.Drawing.Size(95, 95);
            this.programs.TabIndex = 11;
            this.programs.TabStop = false;
            // 
            // prog3
            // 
            this.prog3.AutoSize = true;
            this.prog3.Location = new System.Drawing.Point(6, 65);
            this.prog3.Name = "prog3";
            this.prog3.Size = new System.Drawing.Size(63, 17);
            this.prog3.TabIndex = 2;
            this.prog3.TabStop = true;
            this.prog3.Text = "Loggers";
            this.prog3.UseVisualStyleBackColor = true;
            this.prog3.CheckedChanged += new System.EventHandler(this.prog3_CheckedChanged);
            // 
            // prog2
            // 
            this.prog2.AutoSize = true;
            this.prog2.Location = new System.Drawing.Point(6, 42);
            this.prog2.Name = "prog2";
            this.prog2.Size = new System.Drawing.Size(62, 17);
            this.prog2.TabIndex = 1;
            this.prog2.TabStop = true;
            this.prog2.Text = "Remote";
            this.prog2.UseVisualStyleBackColor = true;
            this.prog2.CheckedChanged += new System.EventHandler(this.prog2_CheckedChanged);
            // 
            // prog1
            // 
            this.prog1.AutoSize = true;
            this.prog1.Location = new System.Drawing.Point(6, 19);
            this.prog1.Name = "prog1";
            this.prog1.Size = new System.Drawing.Size(68, 17);
            this.prog1.TabIndex = 0;
            this.prog1.TabStop = true;
            this.prog1.Text = "Simulator";
            this.prog1.UseVisualStyleBackColor = true;
            this.prog1.CheckedChanged += new System.EventHandler(this.prog1_CheckedChanged);
            // 
            // newConfig
            // 
            this.newConfig.Location = new System.Drawing.Point(6, 45);
            this.newConfig.Name = "newConfig";
            this.newConfig.Size = new System.Drawing.Size(84, 23);
            this.newConfig.TabIndex = 4;
            this.newConfig.Text = "New Config.";
            this.newConfig.UseVisualStyleBackColor = true;
            this.newConfig.Click += new System.EventHandler(this.newConfig_Click);
            // 
            // sumary
            // 
            this.sumary.Location = new System.Drawing.Point(6, 74);
            this.sumary.Multiline = true;
            this.sumary.Name = "sumary";
            this.sumary.ReadOnly = true;
            this.sumary.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.sumary.Size = new System.Drawing.Size(438, 370);
            this.sumary.TabIndex = 13;
            this.sumary.WordWrap = false;
            // 
            // simControls
            // 
            this.simControls.Controls.Add(this.configName);
            this.simControls.Controls.Add(this.sumary);
            this.simControls.Controls.Add(this.newConfig);
            this.simControls.Controls.Add(this.loadConfig);
            this.simControls.Controls.Add(this.saveConfig);
            this.simControls.Location = new System.Drawing.Point(450, 30);
            this.simControls.Name = "simControls";
            this.simControls.Size = new System.Drawing.Size(450, 450);
            this.simControls.TabIndex = 14;
            this.simControls.TabStop = false;
            this.simControls.Text = "Simulation";
            // 
            // remcontrols
            // 
            this.remcontrols.Location = new System.Drawing.Point(450, 30);
            this.remcontrols.Name = "remcontrols";
            this.remcontrols.Size = new System.Drawing.Size(450, 450);
            this.remcontrols.TabIndex = 15;
            this.remcontrols.TabStop = false;
            this.remcontrols.Text = "Remote";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Commands Folder:";
            // 
            // changecmd
            // 
            this.changecmd.Location = new System.Drawing.Point(340, 6);
            this.changecmd.Name = "changecmd";
            this.changecmd.Size = new System.Drawing.Size(84, 23);
            this.changecmd.TabIndex = 16;
            this.changecmd.Text = "Browse";
            this.changecmd.UseVisualStyleBackColor = true;
            this.changecmd.Click += new System.EventHandler(this.changecmd_Click);
            // 
            // cmdfolder
            // 
            this.cmdfolder.BackColor = System.Drawing.SystemColors.Control;
            this.cmdfolder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.cmdfolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdfolder.Location = new System.Drawing.Point(103, 8);
            this.cmdfolder.Name = "cmdfolder";
            this.cmdfolder.ReadOnly = true;
            this.cmdfolder.Size = new System.Drawing.Size(231, 20);
            this.cmdfolder.TabIndex = 14;
            // 
            // logSum
            // 
            this.logSum.Location = new System.Drawing.Point(6, 135);
            this.logSum.Multiline = true;
            this.logSum.Name = "logSum";
            this.logSum.ReadOnly = true;
            this.logSum.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.logSum.Size = new System.Drawing.Size(418, 258);
            this.logSum.TabIndex = 14;
            this.logSum.WordWrap = false;
            // 
            // logTabs
            // 
            this.logTabs.Controls.Add(this.tabPage1);
            this.logTabs.Controls.Add(this.tabPage2);
            this.logTabs.Location = new System.Drawing.Point(6, 19);
            this.logTabs.Name = "logTabs";
            this.logTabs.SelectedIndex = 0;
            this.logTabs.Size = new System.Drawing.Size(438, 425);
            this.logTabs.TabIndex = 18;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.logProgressBar);
            this.tabPage1.Controls.Add(this.LogAccept);
            this.tabPage1.Controls.Add(this.ownmark);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.logConfig);
            this.tabPage1.Controls.Add(this.posfolder);
            this.tabPage1.Controls.Add(this.changepos);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.fileMarkName);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.logSum);
            this.tabPage1.Controls.Add(this.cmdfolder);
            this.tabPage1.Controls.Add(this.changecmd);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(430, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Summary";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // logProgressBar
            // 
            this.logProgressBar.Location = new System.Drawing.Point(6, 106);
            this.logProgressBar.Name = "logProgressBar";
            this.logProgressBar.Size = new System.Drawing.Size(418, 23);
            this.logProgressBar.TabIndex = 31;
            // 
            // LogAccept
            // 
            this.LogAccept.Location = new System.Drawing.Point(340, 78);
            this.LogAccept.Name = "LogAccept";
            this.LogAccept.Size = new System.Drawing.Size(84, 23);
            this.LogAccept.TabIndex = 30;
            this.LogAccept.Text = "Accept";
            this.LogAccept.UseVisualStyleBackColor = true;
            this.LogAccept.Click += new System.EventHandler(this.LogAccept_Click);
            // 
            // ownmark
            // 
            this.ownmark.BackColor = System.Drawing.SystemColors.HighlightText;
            this.ownmark.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ownmark.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ownmark.Location = new System.Drawing.Point(103, 80);
            this.ownmark.Name = "ownmark";
            this.ownmark.Size = new System.Drawing.Size(231, 20);
            this.ownmark.TabIndex = 29;
            this.ownmark.Text = "Var1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "OwnMark:";
            // 
            // logConfig
            // 
            this.logConfig.Location = new System.Drawing.Point(340, 54);
            this.logConfig.Name = "logConfig";
            this.logConfig.Size = new System.Drawing.Size(84, 23);
            this.logConfig.TabIndex = 27;
            this.logConfig.Text = "Configure";
            this.logConfig.UseVisualStyleBackColor = true;
            this.logConfig.Click += new System.EventHandler(this.logConfig_Click);
            // 
            // posfolder
            // 
            this.posfolder.BackColor = System.Drawing.SystemColors.Control;
            this.posfolder.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.posfolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.posfolder.Location = new System.Drawing.Point(103, 32);
            this.posfolder.Name = "posfolder";
            this.posfolder.ReadOnly = true;
            this.posfolder.Size = new System.Drawing.Size(231, 20);
            this.posfolder.TabIndex = 25;
            // 
            // changepos
            // 
            this.changepos.Location = new System.Drawing.Point(340, 31);
            this.changepos.Name = "changepos";
            this.changepos.Size = new System.Drawing.Size(84, 23);
            this.changepos.TabIndex = 26;
            this.changepos.Text = "Browse";
            this.changepos.UseVisualStyleBackColor = true;
            this.changepos.Click += new System.EventHandler(this.changepos_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 24;
            this.label4.Text = "Positions Folder:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "FileMark:";
            // 
            // fileMarkName
            // 
            this.fileMarkName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.fileMarkName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.fileMarkName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileMarkName.Location = new System.Drawing.Point(103, 56);
            this.fileMarkName.Name = "fileMarkName";
            this.fileMarkName.Size = new System.Drawing.Size(231, 20);
            this.fileMarkName.TabIndex = 20;
            this.fileMarkName.Text = "JJDoe";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.imageWorkProgress);
            this.tabPage2.Controls.Add(this.vidoptions);
            this.tabPage2.Controls.Add(this.processVideo);
            this.tabPage2.Controls.Add(this.imageShower);
            this.tabPage2.Controls.Add(this.goToFiLoc);
            this.tabPage2.Controls.Add(this.fileLocation);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(430, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Animations";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // imageWorkProgress
            // 
            this.imageWorkProgress.Location = new System.Drawing.Point(9, 35);
            this.imageWorkProgress.Name = "imageWorkProgress";
            this.imageWorkProgress.Size = new System.Drawing.Size(251, 23);
            this.imageWorkProgress.TabIndex = 28;
            // 
            // vidoptions
            // 
            this.vidoptions.Location = new System.Drawing.Point(268, 35);
            this.vidoptions.Name = "vidoptions";
            this.vidoptions.Size = new System.Drawing.Size(75, 23);
            this.vidoptions.TabIndex = 27;
            this.vidoptions.Text = "Options";
            this.vidoptions.UseVisualStyleBackColor = true;
            this.vidoptions.Click += new System.EventHandler(this.vidoptions_Click);
            // 
            // processVideo
            // 
            this.processVideo.Location = new System.Drawing.Point(349, 35);
            this.processVideo.Name = "processVideo";
            this.processVideo.Size = new System.Drawing.Size(75, 23);
            this.processVideo.TabIndex = 26;
            this.processVideo.Text = "Accept";
            this.processVideo.UseVisualStyleBackColor = true;
            this.processVideo.Click += new System.EventHandler(this.processVideo_Click);
            // 
            // imageShower
            // 
            this.imageShower.Location = new System.Drawing.Point(9, 64);
            this.imageShower.Name = "imageShower";
            this.imageShower.Size = new System.Drawing.Size(415, 329);
            this.imageShower.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imageShower.TabIndex = 25;
            this.imageShower.TabStop = false;
            // 
            // goToFiLoc
            // 
            this.goToFiLoc.Location = new System.Drawing.Point(349, 6);
            this.goToFiLoc.Name = "goToFiLoc";
            this.goToFiLoc.Size = new System.Drawing.Size(75, 23);
            this.goToFiLoc.TabIndex = 24;
            this.goToFiLoc.Text = "Browse";
            this.goToFiLoc.UseVisualStyleBackColor = true;
            this.goToFiLoc.Click += new System.EventHandler(this.goToFiLoc_Click);
            // 
            // fileLocation
            // 
            this.fileLocation.Location = new System.Drawing.Point(85, 6);
            this.fileLocation.Name = "fileLocation";
            this.fileLocation.Size = new System.Drawing.Size(258, 20);
            this.fileLocation.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "File Location: ";
            // 
            // logControls
            // 
            this.logControls.Controls.Add(this.logTabs);
            this.logControls.Location = new System.Drawing.Point(450, 30);
            this.logControls.Name = "logControls";
            this.logControls.Size = new System.Drawing.Size(450, 450);
            this.logControls.TabIndex = 18;
            this.logControls.TabStop = false;
            this.logControls.Text = "Logger";
            // 
            // notes
            // 
            this.notes.Enabled = false;
            this.notes.Location = new System.Drawing.Point(12, 188);
            this.notes.Name = "notes";
            this.notes.Size = new System.Drawing.Size(75, 23);
            this.notes.TabIndex = 19;
            this.notes.Text = "Notes";
            this.notes.UseVisualStyleBackColor = true;
            this.notes.Click += new System.EventHandler(this.notes_Click);
            // 
            // userPersID
            // 
            this.userPersID.Location = new System.Drawing.Point(276, 53);
            this.userPersID.Name = "userPersID";
            this.userPersID.Size = new System.Drawing.Size(65, 20);
            this.userPersID.TabIndex = 20;
            this.userPersID.Text = "0000";
            this.userPersID.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(195, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 21;
            this.label6.Text = "User Identifier:";
            // 
            // usSet
            // 
            this.usSet.AutoSize = true;
            this.usSet.Location = new System.Drawing.Point(347, 55);
            this.usSet.Name = "usSet";
            this.usSet.Size = new System.Drawing.Size(65, 17);
            this.usSet.TabIndex = 22;
            this.usSet.Text = "User set";
            this.usSet.UseVisualStyleBackColor = true;
            this.usSet.CheckedChanged += new System.EventHandler(this.usSet_CheckedChanged);
            // 
            // mainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(926, 530);
            this.Controls.Add(this.logControls);
            this.Controls.Add(this.simControls);
            this.Controls.Add(this.usSet);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.userPersID);
            this.Controls.Add(this.notes);
            this.Controls.Add(this.programs);
            this.Controls.Add(this.run_exe);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.exco);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.remcontrols);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "mainMenu";
            this.Text = "Enki Graphic Simulator";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainMenu_FormClosed);
            this.Load += new System.EventHandler(this.mainMenu_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.programs.ResumeLayout(false);
            this.programs.PerformLayout();
            this.simControls.ResumeLayout(false);
            this.simControls.PerformLayout();
            this.logTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imageShower)).EndInit();
            this.logControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button run_exe;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.Label exco;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button loadConfig;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.TextBox configName;
        private System.Windows.Forms.Button saveConfig;
        private System.Windows.Forms.GroupBox programs;
        private System.Windows.Forms.RadioButton prog3;
        private System.Windows.Forms.RadioButton prog2;
        private System.Windows.Forms.RadioButton prog1;
        private System.Windows.Forms.Button newConfig;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TextBox sumary;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.GroupBox simControls;
        private System.Windows.Forms.GroupBox remcontrols;
        private System.Windows.Forms.TextBox logSum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button changecmd;
        private System.Windows.Forms.TextBox cmdfolder;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.GroupBox logControls;
        private System.Windows.Forms.TabControl logTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button notes;
        private System.Windows.Forms.TextBox fileMarkName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox posfolder;
        private System.Windows.Forms.Button changepos;
        private System.Windows.Forms.Button logConfig;
        private System.Windows.Forms.TextBox ownmark;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox userPersID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox usSet;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.PictureBox imageShower;
        private System.Windows.Forms.Button goToFiLoc;
        private System.Windows.Forms.TextBox fileLocation;
        private System.Windows.Forms.Button processVideo;
        private System.Windows.Forms.Button vidoptions;
        private System.Windows.Forms.ProgressBar imageWorkProgress;
        private System.Windows.Forms.Button LogAccept;
        private System.Windows.Forms.ProgressBar logProgressBar;
    }
}

