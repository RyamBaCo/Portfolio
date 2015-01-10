namespace Editor
{
    partial class Editor
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonOpenLetterGraphics = new System.Windows.Forms.Button();
            this.textBoxLetterGraphicsPath = new System.Windows.Forms.TextBox();
            this.panelGraphics = new System.Windows.Forms.Panel();
            this.pictureBoxDot = new System.Windows.Forms.PictureBox();
            this.pictureBoxLetterGraphics = new System.Windows.Forms.PictureBox();
            this.pictureBoxFrontMaskGraphics = new System.Windows.Forms.PictureBox();
            this.pictureBoxBackGraphics = new System.Windows.Forms.PictureBox();
            this.pictureBoxFrontGraphics = new System.Windows.Forms.PictureBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.folderBrowserDialogLevels = new System.Windows.Forms.FolderBrowserDialog();
            this.progressBarSave = new System.Windows.Forms.ProgressBar();
            this.backgroundWorkerWriteXML = new System.ComponentModel.BackgroundWorker();
            this.radioButtonShowBack = new System.Windows.Forms.RadioButton();
            this.radioButtonShowLetters = new System.Windows.Forms.RadioButton();
            this.radioButtonShowFront = new System.Windows.Forms.RadioButton();
            this.radioButtonShowFrontMask = new System.Windows.Forms.RadioButton();
            this.numericUpDownAccuracy = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.trackBarLettersNeeded = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLettersNeeded = new System.Windows.Forms.TextBox();
            this.textBoxTime = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.panelGraphics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLetterGraphics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrontMaskGraphics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackGraphics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrontGraphics)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAccuracy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLettersNeeded)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOpenLetterGraphics
            // 
            this.buttonOpenLetterGraphics.Location = new System.Drawing.Point(907, 9);
            this.buttonOpenLetterGraphics.Name = "buttonOpenLetterGraphics";
            this.buttonOpenLetterGraphics.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenLetterGraphics.TabIndex = 0;
            this.buttonOpenLetterGraphics.Text = "open";
            this.buttonOpenLetterGraphics.UseVisualStyleBackColor = true;
            this.buttonOpenLetterGraphics.Click += new System.EventHandler(this.buttonOpenLetterGraphics_Click);
            // 
            // textBoxLetterGraphicsPath
            // 
            this.textBoxLetterGraphicsPath.Location = new System.Drawing.Point(741, 11);
            this.textBoxLetterGraphicsPath.Name = "textBoxLetterGraphicsPath";
            this.textBoxLetterGraphicsPath.ReadOnly = true;
            this.textBoxLetterGraphicsPath.Size = new System.Drawing.Size(160, 20);
            this.textBoxLetterGraphicsPath.TabIndex = 1;
            // 
            // panelGraphics
            // 
            this.panelGraphics.AutoScroll = true;
            this.panelGraphics.Controls.Add(this.pictureBoxDot);
            this.panelGraphics.Controls.Add(this.pictureBoxLetterGraphics);
            this.panelGraphics.Controls.Add(this.pictureBoxFrontMaskGraphics);
            this.panelGraphics.Controls.Add(this.pictureBoxBackGraphics);
            this.panelGraphics.Controls.Add(this.pictureBoxFrontGraphics);
            this.panelGraphics.Location = new System.Drawing.Point(11, 12);
            this.panelGraphics.Name = "panelGraphics";
            this.panelGraphics.Size = new System.Drawing.Size(724, 448);
            this.panelGraphics.TabIndex = 2;
            // 
            // pictureBoxDot
            // 
            this.pictureBoxDot.Location = new System.Drawing.Point(296, 12);
            this.pictureBoxDot.Name = "pictureBoxDot";
            this.pictureBoxDot.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxDot.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxDot.TabIndex = 4;
            this.pictureBoxDot.TabStop = false;
            // 
            // pictureBoxLetterGraphics
            // 
            this.pictureBoxLetterGraphics.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxLetterGraphics.Name = "pictureBoxLetterGraphics";
            this.pictureBoxLetterGraphics.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxLetterGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxLetterGraphics.TabIndex = 0;
            this.pictureBoxLetterGraphics.TabStop = false;
            this.pictureBoxLetterGraphics.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxLetterGraphics_MouseClick);
            // 
            // pictureBoxFrontMaskGraphics
            // 
            this.pictureBoxFrontMaskGraphics.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxFrontMaskGraphics.Name = "pictureBoxFrontMaskGraphics";
            this.pictureBoxFrontMaskGraphics.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxFrontMaskGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxFrontMaskGraphics.TabIndex = 2;
            this.pictureBoxFrontMaskGraphics.TabStop = false;
            // 
            // pictureBoxBackGraphics
            // 
            this.pictureBoxBackGraphics.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxBackGraphics.Name = "pictureBoxBackGraphics";
            this.pictureBoxBackGraphics.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxBackGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxBackGraphics.TabIndex = 1;
            this.pictureBoxBackGraphics.TabStop = false;
            // 
            // pictureBoxFrontGraphics
            // 
            this.pictureBoxFrontGraphics.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxFrontGraphics.Name = "pictureBoxFrontGraphics";
            this.pictureBoxFrontGraphics.Size = new System.Drawing.Size(100, 50);
            this.pictureBoxFrontGraphics.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBoxFrontGraphics.TabIndex = 3;
            this.pictureBoxFrontGraphics.TabStop = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(908, 228);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(101, 27);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.Text = "save to XML";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // folderBrowserDialogLevels
            // 
            this.folderBrowserDialogLevels.SelectedPath = "C:\\dev\\xampp\\htdocs\\portfolio";
            // 
            // progressBarSave
            // 
            this.progressBarSave.Location = new System.Drawing.Point(741, 261);
            this.progressBarSave.Name = "progressBarSave";
            this.progressBarSave.Size = new System.Drawing.Size(281, 23);
            this.progressBarSave.TabIndex = 4;
            // 
            // backgroundWorkerWriteXML
            // 
            this.backgroundWorkerWriteXML.WorkerReportsProgress = true;
            this.backgroundWorkerWriteXML.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerWriteXML_DoWork);
            this.backgroundWorkerWriteXML.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerWriteXML_ProgressChanged);
            this.backgroundWorkerWriteXML.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerWriteXML_RunWorkerCompleted);
            // 
            // radioButtonShowBack
            // 
            this.radioButtonShowBack.AutoSize = true;
            this.radioButtonShowBack.Location = new System.Drawing.Point(765, 48);
            this.radioButtonShowBack.Name = "radioButtonShowBack";
            this.radioButtonShowBack.Size = new System.Drawing.Size(261, 17);
            this.radioButtonShowBack.TabIndex = 5;
            this.radioButtonShowBack.Text = "back (save: dotposition, percentage to clear, time)";
            this.radioButtonShowBack.UseVisualStyleBackColor = true;
            this.radioButtonShowBack.CheckedChanged += new System.EventHandler(this.radioButtonShowBack_CheckedChanged);
            // 
            // radioButtonShowLetters
            // 
            this.radioButtonShowLetters.AutoSize = true;
            this.radioButtonShowLetters.Checked = true;
            this.radioButtonShowLetters.Location = new System.Drawing.Point(765, 71);
            this.radioButtonShowLetters.Name = "radioButtonShowLetters";
            this.radioButtonShowLetters.Size = new System.Drawing.Size(155, 17);
            this.radioButtonShowLetters.TabIndex = 6;
            this.radioButtonShowLetters.TabStop = true;
            this.radioButtonShowLetters.Text = "letters (save: letterpositions)";
            this.radioButtonShowLetters.UseVisualStyleBackColor = true;
            this.radioButtonShowLetters.CheckedChanged += new System.EventHandler(this.radioButtonShowLetters_CheckedChanged);
            // 
            // radioButtonShowFront
            // 
            this.radioButtonShowFront.AutoSize = true;
            this.radioButtonShowFront.Location = new System.Drawing.Point(765, 117);
            this.radioButtonShowFront.Name = "radioButtonShowFront";
            this.radioButtonShowFront.Size = new System.Drawing.Size(46, 17);
            this.radioButtonShowFront.TabIndex = 7;
            this.radioButtonShowFront.Text = "front";
            this.radioButtonShowFront.UseVisualStyleBackColor = true;
            this.radioButtonShowFront.CheckedChanged += new System.EventHandler(this.radioButtonShowFront_CheckedChanged);
            // 
            // radioButtonShowFrontMask
            // 
            this.radioButtonShowFrontMask.AutoSize = true;
            this.radioButtonShowFrontMask.Location = new System.Drawing.Point(765, 94);
            this.radioButtonShowFrontMask.Name = "radioButtonShowFrontMask";
            this.radioButtonShowFrontMask.Size = new System.Drawing.Size(74, 17);
            this.radioButtonShowFrontMask.TabIndex = 8;
            this.radioButtonShowFrontMask.Text = "front mask";
            this.radioButtonShowFrontMask.UseVisualStyleBackColor = true;
            this.radioButtonShowFrontMask.CheckedChanged += new System.EventHandler(this.radioButtonShowFrontMask_CheckedChanged);
            // 
            // numericUpDownAccuracy
            // 
            this.numericUpDownAccuracy.Location = new System.Drawing.Point(988, 117);
            this.numericUpDownAccuracy.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownAccuracy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDownAccuracy.Name = "numericUpDownAccuracy";
            this.numericUpDownAccuracy.Size = new System.Drawing.Size(36, 20);
            this.numericUpDownAccuracy.TabIndex = 9;
            this.numericUpDownAccuracy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(890, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "stepping precision";
            // 
            // trackBarLettersNeeded
            // 
            this.trackBarLettersNeeded.Location = new System.Drawing.Point(755, 177);
            this.trackBarLettersNeeded.Maximum = 100;
            this.trackBarLettersNeeded.Name = "trackBarLettersNeeded";
            this.trackBarLettersNeeded.Size = new System.Drawing.Size(254, 45);
            this.trackBarLettersNeeded.TabIndex = 11;
            this.trackBarLettersNeeded.ValueChanged += new System.EventHandler(this.trackBarLettersNeeded_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(752, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "letters needed %";
            // 
            // textBoxLettersNeeded
            // 
            this.textBoxLettersNeeded.Location = new System.Drawing.Point(843, 158);
            this.textBoxLettersNeeded.Name = "textBoxLettersNeeded";
            this.textBoxLettersNeeded.ReadOnly = true;
            this.textBoxLettersNeeded.Size = new System.Drawing.Size(56, 20);
            this.textBoxLettersNeeded.TabIndex = 13;
            this.textBoxLettersNeeded.Text = "0";
            // 
            // textBoxTime
            // 
            this.textBoxTime.Location = new System.Drawing.Point(843, 232);
            this.textBoxTime.Name = "textBoxTime";
            this.textBoxTime.Size = new System.Drawing.Size(56, 20);
            this.textBoxTime.TabIndex = 14;
            this.textBoxTime.Text = "60";
            this.textBoxTime.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxTime_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(757, 235);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "time in seconds";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 470);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTime);
            this.Controls.Add(this.textBoxLettersNeeded);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBarLettersNeeded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDownAccuracy);
            this.Controls.Add(this.radioButtonShowFrontMask);
            this.Controls.Add(this.radioButtonShowFront);
            this.Controls.Add(this.radioButtonShowLetters);
            this.Controls.Add(this.radioButtonShowBack);
            this.Controls.Add(this.progressBarSave);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.panelGraphics);
            this.Controls.Add(this.textBoxLetterGraphicsPath);
            this.Controls.Add(this.buttonOpenLetterGraphics);
            this.Name = "Editor";
            this.Text = "Paper: Editor - written and composed by Christian Mayr";
            this.panelGraphics.ResumeLayout(false);
            this.panelGraphics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLetterGraphics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrontMaskGraphics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBackGraphics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrontGraphics)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownAccuracy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLettersNeeded)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenLetterGraphics;
        private System.Windows.Forms.TextBox textBoxLetterGraphicsPath;
        private System.Windows.Forms.Panel panelGraphics;
        private System.Windows.Forms.PictureBox pictureBoxLetterGraphics;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialogLevels;
        private System.Windows.Forms.ProgressBar progressBarSave;
        private System.ComponentModel.BackgroundWorker backgroundWorkerWriteXML;
        private System.Windows.Forms.PictureBox pictureBoxFrontGraphics;
        private System.Windows.Forms.PictureBox pictureBoxFrontMaskGraphics;
        private System.Windows.Forms.PictureBox pictureBoxBackGraphics;
        private System.Windows.Forms.RadioButton radioButtonShowBack;
        private System.Windows.Forms.RadioButton radioButtonShowLetters;
        private System.Windows.Forms.RadioButton radioButtonShowFront;
        private System.Windows.Forms.RadioButton radioButtonShowFrontMask;
        private System.Windows.Forms.NumericUpDown numericUpDownAccuracy;
        private System.Windows.Forms.PictureBox pictureBoxDot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TrackBar trackBarLettersNeeded;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxLettersNeeded;
        private System.Windows.Forms.TextBox textBoxTime;
        private System.Windows.Forms.Label label3;
    }
}

