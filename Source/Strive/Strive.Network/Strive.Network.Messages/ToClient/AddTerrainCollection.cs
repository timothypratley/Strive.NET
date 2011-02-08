using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
	public class AddTerrainCollection : IMessage {
		public int StartX, StartZ;
		public int GapSize;
		public int Width, Height;
		public Terrain [,] Map;


		public AddTerrainCollection(){}

		public AddTerrainCollection(int gapSize, int width, int height ) {
			this.GapSize = gapSize;
			this.Width = width;
			this.Height = height;
			this.Map = new Terrain[width,height];
		}
	}
}

