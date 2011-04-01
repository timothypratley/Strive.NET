using System.Collections.Generic;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;
using Strive.DataModel;

namespace Strive.Client.Model
{
    public class History
    {
        private readonly RecordedMapModel<int, EntityModel> _recordedWorld = new RecordedMapModel<int, EntityModel>();
        public IEnumerable<EntityModel> Entities { get { return _recordedWorld.Values; } }

        public FSharpMap<int, EntityModel> SnapShot()
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

        public int MaxVersion { get { return _recordedWorld.MaxVersion; } }
        public int CurrentVersion
        {
            get { return _recordedWorld.CurrentVersion; }
            set { _recordedWorld.CurrentVersion = value; }
        }

        public void Move(int key, Vector3D position, Quaternion rotation)
        {
            Set(Get(key).Move(position, rotation));
        }
    }
}
