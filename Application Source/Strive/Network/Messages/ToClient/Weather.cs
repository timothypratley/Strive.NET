using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class Weather : IMessage {
		public int SkyTextureID;
		public int Lighting;
		public int Fog;
		public Weather(){}
		public Weather( int SkyTextureID, int Lighting, int Fog ) {
			this.SkyTextureID = SkyTextureID;
			this.Lighting = Lighting;
			this.Fog = Fog;
		}
	}
}
