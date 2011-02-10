using System;

namespace Strive.Network.Messages.ToServer
{
	[Serializable]
	public class EnterWorldAsMobile : IMessage
	{
        public int InstanceId;
        
        public EnterWorldAsMobile() { }
        public EnterWorldAsMobile(int instanceId)
        {
            InstanceId = instanceId;
        }
	}
}
