using System;
using System.Windows.Media.Media3D;
using FluentAssertions;
using Microsoft.FSharp.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;
using Strive.Data.Events;
using Strive.Model;

namespace Strive.Server.Logic.Tests
{
    [TestClass]
    public class WorldTests
    {
        World world;
        CombatantModel combatant;
        EntityModel entity;

        [TestInitialize]
        public void TestInit()
        {
            world = new World(null, 0, new History());
            combatant = new CombatantModel(0, "Combatant", "bar", new Vector3D(), Quaternion.Identity, 10, 10, EnumMobileState.Running, 2, 20, 20, 20, 20, 20);
            entity = new EntityModel(1, "Entity", "bar", new Vector3D(10, 10, 10), Quaternion.Identity, 10, 10, EnumMobileState.Standing, 2);

        }

        [TestMethod]
        public void BasicOperation()
        {
            /**
            var engine = new Engine(
                new MessageProcessor(
                    new World(1),
                    new MockListener()));
             */

            //var po = new PhysicalObject();
            //w.Add();
        }

        [TestMethod]
        public void EventApplication()
        {
            var mission = new MissionModel(0, EnumMissionAction.Move, entity.Id, DateTime.Now, SetModule.Empty<int>(), DateTime.Now, entity.Position, 1);
            var task = new TaskModel(0, 0, new Vector3D(), null);
            world.Apply(new EntityUpdateEvent(entity, "Test entity event"));
            world.Apply(new MissionUpdateEvent(mission, "Test mission event"));
            world.Apply(new TaskUpdateEvent(task, "Test task event"));
            world.Apply(new TaskCompleteEvent(task, null, "Test task complete event"));
            world.Apply(new MissionCompleteEvent(mission, "Test task complete event"));
            world.Apply(new EntityUpdateEvent(entity, "Test entity event"));
            world.Apply(new SkillEvent(entity, EnumSkill.AcidBlast, entity, true, true, false, "Test skill event"));
        }

        [TestMethod]
        public void Updates()
        {
            var mission = new MissionModel(0, EnumMissionAction.Move, combatant.Id, DateTime.Now, SetModule.Empty<int>(), DateTime.Now, entity.Position, 1);
            world.Apply(new EntityUpdateEvent(combatant, "Test entity event"));
            world.Apply(new MissionUpdateEvent(mission, "Test mission event"));
            world.Apply(new EntityUpdateEvent(entity, "Test entity event"));

            world.Update(DateTime.Now);
            world.History.Head.Task.Count
                .Should().Be(1);

            var newPosition = world.History.Head.Entity[combatant.Id].Position;
            (entity.Position - newPosition).Length
                .Should().BeLessThan((entity.Position - combatant.Position).Length);
        }

        [TestMethod]
        public void TaskCompletion()
        {
            var mission = new MissionModel(0, EnumMissionAction.Move, combatant.Id, DateTime.Now, SetModule.Empty<int>(), DateTime.Now, entity.Position, 1);
            world.Apply(new EntityUpdateEvent(combatant, "Test entity event"));
            world.Apply(new MissionUpdateEvent(mission, "Test mission event"));
            world.Apply(new EntityUpdateEvent(entity, "Test entity event"));

            world.Update(DateTime.Now);
            world.History.Head.Task.Count
                .Should().Be(1);

            world.Apply(new EntityUpdateEvent(combatant.Move(combatant.MobileState, entity.Position, entity.Rotation, DateTime.Now), "Test move to target"));

            world.Update(DateTime.Now);
            world.History.Head.Task.Count
                .Should().Be(0);
        }

        [TestMethod]
        public void FactoryProduction()
        {
            world.Apply(new EntityUpdateEvent(entity, "Test entity event"));
            world.Apply(new ProductionStartedEvent(1, 2, "Test factory production"));
            var when = DateTime.Now;
            world.Update(when + TimeSpan.FromSeconds(1));
            world.History.Head.EntityProducing.Count
                .Should().Be(1);
            world.History.Head.Entity.Count
                .Should().Be(1);
            world.Update(when + TimeSpan.FromSeconds(10));
            world.History.Head.EntityProducing.Count
                .Should().Be(1);
            world.History.Head.Entity.Count
                .Should().Be(2);
        }
    }
}
