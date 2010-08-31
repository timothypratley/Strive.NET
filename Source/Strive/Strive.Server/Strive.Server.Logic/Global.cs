using System;
using System.Configuration;

using Strive.Math3D;
using Common.Logging;

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

        static ILog Log = LogManager.GetCurrentClassLogger();

		public static void ReadConfiguration() {
			// manditory fields
            if (ConfigurationManager.AppSettings["world_id"] == null)
            {
                Log.Error("world_id missing in configuration");
                world_id = 0;
            }
            else
            {
                world_id = int.Parse(ConfigurationManager.AppSettings["world_id"]);
            }

            if (ConfigurationManager.AppSettings["port"] == null)
            {
                Log.Error("port missing in configuration");
                port = 1337;
            }
            else
            {
                port = int.Parse(ConfigurationManager.AppSettings["port"]);
            }

			// optional fields
			logfilename = ConfigurationManager.AppSettings["logFileName"];
			if ( logfilename != null ) {
				//Log.SetLogOutput( logfilename );
			}

			// one and one only of these two should be specified.
			worldfilename = ConfigurationManager.AppSettings["worldFileName"];
			connectionstring = ConfigurationManager.AppSettings["connectionString"];
		}
	}
}

