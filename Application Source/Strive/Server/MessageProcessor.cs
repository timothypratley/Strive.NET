using System;
using System.Collections;
using System.Threading;
using System.Net;

using Strive.Network.Messages;
using Strive.Network.Server;
using Strive.Math3D;

namespace Strive.Server {
	public class MessageProcessor {
		World world;
		Listener listener;

		public MessageProcessor(
			World world, Listener listener
		) {
			this.world = world;
			this.listener = listener;
		}

		public void ProcessNextMessage() {
			ClientMessage clientMessage = (ClientMessage)listener.PopNextMessage();
			ProcessMessage( clientMessage.Client, clientMessage.Message );
		}

		public void ProcessMessage( Client client, IMessage message ) {
			if ( !client.Authenticated ) {
				// new connection... only allow login
				if ( message is Network.Messages.ToServer.Login ) {
					ProcessLoginMessage(
						client, (Network.Messages.ToServer.Login)message
					);
				} else {
					Console.WriteLine( "Non-login message " + message.GetType() + " from " + client.EndPoint );
				}
				return;
			}

			if ( client.Avatar == null ) {
				// no character selected yet... only allow posses
				if ( message is Network.Messages.ToServer.EnterWorldAsMobile ) {
					ProcessEnterWorldAsMobile( client, (Network.Messages.ToServer.EnterWorldAsMobile)message );
				} else {
					Console.WriteLine( "ERROR: Non-posses message " + message.GetType() + " from " + client.EndPoint );
				}
				return;
			}

			// normal message for logged in user
			if ( message is Network.Messages.ToServer.Position ) {
				ProcessPositionMessage( client, (Network.Messages.ToServer.Position)message );
			} else if ( message is Network.Messages.ToServer.GameCommand.Communication ) {
				ProcessChatMessage( client, (Network.Messages.ToServer.GameCommand.Communication)message );
			} else if ( message is Network.Messages.ToServer.GameCommand.TargetAny ) {
				GameCommandProcessor.ProcessTargetAny( client, (Network.Messages.ToServer.GameCommand.TargetAny)message );
			} else {
				System.Console.WriteLine(
					"ERROR: Unknown message " + message.GetType()
				);
			}
		}

		void ProcessLoginMessage(
			Client client, Strive.Network.Messages.ToServer.Login loginMessage
			) {
			if (
				world.UserLookup( loginMessage.username, loginMessage.password )
			) {
				System.Console.WriteLine(
					"User " + loginMessage.username + " logged in"
					);
				client.AuthenticatedUsername = loginMessage.username;
			} else {
				System.Console.WriteLine(
					"Login failed for username " + loginMessage.username
				);
			}
	}

		void ProcessEnterWorldAsMobile( Client client, Strive.Network.Messages.ToServer.EnterWorldAsMobile message ) {
			// try to load the character
			MobileAvatar a = world.LoadMobile( message.SpawnID );
			if ( a == null ) {
				Console.WriteLine( "ERROR: Character "+message.SpawnID+" not found." );
				return;
			}
			a.client = client;
			client.Avatar = a;

			// try to add the character to the world
			try {
				world.Add( client.Avatar );
			} catch {
				// NTR: I don't think this is entirely true.
				Console.WriteLine( "ERROR: Character "+message.SpawnID+" already logged in." );
				client.Avatar = null;
				return;
			}
			// tell the client the characters position
			client.Send( new Strive.Network.Messages.ToClient.AddPhysicalObject( client.Avatar ) );
		}

		void ProcessPositionMessage(
			Client client, Strive.Network.Messages.ToServer.Position message
		) {
			world.Relocate( client.Avatar,
				new Vector3D( message.position_x, message.position_y, message.position_z )
			);
		}

		void ProcessChatMessage(
			Client client, Strive.Network.Messages.ToServer.GameCommand.Communication message
		) {
			foreach ( Client c in listener.Clients.Values ) {
				if (  c == client ) continue;
				c.Send(
					new Network.Messages.ToClient.Communication( client.Avatar.ObjectTemplateName, message.message, message.communicationType )
				);
			}
		}

		public void CleanupDeadConnections() {
			// Make a list of dead connections and remove them all
			ArrayList al = new ArrayList();
			foreach ( Client c in listener.Clients.Values ) {
				if ( (DateTime.Now - c.LastMessageTimestamp).Seconds > 60 ) {
					al.Add( c.EndPoint );
				}
			}
			foreach ( IPEndPoint ep in al ) {
				Console.WriteLine( "Dropping connection to "+ep+" due to inactivity" );
				listener.Clients.Remove( ep );
			}
		}
	}
}
