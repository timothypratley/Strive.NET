using System;
using System.Windows.Media.Media3D;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;

namespace Strive.Server.Logic.Tests
{
    [TestClass]
    public class WorldTests
    {
        [TestMethod]
        public void BasicOperation()
        {
            /**
            var engine = new Engine(
                new MessageProcessor(
                    new World(1),
                    new MockListener()));
             */

            var w = new World(null, 0, new History());
            //var po = new PhysicalObject();
            //w.Add();
        }

        [TestMethod]
        public void EventApplication()
        {
            var world = new World(null, 0, new History());
            var e1 = new CombatantModel(0, "Combatant", "bar", new Vector3D(), Quaternion.Identity, 10, 10, EnumMobileState.Standing, 2, 20, 20, 20, 20, 20);
            var e2 = new EntityModel(1, "Entity", "bar", new Vector3D(), Quaternion.Identity, 10, 10, EnumMobileState.Standing, 2);
            var plan = new PlanModel(0, EnumPlanAction.Move, e1, DateTime.Now, e2, DateTime.Now, e2, 1);
            var task = new TaskModel(0, 0, new Vector3D(), new Vector3D());
            world.Apply(new EntityUpdateEvent(e1, "Test entity event"));
            world.Apply(new PlanUpdateEvent(plan, "Test plan event"));
            world.Apply(new TaskUpdateEvent(task, "Test task event"));
            world.Apply(new TaskCompleteEvent(null, task, "Test task complete event"));
            world.Apply(new PlanCompleteEvent(plan, "Test task complete event"));
            world.Apply(new EntityUpdateEvent(e2, "Test entity event"));
            world.Apply(new SkillEvent(e1, EnumSkill.AcidBlast, e2, true, true, false, "Test skill event"));
        }

        [TestMethod]
        public void Arithmatic()
        {
            Math.Floor(1.5f).Should().Equals(1f);
            Math.Truncate(1.5f).Should().Equals(1f);
            Math.Floor(-1.5f).Should().Equals(2f);
            Math.Truncate(-1.5f).Should().Equals(2f);
        }
    }
}
;