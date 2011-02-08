using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddEquipable.
	/// </summary>
	public class AddEquipable : AddPhysicalObject {
		public Equipable Equipable;

		public AddEquipable(){}
        public AddEquipable(Equipable e)
        {
            Equipable = e;
        }
	}
}
