using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Terrain : PhysicalObject
	{
		public Schema.TemplateTerrainRow terrain;

		public Terrain (
			Schema.TemplateTerrainRow terrain,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
			this.terrain = terrain;
		}
	}
}
