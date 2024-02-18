using Newtonsoft.Json;
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
    public partial class RimuoviTransazione : Form
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User u;

        public event Action<Transazione> TransazioneRimossa;
        public RimuoviTransazione(StreamWriter writer, StreamReader reader, User u)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
            PopolaCombo();
        }

        private void PopolaCombo()
        {
            listBox1.Items.Clear();
            listBox1.Items.AddRange(u.Transazioni.ToArray());
        }

        private Transazione GetTransazioneToDelete()
        {
            Transazione result = null;
            if(listBox1.SelectedIndex==-1)
            {
                MessageBox.Show("Seleziona una transazione da eliminare.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }
            result = (Transazione)listBox1.SelectedItem;
            listBox1.Items.Remove(result);
            return result;
        }

        private void RemoveTransazione(Transazione t)
        {
            if (t == null)
            {
                return;
            }
            try
            {

                string toSend = JsonConvert.SerializeObject(t);
                if (string.IsNullOrEmpty(toSend))
                    return;

                writer.WriteLine("rimuoviTransazione|" + toSend + "|" +u.Username);
                writer.Flush();

                string response = reader.ReadLine();
                bool rimosso = bool.Parse(response);

                if (rimosso)
                {
                    MessageBox.Show("Transazione con id: " + t.id + " rimossa con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PopolaCombo();
                    TransazioneRimossa?.Invoke(t);
                    return;
                }
                else
                {
                    MessageBox.Show("Impossibile rimuovere la transazione", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RemoveTransazione(GetTransazioneToDelete());
            this.Close();
        }
    }


}
