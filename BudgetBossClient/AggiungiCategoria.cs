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
    public partial class AggiungiCategoria : Form
    {
        public event Action<string> CategoriaAggiunta;

        private StreamWriter writer;
        private StreamReader reader;
        private List<Categoria> categorie;
        public AggiungiCategoria(StreamWriter writer, StreamReader reader, List<Categoria> categorie)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.categorie = categorie;
        }

        private void AddCategoria()
        {
            try
            {
                string toSend = this.textBox1.Text;
                if (string.IsNullOrEmpty(toSend))
                    return;
                writer.WriteLine("aggiungiCategoria|" + toSend);
                writer.Flush();

                string response = reader.ReadLine();
                switch(response)
                {
                    case "OK":
                        {
                            MessageBox.Show("Categoria " + toSend + " aggiunta con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            CategoriaAggiunta?.Invoke(toSend);
                            return;
                        }
                    case "already":
                        {
                            MessageBox.Show("La categoria " + toSend + " è già presente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    case "maximum":
                        {
                            MessageBox.Show("Numero massimo di categorie raggiunto", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    default:
                        return;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
    }

        private void button2_Click(object sender, EventArgs e)
        {
            AddCategoria();
            this.Close();
        }
    }

    
}
