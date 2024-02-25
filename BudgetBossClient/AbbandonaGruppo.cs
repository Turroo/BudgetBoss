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
    public partial class AbbandonaGruppo : Form
    {
        public event Action<Gruppo> GruppoAbbandonato;

        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Gruppo> groupsList;
        public AbbandonaGruppo(StreamWriter writer, StreamReader reader, User u, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.groupsList = groupsList;
            this.u = u;
            UpdateValues();
        }

        private void UpdateValues()
        {
            this.comboBox1.Items.Clear();
            this.comboBox1.DataSource = u.gruppiAppartenenza;
        }

        private void LeaveGroup()
        {
            if(comboBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Seleziona un gruppo da abbandonare", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string toLeave = comboBox1.SelectedItem.ToString();
            Gruppo temp = groupsList.Find(gr => gr.nomeGruppo == toLeave);
            if(temp.admin.Username == u.Username)
            {
                MessageBox.Show("Non puoi abbandonare il gruppo se sei l'admin, visita Amministrazione Gruppi", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            writer.WriteLine("abbandonaGruppo|" + toLeave);
            writer.Flush();

            string response = reader.ReadLine();
            bool uscito = bool.Parse(response);

            if (uscito)
            {
                temp.utenti.Remove(u);
                MessageBox.Show("Utente rimosso con successo dal gruppo " + toLeave, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GruppoAbbandonato?.Invoke(temp);
                return;

            }
            else
            {
                MessageBox.Show("Impossibile abbandonare il gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LeaveGroup();
            this.Close();
        }

        
        
    }
}
