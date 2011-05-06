using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

            var w = new World(null, 0);
            //var po = new PhysicalObject();
            //w.Add();
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