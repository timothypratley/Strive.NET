using System;
using System.Net;
using System.Collections;

using Strive.Network.Server;
using Strive.Common;

namespace Strive.Server.Shared {
	/// <summary>
	/// </summary>
	public class Engine {
		int world_id = 1;
		int port = 1337;
		Queue packetQueue = new Queue();
		Listener listener;
		World world;
		MessageProcessor mp;
		StoppableThread engine_thread;

		public Engine() {
			engine_thread = new StoppableThread( new StoppableThread.WhileRunning( UpdateLoop ) );
		}

		public void Start() {
			// read and apply configuration settings
			if ( System.Configuration.ConfigurationSettings.AppSettings["world_id"] == null ) {
				throw new System.Configuration.ConfigurationException( "world_id" );
			}
			world_id = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["world_id"]);
			if ( System.Configuration.ConfigurationSettings.AppSettings["port"] == null ) {
				throw new System.Configuration.ConfigurationException( "port" );
			}
			port = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["port"]);

			listener = new Listener(
				new IPEndPoint( IPAddress.Any, port )
			);

			world = new World( world_id );
			listener.Start();
			mp = new MessageProcessor( world, listener	);
			System.Console.WriteLine( "Listening to new connections..." );

			engine_thread.Start();
		}

		public void Stop() {
			engine_thread.Stop();
		}

		public void Pause() {

		}

		public void UpdateLoop() {
				// handle world changes
				Global.now = DateTime.Now;
				world.Update();

				// handle incomming messages
				while ( listener.MessageCount > 0 ) {
					mp.ProcessNextMessage();
				}
				mp.CleanupDeadConnections();

				if ( (DateTime.Now - Global.now) > TimeSpan.FromSeconds(1) ) {
					System.Console.WriteLine( "WARNING: an update cycle took longer than one second" );
				} else {
					System.Threading.Thread.Sleep( 100 );
				}
		}
	}
}
