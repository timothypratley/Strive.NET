using System;
using System.Windows.Forms;
using System.Collections;

using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Multiverse;
using Strive.Logging;
using Strive.Math3D;
using Strive.Resources;


namespace Strive.UI.WorldView {
	/// <summary>
	/// Summary description for World.
	/// </summary>
	public class World {
		public Hashtable physicalObjectInstances = new Hashtable();
		public IEngine RenderingEngine;
		public IScene RenderingScene;
		public TerrainCollection TerrainPieces;
		public PhysicalObjectInstance CurrentAvatar;
		EnumCameraMode cameraMode = EnumCameraMode.FirstPerson;
		Vector3D cameraPosition = new Vector3D( 0, 0, 0 );
		Vector3D cameraRotation = new Vector3D( 0, 0, 0 );

		public World( IEngine engine ) {
			RenderingEngine = engine;
			RenderingScene = engine.CreateScene();
			TerrainPieces = new TerrainCollection( RenderingEngine, RenderingScene );
		}

		public void InitialiseView( IWin32Window RenderTarget ) {
			RenderingEngine.Initialise( RenderTarget, EnumRenderTarget.PictureBox, Resolution.Automatic );
			RenderingScene.View.FieldOfView = 60;
			RenderingScene.View.ViewDistance = 1000;
			RenderingScene.SetLighting( 255 );
			RenderingScene.SetFog( 500.0f );
		}

		public PhysicalObjectInstance Add( PhysicalObject po ) {
			if ( po is Terrain ) {
				Terrain t = (Terrain)po;
				TerrainPieces.Add( new TerrainPiece( t ) );
				return null;
			} else {
				PhysicalObjectInstance poi = new PhysicalObjectInstance( po );
				physicalObjectInstances.Add( po.ObjectInstanceID, poi );
				RenderingScene.Models.Add( po.ObjectInstanceID, poi.model );
				poi.model.Position = po.Position;
				poi.model.Rotation = po.Rotation;
				return poi;
			}
		}

		public void Remove( int ObjectInstanceID ) {
			TerrainPieces.Remove( ObjectInstanceID );
			physicalObjectInstances.Remove( ObjectInstanceID );
			RenderingScene.Models.Remove( ObjectInstanceID.ToString() );
		}

		public void RemoveAll() {
			TerrainPieces.Clear();
			physicalObjectInstances.Clear();
			RenderingScene.Models.Clear();
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
			if ( o == null ) {
				Log.ErrorMessage( "Failed to possess " + ObjectInstanceID );
				return;
			}
			CurrentAvatar = (PhysicalObjectInstance)o;
			if ( cameraMode == EnumCameraMode.FirstPerson ) {
				CurrentAvatar.model.Hide();
			} else {
				CurrentAvatar.model.Show();
			}
			RepositionCamera();
		}

		public Vector3D CameraPosition {
			set {
				cameraPosition.Set( value );
				RenderingScene.View.Position = cameraPosition;
			}
			get{ return cameraPosition; }
		}

		public Vector3D CameraRotation {
			set {
				cameraRotation.Set( value );
				RenderingScene.View.Rotation = cameraRotation;
			}
			get{ return cameraRotation; }
		}

		public void RepositionCamera() {
			if ( CurrentAvatar == null ) return;

			if ( cameraMode == EnumCameraMode.FirstPerson ) {
				Vector3D newPos = CurrentAvatar.model.Position.Clone();
				newPos.Y += CurrentAvatar.physicalObject.Height*2/5;
				CameraPosition = newPos;
				CameraRotation = CurrentAvatar.model.Rotation;
			} else if ( cameraMode == EnumCameraMode.Chase ) {
				Vector3D newPos = CurrentAvatar.model.Position.Clone();
				newPos.Y += 100;
				newPos.Z -= 100;
				CameraPosition = newPos;
				CameraRotation = new Vector3D( 45, 0, 0 );
			} else if ( cameraMode == EnumCameraMode.Free ) {
				// do nothing;
			} else {
				Log.ErrorMessage( "Unknown camera mode." );
			}
		}

		public void Render() {
			RenderingScene.Render();

			// label everything in the world
			// TODO: optimise this, the text writing is a performance hit atm
			foreach ( PhysicalObjectInstance poi in physicalObjectInstances.Values ) {
				if (
					( poi != CurrentAvatar || cameraMode != EnumCameraMode.FirstPerson )
					&& ( poi.physicalObject is Mobile || poi.physicalObject is Item )
				) {
					//Get the vector between camera and object, put in v1
					//Get the direction vector of the camera (lookat - position normalized) put in v2
					//Compute the Dot product.
					//If Dot(V1, V2) > Cos(FOVInRadian) Then 
					//You can see the object ! 
					//Using FieldOfView of 90degrees,
					//so things offscreen infront will still be labeled.

//					Vector3D v1 = poi.model.Position - CameraPosition;
//					if ( Vector3D.Dot( v1, Helper.GetHeadingFromRotation(CameraRotation) ) <= Math.Cos( Math.PI ) ) {
//						continue;
//					}

					Vector3D labelPos = new Vector3D(
                        poi.model.Position.X,
                        poi.model.Position.Y + poi.physicalObject.Height/2 + 2,
                        poi.model.Position.Z
					);

					RenderingScene.DrawText( labelPos, poi.physicalObject.ObjectTemplateName );
				}
			}
			RenderingScene.Display();
		}

		public void Clear() {
			physicalObjectInstances = new Hashtable();
			RenderingScene.DropAll();
		}

		public void SetSky( ITexture texture ) {
			RenderingScene.SetSky( "sky", texture );
		}

		public EnumCameraMode CameraMode {
			get{
				return cameraMode;
			}
			set{
				if ( cameraMode != value ) {
					if ( CurrentAvatar != null ) {
						cameraMode = value;
						if ( cameraMode == EnumCameraMode.FirstPerson ) {
							CurrentAvatar.model.Hide();
						} else {
							CurrentAvatar.model.Show();
						}
						RepositionCamera();
					} else {
						cameraMode = value;
					}
				}
			}
		}
	}
}
