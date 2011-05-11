using System.Linq;
using System.Windows;
using Engine.EntitySystem;
using Engine.MapSystem;
using Strive.Client.ViewModel;
using WPFAppFramework;
using GameEntities;


namespace Strive.Client.NeoAxisView
{
    public static class WorldView
    {
        public static WorldViewModel WorldViewModel;
        public static bool Init(Window mainWindow, WorldViewModel worldViewModel)
        {
            WorldViewModel = worldViewModel;
            return WPFAppWorld.Init(mainWindow, "user:Logs/Strive.log")
                && WPFAppWorld.MapLoad("Maps/Gr1d/Map.map", true);
        }

        public static void UpdateFromWorldModel()
        {
            var m = WorldViewModel.History.Current.Entity;

            // Add or update all entities in the current scene
            foreach (var kvp in m)
            {
                var entityModel = kvp.Value;
                var neoEntity = (MapObject)Entities.Instance.GetByName(entityModel.Id.ToString());
                if (neoEntity == null)
                {
                    neoEntity = (MapObject)Entities.Instance.Create(entityModel.ModelId, Map.Instance);
                    neoEntity.Name = entityModel.Id.ToString();
                    neoEntity.TextUserData = entityModel.Name;
                    neoEntity.UserData = entityModel.Id;
                    neoEntity.Position = entityModel.Position.ToVec3();
                    neoEntity.Rotation = entityModel.Rotation.ToQuat();
                    neoEntity.PostCreate();
                }
                neoEntity.Position = entityModel.Position.ToVec3();
                neoEntity.Rotation = entityModel.Rotation.ToQuat();
                if (entityModel.Production.Queue.Any() && neoEntity is RTSBuilding && ((RTSBuilding)neoEntity).BuildedProgress <= 0)
                    ((RTSBuilding)neoEntity).StartProductUnit(new RTSUnitType());

            }

            // Remove entities that should no longer be in the scene
            // TODO: this still crashes when using timeline - why? (collection modified)
            foreach (var neoEntity in Entities.Instance.EntitiesCollection
                .Where(x => x is MapObject
                    && x.UserData is int
                    && !m.ContainsKey((int)x.UserData))
                .ToArray())
                neoEntity.SetShouldDelete();
            Entities.Instance.DeleteEntitiesMarkedForDeletion();
        }

        public static void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
