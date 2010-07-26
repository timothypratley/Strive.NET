using System;

namespace Strive.Server.Model
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Terrain : PhysicalObject
	{
		public Terrain(){}
		public EnumTerrainType TerrainType;
		public Terrain (
			Schema.TemplateTerrainRow terrain,
			Schema.TemplateObjectRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
			TerrainType = (EnumTerrainType)terrain.EnumTerrainTypeID;
		}
	}
}
