using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	[Serializable]
	public class Emote : IMessage	{
		public int EmoteID;
		public int MobileID;	// optional target
		public Emote(){}
		public Emote( EmoteType EmoteID, int MobileID )	{
			this.EmoteID = (int)EmoteID;
			this.MobileID = MobileID;
		}

		public enum EmoteType {
			Bow,
			Wave,
			Nod,
			Laugh,
			Cheer,
			ShakeHead,
			Point
		}
	}
}
