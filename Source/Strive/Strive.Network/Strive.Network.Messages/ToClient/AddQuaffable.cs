using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddQuaffable : AddPhysicalObject
    {
        public Quaffable Quaffable;

        public AddQuaffable(Quaffable q)
        {
            Quaffable = q;
        }
    }
}
