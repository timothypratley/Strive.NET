using System;
using System.Net;
using System.Collections;
using System.Configuration;

using Strive.Network.Server;
using Strive.Common;
using Strive.Logging;

namespace Strive.Server.Shared {
	/// <summary>
	/// </summary>
	public class Engine {
		int world_id = 1;
		int port = 1337;
		Queue packetQueue = new Queue();
		UdpHandler listener;
		World world;
		MessageProcessor mp;
		StoppableThread engine_thread;

		public Engine() {
			Strive.Network.Messages.CustomFormatter.Serialize(
				Strive.Network.Messages.ToClient.AddPhysicalObject.CreateMessage(
					new Strive.Multiverse.Mobile() ) );

			engine_thread = new StoppableThread( new StoppableThread.WhileRunning( UpdateLoop ) );

			#region read and apply configuration settings
			if ( ConfigurationSettings.AppSettings["world_id"] == null ) {
				throw new ConfigurationException( "world_id" );
			}
			world_id = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["world_id"]);
			if ( ConfigurationSettings.AppSettings["port"] == null ) {
				throw new ConfigurationException( "port" );
			}
			port = int.Parse(ConfigurationSettings.AppSettings["port"]);
			string logfilename = ConfigurationSettings.AppSettings["logFileName"];
			if ( logfilename != null ) {
				Log.SetLogOutput( logfilename );
			}
			#endregion

			listener = new UdpHandler( new IPEndPoint( IPAddress.Any, port ) );
		}

		public void Start() {
			Log.LogMessage( "Starting game engine..." );
			world = new World( world_id );
			mp = new MessageProcessor( world, listener	);
			engine_thread.Start();
			Log.LogMessage( "Listening to new connections..." );
			listener.Start();
		}

		public void Stop() {
			listener.Stop();
			engine_thread.Stop();
			Log.LogMessage( "Server terminated." );
		}

		public void Pause() {

		}

		public void UpdateLoop() {
			try {
				// handle world changes
				Global.now = DateTime.Now;
				world.Update();

				// handle incomming messages
				while ( listener.MessageCount > 0 ) {
					mp.ProcessNextMessage();
				}
				mp.CleanupDeadConnections();

				if ( (DateTime.Now - Global.now) > TimeSpan.FromSeconds(1) ) {
					Log.WarningMessage( "An update cycle took longer than one second." );
				} else {
					System.Threading.Thread.Sleep( 100 );
				}
			} catch ( Exception e ) {
				// Just log exceptions and stop all threads
				Log.ErrorMessage( e );
				Stop();
			}
		}
	}
}
