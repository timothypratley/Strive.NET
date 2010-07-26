using System;
using System.Configuration;

using Strive.Math3D;

namespace Strive.Server.Logic {
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global {
		public static Random random = new Random();
		public static DateTime now = DateTime.Now;
		public static Vector3D up = new Vector3D( 0, 1, 0 );
		public static Server.Model.Schema modelSchema;
		public static World world;

		public static int world_id;
		public static int port;
		public static string logfilename;
		public static string worldfilename;
		public static string connectionstring;

		public static void ReadConfiguration() {
			// manditory fields
			if ( ConfigurationSettings.AppSettings["world_id"] == null ) {
				throw new ConfigurationException( "world_id" );
			}
			world_id = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["world_id"]);

			if ( ConfigurationSettings.AppSettings["port"] == null ) {
				throw new ConfigurationException( "port" );
			}
			port = int.Parse(ConfigurationSettings.AppSettings["port"]);

			// optional fields
			logfilename = ConfigurationSettings.AppSettings["logFileName"];
			if ( logfilename != null ) {
				//Log.SetLogOutput( logfilename );
			}

			// one and one only of these two should be specified.
			worldfilename = ConfigurationSettings.AppSettings["worldFileName"];
			connectionstring = ConfigurationSettings.AppSettings["connectionString"];
		}
	}
}

