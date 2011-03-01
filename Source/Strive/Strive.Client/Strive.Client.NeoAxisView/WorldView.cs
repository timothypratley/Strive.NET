using System.Windows;
using System.ComponentModel;
using System.Windows.Media.Media3D;
using Engine.MathEx;
using Engine.MapSystem;
using Engine.EntitySystem;
using WPFAppFramework;
using Strive.Client.ViewModel;
using System;


namespace Strive.Client.NeoAxisView
{
    public static class WorldView
    {
        public static WorldViewModel ViewModel;
        public static bool Init(Window mainWindow, WorldViewModel worldViewModel)
        {
            ViewModel = worldViewModel;
            var result = WPFAppWorld.Init(mainWindow, "user:Logs/Strive.log") && LoadMap();
            //var result = LoadTest();
            //worldViewModel.EntityAdded += worldViewModel_EntityAdded;
            //dynamic observable = (ObjectInstance<WorldViewModel>)ForView.Wrap(worldViewModel);
            //observable.Entities.CollectionChanged += new CollectionChangeEventHandler(worldViewModel_CollectionChanged);
            //var eh = observable.ClassInstance.GetEvents()["EntityAdded"];
            //eh.AddEventHandler(observable, new CollectionChangeEventHandler(worldViewModel_CollectionChanged));
            //observable. += observable_PropertyChanged;
            //observable.
            //+= worldViewModel_CollectionChanged;)
            return result;
        }

        static void observable_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Console.Write(e.PropertyName);
        }

        static void worldViewModel_CollectionChanged(object sender, CollectionChangeEventArgs e)
        {
            if (e.Action == CollectionChangeAction.Add)
            {
                /*
                foreach (EntityViewModel entity in e.NewItems)
                {
                    var unit = (RTSUnit) Entities.Instance.Create(EntityTypes.Instance.GetByName("RTSRobot"), Map.Instance);
                    unit.Position = entity.Entity.Position.ToVec3();
                    unit.Rotation = entity.Entity.Rotation.ToQuat();
                    unit.PostCreate();
                }
                 */
            }
        }

        public static bool LoadTest()
        {
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 2; y++)
                    for (int z = 0; z < 2; z++)
                    {
                        ViewModel.Set(
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

                ViewModel.Set(obj.Name, obj.Type.Name, obj.Position.ToVector3D(), obj.Rotation.ToQuaternion());
            });
            return result;
        }

        public static void Shutdown()
        {
            WPFAppWorld.Shutdown();
        }
    }
}
