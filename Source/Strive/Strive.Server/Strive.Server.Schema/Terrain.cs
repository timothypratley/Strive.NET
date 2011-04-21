using Strive.Common;


namespace Strive.Server.DB
{
    public class Terrain : PhysicalObject
    {
        public EnumTerrainType TerrainType;

        public Terrain(
            Schema.TemplateTerrainRow terrain,
            Schema.TemplateObjectRow template,
            Schema.ObjectInstanceRow instance
        )
            : base(template, instance)
        {
            TerrainType = (EnumTerrainType)terrain.EnumTerrainTypeID;
        }
    }
}
