using System.Windows.Media.Media3D;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;

namespace Strive.Model.Tests
{
    [TestClass]
    public class DataModelTests
    {
        [TestMethod]
        public void Instanciation()
        {
            EntityModel e = new EntityModel(1, "Foo", "bar", new Vector3D(), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f);
        }

        [TestMethod]
        public void Heirarchy()
        {
            EntityModel e = new EntityModel(
                1, "Foo", "bar", new Vector3D(), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f);
            e.Move(new Vector3D(1, 2, 3), Quaternion.Identity)
                .Should().BeOfType<EntityModel>();
            CombatantModel c = new CombatantModel(
                2, "Baz", "bar", new Vector3D(), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f,
                20, 20, 20, 20);
            c.Move(new Vector3D(1, 2, 3), Quaternion.Identity)
                .Should().BeOfType<CombatantModel>();
        }
    }
}
