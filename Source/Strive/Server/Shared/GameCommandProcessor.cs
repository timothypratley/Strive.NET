using System;
using Strive.Network.Server;

namespace Strive.Server.Shared
{
	/// <summary>
	/// Summary description for GameCommandProcessor.
	/// </summary>
	public class GameCommandProcessor
	{
		public static void ProcessTargetNone( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetNone message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetNone.CommandType.Depossess:
					Global.log.LogMessage( "Deposses" );
				    break;
				default:
					Global.log.WarningMessage( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetAny( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetAny message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetAny.CommandType.Attack:
					Global.log.LogMessage( "Attack" );
					break;
				default:
					Global.log.WarningMessage( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetMobile( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetMobile message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetMobile.CommandType.Backstab:
					Global.log.LogMessage( "Backstab" );
					//Skills.Backstab( client, message.MobileID );
					break;
				default:
					Global.log.WarningMessage( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetItem( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetItem message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetItem.CommandType.Drop:
					Global.log.LogMessage( "Drop" );
					break;
				default:
					Global.log.WarningMessage( "Unknown CommandID " + message.CommandID );
					break;
			}
		}
	}
}
