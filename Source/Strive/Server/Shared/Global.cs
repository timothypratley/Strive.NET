using System;
using Strive.Math3D;

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
		
		// todo: make this less hax
		public static Multiverse.Schema multiverse = Strive.Data.MultiverseFactory.getMultiverse();
		public static World world;
	}
}
