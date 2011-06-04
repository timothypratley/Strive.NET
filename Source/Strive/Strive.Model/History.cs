using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using Strive.Common;

namespace Strive.Model
{
    public class History
    {
        private readonly Recorded<WorldModel> _recordedWorld = new Recorded<WorldModel>(WorldModel.Empty);
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

        public void Add(MissionModel mission)
        {
            _recordedWorld.Head = _recordedWorld.Head.Add(mission);
        }

        public void Assign(TaskModel task, int doerId)
        {
            _recordedWorld.Head = _recordedWorld.Head.Assign(task.Id, doerId);
        }

        public void Complete(TaskModel task, EntityModel entity)
        {
            _recordedWorld.Head = _recordedWorld.Head.Complete(task, entity);
        }

        public void Complete(MissionModel mission)
        {
            var world = _recordedWorld.Head;
            var opt = world.Requires.TryFind(mission.Id);
            if (opt != null)
                foreach (var t in opt.Value)
                    world = world.Complete(world.Task[t], null);
            _recordedWorld.Head = world.Complete(mission);
        }

        public EntityModel GetEntity(int key)
        {
            var r = _recordedWorld.Head.Entity.TryFind(key);
            if (r != null)
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
