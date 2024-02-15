using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;


public class Server
{
    private const int Port = 1234;
    private static Socket clientSocket;
    private static NetworkStream stream;
    private static StreamWriter writer;
    private static StreamReader reader;
    private static List<User> usersList = new List<User>();
    private static readonly string userListFilePath = "users.json";
    private static User temp;

    public static void Main(string[] args)
    {
        LoadUserList();
        try
        {
            // Crea un'endpoint per il server
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, Port);

            // Crea una socket TCP per accettare le connessioni dei client
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(localEndPoint);
            serverSocket.Listen(10);

            Console.WriteLine("Server avviato. In attesa di connessioni...");
            // Accetta una connessione da un client
            clientSocket = serverSocket.Accept();
            stream = new NetworkStream(clientSocket);
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            while (true)
            {

                // Ricevi e gestisci i dati del client
                HandleClient();
                    
            }
        }
        catch(Exception e)
        {
            Console.WriteLine("Errore durante l'avvio del server " + e.Message);
        }
        finally
        {
            // Chiudi tutti i flussi e il socket handler
            writer?.Close();
            reader?.Close();
            stream?.Close();
            clientSocket?.Close();
        }
    }

    private static void HandleClient()
    {
        try
        {
            Console.WriteLine("Connessione accettata da: " + clientSocket.RemoteEndPoint);

            while(true)
            {
                string requestString = reader.ReadLine();
                string response = HandleRequest(requestString);
                writer.WriteLine(response);
                writer.Flush();
            }

            

            

        }
        catch(Exception e)
        {
            Console.WriteLine("Errore durante la gestione del client " + e.Message);
        }
    }

    private static string HandleRequest(string requestString)
    {
        char separator = '|';
        string[] parts =  requestString.Split(separator);
        string operation = parts[0];

        switch(operation)
        {
            case "login":
                return Login(parts[1]);
            case "register":
                return Register(parts[1]);
            case "finanzeIniziali":
                return AddFinanzeIniziali(parts[1]);
            default:
                return "non ci entra";
        }
    }

    private static string Login(string jsonObject)
    {
        dynamic obj = JsonConvert.DeserializeObject(jsonObject);
        string username = obj.Username;
        string password = obj.Password;

        foreach(User u in usersList)
        {
            if(u.Username == username && u.Password==password)
            {
                Console.WriteLine("Loggato con successo l'utente con user: " + username + " e password: " + password);
                temp = u;
                return "True";
            }
        }


        Console.WriteLine("Combinazione utente/pw errata");
        return "False";
    }

    private static string Register(string jsonObject)
    {
        dynamic obj = JsonConvert.DeserializeObject(jsonObject);
        string username = obj.Username;
        string password = obj.Password;

        if (usersList.Any(u => u.Username == username))
        {
            Console.WriteLine("Utente già esistente");
            return "False";
        }
        User u = new User(username, password);
        usersList.Add(u);
        Console.WriteLine("Registrato con successo l'utente con user: " + username + " e password: " + password);
        temp = u;
        SaveUserList();

        return "True";
    }

    private static string AddFinanzeIniziali(String jsonObject)
    {
        dynamic obj = JsonConvert.DeserializeObject(jsonObject);
        temp.Carte = obj.Carte;
        temp.Contanti = obj.Contanti;
        temp.FinanzeOnline = obj.FinanzeOnline;
        if(ReplaceUser(temp))
        {
            Console.WriteLine("Aggiunte con successo le finanze iniziali all'utente: " + temp.Username);
            SaveUserList();
            return "True";
        }

        Console.WriteLine("Non è stato possibile aggiungere le finanze iniziali all'utente: " + temp.Username);
        return "False";
        


    }

    private static void LoadUserList()
    {
        if (File.Exists(userListFilePath))
        {
            string json = File.ReadAllText(userListFilePath);
            if(!string.IsNullOrEmpty(json))
            {
                usersList = JsonConvert.DeserializeObject<List<User>>(json);
                Console.WriteLine("Aperta con successo la lista utenti, file popolato");
            }
            else
            {
                usersList = new List<User>();
                Console.WriteLine("Aperta con successo la lista utenti, file vuoto");
            }
            
        }
        else
        {
            usersList = new List<User>();
            Console.WriteLine("Aperta con successo la lista utenti, il file non esisteva");
            string path = "users.json";
            File.Create(path).Close();
            Console.WriteLine("File creato con successo.");
        }
    }

    private static void SaveUserList()
    {
        string json = JsonConvert.SerializeObject(usersList);
        File.WriteAllText(userListFilePath, json);
        Console.WriteLine("Scritta su file la nuova lista aggiornata");
    }

    private static bool ReplaceUser(User toAdd)
    {
        foreach (User u in usersList)
        {
            if(u.Username==toAdd.Username)
            {
                usersList.Remove(u);
                usersList.Add(toAdd);
                return true;
            }
        }

        return false;
    }




}

public class User
{
    public String Username { get; set; }
    public String Password { get; set; }
    public double Contanti { get; set; }
    public double Carte { get; set; }
    public double FinanzeOnline {  get; set; }

    public User(String username, String password)
    {
        Username = username;
        Password = password;
        Contanti = 0;
        Carte = 0;
        FinanzeOnline = 0;
    }
}

