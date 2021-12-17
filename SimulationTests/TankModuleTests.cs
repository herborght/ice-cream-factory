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
            Assert.AreEqual(name, T1.Name);
            Assert.AreEqual(0, T1.InFlowTanks.Count);
        }

        TankModule T1 = new TankModule("T1");
        [TestMethod]
        public void SettersTest()
        {
            /* Testing if the setters actuallt sets the values it is supposed to */
            T1.Height = 2;
            Assert.AreEqual(2, T1.Height, "Height setter does not work properly");
            T1.DumpValveOpen = false;
            Assert.AreEqual(T1.DumpValveOpen, false);
            T1.InFlowTemp = 277;
            Assert.AreEqual(T1.InFlowTemp, 277);
            T1.Level = 1.5;
            Assert.AreEqual(T1.Level, 1.5);
            double pres = 1.5 / 2;
            T1.LevelPercentage = pres;
            Assert.AreEqual(T1.LevelPercentage, pres);
            T1.InletFlow = 0.008;
            Assert.AreEqual(T1.InletFlow, 0.008);
            T1.OutFlowTemp = 277;
            Assert.AreEqual(T1.OutFlowTemp, 277);
            T1.OutValveOpen = false;
            Assert.AreEqual(T1.OutValveOpen, false);
            T1.Temperature = 277;
            Assert.AreEqual(T1.Temperature, 277);
        }


        [TestMethod]
        public void ConnectionTest()
        {
            //Adds tanks and assigns them as the inflow for others, test if they exist.
            //Cannot test if value for inflow and outflow is the same as this is not handled by TankModule
            //As it the simulator itself assigns this.
            TankModule Tank1 = new TankModule("Tank1");
            TankModule Tank2 = new TankModule("Tank2");
            TankModule Tank3 = new TankModule("Tank3");

            Assert.AreEqual(0, Tank1.InFlowTanks.Count);


            Tank1.InFlowTanks.Add(Tank2);
            Tank1.InFlowTanks.Add(Tank3);
            Tank3.InFlowTanks.Add(Tank2);

            Tank2.OutLetFlow = 0.01;

            Tank1.InletFlow = Tank1.InFlowTanks.Find(x=>x.Name == Tank2.Name).OutLetFlow;

            Assert.AreEqual(2, Tank1.InFlowTanks.Count);
            Assert.IsTrue(Tank1.InFlowTanks.Exists(x => x.Name == Tank2.Name));
            Assert.IsTrue(Tank1.InFlowTanks.Exists(x => x.Name == Tank3.Name));
            Assert.IsTrue(Tank3.InFlowTanks.Exists(x => x.Name == Tank2.Name));
            Assert.AreEqual(1, Tank3.InFlowTanks.Count);
            Assert.AreEqual(Tank1.InletFlow, Tank2.OutLetFlow);
        }
    }


}
