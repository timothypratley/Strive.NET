using System;

using Strive.Math3D;
using Strive.Rendering.Models;

namespace Strive.Resources
{
	/// <summary>
	/// A quadruply linked list, to store neighbourship relationships
	/// </summary>
	public class TerrainPiece {
		public float x, z;
		public float altitude;
		public int texture_id;
		public int instance_id;

		// neighbours
		public TerrainPiece xplus;
		public TerrainPiece xminus;
		public TerrainPiece zplus;
		public TerrainPiece zminus;

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
			if ( xplus == null || xplus.zplus == null ) count--;

			return (
				altitude
				+ (xplus == null ? 0 : xplus.altitude)
				+ (zplus == null ? 0 : zplus.altitude)
                + (xplus == null || xplus.zplus == null ? 0 : xplus.zplus.altitude)
			) / count;
		}

		public float xpluszminusCornerHeight() {
			int count = 4;
			if ( xplus == null ) count--;
			if ( zminus == null ) count--;
			if ( xplus == null || xplus.zminus == null ) count--;

			return (
				altitude
				+ (xplus == null ? 0 : xplus.altitude)
				+ (zminus == null ? 0 : zminus.altitude)
				+ (xplus == null || xplus.zminus == null ? 0 : xplus.zminus.altitude)
			) / count;
		}

		public float xminuszplusCornerHeight() {
			int count = 4;
			if ( xminus == null ) count--;
			if ( zplus == null ) count--;
			if ( xminus == null || xminus.zplus == null ) count--;

			return (
				altitude
				+ (xminus == null ? 0 : xminus.altitude)
				+ (zplus == null ? 0 : zplus.altitude)
				+ (xminus == null || xminus.zplus == null ? 0 : xminus.zplus.altitude)
			) / count;
		}

		public float xminuszminusCornerHeight() {
			int count = 4;
			if ( xminus == null ) count--;
			if ( zminus == null ) count--;
			if ( xminus == null || xminus.zminus == null ) count--;

			return (
				altitude
				+ (xminus == null ? 0 : xminus.altitude)
				+ (zminus == null ? 0 : zminus.altitude)
				+ (xminus == null || xminus.zminus == null ? 0 : xminus.zminus.altitude)
			) / count;
		}

		public Model CreateModel() {
			ResourceManager.LoadTexture( texture_id );
			return Model.CreatePlane(
				instance_id.ToString(),
				new Vector3D( x-50, xminuszminusCornerHeight(), z-50 ),
				new Vector3D( x+50, xpluszminusCornerHeight(), z-50 ),
				new Vector3D( x+50, xpluszplusCornerHeight(), z+50 ),
				new Vector3D( x-50, xminuszplusCornerHeight(), z+50 ),
				texture_id.ToString(),
				""
			);
		}
	}
}
