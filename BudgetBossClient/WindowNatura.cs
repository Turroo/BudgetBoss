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
    public partial class WindowNatura : Form
    {
        public event Action<string> NaturaSelezionata;
        public WindowNatura()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string naturaPagamento = null;
            if(!this.radioButton2.Checked && !this.radioButton1.Checked)
            {
                MessageBox.Show("Seleziona una bottone ", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(this.radioButton1.Checked)
            {
                naturaPagamento = "Entrata";
            }
            if(this.radioButton2.Checked)
            {
                naturaPagamento = "Uscita";
            }

            NaturaSelezionata?.Invoke(naturaPagamento);
            this.Close();
        }
    }
}
