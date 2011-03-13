using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using clojure.lang;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using Microsoft.FSharp.Collections;


namespace Strive.DataModel.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class CompareRecordedMaps
    {
        public CompareRecordedMaps()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        class Obj
        {
            public int A { get; private set; }
            public string B { get; private set; }
            public Obj(int a, string b)
            {
                A = a;
                B = b;
            }
        }

        [TestMethod]
        public void TestMethod1()
        {
            Var.pushThreadBindings(RT.map(RT.CURRENT_NS, RT.CURRENT_NS.deref()));
            var m1 = new RecordedMapModel<string, Obj>();
            var m2 = new ClojureRecordedMapModel<string, Obj>();
            var m3 = new Dictionary<string, Obj>();
            var m4 = new FSharpMap<string, Obj>(Enumerable.Empty<Tuple<string,Obj>>());
            var m5 = new PersistentTreeMap();

            const int size = 1000;
            var s = new Stopwatch();
            s.Start();
            for (var i = 0; i < size; i++)
                m1.Set(i.ToString(), new Obj(i, i.ToString()));
            s.Stop();
            Console.WriteLine("RecordedMapModel took " + s.Elapsed);
            s.Restart();
            for (var i = 0; i < size; i++)
                m2.Set(i.ToString(), new Obj(i, i.ToString()));
            s.Stop();
            Console.WriteLine("ClojureRecordedMapModel took " + s.Elapsed);
            s.Restart();
            for (var i = 0; i < size*100; i++)
                m3[i.ToString()] = new Obj(i, i.ToString());
            s.Stop();
            Console.WriteLine("Dictionary took " + s.Elapsed);
            s.Restart();
            for (var i = 0; i < size*100; i++)
                m4 = m4.Add(i.ToString(), new Obj(i, i.ToString()));
            s.Stop();
            Console.WriteLine("FSharpMap took " + s.Elapsed);
            s.Restart();
            for (var i = 0; i < size*100; i++)
                m5 = (PersistentTreeMap)m5.assoc(i.ToString(), new Obj(i, i.ToString()));
            s.Stop();
            Console.WriteLine("PersistentTreeMap took " + s.Elapsed);
        }
    }
}
