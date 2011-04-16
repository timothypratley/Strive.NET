namespace Strive.Network.Messages.ToServer
{
    public class TargetItem
    {
        public CommandType CommandId;
        public int ItemId;

        public TargetItem(CommandType commandId, int itemId)
        {
            CommandId = commandId;
            ItemId = itemId;
        }

        public enum CommandType
        {
            Curse,
            Enchant,
            Sharpen,
            Drop,
            Get,
            Wear
        }
    }
}
