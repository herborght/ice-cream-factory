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
    }
}
