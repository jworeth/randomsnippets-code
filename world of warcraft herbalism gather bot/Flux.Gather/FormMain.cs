using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.Serialization;
using System.Windows.Forms;

using Flux.Utilities;
using Flux.WoW;

using Point=Flux.WoW.Point;

namespace Flux.Gather
{
    public partial class FormMain : Form
    {
        private DateTime _startTime;
        public int Deaths;
        public int Kills;
        public int Loots;

        public FormMain()
        {
            InitializeComponent();
            Logging.OnDebug += Logging_OnWrite;
            Logging.OnWrite += Logging_OnWrite;
        }

        private void Logging_OnWrite(string message, Color col)
        {
            if (InvokeRequired)
            {
                Invoke(new Logging.WriteDelegate(Logging_OnWrite), message, col);
            }
            else
            {
                //lock (rtbLog)
                {
                    rtbLog.SelectionColor = col;
                    rtbLog.AppendText(message + Environment.NewLine);
                    rtbLog.ScrollToCaret();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            tmrUpdate.Stop();
            base.OnClosing(e);
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            TimeSpan runTime = (DateTime.Now - _startTime);
            Text = "Flux.Gather - Runtime: " + runTime.Hours + " hr " +
                   runTime.Minutes + " min " + runTime.Seconds + " sec";
            if (Bot.StateMachine.CurrentState != null)
            {
                lblCurrentBotState.Text = Bot.StateMachine.CurrentState.Name;
            }
            if (FluxWoW.Me.IsValid && FluxWoW.Me.SpellInCast.IsValid)
                label1.Text = FluxWoW.Me.SpellInCast.Name;
            //lblKills.Text = "Kills: " + Kills;
            //lblDeaths.Text = "Deaths: " + Deaths;
            //lblLoots.Text = "Loots: " + Loots;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void btnLoadWps_Click(object sender, EventArgs e)
        {
            // TODO: De-ghetto this

            //var ofd = new OpenFileDialog {Title = "Load Flux.Gather path", Filter = "XML Files (*.xml)|*.xml"};
            

            //var tmp = ofd.ShowDialog();

           // if (tmp == DialogResult.OK)
            {
                GatherBrain.CurrentPath = new FluxPath("TempFileSave.xml");
                lstWps.Items.Clear();
                foreach (Point pt in GatherBrain.CurrentPath.Locations)
                {
                    lstWps.Items.Add(pt.ToString());
                }
            }
        }

        private void btnSaveWps_Click(object sender, EventArgs e)
        {
            // TODO: De-ghetto this
            if (GatherBrain.CurrentPath == null)
            {
                MessageBox.Show("Please either load, or record a path first!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //var sfd = new SaveFileDialog {Title = "Save Flux.Gather path", Filter = "XML Files (*.xml)|*.xml"};
            // Always forget this shit...

            //if (sfd.ShowDialog(this) == DialogResult.OK)
            {
                GatherBrain.CurrentPath.Recorder.Save("TempFileSave.xml");
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            GatherBrain.IsRunning = true;
            GatherBrain.Run();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            GatherBrain.IsRunning = false;
        }

        private void btnRecorderStart_Click(object sender, EventArgs e)
        {
            if (GatherBrain.CurrentPath == null)
            {
                GatherBrain.CurrentPath = new FluxPath();
            }

            GatherBrain.CurrentPath.Recorder.IsRecording = true;
        }

        private void btnRecorderStop_Click(object sender, EventArgs e)
        {
            GatherBrain.CurrentPath.Recorder.IsRecording = false;

            lstWps.Items.Clear();
            foreach (var pt in GatherBrain.CurrentPath.Recorder.Waypoints)
            {
                lstWps.Items.Add(pt.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnRecorderClear_Click(object sender, EventArgs e)
        {
            GatherBrain.CurrentPath.Recorder.ClearPath();
            lstWps.Items.Clear();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var tmp = NodeList.Closest;
            if (!tmp.IsValid)
                return;

            Logging.WriteDebug(tmp.Name + " " + tmp.Position);

            //var lsTmp = NodeList.GetNodes();

            //foreach (var o in lsTmp)
            //{
            //    Logging.WriteDebug(o.Name + " " + o.Position);
            //}
        }
    }
}