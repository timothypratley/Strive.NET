using System;
using System.Net;
using System.Net.Sockets;

using System.Windows.Forms;
using Strive.Common;
using Strive.Math3D;
using Strive.Rendering;
using Strive.Resources;
using Strive.UI.Engine;

using Strive.Network.Client;

namespace Strive.UI
{
	/// <summary>
	/// Main Entry Point to the game
	/// </summary>
	public class Game
	{

		#region Public Fields

		public static ServerConnection CurrentServerConnection;// = new ServerConnection();
		public static GameLoop CurrentGameLoop;// = new GameLoop();
		public static Windows.Main CurrentMainWindow;// = new Windows.Main();
		public static Scene CurrentScene;// = new Scene();
		public static Logging.Log CurrentLog;// = new Logging.Log();

		public static int CurrentPlayerID;

		#endregion

		[STAThread]
		static void Main(string[] args)
		{
			// Initialise required objects
			CurrentServerConnection = new ServerConnection();
			CurrentGameLoop = new GameLoop();
			CurrentScene = new Scene();
			CurrentLog = new Logging.Log();
			CurrentMainWindow = new Windows.Main();
			// Configure Resource manager
			if ( System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"] == null ) 
			{
				throw new System.Configuration.ConfigurationException( "ResourcePath" );
			}
			string path = System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"];
			ResourceManager.SetPath( path );
			Application.Run(CurrentMainWindow);

			// must terminate all threads to quit
			CurrentGameLoop.Stop();
			CurrentServerConnection.Stop();
			CurrentScene.DropAll();
		}


		public static void Play(string ServerName, string LoginName, string Password, int Port, IWin32Window RenderTarget) 
		{
			CurrentServerConnection.Stop();
			CurrentGameLoop.Stop();
			CurrentScene.DropAll();

			short screenHeight = System.Convert.ToInt16(((PictureBox)RenderTarget).Height);
			short screenWidth = System.Convert.ToInt16(((PictureBox)RenderTarget).Width);

			CurrentScene.Initialise( RenderTarget, Strive.Rendering.RenderTarget.PictureBox, Resolution.Automatic );
			CurrentScene.View.FieldOfView = 60;
			CurrentScene.View.ViewDistance = 20000;
			CurrentScene.View.Position = new Vector3D( 0, 0, 0 );
			CurrentScene.SetLighting( 255 );
			CurrentScene.SetFog( 100.0f );
			CurrentServerConnection.Start( new IPEndPoint( Dns.GetHostByName( ServerName).AddressList[0], Port ) );
			CurrentServerConnection.Send( new Strive.Network.Messages.ToServer.Login( LoginName, Password));
			CurrentGameLoop.Start(CurrentScene, RenderTarget, CurrentServerConnection);
		}
	}
}
