using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddMobile.
	/// </summary>
	public class AddMobile : AddPhysicalObject {
		public Mobile mobile;
		public AddMobile(){}
		public AddMobile( Mobile m ) {
			this.mobile = m;
		}
	}
}
