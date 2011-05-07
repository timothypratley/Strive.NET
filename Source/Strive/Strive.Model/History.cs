using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Strive.Common;
using System;

namespace Strive.Model
{
    public class History
    {
        private readonly Recorded<WorldModel> _recordedWorld = new Recorded<WorldModel>(WorldModel.Empty);
        public IEnumerable<EntityModel> Entities { get { return _recordedWorld.Current.Entity.Select(x => x.Value); } }
        public WorldModel Current { get { return _recordedWorld.Current; } }
        public WorldModel Head { get { return _recordedWorld.Head; } set { _recordedWorld.Head = value; } }

        public void Add(EntityModel entity)
        {
            _recordedWorld.Head = _recordedWorld.Head.Add(entity);
        }

        public void Add(IEnumerable<EntityModel> entities)
        {
            _recordedWorld.Head = entities.Aggregate(_recordedWorld.Head, (x, y) => x.Add(y));
        }

        public void Add(TaskModel task)
        {
            _recordedWorld.Head = _recordedWorld.Head.Add(task);
        }

        public EntityModel GetEntity(int key)
        {
            var r = _recordedWorld.Head.Entity.TryFind(key);
            if (r!=null)
                return r.Value;
            return null;
        }

        public int MaxVersion { get { return _recordedWorld.MaxVersion; } }
        public int CurrentVersion
        {
            get { return _recordedWorld.CurrentVersion; }
            set { _recordedWorld.CurrentVersion = value; }
        }

        public void Move(int key, EnumMobileState state, Vector3D position, Quaternion rotation, DateTime when)
        {
            Add(GetEntity(key).Move(state, position, rotation, when));
        }
    }
}
