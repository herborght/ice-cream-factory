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
        List<Rectangle> barList;
        public MainWindow(List<TankModule> list)
        {
            tankList = list;
            InitializeComponent();
            createTanks();
            Task.Run(() => UpdateVisuals());
        }

        private void createTanks()
        {
            int height = 200;
            int time = 0;
            int fromTop = 20;
            barList = new List<Rectangle>();
            foreach (TankModule tank in tankList)
            {
                if (time == 3)
                {
                    fromTop += 100;
                    time = 1;
                }

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 100;
                rectangle.Height = height;
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                rectangle.Fill = blueBrush;
                Canvas.SetLeft(rectangle, time * 200 + 20);
                Canvas.SetTop(rectangle, fromTop);

                TextBlock textBlock = new TextBlock();


                Rectangle other = new Rectangle();
                other.Uid = tank.Name;
                other.Width = 100;
                other.Height = 200;
                SolidColorBrush red = new SolidColorBrush();
                red.Color = Colors.Black;
                other.Fill = red;
                Canvas.SetLeft(other, time * 200 + 20);
                Canvas.SetTop(other, fromTop);
                barList.Add(other);

                canvas.Children.Add(rectangle);
                canvas.Children.Add(other);

                time++;
            }
        }

        internal async Task UpdateVisuals()
        {
            for (; ; )
            {
                foreach (Rectangle r in barList)
                {
                    bool uiAccess = r.Dispatcher.CheckAccess();
                    if (uiAccess)// as it is a async task it needs to check permissions
                    {
                        string name = r.Uid;
                        TankModule current = tankList.Find(x => x.Name == name);
                        r.Height = 200 - 200 * current.LevelPercentage / 100;//Canvas seems to see height from top to down
                    }
                    else
                    {
                        r.Dispatcher.Invoke(() => { r.Height = 200 - 200 * tankList.Find(x => x.Name == r.Uid).LevelPercentage / 100; }); 
                    }
                        

                }
                await Task.Delay(1000);
            }
        }

        //internal async Task updateLoop()
        //{
        //    for (; ; )
        //    {

        //        bool uiAccess = testBlock.Dispatcher.CheckAccess();
        //        string msg = "";
        //        foreach (TankModule tank in tankList) //Update with the config files
        //        {
        //            msg += "Tank Information: " + "\n";
        //            msg += "Name: " + tank.Name + "\n";
        //            msg += "Level: " + Math.Round(tank.Level, 3) + "\n";
        //            msg += "Percent: " + Math.Round(tank.LevelPercenatage, 3) + "%" +"\n";
        //            msg += "Temperature: " + Math.Round(tank.Temperature, 3) + "\n";
        //            msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "\n";
        //            msg += "InFlow Temp: " + Math.Round(tank.InFlowTemp, 3) + "\n";
        //            msg += "OutletFlow: " + Math.Round(tank.OutLetFlow, 3) + "\n";
        //            msg += "OutletFlow Temp: " + Math.Round(tank.OutFlowTemp, 3) + "\n";
        //            msg += "\n";
        //            msg += "Valve Information: " + "\n";
        //            msg += tank.Name + " Dump Valve: "  + tank.DumpValveOpen + "\n";
        //            msg += tank.Name + " Out Valve: " +  tank.OutValveOpen + "\n";

        //            msg += "\n";
        //        }
        //        if (uiAccess)
        //            testBlock.Text = msg;
        //        else
        //            testBlock.Dispatcher.Invoke(() => { testBlock.Text = msg; });
        //        await Task.Delay(1000);
        //    }
        //}
    }
}
