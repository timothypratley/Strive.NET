using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

using Common.Logging;
using Strive.Math3D;
using Strive.Client.WinForms.Engine;
using Strive.Network.Messages;
using Strive.Network.Client;
using Strive.Server.Model;

namespace Strive.Client.WinForms
{
	/// <summary>
	/// Main Entry Point to the game
	/// </summary>
	public class Game
	{

		#region Public Fields

		public static ServerConnection CurrentServerConnection;// = new ServerConnection();
		public static MessageProcessor CurrentMessageProcessor;
		public static Windows.Main CurrentMainWindow;// = new Windows.Main();
		public static bool GameControlMode = false;
        public static int CurrentPlayerID;
		public static DateTime now;
		public static string userName;
		public static string password;
		public static Strive.Network.Messages.NetworkProtocolType protocol;

        //public static Strive.Resources.ResourceManager resources;
        #endregion

        static ILog Log = LogManager.GetCurrentClassLogger();

		[STAThread]
		static void Main(string[] args)	{
			// Configure Resource manager
			string path = System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"];
			if ( path == null ) {
				//throw new System.Configuration.ConfigurationException( "ResourcePath" );
			}
			CurrentMessageProcessor = new MessageProcessor();

			// Initialise required objects
			CurrentServerConnection = new ServerConnection();
			CurrentServerConnection.OnConnect += new ServerConnection.OnConnectHandler( HandleConnect );
			CurrentServerConnection.OnDisconnect += new ServerConnection.OnDisconnectHandler( HandleDisconnect );

			CurrentMainWindow = new Windows.Main();
			CurrentMainWindow.Show();
			//CurrentWorld.InitialiseView( CurrentMainWindow, CurrentMainWindow.RenderTarget, CurrentMainWindow.miniMap.RenderTarget );

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
			Log.Info( "Connecting to " + ServerName + ":" + Port );
			CurrentServerConnection.Start( new IPEndPoint( Dns.GetHostByName( ServerName ).AddressList[0], Port ) );
		}

		public static void Stop() {
			//CurrentWorld.CurrentAvatar = null;
			CurrentServerConnection.Stop();
			//CurrentWorld.Clear();
		}

		public static void HandleConnect() {
			Log.Info( "Connected." );
			CurrentServerConnection.Login(userName, password, protocol);
			password = null;
			CurrentServerConnection.RequestPossessable();
		}

		public static void HandleDisconnect() {
			Log.Info( "Disconnected." );
			password = null;
			Stop();
		}

		public static void GameLoop() {
			// Only get the current time once per iteration.
			// This is faster, and makes time apply uniformly for the update.
			now = DateTime.Now;

			// handle network input from the server
			ProcessOutstandingMessages();
		}

		static void ProcessOutstandingMessages() {
			while(CurrentServerConnection.MessageCount > 0) {
				IMessage m = CurrentServerConnection.PopNextMessage();
				if ( m == null ) break;
				CurrentMessageProcessor.Process( m );
                /*
				if ( CurrentInputProcessor.movementTimer.ElapsedSecondsSoFar() > 1 ) {
					Log.Trace( "Processing messages for more than 1 second." );
					// give the engine a chance to render what we have so far
					break;
				}
                 * */
			}
		}
	}
}
