using Microsoft.FSharp.Collections;


namespace Strive.Client.Model
{
    public class WorldModel
    {
        public WorldModel(
            FSharpMap<int, EntityModel> entity,
            FSharpMap<int, TaskModel> task,
            FSharpMap<int, PlanModel> plan,
            FSharpMap<int, FSharpSet<int>> holding,
            FSharpMap<int, FSharpSet<int>> doing,
            FSharpMap<int, FSharpSet<int>> belongsTo)
        {
            Entity = entity;
            Task = task;
            Plan = plan;

            Holding = holding;
            Doing = doing;
            BelongsTo = belongsTo;
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
        public FSharpMap<int, FSharpSet<int>> BelongsTo { get; private set; }

        public WorldModel Add(EntityModel entity)
        {
            return new WorldModel(Entity.Add(entity.Id, entity), Task, Plan, Holding, Doing, BelongsTo);
        }

        public WorldModel Add(TaskModel task)
        {
            return new WorldModel(Entity, Task.Add(task.Id, task), Plan, Holding, Doing, BelongsTo);
        }

        public WorldModel Add(PlanModel plan)
        {
            return new WorldModel(Entity, Task, Plan.Add(plan.Id, plan), Holding, Doing, BelongsTo);
        }

        public WorldModel Put(FSharpSet<int> entities, int on)
        {
            return new WorldModel(Entity, Task, Plan, Holding.Add(on, SetModule.Union(Holding[on], entities)), Doing, BelongsTo);
        }
    }
}
