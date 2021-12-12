using SimulatorUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using NUnit.Framework;

namespace SimulationTests
{
    [TestClass, RequiresSTA]
    public class MainWindowTests
    {

        Page expectedCurr;
        Page result;
        object sender;
        RoutedEventArgs e;
        List<TankModule> tankList = new List<TankModule>();

        [TestMethod]
        public void SwitchViewTest()
        {
        /*This test is currently just checking if the currentPage i different 
        before and after running switchView. Should probably be made a little
        more advanced. */

            Application.Current.Dispatcher.Invoke(delegate
            {
                MainWindow mainWindow = new MainWindow(tankList)
                {
                    currentPage = new SimulationPage(tankList)
                };
                //denne sender jo bare innen tom liste, får nullreferene exception
                //assuming tanklist cant be empty, but it is set in Main and it feels
                // wrong to use Main in this testclass as well.

                expectedCurr = mainWindow.currentPage;
                mainWindow.SwitchView(sender, e);
                result = mainWindow.currentPage;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(expectedCurr, result);
            });
        }
        [TestMethod]
        public void ConstructorTest()
        {
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
