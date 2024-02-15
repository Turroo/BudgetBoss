using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BudgetBossClient
{
    public partial class FinanzeIniziali : Form
    {
        private StreamWriter writer;
        private StreamReader reader;
        public FinanzeIniziali(StreamWriter writer, StreamReader reader)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
        }

        private void txtContanti_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Consenti solo l'inserimento di numeri, il tasto Backspace e il punto
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Consenti solo un punto
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtCarte_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Consenti solo l'inserimento di numeri, il tasto Backspace e il punto
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Consenti solo un punto
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void txtFinanzeOnline_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Consenti solo l'inserimento di numeri, il tasto Backspace e il punto
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Consenti solo un punto
            if (e.KeyChar == ',' && (sender as TextBox).Text.IndexOf(',') > -1)
            {
                e.Handled = true;
            }
        }

        private void InviaFinanzeButton_Click(object sender, EventArgs e)
        {
            string contanti = this.txtContanti.Text;
            string carte = this.txtCarte.Text;
            string finanzeOnline = this.txtFinanzeOnline.Text;

            if(string.IsNullOrEmpty(contanti) || string.IsNullOrEmpty(carte) || string.IsNullOrEmpty(finanzeOnline))
            {
                MessageBox.Show("Inserisci un valore in tutti e tre i campi", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double contantiParsed = Convert.ToDouble(contanti);
            double carteParsed = Convert.ToDouble(carte);
            double finanzeOnlineParsed = Convert.ToDouble(finanzeOnline);

            bool aggiunto = AggiungiFinanzeIniziali(contantiParsed,carteParsed,finanzeOnlineParsed);

            if(aggiunto)
            {
                MessageBox.Show("Finanze iniziali aggiunte con successo!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Login loginForm = new Login(writer, reader);
                loginForm.Show();
                this.Hide();
                return;
            }
            else
            {
                MessageBox.Show("Errore in fase di aggiunta delle finanze iniziali", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool AggiungiFinanzeIniziali(double contanti, double carte, double finanzeOnline)
        {
            try
            {
                var finanze = new {Contanti = contanti, Carte = carte, FinanzeOnline = finanzeOnline};
                string jsonData = JsonConvert.SerializeObject(finanze);

                // Invio al server del tipo di operazione ("login") e dell'oggetto JSON
                writer.WriteLine("finanzeIniziali|" + jsonData);
                writer.Flush();

                // Ricezione dal server di un booleano
                string response = reader.ReadLine();
                return bool.Parse(response);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Errore durante la registrazione: {ex.Message}");
                return false;
            }
        }
    }
}
