using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetMobile.
	/// </summary>
	[Serializable]
	public class TargetMobile : IMessage	{
		public TargetMobile( CommandType CommandID, int MobileID )	{
			this.CommandID = CommandID;
			this.MobileID = MobileID;
		}
		public CommandType CommandID;
		public int MobileID;

		public enum CommandType {
			Possess,
			Summon,
			GiantStrength,
			Backstab
		}
	}
}
