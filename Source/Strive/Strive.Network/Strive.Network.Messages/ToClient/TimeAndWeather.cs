using System;

namespace Strive.Network.Messages.ToClient
{
	public class TimeAndWeather : IMessage {
		public int DaySkyTextureId;
		public int NightSkyTextureId;
		public int CuspSkyTextureId;
		public int SunTextureId;
		public float Fog;
		public float Rain;
		public long ServerNow;
		public int Latency;

		public TimeAndWeather(){}
		public TimeAndWeather( DateTime serverNow, int latency, int daySkyTextureId, int nightSkyTextureId, int cuspSkyTextureId, int sunTextureId, float fog, float rain ) {
			ServerNow = serverNow.Ticks;
			Latency = latency;
			DaySkyTextureId = daySkyTextureId;
			NightSkyTextureId = nightSkyTextureId;
			CuspSkyTextureId = cuspSkyTextureId;
			SunTextureId = sunTextureId;
			Fog = fog;
			Rain = rain;
		}
	}
}
