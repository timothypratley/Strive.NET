using System;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Indicates that a beat has taken place
	/// </summary>
	public class Beat : IMessage
	{
		public int BeatNumber;
		public Beat()
		{
		}
		public Beat(int BeatNumber)
		{
			this.BeatNumber = BeatNumber;
		}
	}
}
