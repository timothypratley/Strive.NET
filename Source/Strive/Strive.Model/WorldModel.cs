using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.FSharp.Collections;
using Strive.Common;


namespace Strive.Model
{
    public class WorldModel
    {
        public WorldModel(
            FSharpMap<int, EntityModel> entity,
            FSharpMap<int, TaskModel> task,
            FSharpMap<int, PlanModel> plan,
            FSharpMap<int, Production> producing,
            FSharpMap<int, FSharpSet<int>> holding,
            FSharpMap<int, FSharpSet<int>> doing,
            FSharpMap<int, FSharpSet<int>> requires,
            FSharpMap<int, FSharpSet<EntityModel>> entityCube)
        {
            Entity = entity;
            Task = task;
            Plan = plan;

            Producing = producing;
            Holding = holding;
            Doing = doing;
            Requires = requires;

            EntityCube = entityCube;
        }

        // Tables / Nodes
        public FSharpMap<int, EntityModel> Entity { get; private set; }
        public FSharpMap<int, TaskModel> Task { get; private set; }
        public FSharpMap<int, PlanModel> Plan { get; private set; }

        // Relations / Edges
        public FSharpMap<int, Production> Producing { get; private set; }
        public FSharpMap<int, FSharpSet<int>> Holding { get; private set; }
        public FSharpMap<int, FSharpSet<int>> Doing { get; private set; }
        public FSharpMap<int, FSharpSet<int>> Requires { get; private set; }

        // Indexes
        public FSharpMap<int, FSharpSet<EntityModel>> EntityCube { get; private set; }

        private static WorldModel _empty = new WorldModel(
                    MapModule.Empty<int, EntityModel>(),
                    MapModule.Empty<int, TaskModel>(),
                    MapModule.Empty<int, PlanModel>(),
                    MapModule.Empty<int, Production>(),
                    MapModule.Empty<int, FSharpSet<int>>(),
                    MapModule.Empty<int, FSharpSet<int>>(),
                    MapModule.Empty<int, FSharpSet<int>>(),
                    MapModule.Empty<int, FSharpSet<EntityModel>>());

        public static WorldModel Empty { get { return _empty; } }

        /// <summary>
        /// Using a simple hashing because Vector3D is not comparable,
        /// hence not suitable as a key to an FSharpMap.
        /// Instead we generate a unique integer based upon the position.
        /// </summary>
        /// <param name="entity">The entity whose position will be used to calculate a cube key.</param>
        /// <returns>A cube key which can be used to lookup the set of Entities in that cube.</returns>
        public static int GetCubeKey(Vector3D position)
        {
            return
                (int)Math.Floor(position.X / Constants.ChunkSize)
                + ((int)Math.Floor(position.Y / Constants.ChunkSize) << 8)
                + ((int)Math.Floor(position.Z / Constants.ChunkSize) << 16);
        }

        public static IEnumerable<int> GetNearbyCubeKeys(int key)
        {
            var range = Enumerable.Range(-Constants.Horizon, Constants.Horizon * 2 + 1);
            return (from x in range
                    from y in range
                    from z in range
                    select key + x + (y << 8) + (z << 16));
        }

        public FSharpSet<EntityModel> GetCube(Vector3D position)
        {
            var r = EntityCube.TryFind(GetCubeKey(position));
            if (r != null)
                return r.Value;
            return null;
        }

        public IEnumerable<EntityModel> GetNearby(Vector3D position)
        {
            var key = GetCubeKey(position);
            var range = Enumerable.Range(-Constants.Horizon, Constants.Horizon * 2 + 1);
            return (from x in range
                    from y in range
                    from z in range
                    select EntityCube.TryFind(key + x + (y << 8) + (z << 16)))
                    .Where(o => o != null)
                    .SelectMany(o => o.Value);
        }

        public WorldModel Add(EntityModel entity)
        {
            var newCube = GetCubeKey(entity.Position);
            var option = EntityCube.TryFind(newCube);
            var newSet = (option == null ? SetModule.Empty<EntityModel>() : option.Value)
                .Add(entity);

            var newEntityCube = EntityCube;

            var old = Entity.TryFind(entity.Id);
            if (old != null)
            {
                var oldCube = GetCubeKey(old.Value.Position);
                newEntityCube = EntityCube.Add(oldCube, EntityCube[oldCube].Remove(old.Value));
            }

            newEntityCube = newEntityCube.Add(newCube, newSet);

            return new WorldModel(
                Entity.Add(entity.Id, entity),
                Task, Plan, Producing, Holding, Doing, Requires,
                newEntityCube);
        }

        public WorldModel Add(TaskModel task)
        {
            var o = Requires.TryFind(task.PlanId);
            var tasks = o == null
                ? new FSharpSet<int>(new[] { task.Id })
                : o.Value.Add(task.Id);

            return new WorldModel(Entity,
                Task.Add(task.Id, task), Plan,
                Producing, Holding, Doing,
                Requires.Add(task.PlanId, tasks), EntityCube);
        }

        public FSharpMap<int, FSharpSet<int>> Dissoc(FSharpMap<int, FSharpSet<int>> map, int key, int id)
        {
            var o = map.TryFind(key);
            var set = o == null
                ? SetModule.Empty<int>()
                : o.Value.Remove(id);
            return set.IsEmpty ? map.Remove(key) : map.Add(key, set);
        }

        public WorldModel Complete(EntityModel doer, TaskModel task)
        {
            Contract.Ensures(!Doing.Select(d => d.Value).Any(tasks => tasks.Contains(task.Id)));

            var doing = doer == null
                ? Doing
                : Dissoc(Doing, doer.Id, task.Id);
            var requires = Dissoc(Requires, task.PlanId, task.Id);
            return new WorldModel(Entity, Task.Remove(task.Id), Plan, Producing, Holding, doing, requires, EntityCube);
        }

        public WorldModel Complete(PlanModel plan)
        {
            Contract.Requires<ArgumentException>(!Task.Any(t => t.Value.PlanId == plan.Id));

            return new WorldModel(Entity, Task, Plan.Remove(plan.Id), Producing, Holding, Doing, Requires, EntityCube);
        }

        public WorldModel Add(PlanModel plan)
        {
            return new WorldModel(Entity, Task, Plan.Add(plan.Id, plan), Producing, Holding, Doing, Requires, EntityCube);
        }

        public WorldModel Put(FSharpSet<int> entities, int on)
        {
            return new WorldModel(Entity, Task, Plan, Producing,
                Holding.Add(on, SetModule.Union(Holding[on], entities)), Doing, Requires, EntityCube);
        }


        public WorldModel WithProduction(int producerId, int productId, DateTime when)
        {
            Contract.Requires<ArgumentException>(Entity.ContainsKey(producerId));

            var current = Producing.TryFind(producerId);
            Production produce = (current == null)
                ? produce = Production.Empty
                : produce = current.Value.WithProduction(productId, when);
            return new WorldModel(Entity, Task, Plan,
                Producing.Add(producerId, produce), Holding, Doing, Requires, EntityCube);
        }

        public WorldModel WithProductionComplete(int producerId, EntityModel entity, DateTime when)
        {
            Contract.Requires<ArgumentException>(Entity.ContainsKey(producerId));
            Contract.Requires<ArgumentException>(!Entity.ContainsKey(entity.Id));
            Contract.Requires<ArgumentException>(Producing.ContainsKey(producerId));

            var current = Producing.TryFind(producerId);
            if (current == null)
                return this;
            var produce = current.Value.WithProductionComplete(when);
            return new WorldModel(Entity.Add(entity.Id, entity), Task, Plan,
                produce.Queue.IsEmpty ? Producing.Remove(producerId) : Producing.Add(producerId, produce),
                Holding, Doing, Requires, EntityCube);
        }

        public WorldModel WithProductionProgressChange(int producerId, float progressChange, DateTime when)
        {
            Contract.Requires<ArgumentException>(Entity.ContainsKey(producerId));
            Contract.Requires<ArgumentException>(Producing.ContainsKey(producerId));

            var current = Producing.TryFind(producerId);
            if (current == null)
                return this;
            return new WorldModel(Entity, Task, Plan,
                Producing.Add(producerId, current.Value.WithProgressChange(progressChange, when)),
                Holding, Doing, Requires, EntityCube);
        }

        #region CodeContractsInvariant

        // TODO: These pure functions are just to suppress a warning from code contracts, can it be fixed a better way?
        [Pure]
        private bool ContainsKey(FSharpMap<int, EntityModel> map, int key)
        {
            return map.ContainsKey(key);
        }

        [Pure]
        private bool ContainsKey(FSharpMap<int, PlanModel> map, int key)
        {
            return map.ContainsKey(key);
        }

        [Pure]
        private bool ContainsKey(FSharpMap<int, TaskModel> map, int key)
        {
            return map.ContainsKey(key);
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            // an Entity exists for every Holding (entities hold entities)
            // and every Entity being held by an Entity can be found
            Contract.Invariant(
                Holding.All(
                    x => ContainsKey(Entity, x.Key)
                        && x.Value.All(y => ContainsKey(Entity, y))));

            // an Entity exists for every Doing (entities do tasks)
            // and every Task being done by an Entity can be found
            Contract.Invariant(
                Doing.All(
                    x => ContainsKey(Entity, x.Key)
                        && x.Value.All(y => ContainsKey(Task, y))));

            // a Plan exists for every Require  (plans require tasks)
            // and every Task required by a Plan can be found
            Contract.Invariant(
                Requires.All(
                    x => ContainsKey(Plan, x.Key)
                        && x.Value.All(y => ContainsKey(Task, y))));

            Contract.Invariant(EntityCube.Sum(x => x.Value.Count) == Entity.Count);
        }

        #endregion
    }
}
