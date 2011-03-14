using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using Engine.MathEx;
using Engine.MapSystem;
using Engine.EntitySystem;
using WPFAppFramework;
using UpdateControls;
using Strive.Client.ViewModel;
using Microsoft.FSharp.Core;
using Strive.Client.Model;


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
            // TODO: remove reference to fsharpmap use an interface?
            var m = WorldViewModel.WorldModel.Snap();

            // Add or update all entities in the current scene
            foreach (var kvp in m)
            {
                var e = kvp.Value;
                var ee = (MapObject)Entities.Instance.GetByName(e.Name);
                if (ee == null)
                {
                    ee = (MapObject)Entities.Instance.Create("StaticBox", Map.Instance);
                    ee.Name = e.Name;
                    ee.Position = e.Position.ToVec3();
                    ee.Rotation = e.Rotation.ToQuat();
                    ee.PostCreate();
                }
                ee.Position = e.Position.ToVec3();
                ee.Rotation = e.Rotation.ToQuat();
            }

            // Remove entities that should no longer be in the scene
            foreach (var e3 in Entities.Instance.EntitiesCollection
                .Where(x => x is MapObject && !m.ContainsKey(x.Name)))
                e3.SetShouldDelete();
        }

        public static void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
