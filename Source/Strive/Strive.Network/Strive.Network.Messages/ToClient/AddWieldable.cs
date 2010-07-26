using System;

using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddWieldable.
	/// </summary>
	public class AddWieldable : AddPhysicalObject {
		public Wieldable weildable;
		public AddWieldable(){}
		public AddWieldable( Wieldable w ) {
			this.weildable = w;
		}
	}
}
