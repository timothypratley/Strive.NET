using System;
using System.Configuration;
using System.Net;
using System.Windows.Forms;

using Strive.Multiverse;
using Strive.Network.Client;
using Strive.UI.Forms;
using Strive.Logging;
using Strive.Resources;

namespace Strive.UI
{
	/// <summary>
	/// Summary description for Main.
	/// </summary>
	public class Global
	{
		public Global()
		{
		}

		internal static ServerConnection _serverConnection = new ServerConnection();
		internal static int _myid;
		internal static Game _game;
		public static Log _log = new Log();

		[STAThread]
		static void Main( string[] args ) 
		{

			#region Retrieve config settings for server location

			// Retreive default server:port
			if ( ConfigurationSettings.AppSettings["server"] == null ) {
				throw new ConfigurationException( "server" );
			}
			if ( ConfigurationSettings.AppSettings["port"] == null ) {
				throw new ConfigurationException( "port" );
			}
			if ( ConfigurationSettings.AppSettings["ResourcePath"] == null ) {
				throw new ConfigurationException( "ResourcePath" );
			}
			int port = int.Parse(ConfigurationSettings.AppSettings["port"]);
			string server = ConfigurationSettings.AppSettings["server"];
			string resourcePath = System.Configuration.ConfigurationSettings.AppSettings["ResourcePath"];
			ResourceManager.SetPath( resourcePath );

			#endregion

			#region Connect to server
			try
			{
				_serverConnection.Start(new IPEndPoint(Dns.GetHostByName(server).AddressList[0], port));
			}
			catch(Exception e)
			{
				_log.ErrorMessage(e.ToString());
			}
			#endregion

			Splash splash = new Splash();

			Application.Run(splash);
			
			_serverConnection.Stop();

			Console.ReadLine();

		}

	}
}
