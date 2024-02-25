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
    public partial class CreaGruppo : Form
    {
        public event Action<Gruppo> GruppoAggiunto;
        
        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Gruppo> groupsList;
        public CreaGruppo(StreamWriter writer, StreamReader reader, User u, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
            this.groupsList = groupsList;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateGroup();
            this.Close();
        }

        private void CreateGroup()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                MessageBox.Show("Inserisci il nome del gruppo da creare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string toCreate = textBox1.Text;

            if (groupsList.Find(g => g.nomeGruppo == toCreate) != null)
            {
                MessageBox.Show("Esiste già un gruppo con questo nome", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            writer.WriteLine("creaGruppo|" + toCreate);
            writer.Flush();

            string response = reader.ReadLine();
            bool creato = bool.Parse(response);

            if (creato)
            {
                Gruppo g = new Gruppo(toCreate, u);
                MessageBox.Show("Creato con successo il gruppo " + toCreate, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GruppoAggiunto?.Invoke(g);
                return;

            }
            else
            {
                MessageBox.Show("Impossibile creare il gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
