using System;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Indicates that a beat has taken place
	/// </summary>
	public class Tick : IMessage
	{
		public int _tick;
		public Tick()
		{
		}
		public Tick(int tick)
		{
			this._tick = tick;
		}
	}
}
