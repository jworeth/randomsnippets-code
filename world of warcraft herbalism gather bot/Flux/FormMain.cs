using System;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Drawing;
using System.Windows.Forms;

using Flux.Bot;
using Flux.Utilities;
using Flux.WoW;
using Flux.WoW.Objects;
using Flux.WoW.Patchables;

namespace Flux
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            Logging.OnDebug += Logging_OnWrite;
            Logging.OnWrite += Logging_OnWrite;

            Logging.Write(Color.Blue, "Flux starting up... please wait...");
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

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private void tmrStateUpdate_Tick(object sender, EventArgs e)
        {
            lblState.Text = StateMachine.CurrentState != null ? StateMachine.CurrentState.Name : "Idle";
            lblWoWTimeStamp.Text = "WoW TimeStamp: " + FluxWoW.TimeStamp;
            if (FluxWoW.GlobalCooldown)
            {
                lblGCDInEffect.Text = "GCD: In Effect";
            }
            else
            {
                lblGCDInEffect.Text = "GCD: None"; 
            }
            if (FluxWoW.Me.IsValid)
                toolStripStatusLabel2.Text = FluxWoW.Me.Class.ToString();
            if (TargetManager.GuiList.Count > 0)
            {
                lstTargets.Items.Clear();
                foreach (string s in TargetManager.GuiList.ToArray())
                {
                    lstTargets.Items.Add(s);
                }

                lblTargetListCount.Text = TargetManager.TargetList.Count.ToString();
            }
            else
            {
                lstTargets.Items.Clear();
                lblTargetListCount.Text = "0";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Logging.WriteDebug(FluxWoW.TimeStamp.ToString());
            //Logging.WriteDebug("Known Spells: ");
            //foreach (WoWSpell spell in FluxWoW.Me.KnownSpells)
            //{
            //    Logging.WriteDebug("Spell: {0} - ID: {1}", spell.Name, spell.ID);
            //}
            //Logging.WriteDebug("Current Buffs: ");
            //for (int i = 0; i <= FluxWoW.Me.BuffCount; i++)
            //{
            //    WoWBuff buff = FluxWoW.Me.GetBuff(i);
            //    Logging.WriteDebug("Buff: {0} - ID: {1}", buff.Name, buff.SpellId);
            //}

            var imp = new WoWSpell("Summon Imp");
            Logging.WriteDebug(imp.IsValid.ToString());
            Logging.WriteDebug(Lua.GetReturnValues("IsUsableSpell(\"Summon Imp\")").ToRealString());
        }

        private void btnStartDot_Click(object sender, EventArgs e)
        {
            StateMachine.Paused = false;
            Logging.Write("FSM Started");
        }

        private void btnStopDot_Click(object sender, EventArgs e)
        {
            StateMachine.Paused = true;
            Logging.Write("FSM Stopped");
        }

        private void btnShowSpells_Click(object sender, EventArgs e)
        {
            FormKnownSpells fks = new FormKnownSpells();
            fks.ShowDialog();
        }

        private void chkAlwaysOnTop_CheckedChanged(object sender, EventArgs e)
        {
            TopMost = chkAlwaysOnTop.Checked;
        }
    }
}