using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{
    [TestClass]
    public class FreezingModuleTests
    {
        [TestMethod]
        public void ConstructorTest()
        {
            //expacted values 
            string expectedName = "Freeze";
            double expectedFreezerTemp = -20;
            double expectedBarrelRotationSpeed = 100;

            FreezingModule freezingModule = new FreezingModule(expectedName, expectedFreezerTemp, expectedBarrelRotationSpeed);

            //assert values 
            Assert.AreEqual(expectedName, freezingModule.Name);
            Assert.AreEqual(expectedFreezerTemp, freezingModule.FreezerTemp);
            Assert.AreEqual(expectedBarrelRotationSpeed, freezingModule.BarrelRotationSpeed);
        }

        [TestMethod]
        public void GettersAndSettersTest()
        {
            FreezingModule freezingModule = new FreezingModule("Freeze", -20, 100);

            //expected values
            bool expectedFreezingOn = true;
            bool expectedDasherOn = true;
            bool expcectedStartLiquidFlavoring = false;
            double expectedParticleSize = 10;
            double expectedMixTemperature = -20;
            double expectedOverrun = 1;
            double expectedPasteurizationUnits = 10;
            bool expectedSendTestValues = true;

            //set values
            freezingModule.FreezingOn = expectedFreezingOn;
            freezingModule.DasherOn = expectedDasherOn;
            freezingModule.StartLiquidFlavoring = expcectedStartLiquidFlavoring;
            freezingModule.ParticleSize = expectedParticleSize;
            freezingModule.MixTemperature = expectedMixTemperature;
            freezingModule.Overrun = expectedOverrun;
            freezingModule.PasteurizationUnits = expectedPasteurizationUnits;
            freezingModule.SendTestValues = expectedSendTestValues;

            //assert values 
            Assert.AreEqual(expectedFreezingOn, freezingModule.FreezingOn);
            Assert.AreEqual(expectedDasherOn, freezingModule.DasherOn);
            Assert.AreEqual(expcectedStartLiquidFlavoring, freezingModule.StartLiquidFlavoring);
            Assert.AreEqual(expectedParticleSize, freezingModule.ParticleSize);
            Assert.AreEqual(expectedMixTemperature, freezingModule.MixTemperature);
            Assert.AreEqual(expectedPasteurizationUnits, freezingModule.PasteurizationUnits);
            Assert.AreEqual(expectedSendTestValues, freezingModule.SendTestValues);
            Assert.AreEqual(expectedOverrun, freezingModule.Overrun);

        }
    }
}
