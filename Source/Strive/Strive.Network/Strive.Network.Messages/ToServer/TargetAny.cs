namespace Strive.Network.Messages.ToServer
{
    public class TargetAny
    {
        public CommandType CommandId;
        public int PhysicalObjectId;

        public TargetAny(CommandType commandId, int physicalObjectId)
        {
            CommandId = commandId;
            PhysicalObjectId = physicalObjectId;
        }

        public enum CommandType
        {
            Attack,
            FairyFire
        }
    }
}
