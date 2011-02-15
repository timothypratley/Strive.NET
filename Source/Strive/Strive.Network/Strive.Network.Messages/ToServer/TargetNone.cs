namespace Strive.Network.Messages.ToServer
{
    public class TargetNone : IMessage
    {
        public CommandType CommandId;

        public TargetNone(CommandType commandId)
        {
            CommandId = commandId;
        }

        public enum CommandType
        {
            Depossess,
            MassHeal,
            Lava,
            Warcry
        }
    }
}
