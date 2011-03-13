using System.Collections.Generic;
using Microsoft.FSharp.Collections;
using Strive.DataModel;

namespace Strive.Client.Model
{
    public class WorldModel
    {
        private readonly RecordedMapModel<string, EntityModel> _recordedWorld = new RecordedMapModel<string, EntityModel>();

        // TODO: don't pass up a fsharp map
        public FSharpMap<string, EntityModel> Snap()
        {
            return _recordedWorld.Map;
        }

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
