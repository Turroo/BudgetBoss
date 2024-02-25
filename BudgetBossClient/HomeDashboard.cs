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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace BudgetBossClient
{
    public partial class HomeDashboard : Form
    {
        private StreamWriter writer;
        private StreamReader reader;
        private FirstView firstView;
        private GestioneCategorie gestioneCategorie;
        private GestioneFinanze gestioneFinanze;
        private GestioneGruppi gestioneGruppi;
        private GestioneAdmin gestioneAdmin;
        private User user;
        private List<Categoria> categorie;
        private List<Gruppo> groupsList;
        public HomeDashboard(StreamWriter writer, StreamReader reader, User user, List<Categoria> categorie, List<Gruppo> groupsList)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.user = user;
            this.categorie = categorie;
            this.groupsList = groupsList;
            this.labelUsername.Text = user.Username;
            addHome();
            if(!user.isAdmin)
            {
                button4.Visible = false;
            }
            
        }

        private void addHome()
        {
            viewPanel.Controls.Clear(); 
            firstView = new FirstView(writer, reader, user);
            firstView.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(firstView);
            firstView.BringToFront();
        }

        private void addGestioneCategorie()
        {
            viewPanel.Controls.Clear();
            gestioneCategorie = new GestioneCategorie(writer, reader, categorie);
            gestioneCategorie.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(gestioneCategorie);
            gestioneCategorie.BringToFront();
        }

        private void addGestioneFinanze()
        {
            viewPanel.Controls.Clear();
            gestioneFinanze = new GestioneFinanze(writer, reader, user,categorie);
            gestioneFinanze.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(gestioneFinanze);
            gestioneFinanze.BringToFront();
        }

        private void addGestioneGruppi()
        {
            viewPanel.Controls.Clear();
            gestioneGruppi = new GestioneGruppi(writer, reader, user, groupsList);
            gestioneGruppi.NewGroupCreated += GestioneGruppi_NewGroupCreated;
            gestioneGruppi.AdminRemoved += GestioneGruppi_AdminRemoved;
            gestioneGruppi.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(gestioneGruppi);
            gestioneGruppi.BringToFront();
        }

        private void addGestioneAdmin()
        {
            viewPanel.Controls.Clear();
            gestioneAdmin = new GestioneAdmin(writer, reader, user, groupsList);
            gestioneAdmin.Dock = DockStyle.Fill;
            viewPanel.Controls.Add(gestioneAdmin);
            gestioneAdmin.BringToFront();
        }

        private void HomeButton_Click(object sender, EventArgs e)
        {
            addHome();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addGestioneCategorie();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addGestioneFinanze();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            addGestioneGruppi();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            addGestioneAdmin();
        }

        private void GestioneGruppi_NewGroupCreated(object sender, GruppoEventArgs e)
        {
            // Abilita il bottone "GestioneAdmin" se l'utente è un admin
            if (user.isAdmin)
            {
                button4.Visible = true;
            }
        }

        private void GestioneGruppi_AdminRemoved(object sender, GruppoEventArgs e)
        {
            button4.Visible = false; // Nasconde la sezione GestioneAdmin se l'utente non è più admin di nessun gruppo
            addHome(); // Reindirizza l'utente alla HomeView
        }
    }
}
