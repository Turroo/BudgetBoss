using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetBossClient
{
    public partial class VisualizzaDettagliGruppo : Form
    {
        private Gruppo g;
        public VisualizzaDettagliGruppo(Gruppo g)
        {
            InitializeComponent();
            this.g = g;
            UpdateValues();
        }

        private void UpdateValues()
        {
            listBox1.Items.Clear();
            this.label4.Text = g.nomeGruppo;
            this.label5.Text = g.admin.Username;
            listBox1.DisplayMember = "Username";

            foreach (var utente in g.utenti)
            {
                listBox1.Items.Add(utente);
            }


        }
    }
}
