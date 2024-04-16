using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Droidbot.Display;
using Sandbox.ModAPI.Ingame;

namespace Tests
{

    public class MockGridProgram : MyGridProgram {

    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
           State s = new State(new MockGridProgram());
        }
    }
}
