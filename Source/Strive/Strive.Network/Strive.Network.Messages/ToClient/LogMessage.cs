namespace Strive.Network.Messages.ToClient
{
    public class LogMessage : IMessage
    {
        public string Message;

        public LogMessage(string message)
        {
            Message = message;
        }
    }
}
