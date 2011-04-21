using System;
using System.IO;
using System.Linq;
using System.Windows.Media.Media3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Strive.Common;
using Strive.Model;
using Strive.Network.Messages.ToClient;

namespace Strive.Network.Messaging.Tests
{
    [TestClass]
    public class CustomFormatterTests
    {
        /// <summary>
        /// Booleans are tricky and require special handling
        /// (in memory they are ints, but on wire we want bytes)
        /// </summary>
        [TestMethod]
        public void SymmetryBool()
        {
            EncDecType(true);
        }

        [TestMethod]
        public void SymmetryComplex()
        {
            EncDecType(new Quaternion(1, 2, 3, 4));
            EncDecType(new Vector3D(1, 2, 3));
            EncDecType(new[] { 1, 2, 3 });
        }

        [TestMethod]
        public void SymmetryBasic()
        {
            EncDecType(0.1f);
            EncDecType(1.1);
            EncDecType(1);
            EncDecType(EnumSkill.AcidBlast);
            EncDecType("");
        }

        private static void EncDecType(object enc)
        {
            var buffer = new MemoryStream();
            CustomFormatter.Encode(enc, buffer, enc.GetType());
            int offset = 0;
            var dec = CustomFormatter.Decode(enc.GetType(), buffer.ToArray(), ref offset);
            AreEqual(enc, dec);
            Assert.AreEqual(buffer.Length, offset);
        }

        [TestMethod]
        public void SymmetryMessages()
        {
            var m = new EntityModel(1, "Foo", "Bar", new Vector3D(1.2, 3, 4), new Quaternion(1.2, 3, 4, 5), 100, 100, EnumMobileState.Standing, 1.7f);
            SerDesMessage(m);
            SerDesMessage(new PositionUpdate(m));
        }

        private static void SerDesMessage(object message)
        {
            var b = CustomFormatter.Serialize(message);
            var o = CustomFormatter.Deserialize(b);
            Assert.AreEqual(o.GetType(), message.GetType());
            AreEqual(message, o);
        }

        private static void AreEqual(object a, object b)
        {
            Type t = a.GetType();
            Assert.AreEqual(t, b.GetType());

            if (t.IsEnum || t.IsPrimitive || t == typeof(string))
                Assert.AreEqual(a, b);
            else if (t.IsArray)
            {
                var aa = (Array)a;
                var bb = (Array)b;
                Assert.AreEqual(aa.Length, bb.Length);
                for (int i = 0; i < aa.Length; ++i)
                    AreEqual(aa.GetValue(i), bb.GetValue(i));
            }
            else
            {
                foreach (var fi in t.GetFields().Where(x => !x.IsStatic))
                    AreEqual(fi.GetValue(a), fi.GetValue(b));
                foreach (var pi in t.GetProperties().Where(x => x.CanWrite))
                    AreEqual(pi.GetValue(a, null), pi.GetValue(b, null));
            }
        }
    }
}
