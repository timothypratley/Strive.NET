namespace Strive.Network.Messages.ToServer
{
    public class CreateParty : IMessage
    {
        public string Name;

        public CreateParty(string name)
        {
            Name = name;
        }
    }
}
