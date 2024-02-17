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
    public partial class WindowCategorie : Form
    {
        private List<Categoria> categorie;
        public event Action<string> CategoriaSelezionata;
        public WindowCategorie(List<Categoria> categorie)
        {
            InitializeComponent();
            this.categorie = categorie;
            ShowCategorie();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string categoria = this.listBox1.Text;
            if(string.IsNullOrEmpty(categoria))
            {
                MessageBox.Show("Seleziona una categoria", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CategoriaSelezionata?.Invoke(categoria);
            this.Close();
        }

        private void ShowCategorie()
        {
            listBox1.Items.Clear();


            foreach (Categoria c in categorie)
            {
                listBox1.Items.Add(c.nomeCategoria);
            }
        }
    }
}
