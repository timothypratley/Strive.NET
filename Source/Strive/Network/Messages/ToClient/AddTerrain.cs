using System;

using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for AddTerrain.
	/// </summary>

	public struct TerrainAtom {
		public float heights;
		public int textureIDs;
		public EnumTerrainType terrainType;
	}

	public class AddTerrain {
		public int startX, startZ;
		public int squareSize;
		public int width, height;
		public TerrainAtom [][] map;


		public AddTerrain(){}

		public AddTerrain( Terrain t, int squareSize, int width, int height ) {
			this.startX = t.startZ;
			this.squareSize = squareSize;
			this.width = width;
			this.height = height;
			this.map = new TerrainAtom[width][height];
		}
	}
}

