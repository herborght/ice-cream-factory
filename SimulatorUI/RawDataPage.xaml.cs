using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulatorUI
{
    
    public partial class RawDataPage : Page
    {
        List<TankModule> tankList;
        public RawDataPage(List<TankModule> list)
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
                foreach (TankModule tank in tankList)
                {
                    /* Old version, doesnt display all properties correctly
                    msg += "Tank Information: " + "\n";
                    msg += "Name: " + tank.Name + "\n";
                    msg += "Level: " + Math.Round(tank.Level, 2) + "\n";
                    msg += "Percent: " + Math.Round(tank.LevelPercentage, 2) + "%" + "\n";
                    msg += "Temperature: " + Math.Round(tank.Temperature, 2) + "\n";
                    if (tank.InFlowTanks.Count > 0)
                    {
                        msg += "InFlow from: ";
                        foreach (var t in tank.InFlowTanks)
                        {
                            msg += t.Name + " ";
                        }
                        msg += "\n";
                    }
                    msg += "InFlow: " + Math.Round(tank.InletFlow, 2) + "\n";
                    msg += "InFlow Temp: " + Math.Round(tank.InFlowTemp, 2) + "\n";
                    msg += "OutletFlow: " + Math.Round(tank.OutLetFlow, 2) + "\n";
                    msg += "OutletFlow Temp: " + Math.Round(tank.OutFlowTemp, 2) + "\n";
                    msg += "\n";
                    msg += "Valve Information: " + "\n";
                    msg += tank.Name + " Dump Valve: " + tank.DumpValveOpen + "\n";
                    msg += tank.Name + " Out Valve: " + tank.OutValveOpen + "\n";
                    */

                    // Copied from SimulationPage.xaml.cs since all tanks' properties are displayed properly there
                    msg += "\n";

                    msg += "Tank Information: " + "\n";
                    msg += "Name: " + tank.Name + "\n";
                    msg += "Level: " + Math.Round(tank.Level, 3) + " m \n";
                    msg += "Percent: " + Math.Round(tank.LevelPercentage, 3) + "%" + "\n";
                    msg += "Temp: " + Math.Round(tank.Temperature, 3) + " K\n";
                    msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + " m3/s\n";
                    msg += "InFow Temp: " + Math.Round(tank.InFlowTemp, 3) + " K \n";
                    msg += "OutFlow: " + Math.Round(tank.OutLetFlow, 3) + " m3/s\n";
                    msg += "OutFlw Temp: " + Math.Round(tank.OutFlowTemp, 3) + " K\n";
                    msg += "\n";
                    msg += "Valve Information: " + "\n";
                    msg += tank.Name + " Dmp. Valve: " + tank.DumpValveOpen + "\n";
                    msg += tank.Name + " Out Valve: " + tank.OutValveOpen + "\n";
                    if (tank.InFlowTanks.Count > 0)
                    {
                        msg += "InFlow from: ";
                        foreach (var t in tank.InFlowTanks)
                        {
                            msg += t.Name + " ";
                        }
                        msg += "\n";
                    }
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
