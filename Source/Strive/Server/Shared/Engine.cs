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
		int port = 1337;
		Queue packetQueue = new Queue();
		Listener networkhandler;
		World world;
		MessageProcessor mp;
		StoppableThread engine_thread;
		int CurrentMilliseconds = 0;
		int CurrentBeat = 0;
		int MillisecondsPerBeat = 10000;


		public Engine() {
			Global.ReadConfiguration();
			engine_thread = new StoppableThread( new StoppableThread.WhileRunning( UpdateLoop ) );
			networkhandler = new Listener( new IPEndPoint( IPAddress.Any, port ) );
		}

		public void Start() {
			Log.LogMessage( "Starting game engine..." );
			world = new World( Global.world_id );
			mp = new MessageProcessor( world, networkhandler );
			engine_thread.Start();
			Log.LogMessage( "Listening to new tcp connections..." );
			networkhandler.Start();
		}

		public void Stop() {
			networkhandler.Stop();
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

				// TODO: where should the message queue live?
				while ( networkhandler.MessageCount > 0 ) {
					mp.ProcessNextMessage();
				}

				CleanupLinkdead();

				if ( (DateTime.Now - Global.now) > TimeSpan.FromSeconds(1) ) {
					Log.WarningMessage( "An update cycle took longer than one second." );
				} else {
					System.Threading.Thread.Sleep( 100 );
				}

				// TODO: TJP imho we shouldn't have ticks at all,
				// instead we should use actual time values.
				// I don't think we need beats, but instead use
				// the weather message to synchronize client/server time.

				// calculate if message needs to be sent:
				int CurrentTicks = Environment.TickCount;
				int BeatIncrement = (CurrentTicks - CurrentMilliseconds) / MillisecondsPerBeat;

				if(BeatIncrement > 0)
				{
					CurrentBeat += BeatIncrement;
					CurrentMilliseconds += BeatIncrement * MillisecondsPerBeat;
					networkhandler.SendToAll(new Strive.Network.Messages.ToClient.Beat(CurrentBeat));
				}
			} catch ( Exception e ) {
				// Just log exceptions and stop all threads
				Log.ErrorMessage( e );
				Stop();
			}
		}

		void CleanupLinkdead() {
			// TODO: instead of looping through the entire world,
			// we should keep a list of players
			foreach ( MobileAvatar ma in (ArrayList)world.mobilesArrayList.Clone() ) {
				if ( ma.IsPlayer() ) {
					if ( ma.client == null || (!ma.client.Active && (Global.now - ma.client.LastMessageTimestamp) > TimeSpan.FromSeconds(60) ) ) {
						ma.client = null;
						world.Remove( ma );
					}
				}
			}
		}
	}
}
