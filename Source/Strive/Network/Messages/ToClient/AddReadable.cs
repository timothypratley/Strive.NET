using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddReadable.
	/// </summary>
	public class AddReadable : AddPhysicalObject {
		public Readable readable;
		public AddReadable(){}
		public AddReadable( Readable r ) {
			this.readable = r;
		}
	}
}
