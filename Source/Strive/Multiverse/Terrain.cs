using System;

namespace Strive.Multiverse
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
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
			TerrainType = (EnumTerrainType)terrain.EnumTerrainTypeID;
		}
	}
}
