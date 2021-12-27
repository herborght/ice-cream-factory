using SimulatorUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;
using NUnit.Framework;
using ABB.InSecTT.Common.Configuration;

namespace SimulationTests
{
    [TestClass, RequiresSTA]
    public class MainWindowTests
    {
        // DSD Vår - Unit tests for main window
        IParameterDataBase parameters;
        IEnumerable<IModule> modules; 
        string configFilePath;
        //Main main = new Main(parameters, modules, configFilePath);

        // Variables for SwitchViewTest
        Page expectedCurr;
        Page result;
        object sender;
        RoutedEventArgs e;

        // Variables for MainWIndow (but think i can just use main.)
        TankModule T1;
        TankModule T2;
        List<TankModule> tankList = new List<TankModule>();



        [TestMethod]
        public void SwitchViewTest()
        {
            // DSD Vår - This test is currently just checking if the currentPage is different before and after running switchView. 

            T1 = new TankModule("T1");
            T2 = new TankModule("T2");
            tankList.Add(T1); //need to run initialize tanks to create tankList with the config path 
            tankList.Add(T2);

            Application.Current.Dispatcher.Invoke(delegate
            {
                MainWindow mainWindow = new MainWindow(tankList)
                {
                    currentPage = new SimulationPage(tankList)
                };
                // Assuming tanklist cant be empty, but it is set in Main and it feels
                // wrong to use Main in this testclass as well.

                expectedCurr = mainWindow.currentPage;
                mainWindow.SwitchView(sender, e);
                result = mainWindow.currentPage;

                Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreNotEqual(expectedCurr, result);
            });
        }
    }
}
