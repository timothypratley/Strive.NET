using System;

using Strive.Math3D;
using Strive.Rendering.Models;

namespace Strive.Resources
{
	/// <summary>
	/// An octally linked list, to store neighbour relationships
	/// </summary>
	public class TerrainPiece {
		public float x, z;
		public float altitude;
		public int texture_id;
		public int instance_id;
		public IModel model = null;

		// neighbours
		public float xplus;
		public bool xplusKnown = false;
		public float zplus;
		public bool zplusKnown = false;
		public float xpluszplus;
		public bool xpluszplusKnown = false;

		public TerrainPiece( int instance_id, float x, float z, float altitude, int texture_id ) {
			this.altitude = altitude;
			this.texture_id = texture_id;
			this.instance_id = instance_id;
			this.x = x;
			this.z = z;
		}
	}
}
