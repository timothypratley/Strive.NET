using System;
using Revolution3D8088c;

namespace Strive.Rendering.Terrain
{
	/// <summary>
	/// Summary description for Landscape.
	/// </summary>
	public class Landscape
	{
		public static void LoadLandscape( string name, string filename ) {
			Interop._instance.PolyVox.Scape_Create( name, filename, 100, true, POLYVOXDETAIL.POLYVOXDETAIL_LOW );
			R3DVector3D scale = new R3DVector3D();
			scale.x = 1;
			scale.y = 1;
			scale.z = 1;
			Interop._instance.PolyVox.Scape_SetScale( ref scale );
			Strive.Rendering.Textures.Texture.LoadTexture( "landtex", filename );
			Interop._instance.PolyVox.Scape_SetTexture( 0, "landtex", R3DLAYERCONFIG.R3DLAYERCONFIG_COLOR );
		}
	}
}
