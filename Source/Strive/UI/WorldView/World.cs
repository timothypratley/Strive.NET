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
using Strive.Common;


namespace Strive.UI.WorldView {
	/// <summary>
	/// Summary description for World.
	/// </summary>
	public class World {
		public Hashtable physicalObjectInstances = new Hashtable();
		public IEngine RenderingEngine;
		public ResourceManager Resources;
		public IScene RenderingScene;
		public TerrainCollection TerrainPieces;
		public PhysicalObjectInstance CurrentAvatar;

		EnumCameraMode cameraMode = EnumCameraMode.FirstPerson;
		Vector3D cameraPosition = new Vector3D( 0, 0, 0 );
		Vector3D cameraRotation = new Vector3D( 0, 0, 0 );
		IViewport renderViewport;
		IViewport miniMapViewport;

		public World( ResourceManager resources, IEngine engine ) {
			Resources = resources;
			RenderingEngine = engine;
		}

		public void InitialiseView( IWin32Window parent, IWin32Window RenderTarget, IWin32Window MiniMapTarget ) {
			RenderingEngine.Initialise( RenderTarget, EnumRenderTarget.PictureBox, Resolution.Automatic );
			RenderingScene = RenderingEngine.CreateScene();
			renderViewport = RenderingEngine.CreateViewport( RenderTarget, "RenderTarget" );
			if ( MiniMapTarget != null ) {
				this.MiniMapTarget = MiniMapTarget;
			}
			//WorldTerrain = RenderingEngine.GetTerrain();
			RenderingScene.SetFog( 500.0f );
			renderViewport.Camera.ViewDistance = 10000;
			TerrainPieces = new TerrainCollection( Resources, RenderingEngine, RenderingScene );
		}

		public IWin32Window MiniMapTarget {
			set {
				if ( miniMapViewport != null ) {
					throw new Exception( "Already have set a minimap" );
				}
				miniMapViewport = RenderingEngine.CreateViewport( value, "Minimap" );
				miniMapViewport.Camera.Rotation = new Vector3D( 90, 0, 0 );
				miniMapViewport.Camera.ViewDistance = 10000;
			}
		}


		public PhysicalObjectInstance Add( PhysicalObject po ) {
			if ( po is Terrain ) {
				Terrain t = (Terrain)po;
				TerrainPieces.Add( t );
				//WorldTerrain.SetHeight( po.Position.X, po.Position.Z, po.Position.Y );
				//WorldTerrain.SetTexture( po.Position.X, po.Position.Z, Resources.GetTexture( po.ResourceID ), po.Rotation.Y );
				return null;
			} else {
				if ( RenderingScene.Models.Contains( po.ObjectInstanceID ) ) {
					Log.WarningMessage( "Trying to add existing PhysicalObject " + po.ObjectInstanceID );
					return (PhysicalObjectInstance)physicalObjectInstances[po.ObjectInstanceID];
				}
				PhysicalObjectInstance poi;
				try {
					poi = new PhysicalObjectInstance( po, Resources );
				} catch ( Exception e ) {
					Log.ErrorMessage( e );
					return null;
				}
				physicalObjectInstances.Add( po.ObjectInstanceID, poi );
				RenderingScene.Models.Add( po.ObjectInstanceID, poi.model );
				poi.model.Position = po.Position;
				poi.model.Rotation = po.Rotation;
				return poi;
			}
		}

		public void Remove( int ObjectInstanceID ) {
			physicalObjectInstances.Remove( ObjectInstanceID );
			RenderingScene.Models.Remove( ObjectInstanceID );
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
				CurrentAvatar.model.Visible = false;
			} else {
				CurrentAvatar.model.Visible = true;
			}
			RepositionCamera();
		}

		public Vector3D CameraPosition {
			set {
				cameraPosition.Set( value );
				renderViewport.Camera.Position = cameraPosition;
				if ( miniMapViewport != null ) {
					cameraPosition.Y += 100;
					miniMapViewport.Camera.Position = cameraPosition;
				}
			}
			get{ return cameraPosition; }
		}

		public Vector3D CameraRotation {
			set {
				cameraRotation.Set( value );
				renderViewport.Camera.Rotation = cameraRotation;
			}
			get{ return cameraRotation; }
		}

		public void RepositionCamera() {
			if ( CurrentAvatar == null ) return;

			if ( cameraMode == EnumCameraMode.FirstPerson ) {
				Vector3D newPos = CurrentAvatar.model.Position.Clone();
				newPos.Y += CurrentAvatar.physicalObject.Height*2F/5F;
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
			Recenter( CurrentAvatar.model.Position.X, CurrentAvatar.model.Position.Y, CurrentAvatar.model.Position.Z );
		}

		const int _max_lod_index = 5;
		const int _first_lod_index = 1;
		void Recenter( float x, float y, float z ) {
			TerrainPieces.Recenter( x, z );
			ArrayList arrayList = new ArrayList(physicalObjectInstances.Keys);
			foreach ( object key in arrayList ) {
				PhysicalObjectInstance poi = (PhysicalObjectInstance)physicalObjectInstances[key];

				float dist = Math.Max( Math.Abs( x-poi.model.Position.X ), Math.Abs( z-poi.model.Position.Z ) );
				dist = Math.Max( dist, Math.Abs( y-poi.model.Position.Y ) );

				// TODO: really big object should probabbly have a bigger objectscoperadius,
				// and be visible from further away.
				// For this to be efficient, you would probabbly want big objects stored in different memory structures.

				if ( dist > Constants.objectScopeRadius*2 ) {
					// TODO: make this area the same as that used by the server,
					// ie: square delimited
					Remove( poi.physicalObject.ObjectInstanceID );
				} else {
					int lod_index = (int)(_first_lod_index + (_max_lod_index-1) * dist / Constants.furthestLOD );
					if ( lod_index > _max_lod_index ) {
						lod_index = _max_lod_index;
					}

					poi.model.SetLOD( (EnumLOD)lod_index );
				}
			}
		}

		/* TODO: probably should make more generic multiple views */
		public void RenderMiniMap() {
			if ( miniMapViewport != null ) {
				miniMapViewport.SetFocus();
				RenderingScene.Clear();
				RenderingScene.RenderAtmosphere();
				TerrainPieces.Render();
				RenderingScene.Render();
				RenderingScene.Display();
			}
		}

		public void Render() {
			renderViewport.SetFocus();
			RenderingScene.Clear();
			RenderingScene.SetTimeOfDay( GetHour() );
			RenderingScene.RenderAtmosphere();
			TerrainPieces.Render();
			RenderingScene.Render();
			RenderingScene.Display();
		}

		public void Clear() {
			physicalObjectInstances.Clear();
			if ( TerrainPieces != null ) TerrainPieces.Clear();
			CurrentAvatar = null;
			if ( RenderingScene != null ) RenderingScene.DropAll();
			Resources.DropAll();
		}

		public void SetSky( ITexture day, ITexture night, ITexture cusp, ITexture sun ) {
			RenderingScene.SetSky( day, night, cusp, sun );
		}

		public void SetClouds( ITexture texture ) {
			RenderingScene.SetClouds( texture );
		}

		DateTime baseWorldTime = DateTime.Now;
		DateTime localTimestamp = DateTime.Now;
		public void SetTime( DateTime worldTime ) {
			baseWorldTime = worldTime;
			localTimestamp = DateTime.Now;
		}

		public float GetHour() {
			DateTime worldNow = GetWorldTime();
			return ((float)(worldNow.Ticks%TimeSpan.TicksPerDay)/TimeSpan.TicksPerHour);
		}

		// TODO: TimeSpan worldTimeOffset = DateTime.Parse("20000101") - DateTime.Parse("00000101");
		TimeSpan worldTimeOffset = new TimeSpan(0);
		const long worldTimeRatio = 960;
		public DateTime GetWorldTime() {
			TimeSpan ts = new TimeSpan((DateTime.Now - localTimestamp).Ticks*worldTimeRatio);  // time elapsed since last sync
			DateTime worldNow = (baseWorldTime - worldTimeOffset + ts);
			return worldNow;
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
							CurrentAvatar.model.Visible = false;
						} else {
							CurrentAvatar.model.Visible = true;
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
