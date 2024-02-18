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
    public partial class AggiungiTransazione : Form
    {
        public event Action<Transazione> TransazioneAggiunta;

        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Categoria> categorie;
        public AggiungiTransazione(StreamWriter writer, StreamReader reader, User u,List<Categoria> categorie)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
            this.categorie = categorie;
            PopulateCombo();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddTransazione();
            this.Close();
        }

        private Transazione createTransazione()
        {
            Transazione result = null;
            if (string.IsNullOrEmpty(this.textBox1.Text) || string.IsNullOrEmpty(this.textBox2.Text)
                || this.comboBox1.SelectedValue == null || this.comboBox2.SelectedValue == null
                || this.comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Completa tutti i campi prima di inviare la transazione", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            result = new Transazione(int.Parse(this.textBox1.Text),double.Parse(this.textBox2.Text),(MetodoDiPagamento)comboBox1.SelectedItem,
                this.dateTimePicker1.Value,(Categoria)comboBox2.SelectedItem,(NaturaTransazione)comboBox3.SelectedItem);

            return result;

        }

        private bool checkBalance(Transazione t)
        {
            if(checkId(t))
            {
                MessageBox.Show("Esiste già una transazione con il medesimo ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            
            if(t.naturaTransazione.Equals(NaturaTransazione.Uscita))
            {
                if(t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
                {
                    if(t.importo > u.Carte)
                    {
                        MessageBox.Show("Non hai abbastanza credito sulla carta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
                {
                    if (t.importo > u.Contanti)
                    {
                        MessageBox.Show("Non hai abbastanza credito in contanti", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }

                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
                {
                    if (t.importo > u.FinanzeOnline)
                    {
                        MessageBox.Show("Non hai abbastanza credito in finanze online", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    return true;
                }
            }

            return true;
        }

        private void AddTransazione()
        {
            Transazione temp = createTransazione();
            if (temp == null)
            {
                return;
            }
            try
            {
                if (!checkBalance(temp))
                    return;
                string toSend = JsonConvert.SerializeObject(temp);
                if (string.IsNullOrEmpty(toSend))
                    return;

                writer.WriteLine("aggiungiTransazione|" + toSend + "|" + u.Username);
                writer.Flush();

                string response = reader.ReadLine();
                bool aggiunto = bool.Parse(response);

                if (aggiunto)
                {
                    MessageBox.Show("Transazione con id: " + temp.id + " aggiunta con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TransazioneAggiunta?.Invoke(temp);
                    return;
                }
                else
                {
                    MessageBox.Show("Impossibile completare la transazione", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private bool checkId(Transazione t)
        {
            foreach(Transazione tr in u.Transazioni)
            {
                if (tr.id == t.id) return true;
            }

            return false;
        }

        private void PopulateCombo()
        {
            // Popola la ComboBox per il MetodoDiPagamento
            comboBox1.DataSource = Enum.GetValues(typeof(MetodoDiPagamento));

            // Popola la ComboBox per la NaturaTransazione
            comboBox3.DataSource = Enum.GetValues(typeof(NaturaTransazione));

            // Popola la ComboBox per le categorie
            comboBox2.DataSource = categorie;
            comboBox2.DisplayMember = "nomeCategoria";
        }
    }
}
