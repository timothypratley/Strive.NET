using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using System.Net;
using Strive.Network;
using Strive.Network.Messages;
using Strive.Multiverse;

namespace Strive.Server.NetworkHandler {
	public class MessageProcessor {
		World world;
		WorldData worldData;
		Hashtable clients = new Hashtable();
		Queue packetQueue;
		BinaryFormatter formatter = new BinaryFormatter();
		bool isRunning = false;

		public MessageProcessor(
			World world, WorldData worldData, Queue packetQueue
		) {
			this.world = world;
			this.worldData = worldData;
			this.packetQueue = packetQueue;
		}

		public class AlreadyRunningException : Exception{}
		public void Start() {
			if ( isRunning ) {
				throw new AlreadyRunningException();
			}
			isRunning = true;
			Thread myThread = new Thread(
				new ThreadStart( Run )
			);
			myThread.Start();
		}

		public void Stop() {
			isRunning = false;
		}

		void Run() {
			// process messages on the message queue
			while ( isRunning ) {
				for ( int i=0; i<10; i++ ) {
					while ( packetQueue.Count > 0 ) {
						ProcessPacket( (Network.Packet)packetQueue.Dequeue() );
					}
					Thread.Sleep( 100 );
				}
				CleanupDeadConnections();
			}
		}

		public void ProcessPacket( Packet packet ) {
			Object message;
			try {
				 message = formatter.Deserialize(
					new MemoryStream( packet.Message )
					);
			} catch ( Exception ) {
				System.Console.WriteLine( "Invalid message received" );
				return;
			}
			Client client = (Client)clients[packet.Endpoint];
			if ( client == null ) {
				// new connection... only allow login
				System.Console.WriteLine(
					"New connection from " + packet.Endpoint
				);
				client = new Client( packet.Endpoint );
				client.LastMessageTimestamp = DateTime.Now;
				clients.Add( packet.Endpoint, client );
				if ( message.GetType() == typeof(Network.Messages.ToServer.Login) ) {
					ProcessLoginMessage(
						client, (Network.Messages.ToServer.Login)message
					);
				} else {
					Console.WriteLine( "Non-login message from " + packet.Endpoint );
				}
				return;
			}
			client.LastMessageTimestamp = DateTime.Now;
			if ( !client.Authenticated ) {
				// not logged in... only allow login
				if ( message.GetType() == typeof(Network.Messages.ToServer.Login) ) {
					ProcessLoginMessage(
						client,	(Network.Messages.ToServer.Login)message
					);					
				} else {
					Console.WriteLine( "ERROR: Non-login message from " + packet.Endpoint );
				}
				return;
			}

			if ( client.Avatar == null ) {
				// no character selected yet... only allow posses
				if ( message.GetType() == typeof( Network.Messages.ToServer.Possess ) ) {
					ProcessPossessMessage( client, (Network.Messages.ToServer.Possess)message );
				} else {
					Console.WriteLine( "ERROR: Non-posses message from " + packet.Endpoint );
				}
				return;
			}

			// normal message for logged in user
			ProcessMessage( client, message );
		}

		void ProcessMessage( Client client, Object message ) {
			if ( message.GetType() == typeof( Network.Messages.ToServer.Position ) ) {
				ProcessPositionMessage( client, (Network.Messages.ToServer.Position)message );
			} else if ( message.GetType() == typeof( Network.Messages.ToServer.Communication ) ) {
				ProcessChatMessage( client, (Network.Messages.ToServer.Communication)message );
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
				worldData.UserLookup( loginMessage.username, loginMessage.password )
			) {
				System.Console.WriteLine(
					"User " + loginMessage.username + " logged in"
				);
				client.AuthenticatedUsername = loginMessage.username;
			}
		}

		void ProcessPossessMessage(
			Client client, Strive.Network.Messages.ToServer.Possess message
			) {
			client.Avatar = worldData.LoadCharacter( message.object_id );
			if ( client.Avatar == null ) {
				Console.WriteLine( "ERROR: Character "+message.object_id+" not found." );
				return;
			}
			try {
				world.Add( client.Avatar );
			} catch {
				Console.WriteLine( "ERROR: Character "+message.object_id+" already logged in." );
				client.Avatar = null;
				return;
			}
		}

		void ProcessPositionMessage(
			Client client, Strive.Network.Messages.ToServer.Position message
		) {
			message.Apply( client.Avatar );
			foreach ( Client c in clients.Values ) {
				if (  c == client ) continue;
				c.Send(
					new Network.Messages.ToClient.Position(
						(Multiverse.PhysicalObject)client.Avatar
					)
				);
			}
		}

		void ProcessChatMessage(
			Client client, Strive.Network.Messages.ToServer.Communication message
		) {
			foreach ( Client c in clients.Values ) {
				if (  c == client ) continue;
				c.Send(
					new Network.Messages.ToClient.Communication( client.Avatar.Name, message.message, message.communicationType )
				);
			}
		}

		void CleanupDeadConnections() {
			// Make a list of dead connections and remove them all
			ArrayList al = new ArrayList();
			foreach ( Client c in clients.Values ) {
				if ( (DateTime.Now - c.LastMessageTimestamp).Seconds > 30 ) {
					al.Add( c.EndPoint );
				}
			}
			foreach ( IPEndPoint ep in al ) {
				Console.WriteLine( "Dropping connection to "+ep+" due to inactivity" );
				clients.Remove( ep );
			}
		}
	}
}
