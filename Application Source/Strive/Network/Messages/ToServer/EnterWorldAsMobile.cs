using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for EnterWorldAsMobile.
	/// </summary>
	[Serializable]
	public class EnterWorldAsMobile : IMessage
	{
		public EnterWorldAsMobile( int WorldID, int SpawnID )
		{
			this.WorldID = WorldID;
			this.SpawnID = SpawnID;
		}
		public int WorldID;
		public int SpawnID;
	}
}
