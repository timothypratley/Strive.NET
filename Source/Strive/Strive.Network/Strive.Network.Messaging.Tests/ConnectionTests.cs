using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;

namespace Strive.Network.Messaging.Tests
{
    [TestClass]
    public class ConnectionTests
    {
        [TestMethod]
        public void CanHost()
        {
            var server = new Listener(new IPEndPoint(IPAddress.Any, 8888));
            server.Start();
        }
    }
}
