using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading;
using Strive.Multiverse;
using Strive.Network;
using Strive.Network.Server;
using Strive.Network.Server.Messages;

namespace Strive.Network.Client.NetworkHandler {
	public class MessageProcessor {
		World world;
		bool isRunning = false;
		Queue packetQueue;
		BinaryFormatter formatter = new BinaryFormatter();

		public MessageProcessor( World world, Queue packetQueue ) {
			this.world = world;
			this.packetQueue = packetQueue;
		}

		public class AlreadyRunningException : Exception {}
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
			while ( isRunning ) {
				// deal with incomming messages
				ProcessOustandingMessages();
				Thread.Sleep( 100 );
			}
		}

		public void ProcessOustandingMessages() {
			while ( packetQueue.Count > 0 ) {
				ProcessPacket(
					(Network.Packet)packetQueue.Dequeue()
				);
			}
		}

		public void ProcessPacket( Packet packet ) {
			Console.WriteLine( "ProcessPacket" );
			Object message = formatter.Deserialize(
				new MemoryStream( packet.Message )
			);
			ProcessMessage( message );
		}

		void ProcessMessage( Object message	) {
			if ( message.GetType() == typeof( Strive.Network.Server.Messages.ToClient.Position ) ) {
				ProcessPositionMessage( (Strive.Network.Server.Messages.ToClient.Position)message );
			} else if ( message.GetType() == typeof( Strive.Network.Server.Messages.ToClient.Communication ) ) {
				ProcessChatMessage( (Strive.Network.Server.Messages.ToClient.Communication)message );
			} else {
				Console.WriteLine( "ERROR: Unknown message " + message.GetType() + " discarded" );
			}
		}

		void ProcessPositionMessage(
			Network.Server.Messages.ToClient.Position positionMessage
		) {
			PhysicalObject po =	(PhysicalObject)world.PhysicalObjects[positionMessage.object_id];
			if ( po == null ) {
				po = new PhysicalObject();
				po.object_id = positionMessage.object_id;
				world.Add( po );
			}
			positionMessage.Apply( po );
			Console.WriteLine( "position message, object_id "+positionMessage.object_id+" x:"+positionMessage.position_x+" y:"+positionMessage.position_y+" z:"+positionMessage.position_z );
		}
		
		void ProcessChatMessage(
			Network.Messages.ToClient.Communication chatMessage
		) {
			switch ( chatMessage.communicationType ) {
				case CommunicationType.Say :
					Console.Write( chatMessage.name );
					Console.Write( " says '" );
					Console.Write( chatMessage.message );
					Console.WriteLine( "'" );
					break;
				default:
					Console.Write( chatMessage.name );
					Console.Write( ": " );
					Console.Write( chatMessage.message );
					Console.WriteLine( "" );
					break;
			}
		}
	}
}
