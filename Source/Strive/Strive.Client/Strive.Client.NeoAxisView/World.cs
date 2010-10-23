using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Engine.MathEx;
using Engine.Renderer;
using Engine.MapSystem;
using Engine.EntitySystem;
using Engine.PhysicsSystem;
using Engine.Utils;
using Engine;
using GameCommon;
using GameEntities;
using WPFAppFramework;

using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    public class World
    {
        public static WorldViewModel ViewModel;
        public static bool Init(WorldViewModel worldViewModel)
        {
            ViewModel = worldViewModel;

            //NeoAxis initialization
            if (!WPFAppWorld.Init(_splash, "user:Logs/WindowsAppExample.log"))
            {
                _splash.Close();
                return false;
            }
            bool result = LoadMap();
            _splash.Hide();
            return result;
        }

        public static bool LoadMap()
        {
            bool result = WPFAppWorld.MapLoad("Maps/Gr1d/Map.map", true);
            for (int x=0; x<10;x++)
                for(int y=0; y<10;y++)
                    for (int z = 0; z < 10; z++)
                    {
                        var mo = (MapObject)Entities.Instance.Create("StaticBox", Map.Instance);
                        mo.Position = new Vec3(x*20, y*20, z*20);
                        mo.PostCreate();
                    }
            Map.Instance.GetObjects(new Sphere(Vec3.Zero, 100000), delegate(MapObject obj) {
                if (!obj.Visible)
                    return;
                if (!obj.EditorSelectable)
                    return;
                if (obj.Name.Length == 0)
                    return;

                ViewModel.AddOrReplace(obj.Name, obj.Type.Name, obj.Position.X, obj.Position.Y, obj.Position.Z);
            });
            return result;
        }

        public void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
