using System;
using Strive.Multiverse;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CurrentHitpoints.
	/// </summary>
	public class CurrentHitpoints : IMessage {
		public int HitPoints;
		public CurrentHitpoints( PhysicalObject po ) {
			HitPoints = (int)po.HitPoints;
		}
	}
}
