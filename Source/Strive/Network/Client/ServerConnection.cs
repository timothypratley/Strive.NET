using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Strive.Common;
using Strive.Network.Messages;
using Strive.Network.Messages.ToServer;
using Strive.Logging;

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
			Log.LogMessage( "remoteEndPoint " + remoteEndPoint );
			serverConnection = new UdpClient();
			myThread.Start();
		}

		public void Stop() {
			if ( serverConnection != null ) {
				serverConnection.Close();
				serverConnection = null;
			}
			myThread.Stop();
		}

		public void Run() {
			byte[] receivedBytes;
			try {
				// Blocks until a message returns on this socket from a remote host.
				receivedBytes = serverConnection.Receive(ref endpoint);
			} catch ( Exception ) {
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
			//Log.LogMessage( "enqueued " + message.GetType() + " message" );
		}

		protected void Send( IMessage message ) {
			// Generic serialization
			//MemoryStream ms = new MemoryStream();
			//formatter.Serialize( ms, message );

			// NR 26 Mar 2003
			// Makes UI less sensitive to crashes when disconnected
			if(serverConnection != null)
			{
				// Custom serialization
				byte [] buffer = CustomFormatter.Serialize( message );
				try 
				{
					serverConnection.Send( buffer, buffer.Length, remoteEndPoint );
				} 
				catch ( ObjectDisposedException ) 
				{
					// do nothing, socket has been closed by another thread
				}
			}
		}

		public int MessageCount {
			get { return messageQueue.Count; }
		}

		public IMessage PopNextMessage() {
			return (IMessage)messageQueue.Dequeue();
		}

		#region Simple Message API

		public void Chat(string message)
		{
			// what to do here?
		}

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

		public void SkillList()
		{
			Send(new Strive.Network.Messages.ToServer.GameCommand.SkillList());
		}

		public void WhoList()
		{
			Send(new Strive.Network.Messages.ToServer.GameCommand.WhoList());
		}

		public void UseSkill(Strive.Multiverse.EnumSkill Skill)
		{
			Send(new Strive.Network.Messages.ToServer.GameCommand.UseSkill(Skill));
		}

		public void UseSkill(Strive.Multiverse.EnumSkill Skill, int[] Targets)
		{
			Send(new Strive.Network.Messages.ToServer.GameCommand.UseSkill(Skill, Targets));
		}

		public void UseSkill(int SkillID)
		{
			this.UseSkill((Strive.Multiverse.EnumSkill)SkillID);
		}

		public void UseSkill(int SkillID, int[] Targets)
		{
			this.UseSkill((Strive.Multiverse.EnumSkill)SkillID, Targets);
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
