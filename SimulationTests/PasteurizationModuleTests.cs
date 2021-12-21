using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{

    [TestClass]
    public class PasteurizationModuleTests
    {

        PasteurizationModule pModule; 

        [TestMethod]
        public void ConstructorTest()
        {
            string expectedName = "T1";
            pModule = new PasteurizationModule(expectedName);
            Assert.AreEqual(expectedName, pModule.Name);
        }

        [TestMethod]
        public void SettersAndGettersTest()
        {
            pModule = new PasteurizationModule("T1");

            //expected results
            Boolean expectedHeaterOn = true;
            Boolean expectedCoolerOn = false;
            double expectedHeaterTemp = 40;
            double expectedCoolerTemp = 0;
            double expectedThickness = 2;
            double expectedHeaterConductivity = 3;
            double expectedCoolerConductivity = 0;

            //set values 
            pModule.HeaterOn = expectedHeaterOn;
            pModule.CoolerOn = expectedCoolerOn;
            pModule.HeaterTemp = expectedHeaterTemp;
            pModule.CoolerTemp = expectedCoolerTemp;
            pModule.Thickness = expectedThickness;
            pModule.HeaterConductivity = expectedHeaterConductivity;
            pModule.CoolerConductivity = expectedCoolerConductivity;

            //assert values 
            Assert.AreEqual(expectedHeaterOn, pModule.HeaterOn);
            Assert.AreEqual(expectedCoolerOn, pModule.CoolerOn);
            Assert.AreEqual(expectedHeaterTemp, pModule.HeaterTemp);
            Assert.AreEqual(expectedCoolerTemp, pModule.CoolerTemp);
            Assert.AreEqual(expectedThickness, pModule.Thickness);
            Assert.AreEqual(expectedHeaterConductivity, pModule.HeaterConductivity);
            Assert.AreEqual(expectedCoolerConductivity, pModule.CoolerConductivity);
        }

    }
}
