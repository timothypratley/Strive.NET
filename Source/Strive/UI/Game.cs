using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Strive.Common;
using Strive.Math3D;
using Strive.Rendering;
using Strive.Rendering.Textures;
using Strive.Resources;
using Strive.UI.WorldView;
using Strive.UI.Engine;
using Strive.Logging;
using Strive.Network.Messages;
using Strive.Network.Client;
using Strive.Multiverse;

namespace Strive.UI
{
	/// <summary>
	/// Main Entry Point to the game
	/// </summary>
	public class Game
	{

		#region Public Fields

		public static ServerConnection CurrentServerConnection;// = new ServerConnection();
		public static InputProcessor CurrentInputProcessor;
		public static MessageProcessor CurrentMessageProcessor;
		public static Windows.Main CurrentMainWindow;// = new Windows.Main();
		public static World CurrentWorld;// = new Scene();
		public static Logging.Log CurrentLog;// = new Logging.Log();
		public static bool GameControlMode = false;
		public static IEngine RenderingFactory = Strive.Rendering.Activator.GetEngine();
        public static int CurrentPlayerID;
		public static DateTime now;
		public static string userName;
		public static string password;
		public static Strive.Network.Messages.NetworkProtocolType protocol;
		public static ResourceManager resources;

		private static EnumSkill currentGameCommand = EnumSkill.None;
		public static EnumSkill CurrentGameCommand {
			get {
				return currentGameCommand;
			}
			set {
				currentGameCommand = value;
				ITexture texture = resources.GetCursor( (int)currentGameCommand );
				CurrentWorld.RenderingScene.SetCursor( texture );
			}
		}

		#endregion


		[STAThread]
		static void Main(string[] args)	{
			resources = new ResourceManager( RenderingFactory );
			// Configure Resource manager
			string path = System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"];
			if ( path == null ) {
				throw new System.Configuration.ConfigurationException( "ResourcePath" );
			}
			resources.SetPath( path );
			CurrentWorld = new World( resources, RenderingFactory );
			CurrentInputProcessor = new InputProcessor( CurrentWorld );
			CurrentMessageProcessor = new MessageProcessor( CurrentWorld );

			// Initialise required objects
			CurrentServerConnection = new ServerConnection();
			CurrentServerConnection.OnConnect += new ServerConnection.OnConnectHandler( HandleConnect );
			CurrentServerConnection.OnDisconnect += new ServerConnection.OnDisconnectHandler( HandleDisconnect );
			CurrentLog = new Logging.Log();

			CurrentMainWindow = new Windows.Main();
			CurrentMainWindow.Show();
			CurrentWorld.InitialiseView( CurrentMainWindow, CurrentMainWindow.RenderTarget, CurrentMainWindow.miniMap.RenderTarget );

			Windows.ChildWindows.Connection con = new Windows.ChildWindows.Connection();
			con.Show();

			while( !CurrentMainWindow.IsDisposed ) {
				// NB: This is a tight renderloop
				// it will chew cpu
				// TODO: could be made nicer by
				// P/Invoke into the Win32 API and call PeekMessage/TranslateMessage/DispatchMessage.  (Doevents actually does something similar, but you can do this without the extra allocations). 
				Application.DoEvents();
				if ( CurrentServerConnection.isRunning ) {
					GameLoop();
				}
				System.Threading.Thread.Sleep( 0 );
			}

			// must terminate all threads to quit
			CurrentServerConnection.Stop();
		}


		public static void Play(string ServerName, string LoginName, string Password, int Port, Strive.Network.Messages.NetworkProtocolType Protocol, IWin32Window RenderTarget) 
		{
			userName = LoginName;
			password = Password;
			protocol = Protocol;
			CurrentServerConnection.protocol = protocol;
			Log.LogMessage( "Connecting to " + ServerName + ":" + Port );
			CurrentServerConnection.Start( new IPEndPoint( Dns.GetHostByName( ServerName ).AddressList[0], Port ) );
		}

		public static void Stop() {
			CurrentWorld.CurrentAvatar = null;
			CurrentServerConnection.Stop();
			CurrentWorld.Clear();
		}

		public static void HandleConnect() {
			Strive.Logging.Log.LogMessage( "Connected." );
			CurrentServerConnection.Login(userName, password, protocol);
			password = null;
			CurrentServerConnection.RequestPossessable();
		}

		public static void HandleDisconnect() {
			Strive.Logging.Log.LogMessage( "Disconnected." );
			password = null;
			Stop();
		}

		public static void GameLoop() {
			// Only get the current time once per iteration.
			// This is faster, and makes time apply uniformly for the update.
			now = DateTime.Now;

			// handle network input from the server
			ProcessOutstandingMessages();

			CurrentInputProcessor.ProcessPlayerInput();

			// display the new world if the form is maximised
			if(Game.CurrentMainWindow.WindowState != FormWindowState.Minimized)	{
				Game.CurrentWorld.Render();
				// TODO: UMG this hack is because TV3D updates its relative mouse state
				// on every render (ghey) AND their absolute mouse pos is shit.
				CurrentInputProcessor.AccumulateMouse();
				Game.CurrentWorld.RenderMiniMap();
			} else {
				System.Threading.Thread.Sleep(100);
			}
		}

		static void ProcessOutstandingMessages() {
			while(
				CurrentServerConnection.MessageCount > 0
			) {
				IMessage m = CurrentServerConnection.PopNextMessage();
				if ( m == null ) break;
				CurrentMessageProcessor.Process( m );
				if ( CurrentInputProcessor.movementTimer.ElapsedSecondsSoFar() > 1 ) {
					Log.DebugMessage( "Processing messages for more than 1 second." );
					// give the engine a chance to render what we have so far
					break;
				}
			}
		}

	}
}
