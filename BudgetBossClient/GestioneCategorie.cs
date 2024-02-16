using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetBossClient
{
    public partial class GestioneCategorie : UserControl
    {
        private StreamWriter writer;
        private StreamReader reader;
        private List<Categoria> categorie;
        public GestioneCategorie(StreamWriter writer, StreamReader reader, List<Categoria> categorie)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.categorie = categorie;
            ShowCategorie();
        }

        public void ShowCategorie()
        {
            listBox1.Items.Clear();

            foreach(Categoria c in categorie)
            {
                listBox1.Items.Add(c.nomeCategoria);
            }
        }
    }
}
