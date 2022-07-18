using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Flux.Utilities;
using Flux.WoW;

namespace Flux
{
    public partial class FormKnownSpells : Form
    {
        public FormKnownSpells()
        {
            InitializeComponent();

            if (!FluxWoW.Me.IsValid)
            {
                MessageBox.Show("Get in game...");
                Close();
                return;
            }

            var spells = Flux.WoW.FluxWoW.Me.KnownSpells;
            foreach (WoWSpell s in spells)
            {
                if (!s.IsValid)
                    return;

                string name = s.Name;
                string rank = s.Rank;
                string range = s.MinRange + " - " + s.MaxRange;

                var mppct = s.ManaCostPercent;
                var mpc = s.ManaCost;

                string manaCost = mppct + "%";
                if (mppct == 0)
                {
                    manaCost = mpc != -1 ? mpc.ToString() : "0";
                }

                ListViewItem lvi = new ListViewItem(new[] {name, rank, range, manaCost, s.PowerType.ToString()});

                lvSpells.Items.Add(lvi);
            }
        }

        private void FormKnownSpells_Load(object sender, EventArgs e)
        {

        }
    }
}
