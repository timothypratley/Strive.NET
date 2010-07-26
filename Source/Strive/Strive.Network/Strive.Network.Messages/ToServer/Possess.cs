using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Chat.
	/// </summary>
	[Serializable]
	public struct Possess : IMessage	{
		public Possess( int object_id )	{
			this.object_id = object_id;
		}

		public int object_id;
	}
}
