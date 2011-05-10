namespace Strive.Network.Messages.ToServer
{
    public class ProduceEntity
    {
        public int Id;
        public string Name;
        public string ModelId;
        public int FactoryId;

        public ProduceEntity(int id, string name, string modelId, int factoryId)
        {
            Id = id;
            Name = name;
            ModelId = modelId;
            FactoryId = factoryId;
        }
    }
}
