using System;
using System.Threading;
using Strive.Network;
using Strive.Network.Client.NetworkHandler;
using System.Windows.Forms;

namespace Strive.Network.Client {
	/// <summary>
	/// WorldViewer is what the user sees,
	/// and notifies the server if the user moves etc
	/// </summary>
	/// 
	public class WorldViewer {
		World world;
		bool isRunning = false;
		ServerConnection serverConnection;
		PhysicalObject avatar = null;

		public WorldViewer( World world, ServerConnection serverConnection ) {
			this.world = world;
			this.serverConnection = serverConnection;
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
			Strive.UI.Forms.Splash s = new Strive.UI.Forms.Splash();
			Application.Run( s );
		}

		public void Handle() {
			avatar.position_x++;
			serverConnection.Send(
				new Strive.Network.Server.Messages.ToServer.Position( avatar )
			);
			serverConnection.Send(
				new Strive.Network.Server.Messages.ToServer.Communication( "w00t w00t", CommunicationType.Say )
			);
		}

		public PhysicalObject Avatar {
			get { return avatar; }
			set { avatar = value; }
		}
	}
}
