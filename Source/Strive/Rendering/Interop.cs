using System;
using R3D089_VBasic;
using DxVBLibA;

namespace Strive.Rendering
{
	/// <summary>
	/// Interop layer that talks to underlying engine
	/// </summary>
	/// <remarks>Implemented in terms of Revolution3D.  Revolution seems to use shared memory to commmunicate between modules, hence the swag of internal static fields.</remarks>
	internal sealed class Interop
	{
		/// <summary>
		/// Revolution3D engine
		/// </summary>
		internal R3D_Engine Engine = new R3D_EngineClass();
		/// <summary>
		/// Revolution3D control, for keyboard, mouse
		/// </summary>
		internal R3D_Control Control = new R3D_ControlClass();
		/// <summary>
		/// Revolution3D rendering pipeline
		/// </summary>
		internal R3D_Pipeline Pipeline = new R3D_PipelineClass();
		/// <summary>
		/// Revolution3D Meshbuilder
		/// </summary>
		internal R3D_MeshBuilder2 Meshbuilder = new R3D_MeshBuilder2Class();
		/// <summary>
		/// Revolution3D cameras
		/// </summary>
		internal R3D_Cameras Cameras = new R3D_CamerasClass();
		/// <summary>
		/// Revolution3D tools
		/// </summary>
//		internal R3D_Tools Tools = new R3D_ToolsClass();
		/// <summary>
		/// Revolution3D model system (MDL specific)
		/// </summary>
		internal R3D_MD2System MdlSystem = new R3D_MD2SystemClass();
		/// <summary>
		/// Revolution3D skydome
		/// </summary>
		internal R3D_SkyDome2 Skydome = new R3D_SkyDome2Class();
		/// <summary>
		/// Revolution3D texture library
		/// </summary>
		internal R3D_TextureLib TextureLib = new R3D_TextureLibClass();
		/// <summary>
		/// Revolution3D matrial library
		/// </summary>
		internal R3D_MaterialLib MaterialLib = new R3D_MaterialLib();
		/// <summary>
		/// Revolution3D polyvov terrain class
		/// </summary>
		internal R3D_PolyVox3 PolyVox = new R3D_PolyVox3Class();
		/// <summary>
		/// Revolution3D math class
		/// </summary>
//		internal R3D_Math Math = new R3D_MathClass();
		/// <summary>
		/// Revolution3D 2d drawing class
		/// </summary>
		internal R3D_Interface5D Interface5D = new R3D_Interface5DClass();
		/// <summary>
		/// Revolution3D power monitor
		/// </summary>
		internal R3D_PowerMonitor PowerMonitor = new R3D_PowerMonitorClass();
		/// <summary>
		/// DirectX object
		/// </summary>
		internal DirectX8Class DirectX =  new DirectX8Class();

		internal static R3DCOLORKEY R3DCOLORKEY_NONE = R3DCOLORKEY.R3DCOLORKEY_NONE;

		/// <summary>
		/// Enforces access to only one Interop object
		/// </summary>
		internal static readonly Interop _instance = new Interop();
		/// <summary>
		/// To ensure that this class is not constructed directly
		/// </summary>
		private Interop()
		{
		}
		/// <summary>
		/// Responsible for correctly cleaning up resources consumed by 3d engine
		/// </summary>
		~Interop()
		{
			//TODO: clean up revolution properly to stop read violations
		}

		/// <summary>
		/// Converts Strive.Rendering.RenderTarget instances to the appropriate underlying instance
		/// </summary>
		internal R3DRENDERTARGET this[RenderTarget target]
		{
			get
			{
				switch(target)
				{
					case RenderTarget.Window:
					{
						return R3DRENDERTARGET.R3DRENDERTARGET_WINDOW;
					}
					case RenderTarget.FullScreen:
					{
						return R3DRENDERTARGET.R3DRENDERTARGET_FULLSCREEN;
					}
					case RenderTarget.PictureBox:
					{
						return R3DRENDERTARGET.R3DRENDERTARGET_PICTUREBOX;
					}
				}
				return new R3DRENDERTARGET();
			}
			set
			{
			}
		}
	}
}
