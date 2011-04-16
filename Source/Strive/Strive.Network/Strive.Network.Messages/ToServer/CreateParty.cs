namespace Strive.Network.Messages.ToServer
{
    public class CreateParty
    {
        public string Name;

        public CreateParty(string name)
        {
            Name = name;
        }
    }
}
