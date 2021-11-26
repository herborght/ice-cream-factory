using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TankModule> tankList;
        public MainWindow(List<TankModule> list)
        {
            tankList = list;
            InitializeComponent();
            Task.Run(() => updateLoop());
        }

        internal async Task updateLoop()
        {
            for (; ; )
            {

                bool uiAccess = testBlock.Dispatcher.CheckAccess();
                string msg = "";
                foreach (TankModule tank in tankList) //Update with the config files
                {
                    msg += "Tank Information: " + "\n";
                    msg += "Name: " + tank.Name + "\n";
                    msg += "Level: " + tank.Level + "\n";
                    msg += "Percent: " + tank.LevelPercenatage + "\n";
                    msg += "Temperature: " + tank.Temperature + "\n";
                    msg += "InFlow: " + tank.InletFlow + "\n";
                    msg += "InFlow Temp: " + tank.InFlowTemp + "\n";
                    msg += "OutletFlow: " + tank.OutLetFlow + "\n";
                    msg += "OutletFlow Temp: " + tank.OutFlowTemp + "\n";
                    msg += "\n";
                    msg += "Valve Information: " + "\n";
                    msg += tank.Name + " Dump Valve: "  + tank.DumpValveOpen + "\n";
                    msg += tank.Name + " Out Valve: " +  tank.OutValveOpen + "\n";

                    msg += "\n";
                }
                if (uiAccess)
                    testBlock.Text = msg;
                else
                    testBlock.Dispatcher.Invoke(() => { testBlock.Text = msg; });
                await Task.Delay(1000);
            }
        }
    }
}
