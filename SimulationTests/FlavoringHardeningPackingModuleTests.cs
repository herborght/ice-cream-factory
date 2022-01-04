using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{
    [TestClass]
    public class FlavoringHardeningPackingModuleTests
    {
        [TestMethod]
        public void ConstructorTest()
        {

            //expected values 
            string expectedName = "fhpM";
            string expectedPackageType = "light";
            double expectedCoolerTemperature = -20;

            //constructor call
            FlavoringHardeningPackingModule fhpModule = new FlavoringHardeningPackingModule(expectedName, expectedPackageType, expectedCoolerTemperature);


            //assert values
            Assert.AreEqual(expectedName, fhpModule.Name);
            Assert.AreEqual(expectedPackageType, fhpModule.PackageType);
            Assert.AreEqual(expectedCoolerTemperature, fhpModule.CoolerTemperature);

        }

        [TestMethod]
        public void GettersAndSettersTest()
        {
            FlavoringHardeningPackingModule fhpModule = new FlavoringHardeningPackingModule("module", "tight", -20);

            //expected values 
            bool expectedStartFlavoring = true;
            bool expectedStartHardening = false;
            bool expectedStartPackaging = false;
            bool expectedFinishBatch = false;

            //set values 
            fhpModule.StartFlavoring = expectedStartFlavoring;
            fhpModule.StartHardening = expectedStartHardening;
            fhpModule.StartPackaging = expectedStartPackaging;
            fhpModule.FinishBatch = expectedFinishBatch;

            //assert values 
            Assert.AreEqual(fhpModule.StartFlavoring, expectedStartFlavoring);
            Assert.AreEqual(fhpModule.StartHardening, expectedStartHardening);
            Assert.AreEqual(fhpModule.StartPackaging, expectedStartPackaging);
            Assert.AreEqual(fhpModule.FinishBatch, expectedFinishBatch);

        }
    }
}
