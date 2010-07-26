using System;

using Strive.Math3D;
using Strive.Rendering.Models;
using Strive.Multiverse;
using Strive.Resources;

namespace Strive.UI.WorldView
{
	/// <summary>
	/// An octally linked list, to store neighbour relationships
	/// </summary>
	public class TerrainPiece : PhysicalObjectInstance {
		// neighbours
		public float xplus;
		public bool xplusKnown = false;
		public float zplus;
		public bool zplusKnown = false;
		public float xpluszplus;
		public bool xpluszplusKnown = false;

		public TerrainPiece( Terrain t, ResourceManager rm ) : base( t, rm ) {
			this.physicalObject = t;
		}

		public float x {
			get { return physicalObject.Position.X; }
			set { physicalObject.Position.X = value; }
		}
		public float z {
			get { return physicalObject.Position.Z; }
			set { physicalObject.Position.Z = value; }
		}
		public float altitude {
			get { return physicalObject.Position.Y; }
			set { physicalObject.Position.Y = value; }
		}
		public int texture_id {
			get { return ((Terrain)physicalObject).ResourceID; }
			set { ((Terrain)physicalObject).ResourceID = value; }
		}
		public int instance_id {
			get { return physicalObject.ObjectInstanceID; }
			set { physicalObject.ObjectInstanceID = value; }
		}
	}
}
