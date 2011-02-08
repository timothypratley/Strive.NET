using System;

namespace Strive.Network.Messages.ToClient
{
	public class WhoList : IMessage {
        public Tuple<int, string>[] Mobiles;

		public WhoList(){}
        public WhoList(Tuple<int, string>[] mobiles)
        {
            Mobiles = mobiles;
        }
	}
}
