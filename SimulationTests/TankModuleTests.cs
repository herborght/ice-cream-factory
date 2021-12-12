using System;
using System.Collections.Generic;
using System.Text;
using SimulatorUI;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SimulationTests
{
    [TestClass]
    public  class TankModuleTests
    {

        [TestMethod]
        public void ConstructorTest()
        {
            /*Checkig if the constructor actually sets the name and if inflow tanks are 0*/
            string name = "T1";
            TankModule T1 = new TankModule("T1");
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(name, T1.Name);
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(0, T1.InFlowTanks.Count);
        }

        TankModule T1 = new TankModule("T1");
        TankModule T2 = new TankModule("T2");
        [TestMethod]
        public void SettersTest()
        {
            /* Testing if the setters actuallt sets the values it is supposed to */
            T1.Height = 2;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(2, T1.Height, "Height setter does not work properly");
            T1.DumpValveOpen = false;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.DumpValveOpen, false);
            T1.InFlowTemp = 277;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.InFlowTemp, 277);
            T1.Level = 1.5;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.Level, 1.5);
            double pres = 1.5 / 2;
            T1.LevelPercentage = pres;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.LevelPercentage, pres);
            T1.InletFlow = 0.0075;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.InletFlow, 0.0075);
            T1.OutFlowTemp = 277;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.OutFlowTemp, 277);
            T1.OutValveOpen = false;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.OutValveOpen, false);
            T1.Temperature = 277;
            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(T1.Temperature, 277);
        }
    }
}
