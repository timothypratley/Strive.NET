using System;

using Strive.Resources;
using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Math3D;

namespace Strive.UI.Engine
{
	/// <summary>
	/// Summary description for TerrainPieceModel.
	/// </summary>
	public class TerrainPieceModel : TerrainPiece
	{
		Model model;
		Scene scene;

		public TerrainPieceModel( Scene scene, int instance_id, float x, float z, float altitude, int texture_id ) : base ( instance_id, x, z, altitude, texture_id ) 
		{
			this.scene = scene;
		}

		public override void Display() {
			// see if we need to do anything
			Update();
			if ( !dirty ) {
				return;
			}
											 
			if ( model != null ) {
				scene.Models.Remove( model.Key );
			} else {
				ResourceManager.LoadTexture( texture_id );
			}
			model = Model.CreatePlane(
				instance_id.ToString(),
				new Vector3D( x, altitude_xminuszminus, z ),
				new Vector3D( x+100, altitude_xpluszminus, z ),
				new Vector3D( x+100, altitude_xpluszplus, z+100 ),
				new Vector3D( x, altitude_xminuszplus, z+100 ),
				texture_id.ToString(),
				"" );
			scene.Models.Add( model );

			dirty = false;
		}
	}
}
