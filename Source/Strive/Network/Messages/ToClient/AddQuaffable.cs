using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddQuaffable.
	/// </summary>
	public class AddQuaffable : AddPhysicalObject {
		public Quaffable quaffable;
		public AddQuaffable(){}
		public AddQuaffable( Quaffable q ) {
			this.quaffable = q;
		}
	}
}