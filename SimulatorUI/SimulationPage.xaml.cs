using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for SimulationPage.xaml
    /// </summary>
    public partial class SimulationPage : Page
    {
        List<TankModule> tankList;
        List<Rectangle> barList;
        List<TextBlock> textBlocks;
        public SimulationPage(List<TankModule> list)
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
            textBlocks = new List<TextBlock>();
            foreach (TankModule tank in tankList)
            {
                if (time == 3)
                {
                    fromTop += 225;
                    time = 0;
                }

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 100;
                rectangle.Height = height;
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                rectangle.Fill = blueBrush;
                Canvas.SetLeft(rectangle, time * 190);
                Canvas.SetTop(rectangle, fromTop);

                TextBlock textBlock = new TextBlock();
                textBlock.Width = 250;
                textBlock.Height = height;
                textBlock.Name = tank.Name;
                Canvas.SetLeft(textBlock, time * 190 + 75);
                Canvas.SetTop(textBlock, fromTop);
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Margin = new Thickness(5);
                textBlocks.Add(textBlock);

                Rectangle other = new Rectangle();
                other.Uid = tank.Name;
                other.Width = 75;
                other.Height = 200;
                SolidColorBrush red = new SolidColorBrush();
                red.Color = Colors.White;
                other.Fill = red;
                Canvas.SetLeft(other, time * 190);
                Canvas.SetTop(other, fromTop);
                other.StrokeThickness = 1;
                other.Stroke = Brushes.Black;
                barList.Add(other);

                canvas.Children.Add(rectangle);
                canvas.Children.Add(other);
                canvas.Children.Add(textBlock);

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
                foreach (TextBlock textBlock in textBlocks)
                {
                    bool uiAccess = textBlock.Dispatcher.CheckAccess();
                    if (uiAccess)
                        textBlock.Text = getTankInfo(textBlock.Name);
                    else
                        textBlock.Dispatcher.Invoke(() => { textBlock.Text = getTankInfo(textBlock.Name); });
                }
                await Task.Delay(1000);
            }
        }
        private string getTankInfo(string name)
        {
            string msg = "";
            TankModule tank = tankList.Find(x => x.Name == name);
            msg += "Name: " + tank.Name + "\n";
            msg += "Level: " + Math.Round(tank.Level, 3) + "\n";
            msg += "Percent: " + Math.Round(tank.LevelPercentage, 3) + "%" + "\n";
            msg += "Temp: " + Math.Round(tank.Temperature, 3) + "\n";
            msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "\n";
            msg += "InFow Temp: " + Math.Round(tank.InFlowTemp, 3) + "\n";
            msg += "OutFlow: " + Math.Round(tank.OutLetFlow, 3) + "\n";
            msg += "OutFlw Temp: " + Math.Round(tank.OutFlowTemp, 3) + "\n";
            msg += tank.Name + " Dmp. Valve: " + tank.DumpValveOpen + "\n";
            msg += tank.Name + " Out Valve: " + tank.OutValveOpen + "\n";
            msg += "\n";
            return msg;
        }
    }

    
}
