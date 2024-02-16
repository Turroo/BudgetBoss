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
    public partial class Login : Form
        
    {
        private StreamWriter writer;
        private StreamReader reader;
        public Login(StreamWriter writer, StreamReader reader)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;  

        }

        private void LinkLabelRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register registerForm = new Register(writer,reader);
            registerForm.Show();
            this.Hide();
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            String username = this.txtUsername.Text;
            String password = this.txtPassword.Text;

            

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Inserisci username e password.", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool autenticato = Autentica(username, password);
            

            

            if(autenticato)
            {
                MessageBox.Show("Login effettuato con successo!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                User currentUser = getUser();
                List<Categoria> categorie = getCategorie();
                if (currentUser!=null && categorie!=null)
                {
                    HomeDashboard homeView = new HomeDashboard(writer, reader, currentUser, categorie);
                    homeView.Show();
                    this.Hide();
                }
                return;
            }
            else
            {
                MessageBox.Show("Username o password errate", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private User getUser()
        {
            try
            {
                writer.WriteLine("getCurrentUser|");
                writer.Flush();
                string response = reader.ReadLine();
                User currentUser = JsonConvert.DeserializeObject<User>(response);
                MessageBox.Show("Oggetto utente ottenuto con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return currentUser;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Impossibile ottenere l'oggetto utente corrente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }  

        private List<Categoria> getCategorie()
        {
            try
            {
                writer.WriteLine("getCategorie|");
                writer.Flush();
                string response = reader.ReadLine();
                List<Categoria> categorie = JsonConvert.DeserializeObject<List<Categoria>>(response);
                MessageBox.Show("Lista categorie ottenuta con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return categorie;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Impossibile ottenere la lista categorie corrente", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private bool Autentica(String username, String password)
        {
            try
            {
                // Creazione dell'oggetto JSON contenente username e password
                var credentials = new { Username = username, Password = password };
                string jsonData = JsonConvert.SerializeObject(credentials);

                // Invio al server del tipo di operazione ("login") e dell'oggetto JSON
                writer.WriteLine("login|" + jsonData);
                writer.Flush();

                // Ricezione dal server di un booleano
                string response = reader.ReadLine();
                return bool.Parse(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore durante l'autenticazione: {ex.Message}");
                return false;
            }
        }
    }
}
