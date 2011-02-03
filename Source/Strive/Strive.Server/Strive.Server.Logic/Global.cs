using System;
using System.Configuration;
using System.Windows.Media.Media3D;

using Common.Logging;

using Strive.Common;

namespace Strive.Server.Logic
{
    /// <summary>
    /// Summary description for Global.
    /// </summary>
    public class Global
    {
        public static Random Rand = new Random();
        public static DateTime Now = DateTime.Now;
        public static Vector3D Up = new Vector3D(0, 1, 0);
        public static Server.Model.Schema ModelSchema;
        public static World World;

        public static int WorldID;
        public static int Port;
        public static string LogFilename;
        public static string WorldFilename;
        public static string ConnectionString;

        static ILog Log = LogManager.GetCurrentClassLogger();

        public static void ReadConfiguration()
        {
            // manditory fields
            string s = ConfigurationManager.AppSettings["WorldID"];
            if (s == null)
            {
                Log.Error("world_id missing in configuration");
                WorldID = 1;
            }
            else
            {
                WorldID = int.Parse(s);
            }

            s = ConfigurationManager.AppSettings["Port"];
            if (s == null)
            {
                Log.Error("Port missing in configuration");
                Port = Constants.DefaultPort;
            }
            else
            {
                Port = int.Parse(s);
            }

            // optional fields
            LogFilename = ConfigurationManager.AppSettings["LogFileName"];

            // one and one only of these two should be specified.
            WorldFilename = ConfigurationManager.AppSettings["WorldFileName"];
            ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        }
    }
}

