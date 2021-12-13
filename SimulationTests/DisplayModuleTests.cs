using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{
    [TestClass]
    public class DisplayModuleTests
    {
        TankModule tank = new TankModule("T1");
        displayModule displayModule;

        [TestMethod]
        public void DisplayModule_DisplaysRightNametest()
        {
            displayModule = new displayModule(tank);
            Assert.AreEqual(tank.Name, displayModule.Name);

        }

        [TestMethod]
        public void DisplayModule_DisplaysRightValuesTest()
        {
            displayModule = new displayModule(tank);

            //expected results 
            string expectedLevel = Math.Round(tank.Level, 3).ToString();
            string expectedPercentage = Math.Round(tank.LevelPercentage, 3).ToString() + "%";
            string expectedTemperature = Math.Round(tank.Temperature, 3).ToString() + " K";
            string expectedInFlow = Math.Round(tank.InletFlow, 3) + " m3/s";
            string expectedInFlowTemp = Math.Round(tank.InFlowTemp, 3) + " K";
            string expectedOutFlow = Math.Round(tank.OutLetFlow, 3) + " m3/s";
            string expectedOutFlowTemp = Math.Round(tank.OutFlowTemp, 3) + " K";

            //assert values 
            Assert.AreEqual(expectedLevel, displayModule.Level);
            Assert.AreEqual(expectedPercentage, displayModule.Percent);
            Assert.AreEqual(expectedTemperature, displayModule.Temperature);
            Assert.AreEqual(expectedInFlow, displayModule.InFlow);
            Assert.AreEqual(expectedInFlowTemp, displayModule.InFlowTemp);
            Assert.AreEqual(expectedOutFlow, displayModule.OutFlow);
            Assert.AreEqual(expectedOutFlowTemp, displayModule.OutFlowTemp);
        }

        [TestMethod]
        public void DisplayModule_DisplaysOpenClosedDmpValveTest()
        {
            tank.DumpValveOpen = true; //could set randomly to check all cases
            tank.OutValveOpen = false; //could set randomly to check all cases (alternatively writing different test for the different cases=
            displayModule = new displayModule(tank);

            //expected results
            string expectedDmpValve;
            if (tank.DumpValveOpen)
            {
                expectedDmpValve = "Open";
            }
            else
            {
                expectedDmpValve = "Closed";
            }

            string expectedOutValve;
            if (tank.OutValveOpen)
            {
                expectedOutValve = "Open";
            }
            else
            {
                expectedOutValve = "Closed";
            }

            //assert values
            Assert.AreEqual(expectedDmpValve, displayModule.DmpValve);
            Assert.AreEqual(expectedOutValve, displayModule.OutValve);
        }
    }
}
