namespace Flux
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
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblState = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWoWTimeStamp = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblGCDInEffect = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tmrStateUpdate = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.btnStartDot = new System.Windows.Forms.Button();
            this.btnStopDot = new System.Windows.Forms.Button();
            this.btnShowSpells = new System.Windows.Forms.Button();
            this.chkAlwaysOnTop = new System.Windows.Forms.CheckBox();
            this.lstTargets = new System.Windows.Forms.ListBox();
            this.lblTargetListCount = new System.Windows.Forms.Label();
            this.statusMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbLog
            // 
            this.rtbLog.Location = new System.Drawing.Point(12, 12);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.Size = new System.Drawing.Size(432, 379);
            this.rtbLog.TabIndex = 0;
            this.rtbLog.Text = "";
            // 
            // statusMain
            // 
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.lblState,
            this.lblWoWTimeStamp,
            this.lblGCDInEffect,
            this.toolStripStatusLabel2});
            this.statusMain.Location = new System.Drawing.Point(0, 414);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(733, 22);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(36, 17);
            this.toolStripStatusLabel1.Text = "State:";
            // 
            // lblState
            // 
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(26, 17);
            this.lblState.Text = "Idle";
            // 
            // lblWoWTimeStamp
            // 
            this.lblWoWTimeStamp.Name = "lblWoWTimeStamp";
            this.lblWoWTimeStamp.Size = new System.Drawing.Size(111, 17);
            this.lblWoWTimeStamp.Text = "WoW Timestamp: 0";
            // 
            // lblGCDInEffect
            // 
            this.lblGCDInEffect.Name = "lblGCDInEffect";
            this.lblGCDInEffect.Size = new System.Drawing.Size(66, 17);
            this.lblGCDInEffect.Text = "GCD: None";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // tmrStateUpdate
            // 
            this.tmrStateUpdate.Enabled = true;
            this.tmrStateUpdate.Interval = 1000;
            this.tmrStateUpdate.Tick += new System.EventHandler(this.tmrStateUpdate_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(450, 146);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "Test Btn";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnStartDot
            // 
            this.btnStartDot.Location = new System.Drawing.Point(450, 12);
            this.btnStartDot.Name = "btnStartDot";
            this.btnStartDot.Size = new System.Drawing.Size(75, 23);
            this.btnStartDot.TabIndex = 5;
            this.btnStartDot.Text = "Start";
            this.btnStartDot.UseVisualStyleBackColor = true;
            this.btnStartDot.Click += new System.EventHandler(this.btnStartDot_Click);
            // 
            // btnStopDot
            // 
            this.btnStopDot.Location = new System.Drawing.Point(450, 42);
            this.btnStopDot.Name = "btnStopDot";
            this.btnStopDot.Size = new System.Drawing.Size(75, 23);
            this.btnStopDot.TabIndex = 6;
            this.btnStopDot.Text = "Stop";
            this.btnStopDot.UseVisualStyleBackColor = true;
            this.btnStopDot.Click += new System.EventHandler(this.btnStopDot_Click);
            // 
            // btnShowSpells
            // 
            this.btnShowSpells.Location = new System.Drawing.Point(450, 71);
            this.btnShowSpells.Name = "btnShowSpells";
            this.btnShowSpells.Size = new System.Drawing.Size(75, 23);
            this.btnShowSpells.TabIndex = 7;
            this.btnShowSpells.Text = "Show Spells";
            this.btnShowSpells.UseVisualStyleBackColor = true;
            this.btnShowSpells.Click += new System.EventHandler(this.btnShowSpells_Click);
            // 
            // chkAlwaysOnTop
            // 
            this.chkAlwaysOnTop.AutoSize = true;
            this.chkAlwaysOnTop.Location = new System.Drawing.Point(450, 100);
            this.chkAlwaysOnTop.Name = "chkAlwaysOnTop";
            this.chkAlwaysOnTop.Size = new System.Drawing.Size(67, 17);
            this.chkAlwaysOnTop.TabIndex = 8;
            this.chkAlwaysOnTop.Text = "Topmost";
            this.chkAlwaysOnTop.UseVisualStyleBackColor = true;
            this.chkAlwaysOnTop.CheckedChanged += new System.EventHandler(this.chkAlwaysOnTop_CheckedChanged);
            // 
            // lstTargets
            // 
            this.lstTargets.FormattingEnabled = true;
            this.lstTargets.Location = new System.Drawing.Point(450, 175);
            this.lstTargets.Name = "lstTargets";
            this.lstTargets.Size = new System.Drawing.Size(271, 225);
            this.lstTargets.TabIndex = 9;
            // 
            // lblTargetListCount
            // 
            this.lblTargetListCount.AutoSize = true;
            this.lblTargetListCount.Location = new System.Drawing.Point(686, 159);
            this.lblTargetListCount.Name = "lblTargetListCount";
            this.lblTargetListCount.Size = new System.Drawing.Size(13, 13);
            this.lblTargetListCount.TabIndex = 10;
            this.lblTargetListCount.Text = "0";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(733, 436);
            this.Controls.Add(this.lblTargetListCount);
            this.Controls.Add(this.lstTargets);
            this.Controls.Add(this.chkAlwaysOnTop);
            this.Controls.Add(this.btnShowSpells);
            this.Controls.Add(this.btnStopDot);
            this.Controls.Add(this.btnStartDot);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.rtbLog);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbLog;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel lblState;
        private System.Windows.Forms.Timer tmrStateUpdate;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnStartDot;
        private System.Windows.Forms.Button btnStopDot;
        private System.Windows.Forms.ToolStripStatusLabel lblWoWTimeStamp;
        private System.Windows.Forms.ToolStripStatusLabel lblGCDInEffect;
        private System.Windows.Forms.Button btnShowSpells;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.CheckBox chkAlwaysOnTop;
        private System.Windows.Forms.ListBox lstTargets;
        private System.Windows.Forms.Label lblTargetListCount;
    }
}

