using System;

namespace Strive.Network.Messages.ToClient
{
    public class CanPossess
    {
        public Tuple<int, string>[] Possesable;

        public CanPossess(Tuple<int, string>[] possesable)
        {
            Possesable = possesable;
        }
    }
}
