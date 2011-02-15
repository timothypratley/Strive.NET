using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddWieldable : AddPhysicalObject
    {
        public Wieldable Weildable;

        public AddWieldable(Wieldable w)
        {
            Weildable = w;
        }
    }
}
