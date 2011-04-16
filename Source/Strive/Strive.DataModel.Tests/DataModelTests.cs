using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;
using Strive.Model;

namespace Strive.DataModel.Tests
{
    [TestClass]
    public class DataModelTests
    {
        [TestMethod]
        public void Instanciation()
        {
            EntityModel e = new EntityModel(1, "Foo", "bar", new Vector3D(), Quaternion.Identity, 100, EnumMobileState.Standing, 1.7);
        }
    }
}
