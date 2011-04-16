namespace Strive.Network.Messages.ToClient
{
    public class Communication
    {
        public string Name;
        public string Message;
        public CommunicationType CommunicationType;

        public Communication(string name, string message, CommunicationType communicationType)
        {
            Name = name;
            Message = message;
            CommunicationType = communicationType;
        }
    }
}
