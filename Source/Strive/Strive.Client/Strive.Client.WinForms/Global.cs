using System;
using System.Windows.Forms;

using Strive.Network.Client;
using Strive.UI.Forms;
using Strive.Logging;
using Strive.Resources;

namespace Strive.UI
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class Global	{
		internal static int _myid;
		internal static Game _game;
		internal static Modules.GameLoop _gameLoop = new Modules.GameLoop();
		public static Log _log = new Log();
		internal static ServerConnection _serverConnection = new ServerConnection();

		[STAThread]
		static void Main( string[] args ) 
		{
			if ( System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"] == null ) {
				throw new System.Configuration.ConfigurationException( "ResourcePath" );
			}
			string path = System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"];
			ResourceManager.SetPath( path );

			_game = new Game();
			Application.Run(_game);
			_serverConnection.Stop();
			_gameLoop.Stop();
		}
	}
}
