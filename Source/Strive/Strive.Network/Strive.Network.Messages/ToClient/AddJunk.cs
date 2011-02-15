using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddJunk : AddPhysicalObject
    {
        public Junk Junk;

        public AddJunk(Junk junk)
        {
            Junk = junk;
        }
    }
}
