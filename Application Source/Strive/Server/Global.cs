using System;
using Strive.Math3D;

namespace Strive.Server
{
	/// <summary>
	/// Summary description for Global.
	/// </summary>
	public class Global
	{
		public static Random random = new Random();
		public static DateTime now = DateTime.Now;
		public static Vector3D up = new Vector3D( 0, 1, 0 );
	}
}
