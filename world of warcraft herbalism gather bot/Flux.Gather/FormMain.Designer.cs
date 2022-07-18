namespace Flux.Gather
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnLoadWps = new System.Windows.Forms.Button();
            this.btnSaveWps = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.grpRecorder = new System.Windows.Forms.GroupBox();
            this.btnRecorderStop = new System.Windows.Forms.Button();
            this.btnRecorderStart = new System.Windows.Forms.Button();
            this.grpMain = new System.Windows.Forms.GroupBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lstWps = new System.Windows.Forms.ListBox();
            this.btnRecorderClear = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblCurrentBotState = new System.Windows.Forms.ToolStripStatusLabel();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grpRecorder.SuspendLayout();
            this.grpMain.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Enabled = true;
            this.tmrUpdate.Interval = 1000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(12, 137);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(356, 203);
            this.rtbLog.TabIndex = 4;
            this.rtbLog.Text = "";
            // 
            // btnLoadWps
            // 
            this.btnLoadWps.Location = new System.Drawing.Point(6, 19);
            this.btnLoadWps.Name = "btnLoadWps";
            this.btnLoadWps.Size = new System.Drawing.Size(75, 23);
            this.btnLoadWps.TabIndex = 5;
            this.btnLoadWps.Text = "Load";
            this.btnLoadWps.UseVisualStyleBackColor = true;
            this.btnLoadWps.Click += new System.EventHandler(this.btnLoadWps_Click);
            // 
            // btnSaveWps
            // 
            this.btnSaveWps.Location = new System.Drawing.Point(6, 48);
            this.btnSaveWps.Name = "btnSaveWps";
            this.btnSaveWps.Size = new System.Drawing.Size(75, 23);
            this.btnSaveWps.TabIndex = 6;
            this.btnSaveWps.Text = "Save";
            this.btnSaveWps.UseVisualStyleBackColor = true;
            this.btnSaveWps.Click += new System.EventHandler(this.btnSaveWps_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.btnLoadWps);
            this.groupBox1.Controls.Add(this.btnSaveWps);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(87, 90);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Load/Save";
            // 
            // grpRecorder
            // 
            this.grpRecorder.AutoSize = true;
            this.grpRecorder.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpRecorder.Controls.Add(this.btnRecorderClear);
            this.grpRecorder.Controls.Add(this.btnRecorderStop);
            this.grpRecorder.Controls.Add(this.btnRecorderStart);
            this.grpRecorder.Location = new System.Drawing.Point(105, 12);
            this.grpRecorder.Name = "grpRecorder";
            this.grpRecorder.Size = new System.Drawing.Size(87, 119);
            this.grpRecorder.TabIndex = 8;
            this.grpRecorder.TabStop = false;
            this.grpRecorder.Text = "Recorder";
            // 
            // btnRecorderStop
            // 
            this.btnRecorderStop.Location = new System.Drawing.Point(6, 48);
            this.btnRecorderStop.Name = "btnRecorderStop";
            this.btnRecorderStop.Size = new System.Drawing.Size(75, 23);
            this.btnRecorderStop.TabIndex = 1;
            this.btnRecorderStop.Text = "Stop";
            this.btnRecorderStop.UseVisualStyleBackColor = true;
            this.btnRecorderStop.Click += new System.EventHandler(this.btnRecorderStop_Click);
            // 
            // btnRecorderStart
            // 
            this.btnRecorderStart.Location = new System.Drawing.Point(6, 19);
            this.btnRecorderStart.Name = "btnRecorderStart";
            this.btnRecorderStart.Size = new System.Drawing.Size(75, 23);
            this.btnRecorderStart.TabIndex = 0;
            this.btnRecorderStart.Text = "Start";
            this.btnRecorderStart.UseVisualStyleBackColor = true;
            this.btnRecorderStart.Click += new System.EventHandler(this.btnRecorderStart_Click);
            // 
            // grpMain
            // 
            this.grpMain.AutoSize = true;
            this.grpMain.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.grpMain.Controls.Add(this.btnStop);
            this.grpMain.Controls.Add(this.btnStart);
            this.grpMain.Location = new System.Drawing.Point(198, 12);
            this.grpMain.Name = "grpMain";
            this.grpMain.Size = new System.Drawing.Size(87, 90);
            this.grpMain.TabIndex = 9;
            this.grpMain.TabStop = false;
            this.grpMain.Text = "Main";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(6, 48);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(6, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(291, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(258, 90);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Stats";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "FILL IN LATER";
            // 
            // lstWps
            // 
            this.lstWps.FormattingEnabled = true;
            this.lstWps.Location = new System.Drawing.Point(374, 128);
            this.lstWps.Name = "lstWps";
            this.lstWps.Size = new System.Drawing.Size(175, 212);
            this.lstWps.TabIndex = 11;
            // 
            // btnRecorderClear
            // 
            this.btnRecorderClear.Location = new System.Drawing.Point(6, 77);
            this.btnRecorderClear.Name = "btnRecorderClear";
            this.btnRecorderClear.Size = new System.Drawing.Size(75, 23);
            this.btnRecorderClear.TabIndex = 2;
            this.btnRecorderClear.Text = "Clear";
            this.btnRecorderClear.UseVisualStyleBackColor = true;
            this.btnRecorderClear.Click += new System.EventHandler(this.btnRecorderClear_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblCurrentBotState});
            this.statusStrip1.Location = new System.Drawing.Point(0, 349);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(561, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "State:";
            // 
            // lblCurrentBotState
            // 
            this.lblCurrentBotState.Name = "lblCurrentBotState";
            this.lblCurrentBotState.Size = new System.Drawing.Size(26, 17);
            this.lblCurrentBotState.Text = "Idle";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 371);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lstWps);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.grpMain);
            this.Controls.Add(this.grpRecorder);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.rtbLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "Flux.Gather - Runtime: 0 hr 0 min 0 sec";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox1.ResumeLayout(false);
            this.grpRecorder.ResumeLayout(false);
            this.grpMain.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        //private System.Windows.Forms.TabControl tabControlMain;
        //private System.Windows.Forms.TabPage tabPageMain;
        //private System.Windows.Forms.TabPage tabPageConfig;
        //private System.Windows.Forms.GroupBox groupBox1;
        //private System.Windows.Forms.Label lblDeaths;
        //private System.Windows.Forms.Label lblLoots;
        //private System.Windows.Forms.Label lblKills;
        //private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.Button btnLoadWps;
        private System.Windows.Forms.Button btnSaveWps;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox grpRecorder;
        private System.Windows.Forms.Button btnRecorderStop;
        private System.Windows.Forms.Button btnRecorderStart;
        private System.Windows.Forms.GroupBox grpMain;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstWps;
        private System.Windows.Forms.Button btnRecorderClear;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblCurrentBotState;
        private System.Windows.Forms.Button button1;
    }
}

