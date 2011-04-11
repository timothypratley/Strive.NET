using System.Linq;
using System.Windows;
using Engine.EntitySystem;
using Engine.MapSystem;
using Strive.Client.Model;
using Strive.Client.ViewModel;
using WPFAppFramework;


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
            var m = WorldViewModel.WorldModel.Current.Entity;

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
                    neoEntity.UserData = entityModel;
                    neoEntity.Position = entityModel.Position.ToVec3();
                    neoEntity.Rotation = entityModel.Rotation.ToQuat();
                    neoEntity.PostCreate();
                }
                neoEntity.Position = entityModel.Position.ToVec3();
                neoEntity.Rotation = entityModel.Rotation.ToQuat();
            }

            // Remove entities that should no longer be in the scene
            // TODO: When there are two views this causes a thread violation
            foreach (var neoEntity in Entities.Instance.EntitiesCollection
                .Where(x => x is MapObject
                    && x.UserData != null
                    && x.UserData is EntityModel
                    && !m.ContainsKey(((EntityModel)x.UserData).Id)))
                neoEntity.SetShouldDelete();
            Entities.Instance.DeleteEntitiesMarkedForDeletion();
        }

        public static void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
