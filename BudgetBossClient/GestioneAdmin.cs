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
    public partial class GestioneAdmin : UserControl
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Gruppo> groupsList;
        public GestioneAdmin(StreamWriter writer, StreamReader reader, User u, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
            this.groupsList = groupsList;
            PopulateCombo();
            SetVisibility();
        }

        private void SetVisibility()
        {
            if (comboBox1.SelectedIndex == -1)
            {
                panel1.Visible = false;
                button3.Visible = false;
            }
            else
            {
                panel1.Visible = true;
                button3.Visible = true;
                LoadUsers(comboBox1.SelectedItem.ToString()); // Carica gli utenti del gruppo selezionato
            }
        }


        private void PopulateCombo()
        {
            comboBox1.Items.Clear();
            foreach (Gruppo g in groupsList)
            {
                if (g.admin.Username == u.Username)
                {
                    comboBox1.Items.Add(g.nomeGruppo);
                }
            }
        }

        private void LoadUsers(string groupName)
        {
            listBox1.Items.Clear();
            Gruppo selectedGroup = groupsList.Find(gr => gr.nomeGruppo == groupName);
            if (selectedGroup != null)
            {
                foreach (User user in selectedGroup.utenti)
                {
                    listBox1.Items.Add(user.Username);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetVisibility();
        }



        private void RemovePartecipante()
        {
            if(listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Seleziona un partecipante da rimuovere", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            string nomeG = comboBox1.SelectedItem.ToString();
            Gruppo g = groupsList.Find(gr => gr.nomeGruppo == nomeG);
            User toRemove = g.utenti.Find(ut => ut.Username == listBox1.SelectedItem.ToString());

            if(toRemove.Username == u.Username)
            {
                MessageBox.Show("Non puoi rimuovere te stesso, devi cancellare il gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            writer.WriteLine("rimuoviPartecipante|"+toRemove.Username+"|"+g.nomeGruppo);
            writer.Flush();

            string response = reader.ReadLine();
            bool rimosso = bool.Parse(response);

            if(rimosso)
            {
                RemoveUserFromGroup(g, toRemove);
                ReplaceGroup(g);
                MessageBox.Show("Utente rimosso con successo dal gruppo " + g.nomeGruppo, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetVisibility();
                return;
            }

            else
            {
                MessageBox.Show("Impossibile rimuovere l'utente selezionato dal gruppo", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

        }

        private void DeleteGroup()
        {
            string nomeG = comboBox1.SelectedItem.ToString();
            Gruppo g = groupsList.Find(gr => gr.nomeGruppo == nomeG);

            writer.WriteLine("eliminaGruppo|" + g.nomeGruppo);
            writer.Flush();

            string response = reader.ReadLine();
            bool cancellato = bool.Parse(response);

            if(cancellato)
            {
                groupsList.Remove(g);
                u.gruppiAppartenenza.Remove(g.nomeGruppo);
                isStillAdmin();
                MessageBox.Show("Eliminato con successo il gruppo " + g.nomeGruppo, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                comboBox1.SelectedIndex = -1;
                PopulateCombo();
                return;


            }
            else
            {
                MessageBox.Show("Impossibile eliminare il gruppo selezionato", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ReplaceGroup(Gruppo toAdd)
        {
            foreach (Gruppo g in groupsList)
            {
                if (g.nomeGruppo == toAdd.nomeGruppo)
                {
                    groupsList.Remove(g);
                    groupsList.Add(toAdd);
                    return;
                }
            }




        }

        private void RemoveUserFromGroup(Gruppo g, User user)
        {
            for (int i = 0; i < g.utenti.Count; i++)
            {
                if (user.Username == g.utenti[i].Username)
                {
                    g.utenti.RemoveAt(i);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            RemovePartecipante();
        }

        private void isStillAdmin()
        {
            if(u.gruppiAppartenenza.Count ==0)
            {
                u.isAdmin = false;
            }
            else
            {
                if(howManyAdmin()==0)
                {
                    u.isAdmin = false;
                }
                else
                { u.isAdmin = true; }
            }
        }

        private int howManyAdmin()
        {
            int result = 0;
            foreach (string g in u.gruppiAppartenenza)
            {
                Gruppo temp = groupsList.Find(gr => gr.nomeGruppo == g);
                if (temp.admin.Username == u.Username)
                    result++;
            }

            return result;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Vuoi procedere con l'eliminazione del gruppo?", "Conferma", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                DeleteGroup();
            }
        }
    }
}
