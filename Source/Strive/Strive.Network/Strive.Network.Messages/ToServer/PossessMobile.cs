namespace Strive.Network.Messages.ToServer
{
    public class PossessMobile : IMessage
    {
        public int InstanceId;

        public PossessMobile(int instanceId)
        {
            InstanceId = instanceId;
        }
    }
}
