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
    public partial class GestioneFinanze : UserControl
    {
        private StreamWriter writer;
        private StreamReader reader;
        private User user;
        private List<Categoria> categorie;
        public GestioneFinanze(StreamWriter writer, StreamReader reader, User user, List<Categoria> categorie)
        {
            InitializeComponent();
            this.writer = writer;
            this.reader = reader;
            this.user = user;
            this.categorie = categorie;
            this.user.Transazioni = user.Transazioni;
            UpdateValues();
            PopolaTabella(user.Transazioni);
        }

        private void UpdateValues()
        {
            this.label7.Text = user.Contanti.ToString() + " €";
            this.label8.Text = user.Carte.ToString() + " €";
            this.label9.Text = user.FinanzeOnline.ToString() + " €";
        }

        private void PopolaRiga(int rowIndex,Transazione t)
        {
            dataGridView1.Rows[rowIndex].Cells["IdColumn"].Value = t.id;
            dataGridView1.Rows[rowIndex].Cells["ImportoColumn"].Value = t.importo;
            dataGridView1.Rows[rowIndex].Cells["MetodoColumn"].Value = t.metodoDiPagamento;
            dataGridView1.Rows[rowIndex].Cells["DateColumn"].Value = t.dateTime;
            dataGridView1.Rows[rowIndex].Cells["CategoriaColumn"].Value = t.categoria.nomeCategoria;
            dataGridView1.Rows[rowIndex].Cells["NaturaColumn"].Value = t.naturaTransazione;
        }

        private void PopolaTabella(List<Transazione> transazioni)
        {
            dataGridView1.Rows.Clear();
            foreach(Transazione t in transazioni)
            {
                int index = dataGridView1.Rows.Add();
                PopolaRiga(index, t);
            }
        }

        private List<Transazione> filtraNatura(List<Transazione>transazioni,NaturaTransazione natura)
        {
            List<Transazione> result = new List<Transazione>();

            foreach(Transazione t in transazioni)
            {
                if(t.naturaTransazione==natura)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        private List<Transazione> filtraMetodo(List<Transazione> transazioni, MetodoDiPagamento metodo)
        {
            List<Transazione> result = new List<Transazione>();

            foreach (Transazione t in transazioni)
            {
                if (t.metodoDiPagamento == metodo)
                {
                    result.Add(t);
                }
            }

            return result;
        }

        private List<Transazione> filtraCategoria(List<Transazione> transazioni, Categoria categoria)
        {
            List<Transazione> result = new List<Transazione>();

            foreach (Transazione t in transazioni)
            {
                if (t.categoria.nomeCategoria == categoria.nomeCategoria)
                {
                    result.Add(t);
                }
            }

            return result;
        }


        private void MetodoPagamentoScelto(string metodo)
        {
            MetodoDiPagamento m = (MetodoDiPagamento)Enum.Parse(typeof(MetodoDiPagamento),metodo);
            PopolaTabella(filtraMetodo(user.Transazioni,m));

        }

        private void CategoriaScelta(string categoria)
        {
            Categoria cat = null;
            foreach(Categoria c in categorie)
            {
                if(c.nomeCategoria == categoria)
                {
                    cat = c; break;
                }
            }
            PopolaTabella(filtraCategoria(user.Transazioni, cat));
        }

        private void NaturaScelta(string natura)
        {
            NaturaTransazione n = (NaturaTransazione)Enum.Parse(typeof(NaturaTransazione), natura);
            PopolaTabella(filtraNatura(user.Transazioni, n));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {

                MetodiPagamento metodiPagamento = new MetodiPagamento();
                metodiPagamento.MetodoPagamentoSelezionato += MetodoPagamentoScelto;
                metodiPagamento.Show();
            }
            else if (radioButton2.Checked)
            {
                WindowCategorie windowCategorie = new WindowCategorie(categorie);
                windowCategorie.CategoriaSelezionata += CategoriaScelta;
                windowCategorie.Show();
            }
            else if (radioButton3.Checked)
            {
                WindowNatura windowNatura = new WindowNatura();
                windowNatura.NaturaSelezionata += NaturaScelta;
                windowNatura.Show();
            }
            else
            {
                PopolaTabella(user.Transazioni);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            PopolaTabella(user.Transazioni);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AggiungiTransazione aggiungiTransazione = new AggiungiTransazione(writer, reader, user,categorie);
            aggiungiTransazione.TransazioneAggiunta += HandleNewTransaction;
            aggiungiTransazione.Show();
        }

        private void HandleNewTransaction(Transazione t)
        {
            user.Transazioni.Add(t);
            if(t.naturaTransazione.Equals(NaturaTransazione.Entrata))
            {
                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
                {
                    user.Carte += t.importo;
                }
                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
                {
                    user.Contanti += t.importo;
                }

                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
                {
                    user.FinanzeOnline += t.importo;
                }
            }
            else
            {
                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
                {
                    user.Carte -= t.importo;
                }
                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
                {
                    user.Contanti -= t.importo;
                }

                if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
                {
                    user.FinanzeOnline -= t.importo;
                }
            }

            UpdateValues();
            PopolaTabella(user.Transazioni);
        }


       
    }


}

