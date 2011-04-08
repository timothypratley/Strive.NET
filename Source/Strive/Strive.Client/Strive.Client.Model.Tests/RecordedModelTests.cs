using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Strive.Client.Model.Tests
{
    [TestClass]
    public class RecordedModelTests
    {
        [TestMethod]
        public void StoreAndRetrieve()
        {
            var world = new RecordedModel<WorldModel>(WorldModel.Empty);
            var foo = new EntityModel(1, "Rich", "bar.3ds", new Vector3D(0, 0, 0), Quaternion.Identity);
            var bar = new EntityModel(2, "Ada", "bar.3ds", new Vector3D(1, 1, 1), Quaternion.Identity);
            world.Head = world.Head.Add(foo);
            world.Head = world.Head.Add(bar);
            Assert.AreEqual(0, world.Head.Entity.TryFind(foo.Id).Value.Position.X);
            world.Head = world.Head.Add(new EntityModel(1, foo.Name, foo.ModelId, new Vector3D(2, 2, 2), Quaternion.Identity));
            Assert.AreEqual(2, world.Head.Entity.TryFind(foo.Id).Value.Position.X);
            Assert.AreEqual(4, world.CurrentVersion);
        }
    }
}
