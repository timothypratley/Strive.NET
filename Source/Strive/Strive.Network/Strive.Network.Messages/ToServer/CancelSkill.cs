namespace Strive.Network.Messages.ToServer
{
    public class CancelSkill
    {
        public int InvokationId;	// this is so the client can cancel specific invokations

        public CancelSkill(int invokationId)
        {
            InvokationId = invokationId;
        }
    }
}
