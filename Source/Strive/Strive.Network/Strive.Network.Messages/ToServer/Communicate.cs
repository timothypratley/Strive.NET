namespace Strive.Network.Messages.ToServer
{
    public class Communicate
    {
        public string To;
        public CommunicationType CommunicationType;
        public string Message;

        public Communicate(string to, CommunicationType communicationType, string message)
        {
            To = to;
            CommunicationType = communicationType;
            Message = message;
        }
    }
}
