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
using WindowsAppFramework;
using System.Windows.Forms;

using Strive.Client.ViewModel;


namespace Strive.Client.NeoAxisView
{
    public class World
    {
        static Form _splash = new Splash();
        public static WorldViewModel ViewModel;
        public static bool Init(WorldViewModel worldViewModel)
        {
            ViewModel = worldViewModel;

            //NeoAxis initialization
            _splash.Show();
            if (!WindowsAppWorld.Init(_splash, "user:Logs/WindowsAppExample.log"))
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
            bool result = WindowsAppWorld.MapLoad("Maps/RTSDemo/Map.map", true);
            Map.Instance.GetObjects(new Sphere(Vec3.Zero, 100000), delegate(MapObject obj) {
                if (!obj.Visible)
                    return;
                if (!obj.EditorSelectable)
                    return;
                if (obj.Name.Length == 0)
                    return;

                ViewModel.AddOrReplace(obj.Name, new ViewEntity(obj.Name, obj.Type.Name, obj.Position.X, obj.Position.Y, obj.Position.Z, 0, 0, 0));
            });
            return result;
        }
    }
}
