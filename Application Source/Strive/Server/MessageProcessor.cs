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
				ProcessPositionMessage( client, message as Network.Messages.ToServer.Position );
			} else if ( message is Network.Messages.ToServer.GameCommand.Communication ) {
				ProcessChatMessage( client, message as Network.Messages.ToServer.GameCommand.Communication );
			} else if ( message is Network.Messages.ToServer.GameCommand.TargetAny ) {
				GameCommandProcessor.ProcessTargetAny( client, message as Network.Messages.ToServer.GameCommand.TargetAny );
			} else if ( message is Network.Messages.ToServer.GameCommand.Attack ) {
				ProcessAttackMessage( client, message as Network.Messages.ToServer.GameCommand.Attack );
			} else if ( message is Network.Messages.ToServer.GameCommand.Flee ) {
				ProcessFleeMessage( client, message as Network.Messages.ToServer.GameCommand.Flee );
			} else if ( message is Network.Messages.ToServer.ReloadWorld ) {
				ProcessReloadWorldMessage( client, message as Network.Messages.ToServer.ReloadWorld );
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
			Strive.Network.Messages.ToClient.CanPossess canPossess = new Strive.Network.Messages.ToClient.CanPossess(
				world.getPossessable( loginMessage.username )
			);
			client.Send( canPossess );
	}

		void ProcessEnterWorldAsMobile( Client client, Strive.Network.Messages.ToServer.EnterWorldAsMobile message ) {
			MobileAvatar a;
			if ( world.physicalObjects.ContainsKey( message.InstanceID ) ) {
				// reconnected
				// simply replace existing connection with the new
				a = (MobileAvatar)world.physicalObjects[message.InstanceID];
				if ( a.client == client ) {
					System.Console.WriteLine( "ERROR: a client " + client.ToString() + " attempted to take control of the same mobile " + a.ObjectInstanceID + " twice... ignoring request." );
				} else if ( a.client != null ) {
					// EEERRR print reconnected message to clients
					System.Console.WriteLine( "Mobile " + a.ObjectInstanceID + " has been taken over by a new connection." );
					a.client = client;
					client.Avatar = a;
				} else {
					a.client = client;
					client.Avatar = a;
				}
			} else {
				// try to load the character
				a = world.LoadMobile( message.InstanceID );
				if ( a == null ) {
					Console.WriteLine( "ERROR: Character "+message.InstanceID+" not found." );
					client.Close();
					return;
				}
				a.client = client;
				client.Avatar = a;

				// try to add the character to the world
				world.Add( client.Avatar );
			}
			world.SendInitialWorldView( client );
		}

		void ProcessPositionMessage(
			Client client, Strive.Network.Messages.ToServer.Position message
		) {
			world.Relocate( client.Avatar,
				new Vector3D( message.position_x, message.position_y, message.position_z ),
				new Vector3D( message.heading_x, message.heading_y, message.heading_z )
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

		void ProcessAttackMessage(
			Client client, Strive.Network.Messages.ToServer.GameCommand.Attack message
		) {
			(client.Avatar as MobileAvatar).Attack( message.targetObjectInstanceID );
		}

		void ProcessFleeMessage(
			Client client, Strive.Network.Messages.ToServer.GameCommand.Flee message
		) {
			(client.Avatar as MobileAvatar).Flee();
		}

		void ProcessReloadWorldMessage(
			Client client, Strive.Network.Messages.ToServer.ReloadWorld message
		) {
			System.Console.WriteLine( "ReloadWorld received" );
			world.Load();
			foreach( Client c in listener.Clients.Values ) {
				if ( c.Avatar != null ) {
					world.SendInitialWorldView( c );
				}
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
