using System;

namespace Strive.Common
{
	/// <summary>
	/// Summary description for AccurateTimer.
	/// </summary>
	public class AccurateTimer {
		long freq = 0;
		public AccurateTimer() {
			if (Win32.QueryPerformanceFrequency(out freq) == 0) {
				throw new Exception( "Failed to QueryPerformanceFrequency" );
			}
		}

		long GetCounter() {
			long count = 0;
			if (Win32.QueryPerformanceCounter(out count) == 0) {
				throw new Exception( "Failed to QueryPerformanceCounter" );
			}
			return count;
		}

		long GetFrequency() {
			return freq;
		}

		long setpoint = 0;
		void Setpoint() {
			setpoint = GetCounter();
		}

		public double ElapsedSeconds() {
			long sp2 = GetCounter();
			long duration = sp2-setpoint;
			setpoint = sp2;
			return (double)duration/(double)freq;
		}

		public double ElapsedSecondsSoFar() {
			long sp2 = GetCounter();
			long duration = sp2-setpoint;
			return (double)duration/(double)freq;
		}
	}
}
