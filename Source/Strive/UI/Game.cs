using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Strive.Common;
using Strive.Math3D;
using Strive.Rendering;
using Strive.Resources;
using Strive.UI.WorldView;
using Strive.UI.Engine;
using Strive.Logging;
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
		public static World CurrentWorld;// = new Scene();
		public static Logging.Log CurrentLog;// = new Logging.Log();
		public static bool GameControlMode = false;
		public static IEngine RenderingFactory = Strive.Rendering.Activator.GetEngine();
        public static int CurrentPlayerID;
		public static DateTime now;
		public static string userName;
		public static string password;

		#endregion

		[STAThread]
		static void Main(string[] args)
		{
			// todo: umg refactor this out of existance
			ResourceManager.factory = RenderingFactory;

			// Initialise required objects
			CurrentServerConnection = new ServerConnection();
			CurrentGameLoop = new GameLoop();
			CurrentWorld = new World( RenderingFactory );
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
			CurrentWorld.Clear();
		}


		public static void Play(string ServerName, string LoginName, string Password, int Port, IWin32Window RenderTarget) 
		{
			CurrentServerConnection.Stop();
			CurrentGameLoop.Stop();
			CurrentWorld.InitialiseView( RenderTarget );
			Log.LogMessage( "Connecting to " + ServerName + ":" + Port );
			Game.CurrentServerConnection.OnConnect += new ServerConnection.OnConnectHandler( HandleConnect );
			Game.CurrentServerConnection.OnDisconnect += new ServerConnection.OnDisconnectHandler( HandleDisconnect );
			CurrentServerConnection.Start( new IPEndPoint( Dns.GetHostByName( ServerName ).AddressList[0], Port ) );
			CurrentGameLoop.Start(CurrentServerConnection);
			userName = LoginName;
			password = Password;
		}

		public static void HandleConnect() {
			Strive.Logging.Log.LogMessage( "Connected." );
			CurrentServerConnection.Login(userName, password);
			password = null;
			CurrentServerConnection.RequestPossessable();
		}

		public static void HandleDisconnect() {
			Strive.Logging.Log.LogMessage( "Disconnected." );
			password = null;
		}
	}
}
