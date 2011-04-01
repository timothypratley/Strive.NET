using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.ImmutableDataModel;

namespace Strive.DataModel.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            EntityModel e = new EntityModel(1, "Foo", "bar", new Vector3D(), Quaternion.Identity);
        }
    }
}
