using System;

namespace Strive.Network.Messages.ToServer
{
	/// <summary>
	/// Summary description for TargetNone.
	/// </summary>
	[Serializable]
	public class TargetNone : IMessage	{
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
