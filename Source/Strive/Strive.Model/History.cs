using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;

namespace Strive.Model
{
    public class History
    {
        private readonly RecordedModel<WorldModel> _recordedWorld = new RecordedModel<WorldModel>(WorldModel.Empty);
        public IEnumerable<EntityModel> Entities { get { return _recordedWorld.Current.Entity.Select(x => x.Value); } }
        public WorldModel Current { get { return _recordedWorld.Current; } }

        public void Add(EntityModel entity)
        {
            _recordedWorld.Head = _recordedWorld.Head.Add(entity);
        }

        public void Add(TaskModel task)
        {
            _recordedWorld.Head = _recordedWorld.Head.Add(task);
        }

        public EntityModel GetEntity(int key)
        {
            return _recordedWorld.Head.Entity.TryFind(key).Value;
        }

        public int MaxVersion { get { return _recordedWorld.MaxVersion; } }
        public int CurrentVersion
        {
            get { return _recordedWorld.CurrentVersion; }
            set { _recordedWorld.CurrentVersion = value; }
        }

        public void Move(int key, Vector3D position, Quaternion rotation)
        {
            Add(GetEntity(key).Move(position, rotation));
        }
    }
}
