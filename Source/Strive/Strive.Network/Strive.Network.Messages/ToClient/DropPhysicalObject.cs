using Strive.Model;


namespace Strive.Network.Messages.ToClient
{
    public class DropPhysical
    {
        public int Id;

        public DropPhysical(EntityModel po)
        {
            Id = po.Id;
        }
    }
}
