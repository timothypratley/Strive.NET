using System;
using System.Linq;
using Microsoft.FSharp.Collections;
using System.Diagnostics.Contracts;


namespace Strive.Model
{
    public class WorldModel
    {
        public WorldModel(
            FSharpMap<int, EntityModel> entity,
            FSharpMap<int, TaskModel> task,
            FSharpMap<int, PlanModel> plan,
            FSharpMap<int, FSharpSet<int>> holding,
            FSharpMap<int, FSharpSet<int>> doing,
            FSharpMap<int, FSharpSet<int>> requires)
        {
            Entity = entity;
            Task = task;
            Plan = plan;

            Holding = holding;
            Doing = doing;
            Requires = requires;
        }

        public static WorldModel Empty
        {
            get
            {
                return new WorldModel(
                    MapModule.Empty<int, EntityModel>(),
                    MapModule.Empty<int, TaskModel>(),
                    MapModule.Empty<int, PlanModel>(),
                    MapModule.Empty<int, FSharpSet<int>>(),
                    MapModule.Empty<int, FSharpSet<int>>(),
                    MapModule.Empty<int, FSharpSet<int>>());
            }
        }

        // Tables / Nodes
        public FSharpMap<int, EntityModel> Entity { get; private set; }
        public FSharpMap<int, TaskModel> Task { get; private set; }
        public FSharpMap<int, PlanModel> Plan { get; private set; }

        // Relations / Edges
        public FSharpMap<int, FSharpSet<int>> Holding { get; private set; }
        public FSharpMap<int, FSharpSet<int>> Doing { get; private set; }
        public FSharpMap<int, FSharpSet<int>> Requires { get; private set; }

        public WorldModel Add(EntityModel entity)
        {
            return new WorldModel(Entity.Add(entity.Id, entity), Task, Plan, Holding, Doing, Requires);
        }

        public WorldModel Add(TaskModel task)
        {
            return new WorldModel(Entity, Task.Add(task.Id, task), Plan, Holding, Doing, Requires);
        }

        public WorldModel Add(PlanModel plan)
        {
            return new WorldModel(Entity, Task, Plan.Add(plan.Id, plan), Holding, Doing, Requires);
        }

        public WorldModel Add(AModel m)
        {
            throw new NotSupportedException("Don't know how to add a " + m.GetType());
        }

        public WorldModel Put(FSharpSet<int> entities, int on)
        {
            return new WorldModel(Entity, Task, Plan, Holding.Add(on, SetModule.Union(Holding[on], entities)), Doing, Requires);
        }

        [ContractInvariantMethod]
        protected void ObjectInvariant()
        {
            Contract.Invariant(
                Holding.All(
                    x => Entity.ContainsKey(x.Key)
                        && x.Value.All(y => Entity.ContainsKey(y))));

            Contract.Invariant(
                Doing.All(
                    x => Entity.ContainsKey(x.Key)
                        && x.Value.All(y => Task.ContainsKey(y))));

            Contract.Invariant(
                Requires.All(
                    x => Plan.ContainsKey(x.Key)
                        && x.Value.All(y => Task.ContainsKey(y))));
        }
    }
}
