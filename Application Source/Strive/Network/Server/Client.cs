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
			try {
				// Generic serialization
				// MemoryStream ms = new MemoryStream();
				// formatter.Serialize( ms, message );
				// byte[] EncodedMessage = ms.ToArray();

				// Custom serialization
				byte[] EncodedMessage = CustomFormatter.Serialize( message );

				connection.Send( EncodedMessage, EncodedMessage.Length );
				// Console.WriteLine( "Sent " + message.GetType() + " message (" + EncodedMessage.Length + " bytes) to " + endPoint );
			} catch ( Exception e ) {
				Console.WriteLine( e );
				connected = false;
			}
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
