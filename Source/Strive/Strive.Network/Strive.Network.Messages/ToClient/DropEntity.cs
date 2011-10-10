using Strive.Model;


namespace Strive.Network.Messages.ToClient
{
    public class DropEntity
    {
        public int Id;

        public DropEntity(EntityModel e)
        {
            Id = e.Id;
        }
    }
}
