using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.FSharp.Collections;
using FluentAssertions;
using Strive.Common;

namespace Strive.Server.Logic.Tests
{
    [TestClass]
    public class Assumptions
    {
        [TestMethod]
        public void Arithmatic()
        {
            Math.Floor(1.5f)
                .Should().Equals(1f);
            Math.Truncate(1.5f)
                .Should().Equals(1f);
            Math.Floor(-1.5f)
                .Should().Equals(2f);
            Math.Truncate(-1.5f)
                .Should().Equals(2f);
        }

        [TestMethod]
        public void DefaultFSharpMap()
        {
            MapModule.Empty<int, FSharpSet<int>>().ValueOrDefault(5)
                .Should().BeNull();
        }
    }
}
