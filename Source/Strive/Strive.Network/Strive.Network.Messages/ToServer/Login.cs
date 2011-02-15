namespace Strive.Network.Messages.ToServer
{
    public class Login : IMessage
    {
        public string Username;
        public string Password;

        public Login(string username, string password)
        {
            Username = username;
            Password = password;
        }
    }
}
