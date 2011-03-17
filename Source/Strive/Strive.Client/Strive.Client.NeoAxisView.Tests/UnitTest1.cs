using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Strive.Client.NeoAxisView.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        /*
        public static bool LoadTest()
        {
            for (int x = 0; x < 2; x++)
                for (int y = 0; y < 2; y++)
                    for (int z = 0; z < 2; z++)
                    {
                        WorldViewModel.Set(
                            string.Concat(x, y, z),
                            "StaticBox",
                            new Vector3D(x, y, z),
                            Quaternion.Identity);
                    }
            return true;
        }

        public static void LoadLargeTest()
        {
            for (int x = 0; x < 10; x++)
                for (int y = 0; y < 10; y++)
                    for (int z = 0; z < 10; z++)
                    {
                        var mo = (MapObject)Entities.Instance.Create("StaticBox", Map.Instance);
                        mo.Position = new Vec3(x * 20, y * 20, z * 20);
                        mo.PostCreate();
                    }
            Map.Instance.GetObjects(new Sphere(Vec3.Zero, 100000), delegate(MapObject obj)
            {
                if (!obj.Visible)
                    return;
                if (!obj.EditorSelectable)
                    return;
                if (obj.Name.Length == 0)
                    return;

                WorldViewModel.Set(obj.Name, obj.Type.Name, obj.Position.ToVector3D(), obj.Rotation.ToQuaternion());
            });
        }
        */
    }
}
