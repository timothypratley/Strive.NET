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
		int CurrentMilliseconds = 0;
		int CurrentBeat = 0;
		int BeatsPerDay = 1000;
		int MillisecondsPerBeat = 10000;


		public Engine() {
			Global.ReadConfiguration();
			engine_thread = new StoppableThread( new StoppableThread.WhileRunning( UpdateLoop ) );
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
			// need to send a beat message every MillisecondsPerBeat milliseconds:
			try {
				// handle world changes
				Global.now = DateTime.Now;
				world.Update();
				if(CurrentMilliseconds == 0)
				{
					CurrentMilliseconds = Environment.TickCount;
				}

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

				// calculate if message needs to be sent:
				int CurrentTicks = Environment.TickCount;
				int BeatIncrement = (CurrentTicks - CurrentMilliseconds) / MillisecondsPerBeat;

				if(BeatIncrement > 0)
				{
					CurrentBeat += BeatIncrement;
					CurrentMilliseconds += BeatIncrement * MillisecondsPerBeat;
					listener.SendToAll(new Strive.Network.Messages.ToClient.Beat(CurrentBeat));
				}

				

			} catch ( Exception e ) {
				// Just log exceptions and stop all threads
				Log.ErrorMessage( e );
				Stop();
			}
		}
	}
}
