using System.Net.Sockets;

public class Server
{
    private const int Port = 1234;
    private static Socket serverSocket;
    private List<User> usersList = new List<User>();

    public static void main(string[] args)
    {
        try
        {

        }
        catch(Exception e)
        {
            Console.WriteLine("Errore durante l'avvio del server " + e.Message);
        }
    }
}

public class User
{
    public String Username { get; set; }
    public String Password { get; set; }

    public User(String username, String password)
    {
        Username = username;
        Password = password;
    }
}

