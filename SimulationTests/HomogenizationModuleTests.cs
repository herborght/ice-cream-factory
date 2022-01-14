using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{
    [TestClass]
    public class HomogenizationModuleTests
    {
        private HomogenizationModule hModule;

        [TestMethod]
        public void ConstructorTest()
        {
            //expected values
            string expectedName = "H1";
            double expectedStage1Pressure = 2;
            double expectedStage2Pressure = 5;

            //set values 
            hModule = new HomogenizationModule(expectedName, expectedStage1Pressure, expectedStage2Pressure);

            //assert values
            Assert.AreEqual(expectedName, hModule.Name);
            Assert.AreEqual(expectedStage1Pressure, hModule.Stage1Pressure);
            Assert.AreEqual(expectedStage2Pressure, hModule.Stage2Pressure);

        }

        [TestMethod]
        public void GettersAndSettersTest()
        {
            hModule = new HomogenizationModule("H1", 4, 5);

            //expected values
            bool expectedHomogenizationOn = true;
            bool expectedAgeingCoolingOn = false;
            double expectedParticleSize = 3;
            double expectedMixTemperature = 100;

            //set values 
            hModule.HomogenizationOn = expectedHomogenizationOn;
            hModule.AgeingCoolingOn = expectedAgeingCoolingOn;
            hModule.ParticleSize = expectedParticleSize;
            hModule.MixTemperature = expectedMixTemperature;

            //assert values 
            Assert.AreEqual(hModule.HomogenizationOn, expectedHomogenizationOn);
            Assert.AreEqual(hModule.AgeingCoolingOn, expectedAgeingCoolingOn);
            Assert.AreEqual(hModule.ParticleSize, expectedParticleSize);
            Assert.AreEqual(hModule.MixTemperature, expectedMixTemperature);
        }

    }
}
