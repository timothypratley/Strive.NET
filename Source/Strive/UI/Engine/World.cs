using System;
using System.Windows.Forms;
using System.Collections;

using Strive.Rendering;
using Strive.Rendering.Models;
using Strive.Multiverse;
using Strive.Logging;
using Strive.Math3D;

namespace Strive.UI.Engine {
	/// <summary>
	/// Summary description for World.
	/// </summary>
	public class World {
		public Hashtable physicalObjectInstances = new Hashtable();
		Scene scene = new Scene();
		public PhysicalObjectInstance CurrentAvatar;
		EnumCameraMode cameraMode = EnumCameraMode.FirstPerson;

		public World() {
			//
			// TODO: refactor into a strongly typed collection?
			//
		}

		public void InitialiseView(IWin32Window RenderTarget) {
			scene.DropAll();
			scene.Initialise( RenderTarget, Strive.Rendering.RenderTarget.PictureBox, Resolution.Automatic );
			scene.View.FieldOfView = 60;
			scene.View.ViewDistance = 1000;
			scene.SetLighting( 255 );
			scene.SetFog( 500.0f );
		}

		public void Add( PhysicalObject po, Vector3D position, Vector3D rotation ) {
			PhysicalObjectInstance poi = new PhysicalObjectInstance( po );
			physicalObjectInstances.Add( po.ObjectInstanceID, poi );
			scene.Models.Add( poi.model );
			poi.model.Position = position;
			poi.model.Rotation = rotation;
			poi.model.label = po.ObjectTemplateName;
		}

		public void Remove( int ObjectInstanceID ) {
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
				scene.View.Position = CurrentAvatar.model.Position;
				scene.View.Rotation = CurrentAvatar.model.Rotation;
			} else if ( cameraMode == EnumCameraMode.Chase ) {
				Vector3D camPos = CurrentAvatar.model.Position;
				camPos.Y += 100;
				camPos.Z += 100;
				scene.View.Position = camPos;
				scene.View.Rotation = CurrentAvatar.model.Rotation;				
			} else {
				Log.ErrorMessage( "Unknown camera mode." );
			}
		}

		void ProcessAnimations() {
			foreach ( Model model in scene.Models.Values ) {
				if ( model.ModelFormat == ModelFormat.MD2 ) {
					model.nextFrame();
				}
			}
		}

		public void Render() {
			ProcessAnimations();
			scene.Render();
			// label everything
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
	}
}
