using System;

using Strive.Multiverse;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Resources;
using Strive.Math3D;

namespace Strive.UI.WorldView
{
	/// <summary>
	/// Note: position and heading of the physical object
	/// are not maintained. This info is kept with the model
	/// so only use those values.
	/// </summary>
	public class PhysicalObjectInstance {
		public IModel model;
		public PhysicalObject physicalObject;

		public PhysicalObjectInstance( PhysicalObject po ) {
			physicalObject = po;
			if ( !(po is Terrain) ) {
				model = ResourceManager.LoadModel( po.ObjectInstanceID, po.ModelID, po.Height );
			}
		}
	}
}
