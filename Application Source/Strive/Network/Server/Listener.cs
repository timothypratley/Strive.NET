using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using Strive.Network.Messages;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Strive.Network.Server {
	/// <summary>
	/// Summary description for NetworkHandler.
	/// </summary>
	public class Listener	{
		Queue clientMessageQueue = new Queue();
		IPEndPoint localEndPoint;
		bool isRunning;
		//BinaryFormatter formatter = new BinaryFormatter();
		Hashtable clients = new Hashtable();

		public Listener( IPEndPoint localEndPoint ) {
			this.localEndPoint = localEndPoint;
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

		public void Run() {
			//Creates a UdpClient for reading incoming data.
			UdpClient receivingUdpClient = new UdpClient( localEndPoint );

			//Creates an IPEndPoint to record the IP Address and port number of the sender. 
			// The IPEndPoint will allow you to read datagrams sent from any source.
			IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
			try{
				while ( isRunning ) {
					// Blocks until a message returns on this socket from a remote host.
					Byte[] receivedBytes = receivingUdpClient.Receive(ref endpoint);
					IMessage message;
					try {
						// Generic serialization
						//message = (IMessage)formatter.Deserialize(
						//	new MemoryStream( receivedBytes )
						//);

						// Custom serialization
						message = CustomFormatter.Deserialize( receivedBytes );
					} catch ( Exception ) {
						System.Console.WriteLine( "Invalid packet received" );
						return;
					}
					Client client = (Client)clients[endpoint];
					if ( client == null ) {
						// new connection
						System.Console.WriteLine(
							"New connection from " + endpoint
							);
						client = new Client( endpoint );
						clients.Add( endpoint, client );
					}
					client.LastMessageTimestamp = DateTime.Now;
					ClientMessage clientMessage = new ClientMessage(
						client, message
					);
					clientMessageQueue.Enqueue( clientMessage );
					//Console.WriteLine( message.GetType() + " message enqueued from " + endpoint );
				}
			} catch ( Exception e ) {
				Console.WriteLine(e.ToString()); 
			}
		}

		public int MessageCount {
			get { return clientMessageQueue.Count; }
		}

		public ClientMessage PopNextMessage() {
			return (ClientMessage)clientMessageQueue.Dequeue();
		}

		public Hashtable Clients {
			get { return clients; }
		}
	}
}
