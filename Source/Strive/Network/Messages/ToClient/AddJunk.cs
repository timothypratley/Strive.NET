using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddJunk.
	/// </summary>
	public class AddJunk : AddPhysicalObject {
		public Junk junk;
		public AddJunk(){}
		public AddJunk( Junk j ) {
			this.junk = j;
		}
	}
}
