using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddEquipable.
	/// </summary>
	public class AddEquipable : AddPhysicalObject {
		public Equipable equipable;
		public AddEquipable(){}
		public AddEquipable( Equipable e ) {
			this.equipable = e;
		}
	}
}
