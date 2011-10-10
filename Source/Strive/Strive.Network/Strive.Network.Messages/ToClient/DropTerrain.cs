using Strive.Model;
using System.Windows.Media.Media3D;


namespace Strive.Network.Messages.ToClient
{
    public class DropTerrain
    {
        public Vector3D Position;

        public DropTerrain(TerrainModel t)
        {
            Position = t.Position;
        }
    }
}
