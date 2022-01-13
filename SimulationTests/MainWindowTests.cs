using ABB.InSecTT.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using SimulatorUI;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace SimulationTests
{
    public class STATestMethodAttribute : TestMethodAttribute // Customized test for running STA thread, from https://github.com/microsoft/XamlBehaviorsWpf/blob/master/Test/UnitTests/STATestMethodAttribute.cs
    {
        public override TestResult[] Execute(ITestMethod testMethod)
        {
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
                return Invoke(testMethod);

            TestResult[] result = null;
            var thread = new Thread(() => result = Invoke(testMethod));
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
            return result;
        }

        private TestResult[] Invoke(ITestMethod testMethod)
        {
            return new[] { testMethod.Invoke(null) };
        }
    }

    [TestClass, RequiresSTA]
    public class MainWindowTests
    {
        // DSD Vår - Unit tests for main window
        private IParameterDataBase parameters;
        private IEnumerable<IModule> modules;
        private string configFilePath;

        //Main main = new Main(parameters, modules, configFilePath);

        // Variables for SwitchViewTest
        private Page expectedCurr;
        private Page result;
        private object sender;
        private RoutedEventArgs e;

        // Variables for MainWIndow (but think i can just use main.)
        private TankModule T1;
        private TankModule T2;
        private List<TankModule> tankList = new List<TankModule>();



        [STATestMethod]
        public void SwitchViewTest()
        {
            // DSD Vår - This test is currently just checking if the currentPage is different before and after running switchView. 

            T1 = new TankModule("T1");
            T2 = new TankModule("T2");
            tankList.Add(T1); //need to run initialize tanks to create tankList with the config path 
            tankList.Add(T2);

            if (System.Windows.Application.Current == null)
            { new System.Windows.Application { ShutdownMode = ShutdownMode.OnExplicitShutdown }; }

            Application.Current.Dispatcher.Invoke(delegate
            {
                MainWindow mainWindow = new MainWindow(tankList, "test", 277)
                {
                    currentPage = new SimulationPage(tankList, 277)
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
