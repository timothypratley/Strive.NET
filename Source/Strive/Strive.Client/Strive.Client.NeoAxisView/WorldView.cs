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
            var result = WPFAppWorld.Init(mainWindow, "user:Logs/Strive.log")
                && LoadMap();
            //var result = LoadTest();

            return result;
        }

        public static void UpdateFromWorldModel()
        {
            // add or update all entities in the current scene
            // Remove entities that should no longer be in the scene
            var m = WorldViewModel.WorldModel.Snap();
            foreach (var e in Entities.Instance.EntitiesCollection
                .Where(x => x is MapObject)
                .Cast<MapObject>())
            {
                // TODO: remove reference to fsharpmap use an interface?
                var x = m.TryFind(e.Name);
                if (x == FSharpOption<EntityModel>.None)
                    e.SetShouldDelete();
                else
                {
                    var z = x.Value;
                    e.Position = z.Position.ToVec3();
                    e.Rotation = z.Rotation.ToQuat();
                }
            }
        }

        public static bool LoadTest()
        {
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 2; y++)
                    for (int z = 0; z < 2; z++)
                    {
                        WorldViewModel.Set(
                            string.Concat(x,y,z),
                            "StaticBox",
                            new Vector3D(x, y, z),
                            Quaternion.Identity);
                    }
            return true;
        }

        public static bool LoadMap()
        {
            bool result = WPFAppWorld.MapLoad("Maps/Gr1d/Map.map", true);
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    for (int z = 0; z < 10; z++)
                    {
                        var mo = (MapObject)Entities.Instance.Create("StaticBox", Map.Instance);
                        mo.Position = new Vec3(x * 20, y * 20, z * 20);
                        mo.PostCreate();
                    }
            Map.Instance.GetObjects(new Sphere(Vec3.Zero, 100000), delegate(MapObject obj) {
                if (!obj.Visible)
                    return;
                if (!obj.EditorSelectable)
                    return;
                if (obj.Name.Length == 0)
                    return;

                WorldViewModel.Set(obj.Name, obj.Type.Name, obj.Position.ToVector3D(), obj.Rotation.ToQuaternion());
            });
            return result;
        }

        public static void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
