using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Revolution3D8087b;
using DxVBLibA;

namespace Strive.Network.Client
{
	/// <summary>
	/// Summary description for viewer.
	/// </summary>
	public class viewer : System.Windows.Forms.Form
	{

		#region "Public Revolution Fields"

		public R3D_Engine Engine = new R3D_EngineClass();
		public R3D_Control Control = new R3D_ControlClass();
		public R3D_Pipeline Pipeline = new R3D_PipelineClass();
		public R3D_MeshBuilder Meshbuilder = new R3D_MeshBuilderClass();
		public R3D_Camera Camera = new R3D_CameraClass();
		public R3D_Tools Tools = new R3D_ToolsClass();
		public R3D_MDLSystem MdlSystem = new R3D_MDLSystemClass();
		public R3D_SkyDome Skydome = new R3D_SkyDomeClass();
		public R3D_TextureLib TextureLib = new R3D_TextureLibClass();
		public R3D_PolyVox2 PolyVox = new R3D_PolyVox2Class();
		public R3D_Math Math = new R3D_MathClass();

		public R3D_Interface2D Interface2D = new R3D_Interface2DClass();
		public R3D_PowerMonitor PowerMonitor = new R3D_PowerMonitorClass();

		public R3DRENDERTARGET R3DRENDERTARGET_CURRENT;

		public Avatar avatar = new Avatar();

		#endregion

		#region "Strive3d.net support"


		public Hashtable avatars = new Hashtable();
//		StriveConnection currentConnection;
		int startX;
		int startY;
		int startZ;
		bool IsSinglePlayer;
		bool IsRunning;
		bool IsFullScreen;
		public string LastChattedMessage = "";

		public void viewer_Load(object sender, System.EventArgs e)
		{
		}

		public void viewer_Closed(object sender, System.EventArgs e)
		{
			IsRunning = false;
		}

		public void Start(string Server, string CharacterName, string Password, bool IsSinglePlayer, bool IsFullScreen, int x, int y, int z)
		{
			startX = x;
			startY = y;
			startZ = z;

			Thread renderThread = new Thread(new ThreadStart(Render));
			renderThread.Start();
			if(IsFullScreen)
			{
				R3DRENDERTARGET_CURRENT = R3DRENDERTARGET.R3DRENDERTARGET_FULLSCREEN;
			}
			else
			{
				R3DRENDERTARGET_CURRENT = R3DRENDERTARGET.R3DRENDERTARGET_WINDOW;
			}
			
			this.IsSinglePlayer = IsSinglePlayer;
			this.IsFullScreen = IsFullScreen;

			if(!IsSinglePlayer)
			{
//				currentConnection = new StriveConnection(
//					Server, 1337, this
//				);
			
//				Thread sc_thread = new Thread( new ThreadStart( currentConnection.Start ) );
//				sc_thread.Start();
//				currentConnection.SendLogin( CharacterName, Password );
			}
		}

		#endregion
		
		#region "Rendering routines"

		/// <summary>
		/// Main rendering function.  Run in a seperate thread.
		/// </summary>
		public void Render()
		{
			InitialiseEngine();
			InitialisePipeline();
			LoadTerrain();
		
			Pipeline.Renderer_Clear();
			Pipeline.Renderer_Render();
			Pipeline.Renderer_Display();

			IsRunning = true;

			//startX = 0;
			//startY = 0;
			//startZ = -100;

			Camera.SetPosition(startX, startY, startZ);
			avatar.x = startX;
			avatar.y = startY;
			avatar.z = startZ;
			if(!IsSinglePlayer)
			{
//				this.currentConnection.SendPosition(startX, startY, startZ, 0.0);
			}

			MdlSystem.MDL_Load("A", "Bdroid_c.mdl");
			MdlSystem.MDL_SetPointer("A");
			MdlSystem.MDL_SetPosition(0,0,0);
			MdlSystem.MDL_SetRotation(0,90,0);
			MdlSystem.MDL_SequenceSet("walk");

			while(IsRunning)
			{
				Pipeline.Renderer_Clear();


				int elapsedFrames = PowerMonitor.GetElapsedFrames();
				float frameTime = PowerMonitor.GetFrameTime();
				Interface2D.Primitive_DrawText(0, 0, (frameTime / elapsedFrames).ToString() + LastChattedMessage );

				R3DKey R3DKey_Enter = R3DKey.R3DKEY_RETURN;
				R3DKey R3DKey_Escape = R3DKey.R3DKEY_ESCAPE;
				R3DKey R3DW = R3DKey.R3DKEY_W;
				R3DKey R3DA = R3DKey.R3DKEY_A;
				R3DKey R3DS = R3DKey.R3DKEY_S;
				R3DKey R3DD = R3DKey.R3DKEY_D;
				Control.Keyboard_ReceiveKeys();

				if(Control.Keyboard_GetKeyState(ref R3DKey_Enter) && !IsFullScreen)
				{
					string ChatMessage = Microsoft.VisualBasic.Interaction.InputBox("Joor chat message", "Strive3D.NET chat", "", 200, 200);
					if(!IsSinglePlayer)
					{
						System.Diagnostics.Debug.WriteLine("Calling SendChat(\"" + ChatMessage + "\")");
//						this.currentConnection.SendChat(ChatMessage);
					}
				}

				if(Control.Keyboard_GetKeyState(ref R3DW))
				{
					avatar.MoveForward( 1 );					
				}
				if(Control.Keyboard_GetKeyState(ref R3DA))
				{
					avatar.Straffe( -0.5f );
				}
				if(Control.Keyboard_GetKeyState(ref R3DD))
				{
					avatar.Straffe( 0.5f );
				}
				if(Control.Keyboard_GetKeyState(ref R3DS))
				{
					avatar.MoveForward( -0.5f );
				}
				Camera.SetPosition( avatar.x, avatar.y+30, avatar.z );


				int MouseX = 0;
				int MouseY = 0;
				bool MouseButton1 = false;
				bool MouseButton2 = false;
				bool MouseButton3 = false;
				bool MouseButton4 = false;
				Control.Mouse_GetState(ref MouseX, ref MouseY, ref MouseButton1, ref MouseButton2, ref MouseButton3, ref MouseButton4, true);

				Camera.RotateX(MouseY/5);
				Camera.RotateY(MouseX/5);
				float headx=0;
				float heady=0;
				float headz=0;
				Camera.GetRotation(ref headx, ref heady, ref headz);

				avatar.SetHeadingDegrees( heady );
				
				if(MouseX != 0 ||
					MouseY != 0 ||
					Control.Keyboard_GetKeyState(ref R3DS) ||
					Control.Keyboard_GetKeyState(ref R3DA) ||
					Control.Keyboard_GetKeyState(ref R3DW) ||
					Control.Keyboard_GetKeyState(ref R3DD) 
					)
				{
					if(!IsSinglePlayer)
					{
//						this.currentConnection.SendPosition(avatar.x, avatar.y, avatar.z, avatar.GetHeadingDegrees());
						System.Diagnostics.Debug.WriteLine(heady.ToString());
						System.Diagnostics.Debug.WriteLine(MouseX);
					}

				}



				if(Control.Keyboard_GetKeyState(ref R3DKey_Escape))
				{
					IsRunning = false;
					break;
				}

				Pipeline.Renderer_Render();
				Pipeline.Renderer_Display();
			}
//			currentConnection.Stop();
			Engine.TerminateMe();
			this.Close();
		}

		void InitialiseEngine()
		{
			Engine.Inf_SetFieldOfView(50);
			Engine.Inf_SetViewDistance(500);
			Engine.Inf_SetRenderTarget(this.Handle.ToInt32(), ref R3DRENDERTARGET_CURRENT);
			Engine.Inf_ForceResolution(1024, 768, 32);
			Engine.InitializeMe(true);
		}

		void InitialisePipeline()
		{
			Pipeline.SetAmbientLight(255, 255, 255);
			Pipeline.SetBackColor(0, 0, 0);
			Pipeline.SetDithering(true);
			Pipeline.SetSpecular(true);
			Pipeline.SetMipMapping(false);
			Pipeline.SetFillMode(R3DFILLMODE.R3DFILLMODE_SOLID);
			Pipeline.SetShadeMode(R3DSHADEMODE.R3DSHADEMODE_GOURAUD);
			Pipeline.SetTextureFilter(R3DTEXTUREFILTER.R3DTEXTUREFILTER_LINEARFILTER);
			R3DColor col;
			col.r = 50;
			col.g = 50;
			col.b = 50;
			col.a = 0;
			//Pipeline.SetFog(ref col, R3DFOGTYPE.R3DFOGTYPE_PIXELTABLE, R3DFOGMODE.R3DFOGMODE_EXP, 1, 500, 5);
		}

		void LoadTerrain()
		{
//			R3DCOLORKEY R3DCOLORKEY_NONE = R3DCOLORKEY.R3DCOLORKEY_NONE;
//			TextureLib.Texture_Load("GroundTexture", Application.StartupPath + "\\..\\..\\Resources\\Textures\\ground.bmp", ref R3DCOLORKEY_NONE );

//			R3DCOLORKEY R3DCOLORKEY_NONE = R3DCOLORKEY.R3DCOLORKEY_NONE;

//			PolyVox.Scape_Create("GroundTexture", Application.StartupPath + "\\..\\..\\Resources\\Textures\\height.bmp", 100, true, POLYVOXDETAIL.POLYVOXDETAIL_AVERANGE);
			
//			PolyVox.Blender_CreateHeightMap(Application.StartupPath + "\\..\\..\\Resources\\Textures\\height.bmp", R3DTEXTURERESOLUTION.R3DTEXTURERESOLUTION_AUTODETECT);
//			//PolyVox.Scape_Create("GroundT", "Ground 
		}

		void LoadSky()
		{
			R3DCOLORKEY R3DCOLORKEY_BLUE = R3DCOLORKEY.R3DCOLORKEY_BLUE;
			TextureLib.Texture_Load("SkyLeft", Application.StartupPath + "\\..\\..\\Resources\\Textures\\left.bmp", ref R3DCOLORKEY_BLUE);
			TextureLib.Texture_Load("SkyRight", Application.StartupPath + "\\..\\..\\Resources\\Textures\\Right.bmp", ref R3DCOLORKEY_BLUE);
			TextureLib.Texture_Load("SkyFront", Application.StartupPath + "\\..\\..\\Resources\\Textures\\Front.bmp", ref R3DCOLORKEY_BLUE);
			TextureLib.Texture_Load("SkyBack", Application.StartupPath + "\\..\\..\\Resources\\Textures\\Back.bmp", ref R3DCOLORKEY_BLUE);
			TextureLib.Texture_Load("SkyUp", Application.StartupPath + "\\..\\..\\Resources\\Textures\\up.bmp", ref R3DCOLORKEY_BLUE);
			TextureLib.Texture_Load("SkyDown", Application.StartupPath + "\\..\\..\\Resources\\Textures\\down.bmp", ref R3DCOLORKEY_BLUE);
			string SkyDown = "SkyDown";
			Skydome.Cube_Create("SkyLeft", "SkyRight", "SkyFront", "SkyBack", "SkyUp", ref SkyDown, 256, "SkyMaterial");
		}


		#endregion

		#region "Winforms support"

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public viewer()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
			this.Closed += new System.EventHandler(viewer_Closed);

		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// viewer
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(640, 480);
			this.Name = "viewer";
			this.Text = "viewer";
			this.Load += new System.EventHandler(this.viewer_Load);

		}
		#endregion


	}
}
