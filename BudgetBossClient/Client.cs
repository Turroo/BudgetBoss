using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Windows.Forms;

namespace BudgetBossClient
{
    internal static class Client
    {
        private static Socket clientSocket;
        private static StreamWriter writer;
        private static StreamReader reader;

        /// <summary>
        /// Punto di ingresso principale dell'applicazione.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Connessione al server
            if (ConnectToServer())
            {
                // Avvio dell'applicazione client
                Application.Run(new Login(writer, reader));
            }
            else
            {
                MessageBox.Show("Impossibile connettersi al server. Verifica che il server sia in esecuzione e riprova.", "Errore di connessione", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static bool ConnectToServer()
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                clientSocket.Connect("localhost", 1234);

                Console.WriteLine("Connesso con successo al server\n");

                // Inizializzazione degli stream per la comunicazione
                NetworkStream stream = new NetworkStream(clientSocket);
                writer = new StreamWriter(stream);
                reader = new StreamReader(stream);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errore nella connessione al server: " + ex.Message);
                return false;
            }
        }

        static void DisconnectFromServer()
        {
            try
            {
                // Invio richiesta di disconnessione al server
                writer.WriteLine("disconnect");
                writer.Flush();

                // Chiusura della connessione e degli stream
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
                Console.WriteLine("Disconnesso con successo dal server");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossibile disconnettersi dal server: " + ex.ToString());
            }
        }
    }

    public class User
    {
        public String Username { get; set; }
        public String Password { get; set; }
        public double Contanti { get; set; }
        public double Carte { get; set; }
        public double FinanzeOnline { get; set; }

        public List<Transazione> Transazioni { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Contanti = 0;
            Carte = 0;
            FinanzeOnline = 0;
            Transazioni = new List<Transazione>();
        }

        
    }

    public class Categoria
    {
        public string nomeCategoria { get; set; }

        public Categoria(string nomeCategoria)
        {
            this.nomeCategoria = nomeCategoria;
        }

    }

    public class Transazione
    {
        public int id { get; set; }
        public double importo { get; set; }
        public MetodoDiPagamento metodoDiPagamento { get; set; }
        public DateTime dateTime { get; set; }
        public Categoria categoria { get; set; }
        public NaturaTransazione naturaTransazione { get; set; }

        public Transazione(int id, double importo, MetodoDiPagamento metodoDiPagamento, DateTime dateTime, Categoria categoria, NaturaTransazione naturaTransazione)
        {
            this.id = id;
            this.importo = importo;
            this.metodoDiPagamento = metodoDiPagamento;
            this.dateTime = dateTime;
            this.categoria = categoria;
            this.naturaTransazione = naturaTransazione;
        }


    }

    public enum MetodoDiPagamento
    {
        Contanti, Carte, FinanzeOnline
    }

    public enum NaturaTransazione
    {
        Entrata, Uscita
    }

}
