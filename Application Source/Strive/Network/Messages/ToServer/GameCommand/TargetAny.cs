using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetAny.
	/// </summary>
	[Serializable]
	public class TargetAny : IMessage	{
		public TargetAny( CommandType CommandID, int PhysicalObjectID )	{
			this.CommandID = CommandID;
			this.PhysicalObjectID = PhysicalObjectID;
		}
		public CommandType CommandID;
		public int PhysicalObjectID;

		public enum CommandType {
			Attack,
			FairyFire
		}
	}
}
