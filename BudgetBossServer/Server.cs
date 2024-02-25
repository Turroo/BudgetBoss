using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;


public class Server
{
    private const int Port = 1234;
    private static Socket clientSocket;
    private static NetworkStream stream;
    private static StreamWriter writer;
    private static StreamReader reader;
    private static List<User> usersList = new List<User>();
    private static List<Categoria> categorie = new List<Categoria>();
    private static List<Gruppo> groupsList = new List<Gruppo>();
    private static readonly string userListFilePath = "users.json";
    private static readonly string categorieFilePath = "categorie.json";
    private static readonly string gruppiFilePath = "gruppi.json";
    private static User temp;

    public static void Main(string[] args)
    {
        LoadUserList();
        LoadCategorie();
        LoadGroupsList();
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
            case "getCurrentUser":
                return SendCurrentUser(temp);
            case "getCategorie":
                return SendCategorie(categorie);
            case "getGruppi":
                return SendGruppi(groupsList);
            case "aggiungiCategoria":
                return AggiungiCategoria(categorie, parts[1]);
            case "rimuoviCategoria":
                return RimuoviCategoria(categorie, parts[1]);
            case "aggiungiTransazione":
                return AggiungiTransazione(parts[1], parts[2]);
            case "rimuoviTransazione":
                return RimuoviTransazione(parts[1], parts[2]);
            case "creaGruppo":
                return CreaGruppo(parts[1]);
            case "uniscitiAGruppo":
                return UniscitiAGruppo(parts[1]);
            case "abbandonaGruppo":
                return AbbandonaGruppo(parts[1]);
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

    private static string SendCurrentUser(User u)
    {
        Console.WriteLine("Inviato con successo al client l'utente: " +  u.Username);
        return JsonConvert.SerializeObject(u);
    }

    private static string SendCategorie(List<Categoria> categorie)
    {
        Console.WriteLine("Inviato con successo al client la lista di categorie");
        return JsonConvert.SerializeObject(categorie);
    }

    private static string SendGruppi(List<Gruppo> gruppi)
    {
        Console.WriteLine("Inviato con successo al client la lista di gruppi");
        return JsonConvert.SerializeObject(gruppi);
    }

    private static string AggiungiCategoria(List<Categoria> categorie,string categoria)
    {
        if(categorie.Count == 8)
        {
            Console.WriteLine("Numero massimo di categorie raggiunto (8)");
            return "maximum";
        }

        else if(checkCategoria(categorie,categoria))
        {
            Console.WriteLine("Categoria " + categoria + " già presente in memoria");
            return "already";
        }

        else
        {
            Console.WriteLine("Aggiungo la nuova categoria: " + categoria);
            categorie.Add(new Categoria(categoria));
            SaveCategorieList();
            return "OK";
        }
    }

    private static string RimuoviCategoria(List<Categoria> categorie, string categoria)
    {
        if(checkCategoria(categorie,categoria))
        {
            Console.WriteLine("Rimuovo la categoria: " + categoria);
            Categoria toRemove = categorie.FirstOrDefault(c => c.nomeCategoria == categoria);
            categorie.Remove(toRemove);
            SaveCategorieList();
            return "True";
        }
        else
        {
            Console.WriteLine("Categoria non presente");
            return "False";
        }
    }

    private static string CreaGruppo(string nomeGruppo)
    {
        Gruppo g = new Gruppo(nomeGruppo, temp);
        groupsList.Add(g);
        temp.isAdmin = true;
        temp.gruppiAppartenenza.Add(g.nomeGruppo);
        Console.WriteLine("Creato con successo il gruppo " + g.nomeGruppo);
        SaveGroupsList();
        SaveUserList();
        return "True";
    }

    private static string UniscitiAGruppo(string nomeGruppo)
    {
        Gruppo g = groupsList.Find(gr => gr.nomeGruppo  == nomeGruppo);
        g.utenti.Add(temp);
        temp.gruppiAppartenenza.Add(g.nomeGruppo);
        Console.WriteLine("Unito con successo al gruppo " + g.nomeGruppo);
        SaveGroupsList();
        SaveUserList();
        return "True";
    }

    private static string AbbandonaGruppo(string nomeGruppo)
    {
        Gruppo g = groupsList.Find(gr => gr.nomeGruppo == nomeGruppo);
        RemoveUserFromGroup(g, temp);
        ReplaceGroup(g);
        temp.gruppiAppartenenza.Remove(g.nomeGruppo);
        Console.WriteLine("Rimosso con successo dal gruppo " + g.nomeGruppo);
        SaveGroupsList();
        SaveUserList();
        return "True";
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
            Console.WriteLine("File utenti creato con successo.");
        }
    }

    private static void LoadGroupsList()
    {
        if (File.Exists(gruppiFilePath))
        {
            string json = File.ReadAllText(gruppiFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                groupsList = JsonConvert.DeserializeObject<List<Gruppo>>(json);
                Console.WriteLine("Aperta con successo la lista gruppi, file popolato");
            }
            else
            {
                groupsList = new List<Gruppo>();
                Console.WriteLine("Aperta con successo la lista gruppi, file vuoto");
            }

        }
        else
        {
            usersList = new List<User>();
            Console.WriteLine("Aperta con successo la lista gruppi, il file non esisteva");
            string path = gruppiFilePath;
            File.Create(path).Close();
            Console.WriteLine("File gruppi creato con successo.");
        }
    }

    private static bool checkCategoria(List<Categoria> categorie, string categoria)
    {
        foreach(Categoria c in categorie)
        {
            if (c.nomeCategoria.ToLower().Equals(categoria.ToLower()))
                return true;
        }

        return false;
    }

    private static void LoadCategorie()
    {
        if (File.Exists(categorieFilePath))
        {
            string json = File.ReadAllText(categorieFilePath);
            if (!string.IsNullOrEmpty(json))
            {
                categorie = JsonConvert.DeserializeObject<List<Categoria>>(json);
                Console.WriteLine("Aperta con successo la lista categorie, file popolato");
            }
            else
            {
                categorie =
                [
                    new Categoria("Cibo"),
                    new Categoria("Intrattenimento"),
                    new Categoria("Shopping"),
                    new Categoria("Stipendio"),
                ];
                SaveCategorieList();
                Console.WriteLine("Aperta con successo la lista categorie, file vuoto");
            }

        }
        else
        {
            categorie =
                [
                    new Categoria("Cibo"),
                    new Categoria("Intrattenimento"),
                    new Categoria("Shopping"),
                    new Categoria("Stipendio"),
                ];
            Console.WriteLine("Aperta con successo la lista categorie, il file non esisteva");
            File.Create(categorieFilePath).Close();
            SaveCategorieList();
            Console.WriteLine("File categorie creato con successo.");
        }
    }

    private static void SaveUserList()
    {
        string json = JsonConvert.SerializeObject(usersList);
        File.WriteAllText(userListFilePath, json);
        Console.WriteLine("Scritta su file la nuova lista utenti aggiornata");
    }

    private static void SaveCategorieList()
    {
        string json = JsonConvert.SerializeObject(categorie);
        File.WriteAllText(categorieFilePath, json);
        Console.WriteLine("Scritta su file la nuova lista categorie aggiornata");
    }

    private static void SaveGroupsList()
    {
        string json = JsonConvert.SerializeObject(groupsList);
        File.WriteAllText(gruppiFilePath, json);
        Console.WriteLine("Scritta su file la nuova lista gruppi aggiornata");
    }

    private static string AggiungiTransazione(string transazioneJson, string username)
    {
        User u = GetUser(username);
        Transazione t = JsonConvert.DeserializeObject<Transazione>(transazioneJson);
        if(PostAddTransaction(u,t))
        {
            Console.WriteLine("Transazione con id: " + t.id + " aggiunta con successo");
            SaveUserList();
            return "True";
        }
        else
        {
            Console.WriteLine("Impossibile aggiungere la transazione con id: " + t.id);
            return "False";
        }



    }

    private static string RimuoviTransazione(string jsonTransaction,string username)
    {
        User u = GetUser(username);
        Transazione t = JsonConvert.DeserializeObject<Transazione>(jsonTransaction);
        if (PostRemoveTransaction(u, t))
        {
            Console.WriteLine("Transazione con id: " + t.id + " rimossa con successo");
            SaveUserList();
            return "True";
        }
        else
        {
            Console.WriteLine("Impossibile rimuovere la transazione con id: " + t.id);
            return "False";
        }
    }

    private static bool PostAddTransaction(User u, Transazione t)
    {
        u.Transazioni.Add(t);
        if (t.naturaTransazione.Equals(NaturaTransazione.Entrata))
        {
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
            {
                u.Carte += t.importo;
            }
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
            {
                u.Contanti += t.importo;
            }

            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
            {
                u.FinanzeOnline += t.importo;
            }
        }
        else
        {
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
            {
                u.Carte -= t.importo;
            }
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
            {
                u.Contanti -= t.importo;
            }

            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
            {
                u.FinanzeOnline -= t.importo;
            }
        }

        return ReplaceUser(u);
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

    private static void ReplaceGroup(Gruppo toAdd)
    {
        foreach(Gruppo g in groupsList)
        {
            if(g.nomeGruppo == toAdd.nomeGruppo)
            {
                groupsList.Remove(g);
                groupsList.Add(toAdd);
                return;
            }
        }

        
    }

    private static void RemoveUserFromGroup(Gruppo g, User user)
    {
        for (int i = 0; i < g.utenti.Count; i++)
        {
            if (user.Username == g.utenti[i].Username)
            {
                g.utenti.RemoveAt(i);
            }
        }
    }

    private static bool PostRemoveTransaction(User u, Transazione t)
    {
        if (!RemoveTransactionFromUser(u, t.id))
            return false;
        if (t.naturaTransazione.Equals(NaturaTransazione.Entrata))
        {
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
            {
                u.Carte -= t.importo;
            }
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
            {
                u.Contanti -= t.importo;
            }

            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
            {
                u.FinanzeOnline -= t.importo;
            }
        }
        else
        {
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Carte))
            {
                u.Carte += t.importo;
            }
            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.Contanti))
            {
                u.Contanti += t.importo;
            }

            if (t.metodoDiPagamento.Equals(MetodoDiPagamento.FinanzeOnline))
            {
                u.FinanzeOnline += t.importo;
            }
        }

        return ReplaceUser(u);

    }

    private static bool RemoveTransactionFromUser(User u,int id)
    {
        foreach(User user in usersList)
        {
            if(user.Username==u.Username) 
            {
                foreach(Transazione t in user.Transazioni)
                {
                    if(t.id == id)
                    {
                        user.Transazioni.Remove(t);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private static User GetUser(string username)
    {
        User result = null;
        foreach(User u in usersList)
        {
            if(u.Username == username)
            {
                result = u;
                return result;
            }
                
        }

        return result;

    }




}

public class User
{
    public String Username { get; set; }
    public String Password { get; set; }
    public double Contanti { get; set; }
    public double Carte { get; set; }
    public double FinanzeOnline {  get; set; }

    public bool isAdmin { get; set; }
    public List<string> gruppiAppartenenza { get; set; }

    public List<Transazione> Transazioni {  get; set; }

    public User(String username, String password)
    {
        Username = username;
        Password = password;
        Contanti = 0;
        Carte = 0;
        FinanzeOnline = 0;
        Transazioni = new List<Transazione>();
        this.isAdmin = false;
        this.gruppiAppartenenza = new List<string>();
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
    public NaturaTransazione naturaTransazione{ get; set; }
    
    
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
    Contanti,Carte,FinanzeOnline
}

public enum NaturaTransazione
{
    Entrata, Uscita
}

public class Gruppo
{
    public string nomeGruppo { get; set; }
    public User admin {  get; set; }
    public List<User> utenti {  get; set; }

    public Gruppo(string nomeGruppo,User u)
    {
        this.nomeGruppo = nomeGruppo;
        this.admin = u;
        this.utenti = new List<User>();
        utenti.Add(u);
    }
    
}

