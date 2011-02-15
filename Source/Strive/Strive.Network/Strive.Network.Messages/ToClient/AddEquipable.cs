using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddEquipable : AddPhysicalObject
    {
        public Equipable Equipable;

        public AddEquipable(Equipable e)
        {
            Equipable = e;
        }
    }
}
