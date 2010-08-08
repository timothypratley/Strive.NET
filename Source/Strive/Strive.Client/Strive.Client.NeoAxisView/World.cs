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
            // hook up event handlers

            //NeoAxis initialization
            _splash.Show();
            if (!WindowsAppWorld.Init(_splash, "user:Logs/WindowsAppExample.log"))
            {
                _splash.Close();
                return false;
            }
            _splash.Hide();

            //load map
            WindowsAppWorld.MapLoad("Maps/RTSDemo/Map.map", true);
            return true;
        }
    }
}
