using System;
using System.Net;

namespace Strive.Network.Messages {
	/// <summary>
	/// Summary description for NetworkMessage.
	/// </summary>
	public class Packet {
		public Packet( IPEndPoint endpoint, Byte[] message ) {
			this.message = message;
			this.endpoint = endpoint;
		}

		public Byte[] Message {
			get { return message; }
		}

		public IPEndPoint Endpoint {
			get { return endpoint; }
		}

		Byte[] message;
		IPEndPoint endpoint;
	}
}
