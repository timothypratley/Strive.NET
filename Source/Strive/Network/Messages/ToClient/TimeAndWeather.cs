using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class TimeAndWeather : IMessage {
		public int SkyTextureID;
		public float Lighting;
		public int Fog;
		public int Rain;
		public DateTime ServerNow;
		public int Latency;
		public TimeAndWeather(){}
		public TimeAndWeather( DateTime ServerNow, int Latency, int SkyTextureID, float Lighting, int Fog, int Rain ) {
			this.ServerNow = ServerNow;
			this.Latency = Latency;
			this.SkyTextureID = SkyTextureID;
			this.Lighting = Lighting;
			this.Fog = Fog;
			this.Rain = Rain;
		}
	}
}
