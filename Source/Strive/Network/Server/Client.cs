using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Strive.Multiverse;
using Strive.Network.Messages;

namespace Strive.Network.Server {
	/// <summary>
	/// Summary description for Client.
	/// </summary>
	public class Client	{
		IPEndPoint endPoint;
		Mobile avatar = null;
		string authenticatedUsername = null;
		public int PlayerID = 0;
		bool connected = false;
		//BinaryFormatter formatter = new BinaryFormatter();
		UdpClient connection = new UdpClient();
		DateTime lastMessageTimestamp;

		public Client( IPEndPoint endPoint ) {
			this.endPoint = endPoint;
			connection.Connect( endPoint );
		}

		public bool Authenticated {
			get { return authenticatedUsername != null; }
		}

		public string AuthenticatedUsername {
			get { return authenticatedUsername; }
			set {
				authenticatedUsername = value;
				connected = true;
			}
		}

		public void Send( IMessage message ) {
			if ( !Connected ) {
				Console.WriteLine( "ERROR: trying to send message without active connection" );
				return;
			}
				// Generic serialization
				// MemoryStream ms = new MemoryStream();
				// formatter.Serialize( ms, message );
				// byte[] EncodedMessage = ms.ToArray();

				// Custom serialization
				byte[] EncodedMessage = CustomFormatter.Serialize( message );

				try {
					connection.Send( EncodedMessage, EncodedMessage.Length );
				} catch ( ObjectDisposedException ) {
					// do nothing, socket has been closed by another thread
				}
				// Console.WriteLine( "Sent " + message.GetType() + " message (" + EncodedMessage.Length + " bytes) to " + endPoint );
		}

		public void Close() {
			connection.Close();
			connected = false;
		}

		public Mobile Avatar {
			get { return avatar; }
			set { avatar = value; }
		}

		public bool Connected {
			get { return connected; }
		}

		public DateTime LastMessageTimestamp {
			get { return lastMessageTimestamp; }
			set { lastMessageTimestamp = value; }
		}

		public IPEndPoint EndPoint {
			get { return endPoint; }
		}
	}
}
