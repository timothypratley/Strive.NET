using System;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class TimeAndWeather : IMessage {
		public int DaySkyTextureID;
		public int NightSkyTextureID;
		public int CuspSkyTextureID;
		public int SunTextureID;
		public float Fog;
		public float Rain;
		public long ServerNow;
		public int Latency;
		public TimeAndWeather(){}
		public TimeAndWeather( DateTime ServerNow, int Latency, int DaySkyTextureID, int NightSkyTextureID, int CuspSkyTextureID, int SunTextureID, float Fog, float Rain ) {
			this.ServerNow = ServerNow.Ticks;
			this.Latency = Latency;
			this.DaySkyTextureID = DaySkyTextureID;
			this.NightSkyTextureID = NightSkyTextureID;
			this.CuspSkyTextureID = CuspSkyTextureID;
			this.SunTextureID = SunTextureID;
			this.Fog = Fog;
			this.Rain = Rain;
		}
	}
}
