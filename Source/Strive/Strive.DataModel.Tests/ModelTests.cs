using System;
using System.Windows.Media.Media3D;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;

namespace Strive.Model.Tests
{
    [TestClass]
    public class ModelTests
    {
        EntityModel MakeEntity()
        {
            return new EntityModel(1, "Foo", "bar", new Vector3D(), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f);
        }

        [TestMethod]
        public void Instanciation()
        {
            MakeEntity().Should().NotBeNull();
        }

        [TestMethod]
        public void HeirarchyIsPreserved()
        {
            MakeEntity()
                .Move(EnumMobileState.Running, new Vector3D(1, 2, 3), Quaternion.Identity, DateTime.Now)
                .Should().BeOfType<EntityModel>();

            new CombatantModel(
                2, "Baz", "bar", new Vector3D(), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f,
                20, 20, 20, 20, 20)
                .Move(EnumMobileState.Running, new Vector3D(1, 2, 3), Quaternion.Identity, DateTime.Now)
                .Should().BeOfType<CombatantModel>();
        }

        [TestMethod]
        public void WorldModelWorks()
        {
            WorldModel.GetCubeKey(new Vector3D())
                .Should().Equals(0);

            WorldModel.GetCubeKey(new Vector3D(1000, 1000, 1000))
                .Should().Equals(65793000);

            WorldModel.Empty
                .Add(MakeEntity())
                .Add(new EntityModel(2, "Bob", "bob", new Vector3D(1000, 1000, 1000), Quaternion.Identity, 100, 100, EnumMobileState.Standing, 1.7f))
                .Add(MakeEntity())
                .GetNearby(new Vector3D(1001, 1001, 1001))
                .Should().NotBeEmpty()
                .And.HaveCount(1);
        }
    }
}
