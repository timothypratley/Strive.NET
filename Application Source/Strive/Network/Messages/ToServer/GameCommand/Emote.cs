using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for Animation.
	/// </summary>
	[Serializable]
	public struct Emote : IMessage	{
		public Emote( EmoteType EmoteID, int MobileID )	{
			this.EmoteID = EmoteID;
			this.MobileID = MobileID;
		}
		public EmoteType EmoteID;
		public int MobileID;	// optional target

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
