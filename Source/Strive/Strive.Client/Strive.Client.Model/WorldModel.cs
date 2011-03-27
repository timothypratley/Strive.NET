using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;
using Strive.DataModel;

namespace Strive.Client.Model
{
    public class WorldModel
    {
        private readonly RecordedMapModel<int, EntityModel> _recordedWorld = new RecordedMapModel<int, EntityModel>();
        public RecordedMapModel<int, EntityModel> History { get { return _recordedWorld; } }
        public IEnumerable<EntityModel> Values { get { return _recordedWorld.Values; } }

        public FSharpMap<int, EntityModel> Snap()
        {
            return _recordedWorld.Map;
        }

        public bool ContainsKey(int key)
        {
            return _recordedWorld.ContainsKey(key);
        }

        public void Set(EntityModel entity)
        {
            _recordedWorld.Set(entity.Id, entity);
        }

        public EntityModel Get(int key)
        {
            return _recordedWorld.Get(key);
        }

        public void Move(int key, Vector3D position, Quaternion rotation)
        {
            Set(Get(key).Move(position, rotation));
        }
    }
}
