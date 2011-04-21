using System.Windows.Media.Media3D;

namespace Strive.Model
{
    public class TerrainModel : AModel
    {
        public TerrainModel(Vector3D position, int textureId)
        {
            Position = position;
            TextureId = textureId;
        }

        public Vector3D Position { get; private set; }
        public int TextureId { get; private set; }
    }
}
