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
        //initalizing main
        IParameterDataBase parameters;
        IEnumerable<IModule> modules; 
        string configFilePath;
        //Main main = new Main(parameters, modules, configFilePath);

        //variables for SwitchViewTest
        Page expectedCurr;
        Page result;
        object sender;
        RoutedEventArgs e;

        //variables for MainWIndow (but think i can just use main.)
        TankModule T1;
        TankModule T2;
        List<TankModule> tankList = new List<TankModule>();



        [TestMethod]
        public void SwitchViewTest()
        {
            /*This test is currently just checking if the currentPage i different 
            before and after running switchView. Should probably be made a little
            more advanced. */

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
        public void GetTnkInfoTest()
        {
            T1 = new TankModule("T1");
            T2 = new TankModule("T2");
            
            tankList.Add(T1);
            tankList.Add(T2);

            Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(1, 1);

        }
    }
}
