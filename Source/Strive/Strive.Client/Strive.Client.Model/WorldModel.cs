using System.Collections.Generic;
using Strive.DataModel;

namespace Strive.Client.Model
{
    public class WorldModel
    {
        private readonly RecordedMapModel<string, EntityModel> _recordedWorld = new RecordedMapModel<string, EntityModel>();

        public bool ContainsKey(string key)
        {
            return _recordedWorld.ContainsKey(key);
        }

        public void Set(EntityModel entity)
        {
            _recordedWorld.Set(entity.Name, entity);
        }

        public EntityModel Get(string key)
        {
            return _recordedWorld.Get(key);
        }

        public IEnumerable<EntityModel> Values { get { return _recordedWorld.Values; } }
    }
}
