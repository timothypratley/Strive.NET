using System;

using Strive.Math3D;

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
		public bool dirty = false;

		// neighbours
		public TerrainPiece xplus;
		public TerrainPiece xminus;
		public TerrainPiece zplus;
		public TerrainPiece zminus;
		public TerrainPiece xpluszplus;
		public TerrainPiece xpluszminus;
		public TerrainPiece xminuszplus;
		public TerrainPiece xminuszminus;

		// Height of each corner
		public float altitude_xpluszplus = 0;
		public float altitude_xpluszminus = 0;
		public float altitude_xminuszplus = 0;
		public float altitude_xminuszminus = 0;

		public TerrainPiece( int instance_id, float x, float z, float altitude, int texture_id ) {
			this.altitude = altitude;
			this.texture_id = texture_id;
			this.instance_id = instance_id;
			this.x = x;
			this.z = z;
		}

		public float xpluszplusCornerHeight() {
			int count = 4;
			if ( xplus == null ) count--;
			if ( zplus == null ) count--;
			if ( xpluszplus == null ) count--;

			return (
				altitude
				+ (xplus == null ? 0 : xplus.altitude)
				+ (zplus == null ? 0 : zplus.altitude)
                + (xpluszplus == null ? 0 : xpluszplus.altitude)
			) / count;
		}

		public float xpluszminusCornerHeight() {
			int count = 4;
			if ( xplus == null ) count--;
			if ( zminus == null ) count--;
			if ( xpluszminus == null ) count--;

			return (
				altitude
				+ (xplus == null ? 0 : xplus.altitude)
				+ (zminus == null ? 0 : zminus.altitude)
				+ (xpluszminus == null ? 0 : xpluszminus.altitude)
			) / count;
		}

		public float xminuszplusCornerHeight() {
			int count = 4;
			if ( xminus == null ) count--;
			if ( zplus == null ) count--;
			if ( xminuszplus == null ) count--;

			return (
				altitude
				+ (xminus == null ? 0 : xminus.altitude)
				+ (zplus == null ? 0 : zplus.altitude)
				+ (xminuszplus == null ? 0 : xminuszplus.altitude)
			) / count;
		}

		public float xminuszminusCornerHeight() {
			int count = 4;
			if ( xminus == null ) count--;
			if ( zminus == null ) count--;
			if ( xminuszminus == null ) count--;

			return (
				altitude
				+ (xminus == null ? 0 : xminus.altitude)
				+ (zminus == null ? 0 : zminus.altitude)
				+ (xminuszminus == null ? 0 : xminuszminus.altitude)
			) / count;
		}

		public void Update() {
			float f;
			f = xminuszminusCornerHeight();
			if ( altitude_xminuszminus != f ) {
				altitude_xminuszminus = f;
				dirty = true;
			}
			f = xminuszplusCornerHeight();
			if ( altitude_xminuszplus != f ) {
				altitude_xminuszplus = f;
				dirty = true;
			}
			f = xpluszminusCornerHeight();
			if ( altitude_xpluszminus != f ) {
				altitude_xpluszminus = f;
				dirty = true;
			}
			f = xpluszplusCornerHeight();
			if ( altitude_xpluszplus != f ) {
				altitude_xpluszplus = f;
				dirty = true;
			}
		}

		public virtual void Display() {
		}
	}
}
