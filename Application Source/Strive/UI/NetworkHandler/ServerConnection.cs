using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Strive.Common;
using Strive.Network;
using Strive.Network.Messages;

namespace Strive.Client.NetworkHandler {
	public class ServerConnection {
		Queue packetQueue;
		IPEndPoint remoteEndPoint;
		UdpClient serverConnection = new UdpClient();
		BinaryFormatter formatter = new BinaryFormatter();
		StoppableThread myThread;
		//Creates an IPEndPoint to record the IP Address and port number of the sender. 
		// The IPEndPoint will allow you to read datagrams sent from any source.
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
		
		public ServerConnection( Queue packetQueue ) {
			this.packetQueue = packetQueue;
			myThread = new StoppableThread( new StoppableThread.WhileRunning( Run ) );
		}

		public class AlreadyRunningException : Exception{}
		public void Start( IPEndPoint remoteEndPoint ) {
			this.remoteEndPoint = remoteEndPoint;
			Console.WriteLine( "remoteEndPoint " + remoteEndPoint );
			myThread.Start();
		}

		public void Stop() {
			myThread.Stop();
		}

		public void Run() {
					// Blocks until a message returns on this socket from a remote host.
					byte[] receivedBytes = serverConnection.Receive(ref endpoint); 
					packetQueue.Enqueue( new Packet( endpoint, receivedBytes ) );
					Console.WriteLine( "Enqueued packet" );
		}

		public void Send( IMessage message ) {
			try {
				MemoryStream ms = new MemoryStream();
				formatter.Serialize( ms, message );
				serverConnection.Send( ms.GetBuffer(), ms.GetBuffer().Length, remoteEndPoint );
			} catch ( Exception e ) {
				Console.WriteLine( e );
				Stop();
			}
		}
	}
}
