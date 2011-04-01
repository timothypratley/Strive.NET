using Microsoft.FSharp.Collections;


namespace Strive.Client.Model
{
    public class WorldModel
    {
        public WorldModel(
            FSharpMap<int, EntityModel> entity,
            FSharpMap<int, TaskModel> task,
            FSharpMap<int, PlanModel> plan,
            FSharpMap<EntityModel, FSharpSet<EntityModel>> holding,
            FSharpMap<EntityModel, FSharpSet<TaskModel>> doing,
            FSharpMap<TaskModel, FSharpSet<PlanModel>> belongsTo)
        {
            Entity = entity;
            Task = task;
            Plan = plan;

            Holding = holding;
            Doing = doing;
            BelongsTo = belongsTo;
        }

        // Tables / Nodes
        public FSharpMap<int, EntityModel> Entity { get; private set; }
        public FSharpMap<int, TaskModel> Task { get; private set; }
        public FSharpMap<int, PlanModel> Plan { get; private set; }

        // Relations / Edges
        // TODO: Actually better to use ints, as we only care about the ID
        public FSharpMap<EntityModel, FSharpSet<EntityModel>> Holding { get; private set; }
        public FSharpMap<EntityModel, FSharpSet<TaskModel>> Doing { get; private set; }
        public FSharpMap<TaskModel, FSharpSet<PlanModel>> BelongsTo { get; private set; }

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

        public WorldModel Put(FSharpSet<EntityModel> entities, EntityModel on)
        {
            return new WorldModel(Entity, Task, Plan, Holding.Add(on, SetModule.Union(Holding[on], entities)), Doing, BelongsTo);
        }
    }
}
