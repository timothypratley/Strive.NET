using System;
using System.Windows.Forms;
using System.Collections;

using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Multiverse;
using Strive.Logging;
using Strive.Math3D;
using Strive.Resources;


namespace Strive.UI.Engine {
	/// <summary>
	/// Summary description for World.
	/// </summary>
	public class World {
		// TODO: refactor into a strongly typed collection?
		public Hashtable physicalObjectInstances = new Hashtable();
		Scene scene = new Scene();
		TerrainCollection terrainPieces;
		public PhysicalObjectInstance CurrentAvatar;
		EnumCameraMode cameraMode = EnumCameraMode.FirstPerson;
		Vector3D cameraHeading;
		Vector3D cameraPosition;

		public World() {
			terrainPieces = new TerrainCollection( scene );
		}

		public void InitialiseView(IWin32Window RenderTarget) {
			scene.DropAll();
			scene.Initialise( RenderTarget, Strive.Rendering.RenderTarget.PictureBox, Resolution.Automatic );
			scene.View.FieldOfView = 60;
			scene.View.ViewDistance = 1000;
			scene.SetLighting( 255 );
			scene.SetFog( 500.0f );
		}

		public void Add( PhysicalObject po ) {
			PhysicalObjectInstance poi = new PhysicalObjectInstance( po );
			physicalObjectInstances.Add( po.ObjectInstanceID, poi );
			scene.Models.Add( poi.model );

			if ( po is Terrain ) {
				Terrain t = (Terrain)po;
				terrainPieces.Add( new TerrainPiece( t.ObjectInstanceID, t.Position.X, t.Position.Z, t.Position.Y, t.ModelID ) );
			}

			//todo: serverside ground level/gravity control
			//instead of clientside.
			po.Position.Y = GroundLevel( po.Position.X, po.Position.Z );

			poi.model.Position = po.Position;
			poi.model.Rotation = po.Rotation;
		}

		public void Remove( int ObjectInstanceID ) {
			terrainPieces.Remove( ObjectInstanceID );
			physicalObjectInstances.Remove( ObjectInstanceID );
			scene.Models.Remove( ObjectInstanceID );
		}

		public PhysicalObjectInstance Find( int ObjectInstanceID ) {
			object o = physicalObjectInstances[ObjectInstanceID];
			if ( o != null ) {
				return o as PhysicalObjectInstance;
			} else {
				return null;
			}
		}

		public void Possess( int ObjectInstanceID ) {
			Object o = physicalObjectInstances[ObjectInstanceID];
			CurrentAvatar = o as PhysicalObjectInstance;
			RepositionCamera();
		}

		public void RepositionCamera() {
			if ( CurrentAvatar == null ) return;

			if ( cameraMode == EnumCameraMode.FirstPerson ) {
				cameraPosition = CurrentAvatar.model.Position.Clone();
				cameraPosition.Y += 20;
				scene.View.Position = cameraPosition;
				scene.View.Rotation = CurrentAvatar.model.Rotation;
				cameraHeading = Helper.GetHeadingFromRotation( CurrentAvatar.model.Rotation );
			} else if ( cameraMode == EnumCameraMode.Chase ) {
				cameraPosition = CurrentAvatar.model.Position.Clone();
				cameraPosition.Y += 100;
				cameraPosition.Z += 100;
				scene.View.Position = cameraPosition;
				scene.View.Rotation = CurrentAvatar.model.Rotation;
				cameraHeading = Helper.GetHeadingFromRotation( CurrentAvatar.model.Rotation );
			} else {
				Log.ErrorMessage( "Unknown camera mode." );
			}
		}

		void ProcessAnimations() {
			// todo: make this mobile based,
			// and only if they are moving.
			foreach ( Model model in scene.Models.Values ) {
				if ( model.ModelFormat == ModelFormat.MD2 ) {
					model.nextFrame();
				}
			}
		}

		public void Render() {
			ProcessAnimations();
			scene.Render();
			foreach ( PhysicalObjectInstance poi in physicalObjectInstances.Values ) {
				if ( poi.physicalObject is Mobile || poi.physicalObject is Item ) {
					//Get the vector between camera and object, put in v1
					//Get the direction vector of the camera (lookat - position normalized) put in v2
					//Compute the Dot product.
					//If Dot(V1, V2) > Cos(FOVInRadian) Then 
					//You can see the object ! 
					//Using FieldOfView of 90degrees,
					//so things offscreen infront will still be labeled.

					/** todo: fix this logic
					 * problem is in cameraHeading I think?
					 * rotation2heading is not purrfect
					Vector3D v1 = poi.model.Position - cameraPosition;
					if ( Vector3D.Dot( v1, cameraHeading ) <= Math.Cos( Math.PI ) ) {
						continue;
					}
					 */

					Vector3D labelPos = new Vector3D(
                        poi.model.Position.X,
                        poi.model.Position.Y+15,
                        poi.model.Position.Z
					);
					Vector2D nameLoc = scene.View.ProjectPoint( labelPos );

					// center the text
					// todo: correct this
					nameLoc.X -= poi.physicalObject.ObjectTemplateName.Length/2;

					// nameDist = namePos - camPos; -> set font size
					// todo: enable tis

					scene.DrawText(nameLoc, poi.physicalObject.ObjectTemplateName );
				}
			}
			scene.Display();
		}

		public void Clear() {
			physicalObjectInstances = new Hashtable();
			scene.DropAll();
		}

		public void SetSky( string texture_name ) {
			scene.SetSky( "sky", texture_name );
		}

		public EnumCameraMode CameraMode {
			get{
				return cameraMode;
			}
			set{
				if ( CurrentAvatar != null && cameraMode != value ) {
					if ( value == EnumCameraMode.FirstPerson ) {
						CurrentAvatar.model.Hide();
					} else {
						CurrentAvatar.model.Show();
					}
					cameraMode = value;
					RepositionCamera();
				} else {
					cameraMode = value;
				}
			}
		}

		int terrainSize = 100;
		public float GroundLevel( float x, float z ) {
			// check every terrain piece, is this point on it?
			foreach ( PhysicalObjectInstance poi in physicalObjectInstances.Values ) {
				// todo: Terrain pieces should be cross index in their own
				// collection for speed.
				if ( !(poi.physicalObject is Terrain) ) continue;

				Terrain t = (Terrain)poi.physicalObject;
				if (
					x >= t.Position.X && x < t.Position.X+terrainSize
					&& z >= t.Position.Z && z < t.Position.Z+terrainSize
				) {
					// w00t on this piece lookup its height
					return poi.physicalObject.Position.Y;
				}
			}

			// ack, this is not on terrain!
			return 0;
		}
	}
}
