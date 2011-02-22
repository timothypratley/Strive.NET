using System.IO;
using System.Windows.Media.Media3D;
using NUnit.Framework;
using Strive.Network.Messages;
using Strive.Network.Messages.ToClient;
using Strive.Server.Model;

namespace Strive.Network.Messaging.Tests
{
    [TestFixture]
    class CustomFormatterTests
    {
        [Test]
        public void SymmetryBool ()
        {
            EncDecType(true);
        }
            
        [Test]
        public void Symmetry ()
        {
            EncDecType(0.1f);
            EncDecType(1.1);
            EncDecType(1);
            EncDecType(Messages.EnumCombatEvent.Misses);
            EncDecType("");
            EncDecType(Quaternion.Identity);
            EncDecType(new Vector3D(0, 0, 0));
            EncDecType(new [] {1, 2, 3});
        }

        private static void EncDecType(object m1)
        {
            var buffer = new MemoryStream();
            CustomFormatter.Encode(m1,buffer,m1.GetType());
            int offset = 0;
            var o1 = CustomFormatter.Decode(m1.GetType(), buffer.ToArray(), ref offset);
            Assert.AreEqual(o1.GetType(), m1.GetType());
            Assert.AreEqual(buffer.Length, offset);
        }

        [Test]
        public void SymmetryMessages()
        {
            SerDesMessage(new AddMobile(new Mobile()));
            SerDesMessage(new PositionUpdate(new Mobile()));
        }

        private void SerDesMessage(IMessage message)
        {
            var b = CustomFormatter.Serialize(message);
            var o = CustomFormatter.Deserialize(b);
            Assert.AreEqual(o.GetType(), message.GetType());
        }
    }
}
