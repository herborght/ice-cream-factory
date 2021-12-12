using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{
    [TestClass]
    class Class1
    {
        [TestMethod]
        public void ConstructorTest()
        {
            string name = "T1";
            TankModule T1 = new TankModule("T1");
            Assert.AreEqual(name, T1.Name);
            Assert.AreEqual(0, T1.InFlowTanks);
        }
    }
}
