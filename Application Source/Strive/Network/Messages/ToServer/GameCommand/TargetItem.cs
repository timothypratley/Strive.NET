using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetItem.
	/// </summary>
	[Serializable]
	public struct TargetItem : IMessage	{
		public TargetItem( CommandType CommandID, int ItemID )	{
			this.CommandID = CommandID;
			this.ItemID = ItemID;
		}
		public CommandType CommandID;
		public int ItemID;

		public enum CommandType {
			Curse,
			Enchant,
			Sharpen,
			Drop,
			Get,
			Wear
		}
	}
}
