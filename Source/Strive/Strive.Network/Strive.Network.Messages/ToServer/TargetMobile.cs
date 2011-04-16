namespace Strive.Network.Messages.ToServer
{
    public class TargetMobile
    {
        public CommandType CommandId;
        public int MobileId;

        public TargetMobile(CommandType commandId, int mobileId)
        {
            CommandId = commandId;
            MobileId = mobileId;
        }

        public enum CommandType
        {
            Possess,
            Summon,
            GiantStrength,
            Backstab
        }
    }
}
