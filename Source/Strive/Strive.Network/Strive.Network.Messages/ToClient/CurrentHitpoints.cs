using System;
using Strive.Server.Model;

namespace Strive.Network.Messages.ToClient
{
    [Serializable]
	public class CurrentHitpoints : IMessage {
		public float HitPoints;

		public CurrentHitpoints(){}
        public CurrentHitpoints(PhysicalObject po)
        {
            HitPoints = po.HitPoints;
        }
	}
}
