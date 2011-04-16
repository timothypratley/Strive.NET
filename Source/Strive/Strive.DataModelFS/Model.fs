namespace Strive.ModelFS
open System.Windows.Media.Media3D;
open Strive.Common;


type EntityModel(id:int, name:string, modelId:string, position:Vector3D, rotation:Quaternion, health:float32, mobileState:EnumMobileState, height:float32) = class
    member x.Id with get() = id
    member x.Name with get() = name
    member x.Position with get() = position
    member x.Rotation with get() = rotation
    member x.Health with get() = health
    member x.MobileState with get() = mobileState
    member x.Height with get() = height

    member x.Move(newPosition, newRotation) = new EntityModel(id, name, modelId, newPosition, newRotation, health, mobileState, height)
end

type TaskModel(id:int, start:Vector3D, finish:Vector3D) = class
    member x.Id with get() = id
    member x.Start with get() = start
    member x.Finish with get() = finish
end

type WorldModel(entity:Map<int,EntityModel>, task:Map<int,TaskModel>, holding:Map<int,Set<int>>, doing:Map<int,int>) = class
    member x.Entity with get() = entity
    member x.Task with get() = task
    member x.Holding with get() = holding
    member x.Doing with get() = doing

    static member Empty = new WorldModel(Map.empty, Map.empty, Map.empty, Map.empty)

    member x.Add(newEntity:EntityModel) = new WorldModel(entity.Add(newEntity.Id, newEntity), task, holding, doing)
    member x.Add(newTask:TaskModel) = new WorldModel(entity, task.Add(newTask.Id, newTask), holding, doing)
    member x.Take(e:EntityModel, entities) = new WorldModel(entity, task, holding.Add(e.Id, Set.union holding.[e.Id] entities), doing)
    member x.Drop(e:EntityModel, entities) = new WorldModel(entity, task, holding.Add(e.Id, Set.difference holding.[e.Id] entities), doing)
    member x.Do(e:EntityModel, t:TaskModel) = new WorldModel(entity, task, holding, doing.Add(e.Id,t.Id))
    member x.Done(e:EntityModel) = new WorldModel(entity, task, holding, doing.Remove(e.Id))
end

type CombatantModel(id, name, modelId, position, rotation, health, mobileState, height, constitution:int, dexterity:int, willpower:int, strength:int) =
    inherit EntityModel(id, name, modelId, position, rotation, health, mobileState, height) 
    member x.Constitution with get() = constitution
    member x.Dexterity with get() = dexterity
    member x.Willpower with get() = willpower
    member x.Strength with get() = strength
