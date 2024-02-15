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
    public partial class Register : Form
    {
        private StreamWriter writer;
        private StreamReader reader;
        public Register(StreamWriter writer, StreamReader reader)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
        }

        private void LinkLabelLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login(writer, reader);
            loginForm.Show();
            this.Hide();
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            String username = this.txtUsername.Text;
            String password = this.txtPassword.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Inserisci username e password.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool registrato = Registra(username,password);

            if (registrato)
            {
                MessageBox.Show("Registrazione effettuata con successo!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                MessageBox.Show("Errore in fase di registrazione", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private bool Registra(String username, String password)
        {
            try
            {
                // Creazione dell'oggetto JSON contenente username e password
                var credentials = new { Username = username, Password = password };
                string jsonData = JsonConvert.SerializeObject(credentials);

                // Invio al server del tipo di operazione ("login") e dell'oggetto JSON
                writer.WriteLine("register|" + jsonData);
                writer.Flush();

                // Ricezione dal server di un booleano
                string response = reader.ReadLine();
                return bool.Parse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante la registrazione: {ex.Message}");
                return false;
            }
        }
    }
}
