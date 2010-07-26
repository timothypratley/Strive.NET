using System;
using System.Net;
using Strive.Network.Messages;

namespace Strive.Network.Server {
	/// <summary>
	/// Summary description for ClientMessage.
	/// </summary>
	public class ClientMessage {
		public ClientMessage( Client client, IMessage message ) {
			this.client = client;
			this.message = message;
		}

		public IMessage Message {
			get { return message; }
		}

		public Client Client {
			get { return client; }
		}

		IMessage message;
		Client client;
	}
}
