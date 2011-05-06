namespace Strive.Network.Messages.ToServer
{
    public class InviteToParty
    {
        public string User;

        public InviteToParty(string user)
        {
            User = user;
        }
    }
}
