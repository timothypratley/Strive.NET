using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Strive.Network;
using Strive.Network.Messages;

namespace Strive.Client.NetworkHandler {
	public class ServerConnection {
		Queue packetQueue;
		IPEndPoint remoteEndPoint;
		bool isRunning = false;
		UdpClient serverConnection = new UdpClient();
		BinaryFormatter formatter = new BinaryFormatter();
		
		public ServerConnection( Queue packetQueue ) {
			this.packetQueue = packetQueue;
		}

		public class AlreadyRunningException : Exception{}
		public void Start( IPEndPoint remoteEndPoint ) {
			if ( isRunning ) {
				throw new AlreadyRunningException();
			}
			this.remoteEndPoint = remoteEndPoint;
			Console.WriteLine( "remoteEndPoint " + remoteEndPoint );
			isRunning = true;
			Thread myThread = new Thread(
				new ThreadStart( Run )
			);
			myThread.Start();
		}

		public void Stop() {
			serverConnection.Close();
			isRunning = false;
		}

		public void Run() {
			//Creates an IPEndPoint to record the IP Address and port number of the sender. 
			// The IPEndPoint will allow you to read datagrams sent from any source.
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
			try {
				while ( isRunning ) {
					// Blocks until a message returns on this socket from a remote host.
					byte[] receivedBytes = serverConnection.Receive(ref endpoint); 
					packetQueue.Enqueue( new Packet( endpoint, receivedBytes ) );
					Console.WriteLine( "Enqueued packet" );
				}
			} catch ( Exception e ) {
				Console.WriteLine( e );
				Stop();
			}
		}

		public void Send( IMessage message ) {
			if ( !isRunning ) {
				Console.WriteLine( "ERROR: trying to send message without active connection" );
				return;
			}
			try {
				MemoryStream ms = new MemoryStream();
				formatter.Serialize( ms, message );
				serverConnection.Send( ms.GetBuffer(), ms.GetBuffer().Length, remoteEndPoint );
			} catch ( Exception e ) {
				Console.WriteLine( e );
				Stop();
			}
		}

		public bool IsRunning {
			get { return isRunning; }
		}
	}
}
