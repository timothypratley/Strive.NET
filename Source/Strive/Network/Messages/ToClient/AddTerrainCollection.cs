using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddTerrain.
	/// </summary>

	public class AddTerrainCollection : IMessage {
		public int startX, startZ;
		public int gap_size;
		public int width, height;
		public Terrain [,] map;


		public AddTerrainCollection(){}

		public AddTerrainCollection(int gap_size, int width, int height ) {
			this.gap_size = gap_size;
			this.width = width;
			this.height = height;
			this.map = new Terrain[width,height];
		}
	}
}

