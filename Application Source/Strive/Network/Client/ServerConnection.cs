using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Strive.Network.Messages;

namespace Strive.Network.Client {
	public class ServerConnection {
		Queue messageQueue = new Queue();
		IPEndPoint remoteEndPoint;
		bool isRunning = false;
		UdpClient serverConnection = new UdpClient();
		//BinaryFormatter formatter = new BinaryFormatter();
		
		public ServerConnection() {
		}

		public class AlreadyRunningException : Exception{}
		public void Start( IPEndPoint remoteEndPoint ) {
			if ( isRunning ) {
				throw new AlreadyRunningException();
			}
			this.remoteEndPoint = remoteEndPoint;
			System.Diagnostics.Trace.WriteLine( "remoteEndPoint " + remoteEndPoint );
			isRunning = true;
			Thread myThread = new Thread(
				new ThreadStart( Run )
			);
			myThread.Start();
		}

		public void Stop() {
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
					try {
						// Generic serialization
						//IMessage message = (IMessage)formatter.Deserialize(
						//	new MemoryStream( receivedBytes )
						//);

						// Custom serialization
						IMessage message = CustomFormatter.Deserialize( receivedBytes );
						messageQueue.Enqueue( (IMessage)message );
						//Console.WriteLine( "enqueued " + message.GetType() + " message" );
					} catch ( Exception ) {
						Console.WriteLine( "ERROR: bad message discarded" );
					}
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
				// Generic serialization
				//MemoryStream ms = new MemoryStream();
				//formatter.Serialize( ms, message );

				// Custom serialization
				byte [] buffer = CustomFormatter.Serialize( message );
				serverConnection.Send( buffer, buffer.Length, remoteEndPoint );
			} catch ( Exception e ) {
				Console.WriteLine( e );
				Stop();
			}
		}

		public bool IsRunning {
			get { return isRunning; }
		}

		public int MessageCount {
			get { return messageQueue.Count; }
		}

		public IMessage PopNextMessage() {
			return (IMessage)messageQueue.Dequeue();
		}
	}
}
