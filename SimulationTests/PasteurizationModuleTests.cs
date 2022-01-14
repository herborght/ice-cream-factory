using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimulatorUI;

namespace SimulationTests
{

    [TestClass]
    public class PasteurizationModuleTests
    {
        // DSD Vår - Unit tests for pasteurization module
        private PasteurizationModule pModule;

        [TestMethod]
        public void ConstructorTest()
        {
            string expectedName = "T1";
            double expectedHeaterTemp = 40;
            double expectedCoolerTemp = 0;
            double expectedThickness = 2;
            double expectedHeaterConductivity = 3;
            double expectedCoolerConductivity = 0;

            pModule = new PasteurizationModule(expectedName, expectedHeaterTemp, expectedCoolerTemp, expectedThickness, expectedHeaterConductivity, expectedCoolerConductivity);
            Assert.AreEqual(expectedName, pModule.Name);
            Assert.AreEqual(expectedHeaterTemp, pModule.HeaterTemp);
            Assert.AreEqual(expectedCoolerTemp, pModule.CoolerTemp);
            Assert.AreEqual(expectedHeaterConductivity, pModule.HeaterConductivity);
            Assert.AreEqual(expectedCoolerConductivity, pModule.CoolerConductivity);
        }

        [TestMethod]
        public void SettersAndGettersTest()
        {

            // Expected results
            bool expectedHeaterOn = true;
            bool expectedCoolerOn = false;
            double expectedHeaterTemp = 40;
            double expectedCoolerTemp = 0;
            double expectedThickness = 2;
            double expectedHeaterConductivity = 3;
            double expectedCoolerConductivity = 0;
            pModule = new PasteurizationModule("T1", expectedHeaterTemp, expectedCoolerTemp, expectedThickness, expectedHeaterConductivity, expectedCoolerConductivity)
            {
                HeaterOn = expectedHeaterOn,
                CoolerOn = expectedCoolerOn
            };

            // Assert values 
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
