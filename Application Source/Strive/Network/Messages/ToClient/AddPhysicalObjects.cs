using System;
using System.Collections;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddPhysicalObject.
	/// </summary>
	[Serializable]
	public class AddPhysicalObjects : IMessage
	{
		public AddPhysicalObjects(){}
		public AddPhysicalObjects( ArrayList physicalObjects ) {
			this.physialObjects = physicalObjects;
		}

		ArrayList physialObjects;
	}
}
