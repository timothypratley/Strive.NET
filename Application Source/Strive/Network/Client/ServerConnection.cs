using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Strive.Common;
using Strive.Network.Messages;

namespace Strive.Network.Client {
	public class ServerConnection {
		Queue messageQueue;
		IPEndPoint remoteEndPoint;
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
		UdpClient serverConnection = new UdpClient();
		//BinaryFormatter formatter = new BinaryFormatter();
		StoppableThread myThread;
		
		public ServerConnection() {
			myThread = new StoppableThread( new StoppableThread.WhileRunning( Run ) );
		}

		public class AlreadyRunningException : Exception{}
		public void Start( IPEndPoint remoteEndPoint ) {
			this.remoteEndPoint = remoteEndPoint;
			messageQueue = new Queue();
			Console.WriteLine( "remoteEndPoint " + remoteEndPoint );
			myThread.Start();
		}

		public void Stop() {
			//serverConnection.Close();
			myThread.Stop();
		}

		public void Run() {
					// Blocks until a message returns on this socket from a remote host.
					byte[] receivedBytes = serverConnection.Receive(ref endpoint);
					try {
						// Generic serialization
						//IMessage message = (IMessage)formatter.Deserialize(
						//	new MemoryStream( receivedBytes )
						//);

						// Custom serialization
						IMessage message = (IMessage)CustomFormatter.Deserialize( receivedBytes );
						messageQueue.Enqueue( message );
						//Console.WriteLine( "enqueued " + message.GetType() + " message" );
					} catch ( Exception e ) {
						Console.WriteLine( e );
						Console.WriteLine( "ERROR: bad message discarded" );
					}
		}

		public void Send( IMessage message ) {
			try {
				// Generic serialization
				//MemoryStream ms = new MemoryStream();
				//formatter.Serialize( ms, message );

				// Custom serialization
				byte [] buffer = CustomFormatter.Serialize( message );
				serverConnection.Send( buffer, buffer.Length, remoteEndPoint );
			} catch ( Exception e ) {
				Stop();
				throw e;
			}
		}

		public int MessageCount {
			get { return messageQueue.Count; }
		}

		public IMessage PopNextMessage() {
			return (IMessage)messageQueue.Dequeue();
		}
	}
}
