using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for EnterWorldAsMobile.
	/// </summary>
	[Serializable]
	public class EnterWorldAsMobile : IMessage
	{
		public EnterWorldAsMobile(){}
		public EnterWorldAsMobile( int InstanceID )
		{
			this.InstanceID = InstanceID;
		}
		public int InstanceID;
	}
}
