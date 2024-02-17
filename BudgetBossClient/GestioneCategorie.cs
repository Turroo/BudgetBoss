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

        private void button1_Click(object sender, EventArgs e)
        {
            AggiungiCategoria aggiungiCategoria = new AggiungiCategoria(writer, reader, categorie);

            aggiungiCategoria.CategoriaAggiunta += NuovaCategoriaAggiunta;
            aggiungiCategoria.Show();
        }

        private void NuovaCategoriaAggiunta(string nuovaCategoria)
        {
            categorie.Add(new Categoria(nuovaCategoria));
            ShowCategorie();

        }

        private void RimuoviCategoria()
        {
            try
            {
                string toRemove = listBox1.Text;
                if(string.IsNullOrEmpty(toRemove))
                {
                    MessageBox.Show("Seleziona una categoria dalla lista", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                writer.WriteLine("rimuoviCategoria|" + toRemove);
                writer.Flush();

                string response = reader.ReadLine();
                bool rimosso = bool.Parse(response);
                if(rimosso) 
                {
                    MessageBox.Show("Categoria " + toRemove + " rimossa con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Categoria daRimuovere = categorie.FirstOrDefault(c => c.nomeCategoria == toRemove);
                    categorie.Remove(daRimuovere);
                    return;
                }
                else
                {
                    MessageBox.Show("Impossibile rimuovere la categoria", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            RimuoviCategoria();
            ShowCategorie();
        }
    }
}
