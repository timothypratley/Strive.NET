using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    /// <summary>
    /// Summary description for AddTerrain.
    /// </summary>
    public class AddTerrain : AddPhysicalObject
    {
        public Terrain Terrain;

        public AddTerrain(Terrain t)
        {
            Terrain = t;
        }
    }
}
