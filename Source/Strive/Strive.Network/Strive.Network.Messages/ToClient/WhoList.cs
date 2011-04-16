using System;

namespace Strive.Network.Messages.ToClient
{
    public class WhoList
    {
        public Tuple<int, string>[] Mobiles;

        public WhoList(Tuple<int, string>[] mobiles)
        {
            Mobiles = mobiles;
        }
    }
}
