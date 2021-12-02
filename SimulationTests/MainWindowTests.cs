using SimulatorUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows;

namespace SimulationTests
{
    [TestClass()]
    public class MainWindowTests
    {

        Page expectedCurr;
        Page result;
        object sender;
        RoutedEventArgs e;
        List<TankModule> tankList = new List<TankModule>();

        [TestMethod]
        /*This test is currently just checking if the currentPage i different 
        before and after running switchView. Should probably be made a little
        more advanced. */
        public void SwitchViewTest()
        {
            MainWindow mainWindow = new MainWindow(tankList); 
            //denne sender jo bare innen tom liste

            expectedCurr = mainWindow.currentPage;
            mainWindow.SwitchView(sender, e);
            result = mainWindow.currentPage;

            Assert.AreNotEqual(expectedCurr, result);
        }
    }
}
