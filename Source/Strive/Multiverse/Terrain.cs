using System;

namespace Strive.Multiverse
{
	/// <summary>
	/// Summary description for Mobile.
	/// </summary>
	public class Terrain : PhysicalObject
	{
		public Terrain(){}
		public Terrain (
			Schema.TemplateTerrainRow terrain,
			Schema.ObjectTemplateRow template,
			Schema.ObjectInstanceRow instance
		) : base ( template, instance ) {
		}
	}
}
