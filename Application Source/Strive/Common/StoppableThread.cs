using System;
using System.Threading;

namespace Strive.Common
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class StoppableThread
	{
		Thread thisThread;
		AutoResetEvent iHaveStopped = new AutoResetEvent(false);
		bool isRunning = false;

		public delegate void WhileRunning();
		WhileRunning wr;

		public StoppableThread( WhileRunning wr ) {
			this.wr = wr;
		}

		public void Start() {
			thisThread = new Thread( new ThreadStart( ThreadLoop ) );
			isRunning = true;
			thisThread.Start();
		}

		public void Stop() {
			if(!isRunning) {
				return;
			}
			isRunning = false;
			WaitHandle.WaitAny( new AutoResetEvent[]{iHaveStopped} );
		}

		void ThreadLoop() {
			while ( isRunning ) {
				wr();
			}
			iHaveStopped.Set();
		}
	}
}
