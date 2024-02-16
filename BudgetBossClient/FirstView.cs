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
    public partial class FirstView : UserControl
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User user;
        public FirstView(StreamWriter writer, StreamReader reader, User user)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.user = user;
            InitializeValues();
        }

        private void InitializeValues()
        {
            this.label7.Text = user.Contanti.ToString() + " €";
            this.label8.Text = user.Carte.ToString() + " €";
            this.label9.Text = user.FinanzeOnline.ToString() + " €";
        }

        
    }
}
