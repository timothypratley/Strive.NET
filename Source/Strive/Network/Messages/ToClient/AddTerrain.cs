using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddTerrain.
	/// </summary>
	public class AddTerrain : AddPhysicalObject {
		public Terrain terrain;
		public AddTerrain(){}
		public AddTerrain( Terrain t ) {
			this.terrain = t;
		}
	}
}
