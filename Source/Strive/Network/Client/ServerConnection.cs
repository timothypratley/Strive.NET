using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Strive.Common;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;

namespace Strive.Network.Client {
	public class ServerConnection {
		Queue messageQueue;
		IPEndPoint remoteEndPoint;
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, 0);
		UdpClient serverConnection;
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
			serverConnection = new UdpClient();
			myThread.Start();
		}

		public void Stop() {
			if ( serverConnection != null ) {
				serverConnection.Close();
			}
			myThread.Stop();
		}

		public void Run() {
			byte[] receivedBytes;
			try {
				// Blocks until a message returns on this socket from a remote host.
				receivedBytes = serverConnection.Receive(ref endpoint);
			} catch ( ObjectDisposedException ) {
				// do nothing, user has quit the client, causing the
				// connection to be closed
				return;
			}
			// Generic serialization
			//IMessage message = (IMessage)formatter.Deserialize(
			//	new MemoryStream( receivedBytes )
			//);

			// Custom serialization
			IMessage message = (IMessage)CustomFormatter.Deserialize( receivedBytes );
			messageQueue.Enqueue( message );
			//Console.WriteLine( "enqueued " + message.GetType() + " message" );
		}

		private void Send( IMessage message ) {
			// Generic serialization
			//MemoryStream ms = new MemoryStream();
			//formatter.Serialize( ms, message );

			// Custom serialization
			byte [] buffer = CustomFormatter.Serialize( message );
			try {
				serverConnection.Send( buffer, buffer.Length, remoteEndPoint );
			} catch ( ObjectDisposedException ) {
				// do nothing, socket has been closed by another thread
			}
		}

		public int MessageCount {
			get { return messageQueue.Count; }
		}

		public IMessage PopNextMessage() {
			return (IMessage)messageQueue.Dequeue();
		}

		#region Simple Message API

		public void PossessMobile(int mobileId)
		{
			Send(new EnterWorldAsMobile(mobileId));
		}

		public void Login(string username, string password)
		{
			Send(new Login(username, password));
		}

		public void Logout()
		{
			Send(new Logout());
		}

		public void Position(float position_x,
			float position_y,
			float position_z,
			float heading_x,
			float heading_y,
			float heading_z)
		{
			Send(new Position(position_x,
				position_y,
				position_z,
				heading_x,
				heading_y,
				heading_z));
		}


		public void RequestPossessable()
		{
			Send(new RequestPossessable());
		}

		#endregion

	}
}
