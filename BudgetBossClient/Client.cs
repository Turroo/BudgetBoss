using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
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
            if(ConnectToServer())
            {
                Application.Run(new Login(writer,reader));
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

                writer = new StreamWriter(new NetworkStream(clientSocket));
                reader = new StreamReader(new NetworkStream(clientSocket));

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Errore nella connessione al server: " + ex.Message);
                return false;
            }
        }

        static void DisconnectFromServer()
        {
            try
            {
                clientSocket.Close();
                Console.WriteLine("Disconnesso con successo dal server");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Impossibile disconnettersi dal server: " + ex.ToString());
            }
        }
    }
}
