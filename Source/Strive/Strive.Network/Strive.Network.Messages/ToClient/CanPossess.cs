using System;

namespace Strive.Network.Messages.ToClient
{
	public class CanPossess : IMessage {
        public Tuple<int, string>[] Possesable;

		public CanPossess(){}
        public CanPossess(Tuple<int, string>[] possesable)
        {
			Possesable = possesable;
		}
	}
}
