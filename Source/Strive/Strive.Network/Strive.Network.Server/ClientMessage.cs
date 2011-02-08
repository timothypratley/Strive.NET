using Strive.Network.Messages;

namespace Strive.Network.Server {
	public class ClientMessage {
		public ClientMessage( Client client, IMessage message ) {
			_client = client;
			_message = message;
		}

		public IMessage Message {
			get { return _message; }
		}

		public Client Client {
			get { return _client; }
		}

	    readonly IMessage _message;
	    readonly Client _client;
	}
}
