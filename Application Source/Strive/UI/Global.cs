using System;
using System.Windows.Forms;

using Strive.Network.Client;
using Strive.UI.Forms;
using Strive.Logging;

namespace Strive.UI
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class Global	{
		internal static int _myid;
		internal static Game _game;
		public static Log _log = new Log();
		internal static ServerConnection _serverConnection = new ServerConnection();

		[STAThread]
		static void Main( string[] args ) 
		{
			_game = new Game();
			Application.Run(_game);
		}
	}
}
