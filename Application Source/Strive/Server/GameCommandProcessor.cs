using System;
using Strive.Network.Server;

namespace Strive.Server
{
	/// <summary>
	/// Summary description for GameCommandProcessor.
	/// </summary>
	public class GameCommandProcessor
	{
		public static void ProcessTargetNone( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetNone message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetNone.CommandType.Depossess:
					System.Console.WriteLine( "Deposses" );
				    break;
				default:
					System.Console.WriteLine( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetAny( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetAny message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetAny.CommandType.Attack:
					System.Console.WriteLine( "Attack" );
					break;
				default:
					System.Console.WriteLine( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetMobile( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetMobile message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetMobile.CommandType.Backstab:
					System.Console.WriteLine( "Backstab" );
					//Skills.Backstab( client, message.MobileID );
					break;
				default:
					System.Console.WriteLine( "Unknown CommandID " + message.CommandID );
					break;
			}
		}

		public static void ProcessTargetItem( Client client, Strive.Network.Messages.ToServer.GameCommand.TargetItem message ) {
			switch ( message.CommandID ) {
				case Strive.Network.Messages.ToServer.GameCommand.TargetItem.CommandType.Drop:
					System.Console.WriteLine( "Drop" );
					break;
				default:
					System.Console.WriteLine( "Unknown CommandID " + message.CommandID );
					break;
			}
		}
	}
}
