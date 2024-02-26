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
    public partial class MetodiPagamento : Form
    {
        public event Action<string> MetodoPagamentoSelezionato;
        public MetodiPagamento()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string metodoPagamento = null;
            if (!this.radioButton2.Checked && !this.radioButton1.Checked && !this.radioButton3.Checked)
            {
                MessageBox.Show("Seleziona una bottone ", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (this.radioButton1.Checked)
            {
                metodoPagamento = "Contanti";
            }
            if(this.radioButton2.Checked)
            {
                metodoPagamento = "FinanzeOnline";
            }
            if(this.radioButton3.Checked)
            {
                metodoPagamento = "Carte";
            }

            MetodoPagamentoSelezionato?.Invoke(metodoPagamento);

            this.Close();
        }

        
    }
}
