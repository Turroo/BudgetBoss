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
    public partial class AggiungiTransazione : Form
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User u;
        public AggiungiTransazione(StreamWriter writer, StreamReader reader, User u)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.u = u;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Transazione temp = createTransazione();
            if(temp == null)
            {
                return;
            }
            try
            {
                string toSend = JsonConvert.SerializeObject(temp);
                if (string.IsNullOrEmpty(toSend))
                    return;
                writer.WriteLine("aggiungiTransazione|" + toSend);
                writer.Flush();

                string response = reader.ReadLine();
                bool aggiunto = bool.Parse(response);

                if (aggiunto)
                {
                    MessageBox.Show("Transazione con id: " + temp.id + " aggiunta con successo", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private Transazione createTransazione()
        {
            Transazione result = null;
            if (string.IsNullOrEmpty(this.textBox1.Text) || string.IsNullOrEmpty(this.textBox2.Text)
                || this.comboBox1.SelectedValue == null || this.comboBox2.SelectedValue == null
                || this.comboBox3.SelectedValue == null)
            {
                MessageBox.Show("Completa tutti i campi prima di inviare la transazione", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return result;
            }

            result = new Transazione(int.Parse(this.textBox1.Text),double.Parse(this.textBox2.Text),(MetodoDiPagamento)comboBox1.SelectedItem,
                this.dateTimePicker1.Value,(Categoria)comboBox2.SelectedItem,(NaturaTransazione)comboBox3.SelectedItem);

            return result;

        }
    }
}
