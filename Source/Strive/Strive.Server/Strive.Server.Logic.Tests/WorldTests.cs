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

            var w = new World(0);
            w.Load();
            //var po = new PhysicalObject();
            //w.Add();
        }
    }
}
