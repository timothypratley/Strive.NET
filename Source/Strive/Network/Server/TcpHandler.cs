using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Threading;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

using Strive.Network.Messages;
using Strive.Common;

namespace Strive.Network.Server {
	/// <summary>
	/// Summary description for NetworkHandler.
	/// </summary>
	public class TcpHandler {
		Queue clientMessageQueue = new Queue();
		IPEndPoint localEndPoint;
		//BinaryFormatter formatter = new BinaryFormatter();
		Hashtable clients = new Hashtable();
		StoppableThread myThread;
		TcpListener listener;

		public TcpHandler( IPEndPoint localEndPoint ) {
			this.localEndPoint = localEndPoint;
			listener = new TcpListener( localEndPoint );
			myThread = new StoppableThread( new StoppableThread.WhileRunning( Run ) );
		}

		public void Start() {
			listener.Start();
			myThread.Start();
		}

		public void Stop() {
			if ( listener != null ) {
				listener.Stop();
			}
			myThread.Stop();
		}

		public void Run() {
			listener.AcceptTcpClient();
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
