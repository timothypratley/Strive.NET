using System;
using System.Collections.Generic;

namespace Strive.Network.Messages.ToClient
{
	/// <summary>
	/// Summary description for CanPossess.
	/// </summary>
	public class CanPossess : IMessage {
        public Tuple<int, string>[] Possesable;

		public CanPossess(){}
        public CanPossess(Tuple<int, string>[] possesable)
        {
			Possesable = possesable;
		}
	}
}
