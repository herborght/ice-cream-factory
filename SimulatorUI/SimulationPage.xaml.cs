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
        List<KeyValuePair<string, KeyValuePair<int, Point>>> pointList;
        List<Ellipse> connectedValves;
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
            int rows = 0;
            barList = new List<Rectangle>();
            textBlocks = new List<TextBlock>();
            pointList = new List<KeyValuePair<string, KeyValuePair<int, Point>>>();
            connectedValves = new List<Ellipse>();
            foreach (TankModule tank in tankList)
            {
                if (time == 3)
                {
                    fromTop += 300;
                    time = 0;
                    rows++;
                }

                Rectangle rectangle = new Rectangle();
                rectangle.Width = 75;
                rectangle.Height = height;
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                rectangle.Fill = blueBrush;
                Canvas.SetLeft(rectangle, time * 190);
                Canvas.SetTop(rectangle, fromTop);
                rectangle.StrokeThickness = 2;
                rectangle.Stroke = Brushes.Black;

                TextBlock textBlock = new TextBlock();
                textBlock.Width = 250;
                textBlock.Height = height;
                textBlock.Name = tank.Name;
                textBlock.Margin = new Thickness(5);
                Canvas.SetLeft(textBlock, time * 190 + 75);
                Canvas.SetTop(textBlock, fromTop);
                textBlock.TextWrapping = TextWrapping.Wrap;
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
                other.StrokeThickness = 2;
                other.Stroke = Brushes.Black;
                barList.Add(other);

                Point point = new Point();
                point.X = time * 190 + 37.5;
                point.Y = fromTop + height;
                KeyValuePair<string, KeyValuePair<int, Point>> keyValuePair = new KeyValuePair<string, KeyValuePair<int, Point>>(tank.Name, new KeyValuePair<int, Point>(rows, point));

                pointList.Add(keyValuePair);

                canvas.Children.Add(rectangle);
                canvas.Children.Add(other);
                canvas.Children.Add(textBlock);

                time++;
            }
            int[] times = new int[rows + 1]; //Used to increment the length of which the lines are apart from eachother
            foreach(TankModule tank in tankList)
            {
                foreach(TankModule connected in tank.InFlowTanks) //This is a bit backward initial is the destination of the connection while target is the source
                {
                    KeyValuePair<int, Point> initialPair = pointList.Find(x => x.Key == tank.Name).Value;
                    KeyValuePair<int, Point> targetPair = pointList.Find(x => x.Key == connected.Name).Value;
                    int initialRow = initialPair.Key;
                    times[initialRow]++;
                    int targetRow = targetPair.Key;
                    if(initialRow != targetRow)
                    {
                        times[targetRow]++;
                    }
                    Polyline line = new Polyline();
                    line.Stroke = Brushes.Black;
                    PointCollection points = new PointCollection();
                    Point initial = initialPair.Value;
                    Point target = targetPair.Value;
                    points.Add(initial);
                    Ellipse ellipse = new Ellipse();
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                    ellipse.Fill = Brushes.Black;
                    ellipse.StrokeThickness = 2;
                    ellipse.Stroke = Brushes.Black;
                    ellipse.Uid = tank.Name + "_" + connected.Name;
                    connectedValves.Add(ellipse);

                    if (initial.Y == target.Y)
                    {
                        Point first = new Point();
                        first.Y = initial.Y + times[initialRow] * 10;
                        first.X = initial.X;
                        Point second = new Point();
                        second.Y = first.Y;
                        second.X = target.X;
                        points.Add(first);
                        points.Add(second);                    
                        Canvas.SetLeft(ellipse, first.X + (second.X - first.X) / 2);
                        Canvas.SetTop(ellipse, first.Y - 5);
                    }
                    else
                    {
                        Point first = new Point();
                        first.Y = initial.Y + times[initialRow] * 10;
                        first.X = initial.X;
                        Point second = new Point();
                        second.Y = first.Y;
                        second.X = first.X + 75;
                        Point third = new Point();
                        third.Y = target.Y + times[targetRow] * 10;
                        third.X = second.X;
                        Point fourth = new Point();
                        fourth.Y = target.Y + times[targetRow] * 10;
                        fourth.X = target.X;
                        points.Add(first);
                        points.Add(second);
                        points.Add(third);
                        points.Add(fourth);
                        Canvas.SetLeft(ellipse, third.X - 5);
                        Canvas.SetTop(ellipse, second.Y + (third.Y - second.Y) / 2);
                    }
                    points.Add(target);
                    line.Points = points;
                    canvas.Children.Add(line);
                    canvas.Children.Add(ellipse);
                }
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
                foreach (Ellipse v in connectedValves)
                {
                    bool uiAccess = v.Dispatcher.CheckAccess();
                    if (uiAccess)// as it is a async task it needs to check permissions
                    {
                        string name = v.Uid;
                        TankModule target = tankList.Find(x => x.Name == name.Split('_')[0]);
                        TankModule source = target.InFlowTanks.Find(x => x.Name == name.Split('_')[1]);
                        if (source != null)
                        {
                            if(source.OutValveOpen && target.InletFlow > 0)
                            {
                                v.Fill = Brushes.White;
                            }
                            else
                            {
                                v.Fill = Brushes.Black;
                            }
                        }

                    }
                    else
                    {
                        v.Dispatcher.Invoke(() => {
                            string name = v.Uid;
                            TankModule target = tankList.Find(x => x.Name == name.Split('_')[0]);
                            TankModule source = target.InFlowTanks.Find(x => x.Name == name.Split('_')[1]);
                            if (source != null)
                            {
                                if (source.OutValveOpen && target.InletFlow > 0)
                                {
                                    v.Fill = Brushes.White;
                                }
                                else
                                {
                                    v.Fill = Brushes.Black;
                                }
                            }
                        });
                    }
                }
                //foreach (TextBlock textBlock in textBlocks)
                //{
                //    bool uiAccess = textBlock.Dispatcher.CheckAccess();
                //    if (uiAccess)
                //        textBlock.Text = getTankInfo(textBlock.Name);
                //    else
                //        textBlock.Dispatcher.Invoke(() => { textBlock.Text = getTankInfo(textBlock.Name); });
                //}
                await Task.Delay(1000);
            }
        }
        private string getTankInfo(string name)
        {
            string msg = "";
            TankModule tank = tankList.Find(x => x.Name == name);
            msg += "Name: " + tank.Name + "\n";
            msg += "Level: " + Math.Round(tank.Level, 3) + " m \n";
            msg += "Percent: " + Math.Round(tank.LevelPercentage, 3) + "%" + "\n";
            msg += "Temp: " + Math.Round(tank.Temperature, 3) + "\n";
            if(tank.InFlowTanks.Count > 0)
            {
                msg += "InFlow from: ";
                foreach(var t in tank.InFlowTanks)
                {
                    msg += t.Name + " ";
                }
                msg += "\n";
            }
            msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "m3/s\n"; 
            msg += "InFow Temp: " + Math.Round(tank.InFlowTemp, 3) + "K\n";
            msg += "OutFlow: " + Math.Round(tank.OutLetFlow, 3) + "K\n";
            msg += "OutFlw Temp: " + Math.Round(tank.OutFlowTemp, 3) + "K\n";
            msg += tank.Name + " Dmp. Valve: " + tank.DumpValveOpen + "\n";
            msg += tank.Name + " Out Valve: " + tank.OutValveOpen + "\n";
            msg += "\n";
            return msg;
        }
    }

    
}
