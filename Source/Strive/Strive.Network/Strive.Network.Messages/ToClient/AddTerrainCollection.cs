using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    public class AddTerrainCollection : IMessage
    {
        public int StartX, StartZ;
        public int GapSize;
        public int Width, Height;
        public Terrain[,] Map;

        public AddTerrainCollection(int gapSize, int width, int height)
        {
            GapSize = gapSize;
            Width = width;
            Height = height;
            Map = new Terrain[width, height];
        }
    }
}

