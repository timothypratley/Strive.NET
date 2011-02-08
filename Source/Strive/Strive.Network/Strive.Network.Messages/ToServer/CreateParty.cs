using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class CreateParty : IMessage	{
		public string Name;

		public CreateParty(){}
        public CreateParty(string name)
        {
            Name = name;
        }
	}
}
