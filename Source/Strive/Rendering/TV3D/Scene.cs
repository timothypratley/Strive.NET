using System;
using System.Threading;
using System.Windows.Forms;

using Strive.Rendering;
using Strive.Rendering.Cameras;
using Strive.Rendering.Models;
using Strive.Rendering.Textures;
using Strive.Rendering.TV3D.Models;
using Strive.Math3D;
using TrueVision3D;

namespace Strive.Rendering.TV3D
{

	/// <summary>
	/// Represent a Scene.  A scene contains everything that will be rendered.
	/// </summary>
	public class Scene : IScene
	{
		#region "Private fields"		
		// Singleton support
		private static bool _constructed = false;
		private bool _isRendering = false;
		private ModelCollection _models = new ModelCollection();
		private Cameras.CameraCollection _views = new Cameras.CameraCollection();
		#endregion

		#region "Constructors"
		/// <summary>
		/// Creates a new Scene
		/// </summary>
		public Scene()
		{
			// Singleton support
			if(Scene._constructed)
			{
				throw new SceneAlreadyExistsException();
			}			

			Scene._constructed = true;

		}
		/// <summary>
		/// Destuctor for Scene.  Unsets static _constructed field
		/// </summary>
		~Scene()
		{
			Scene._constructed = false;
		}
		#endregion

		#region "Methods"
		public void DropAll() {
			_models = new ModelCollection();
			_views = new Cameras.CameraCollection();
		}

		public void SetSky( ITexture texture ) {
			Engine.Atmosphere.SkyBox_SetDistance( 1000 );
			Engine.Atmosphere.SkyBox_SetTexture( texture.ID,texture.ID,texture.ID,texture.ID,texture.ID,texture.ID );
			Engine.Atmosphere.SkyBox_Enable(true, true);
		}

		public void SetClouds( ITexture texture ) {
			// TODO: the fact that its a dds means our current resource manager
			// wont load it. need to make resources more flexible... in the
			// meantime its hardcoded ofc.
			int tex_id = Engine.TexFactory.LoadTexture(@"C:\TV3DSDK\Media\cloud1.dds", "Clouds",-1 ,-1 , CONST_TV_COLORKEY.TV_COLORKEY_BLACK,true,true);
			Engine.Land.InitClouds( tex_id, CONST_TV_LAND_CLOUDMODE.TV_CLOUD_MOVE, 250f, 1, 1, 2f, 2f, 1024f);
			Engine.Land.SetCloudVelocity(1, 0.01f, 0.01f);
		}

		public void SetLighting( short level ) {
		}

		public void SetFog( float level ) {
		}

		public void DrawText( Vector3D location, string message ) {
			Engine.Screen2DText.TextureFont_DrawBillboardText( message, location.X, location.Y, location.Z, Engine.Gl.RGBA(0.0f, 1.0f, 1.0f, 0.8f), Engine.FontIndex, 1, 1 );

			/*
			Engine.Screen2DText.ACTION_BeginText();
			Engine.Screen2DText.NormalFont_DrawText( message, (int)location.X, (int)location.Y, Engine.Gl.RGBA(1f, 0f, 1f, 1f), "TV" );
			Engine.Screen2DText.ACTION_EndText();
			*/
		}

		/// <summary>
		/// Public rendering routine
		/// </summary>
		/// <remarks>This method renders the scene into video memory</remarks>
		public void Render()
		{
			try	{
				Engine.TV3DEngine.Clear( false );
			} catch(Exception e) {
				throw new RenderingException("Call to 'Clear()' failed", e);
			}
			try {
				// render the atmosphere
				Engine.Atmosphere.Atmosphere_Render();

				// for us, land only contains the clouds atm
				Engine.Land.Render(false, false);

				// render static models
				Engine.TV3DScene.RenderAllMeshes( false );

				// render character models and object labels
				Engine.Screen2DText.ACTION_BeginText();
				foreach( IModel m in _models.Values ) {
					if ( m is Actor ) {
						((Actor)m).Render();
					}
					if ( m.Visible && m.Label != null ) {
						//Get the vector between camera and object, put in v1
						//Get the direction vector of the camera (lookat - position normalized) put in v2
						//Compute the Dot product.
						//If Dot(V1, V2) > Cos(FOVInRadian) Then 
						//You can see the object ! 
						//Using FieldOfView of 90degrees,
						//so things offscreen infront will still be labeled.
						
						Vector3D v1 = m.Position - View.Position;
						if ( Vector3D.Dot( v1, Helper.GetHeadingFromRotation(View.Rotation) ) <= Math.Cos( View.FieldOfView * Math.PI / 180 ) ) 
						{
							continue;
						}

						Vector3D labelPos = new Vector3D(
							m.Position.X,
							m.Position.Y + m.Height/2 + 2,
							m.Position.Z
							);

						DrawText( labelPos, m.Label );
					}
				}
				Engine.Screen2DText.ACTION_EndText();
			} catch(Exception e) {
				throw new RenderingException("Call to 'Render()' failed with '" + e.ToString() + "'", e);
			}

			//#if DEBUG
			//R3DColor black = new R3DColor();
			//black.b = 255;
			//black.r = 255;
			//black.g = 255;
			//EEERRR setting the draw color fails to write text in 89
			//Engine.Interface5D.Primitive_SetDrawColor(ref black);
	        //ngine.Screen2D.DrawText(ref zero, "Fp/S: " + Engine.PowerMonitor.lGetFramesPerSecond().ToString() +
//                                                            ", Vertices: " + Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame().ToString() + 
                                                            //", Verts/Sec:  " + (Engine.PowerMonitor.lGetFramesPerSecond() * Engine.PowerMonitor.lGetNumVerticesPerSinceLastFrame()).ToString() );
//			Engine.Interface2D.Primitive_DrawText(0,0, (Engine.PowerMonitor.lGetFramesPerSecond()).ToString());
//#endif

			/*
			try
			{
				Engine.Pipeline.Renderer_Display();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed with '" + e.ToString() + "'", e);
			}
			*/
		}

		/// <summary>
		/// Displays the rendered screen
		/// </summary>
		public void Display()
		{
			try
			{
				Engine.TV3DEngine.RenderToScreen();
			}
			catch(Exception e)
			{
				throw new RenderingException("Call to 'Display()' failed", e);
			}
		}

		public int RayCollision(
			Vector3D start_point, Vector3D end_point, int collision_type
		) {
			return 1;
		}

		public IModel MousePick( int x, int y ) {
			DxVBLibA.D3DVECTOR dxo = new DxVBLibA.D3DVECTOR();
			DxVBLibA.D3DVECTOR dxd = new DxVBLibA.D3DVECTOR();
			Engine.Gl.MousePickVector( x, y, ref dxo, ref dxd);
			TV_COLLISIONRESULT cr = new TV_COLLISIONRESULT();
			if ( Engine.TV3DScene.AdvancedCollision( ref dxo, ref dxd, ref cr, CONST_TV_OBJECT_TYPE.TV_COLLIDE_MESH | CONST_TV_OBJECT_TYPE.TV_COLLIDE_ACTOR, CONST_TV_TESTTYPE.TV_TESTTYPE_ACCURATETESTING, true) ) {
				// TODO: don't loop through, a userdata field?
				foreach ( IModel m in Models.Values ) {
					if (
						(cr.collidedobjecttype == 1 && (m is IActor) && m.ID == cr.MeshID)
						|| (cr.collidedobjecttype != 1 && !(m is IActor) && m.ID == cr.MeshID)
					) {
						return m;
					}
				}
			}
			return null;
		}

		public ICameraCollection CameraCollection {
			get { return _views; }
		}

		#endregion
 
		#region "Properties"
		/// <summary>
		/// Indiactes whether the scene is being rendered
		/// </summary>
		public bool IsRendering
		{
			get
			{
				return _isRendering;
			}
			set
			{
				_isRendering = value;
			}
		}

		/// <summary>
		/// Model collection
		/// </summary>
		public IModelCollection Models
		{
			get
			{
				return _models;
			}
		}

		/// <summary>
		/// Returns the View of the current scene.
		/// </summary>
		public ICameraCollection Views
		{
			get
			{
				return _views;
			}
		}

		/// <summary>
		/// Returns the default view
		/// </summary>
		public ICamera View
		{
			get
			{
				if(!Views.Contains(EnumCommonCameraView.Default.ToString()))
				{
					return Views.CreateCamera( EnumCommonCameraView.Default );
				} else {
					return (ICamera)this.Views[EnumCommonCameraView.Default.ToString()];
				}
			}
		}
		

		#endregion


	}
}
