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
    public partial class UniscitiAGruppo : Form
    {
        public event Action<Gruppo> GruppoUnito;

        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Gruppo> groupsList;
        public UniscitiAGruppo(StreamWriter writer, StreamReader reader, User u, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.groupsList = groupsList;
            this.u = u;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JoinGroup();
            this.Close();
        }

        private void JoinGroup()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Inserisci il nome del gruppo a cui unirsi", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string toJoin = textBox1.Text;
            Gruppo temp = groupsList.Find(g => g.nomeGruppo == toJoin);
            if (temp == null)
            {
                MessageBox.Show("Il gruppo inserito non esiste", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            User tempUser = temp.utenti.Find(us => us.Username == u.Username);
            if (tempUser != null)
            {
                MessageBox.Show("L'utente è già membro di questo gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            writer.WriteLine("uniscitiAGruppo|" + toJoin);
            writer.Flush();

            string response = reader.ReadLine();
            bool entrato = bool.Parse(response);

            if (entrato)
            {
                temp.utenti.Add(u);
                MessageBox.Show("Utente unito con successo al gruppo " + toJoin, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GruppoUnito?.Invoke(temp);
                return;

            }
            else
            {
                MessageBox.Show("Impossibile unirsi al gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
