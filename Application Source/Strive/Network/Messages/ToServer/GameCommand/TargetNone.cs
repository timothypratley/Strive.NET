using System;

namespace Strive.Network.Messages.ToServer.GameCommand
{
	/// <summary>
	/// Summary description for TargetNone.
	/// </summary>
	[Serializable]
	public struct TargetNone : IMessage	{
		public TargetNone( CommandType CommandID )	{
			this.CommandID = CommandID;
		}

		public CommandType CommandID;

		public enum CommandType {
			Depossess,
			MassHeal,
			Lava,
			Warcry
		}
	}
}
