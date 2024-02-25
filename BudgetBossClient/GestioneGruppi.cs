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
    public partial class GestioneGruppi : UserControl
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        private List<Gruppo> groupsList;
        public GestioneGruppi(StreamWriter writer, StreamReader reader, User u, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
            this.groupsList = groupsList;
            UpdateValues();
        }

        private void UpdateValues()
        {
            listBox1.Items.Clear();
            var toAdd = u.gruppiAppartenenza.ToArray();
            listBox1.Items.AddRange(toAdd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                CreaGruppo creaGruppo = new CreaGruppo(writer, reader, u, groupsList);
                creaGruppo.GruppoAggiunto += HandleNewGroup;
                creaGruppo.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                UniscitiAGruppo uniscitiAGruppo = new UniscitiAGruppo(writer, reader, u, groupsList);
                uniscitiAGruppo.GruppoUnito += HandleNewJoin;
                uniscitiAGruppo.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                AbbandonaGruppo abbandonaGruppo = new AbbandonaGruppo(writer, reader, u, groupsList);
                abbandonaGruppo.GruppoAbbandonato += HandleNewLeave;
                abbandonaGruppo.Show();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Gruppo toPass = null;
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Seleziona un gruppo di cui visualizzarne i dettagli", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            toPass = groupsList.Find(gr => gr.nomeGruppo == listBox1.SelectedItem.ToString());
            VisualizzaDettagliGruppo visualizzaDettagliGruppo = new VisualizzaDettagliGruppo(toPass);
            visualizzaDettagliGruppo.Show();
        }



        private void HandleNewGroup(Gruppo g)
        {
            groupsList.Add(g);
            u.isAdmin = true;
            u.gruppiAppartenenza.Add(g.nomeGruppo);
            UpdateValues();

        }

        private void HandleNewJoin(Gruppo g)
        {
            Gruppo toEdit = groupsList.Find(gr => gr.nomeGruppo == g.nomeGruppo);

            // Verifica se l'utente è già presente nel gruppo
            if (!toEdit.utenti.Any(u => u.Username == this.u.Username))
            {
                // Aggiungi l'utente solo se non è già presente
                toEdit.utenti.Add(this.u);
            }
            u.gruppiAppartenenza.Add(toEdit.nomeGruppo);
            UpdateValues();
        }


        private void HandleNewLeave(Gruppo g)
        {
            Gruppo toEdit = groupsList.Find(gr => gr.nomeGruppo == g.nomeGruppo);
            RemoveUserFromGroup(toEdit, u);
            ReplaceGroup(toEdit);
            u.gruppiAppartenenza.Remove(toEdit.nomeGruppo);
            UpdateValues();
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

        private void RemoveUserFromGroup(Gruppo g,User user)
        {
            for(int i=0;i<g.utenti.Count;i++)
            {
                if(user.Username == g.utenti[i].Username)
                {
                    g.utenti.RemoveAt(i);
                }
            }
        }
    }
}
