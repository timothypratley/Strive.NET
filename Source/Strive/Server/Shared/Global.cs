using System;
using System.Configuration;

using Strive.Math3D;
using Strive.Logging;

namespace Strive.Server.Shared
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global
	{
		public static Random random = new Random();
		public static DateTime now = DateTime.Now;
		public static Vector3D up = new Vector3D( 0, 1, 0 );
		static string logfilename = ConfigurationSettings.AppSettings["logFileName"];
		public static Log log = new Log( logfilename );
		public static Multiverse.Schema multiverse = Strive.Data.MultiverseFactory.getMultiverse();
		public static World world;
	}
}
