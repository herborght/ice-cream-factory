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
        List<Rectangle> barList; //List of the rectangles visualizing the tank level
        List<KeyValuePair<string, KeyValuePair<int, Point>>> pointList; //The points of connectections for the tanks
        List<Ellipse> connectedValves; //The visualization of the valves
        List<Ellipse> dumpValves;
        List<TextBlock> labels;
        List<Expander> detailsExpanders;
        List<TextBlock> symbols; //Could be replaced with images, for example pasteurization could use a snowflake and a flame
        public SimulationPage(List<TankModule> list)
        {
            tankList = list;
            InitializeComponent();
            createTanks();
            Task.Run(() => UpdateVisuals());

        }

        private void createTanks()
        {
            int height = 200; //General height of the tank elements
            int time = 0; //How many tanks are in this row
            int fromTop = 0; //The height of the row
            int rows = 0; //Shows which row is the current
            int distance = 230; //Distance between each tank
            barList = new List<Rectangle>();
            pointList = new List<KeyValuePair<string, KeyValuePair<int, Point>>>();
            connectedValves = new List<Ellipse>();
            dumpValves = new List<Ellipse>();
            labels = new List<TextBlock>();
            symbols = new List<TextBlock>();
            detailsExpanders = new List<Expander>();

            foreach (TankModule tank in tankList) //Setup the shapes and connection points
            {
                if (time == 3)
                {
                    fromTop += 300;
                    time = 0;
                    rows++;
                }

                Rectangle rectangle = new Rectangle(); //The rectangle for visualizing the tank level
                rectangle.Width = 75;
                rectangle.Height = height;
                SolidColorBrush blueBrush = new SolidColorBrush();
                blueBrush.Color = Colors.Blue;
                rectangle.Fill = blueBrush;
                Canvas.SetLeft(rectangle, time * distance);
                Canvas.SetTop(rectangle, fromTop);
                rectangle.StrokeThickness = 2;
                rectangle.Stroke = Brushes.Black;

                Expander detailsExpander = new Expander(); // DSD Emil - Expander used for details dropdown
                detailsExpander.Uid = tank.Name;
                TextBlock headerText = new TextBlock();
                headerText.Text = "Name: " + tank.Name;
                headerText.Margin = new Thickness(0, 0, 3, 0);
                detailsExpander.Header = headerText;
                detailsExpander.Background = Brushes.White;
                detailsExpander.BorderBrush = Brushes.Black;
                detailsExpander.BorderThickness = new Thickness(2);
                Canvas.SetZIndex(detailsExpander, 10); // Set z-index to draw ontop ofother elements (such as tank connections)
                Canvas.SetLeft(detailsExpander, time * distance+80);
                Canvas.SetTop(detailsExpander, fromTop-1); // yeah its stupid, but the expander box was visually a tiny bit under the top of the tank
                detailsExpanders.Add(detailsExpander);

                Rectangle other = new Rectangle(); //The rectangle showing how empty the tank is 
                other.Uid = tank.Name;
                other.Width = 75;
                other.Height = 200;
                SolidColorBrush red = new SolidColorBrush();
                red.Color = Colors.White;
                other.Fill = red;
                Canvas.SetLeft(other, time * distance);
                Canvas.SetTop(other, fromTop);
                other.StrokeThickness = 2;
                other.Stroke = Brushes.Black;
                barList.Add(other);

                Point point = new Point();//The point at which the tank will have its connections
                point.X = time * distance + 37.5;
                point.Y = fromTop + height;
                KeyValuePair<string, KeyValuePair<int, Point>> keyValuePair = new KeyValuePair<string, KeyValuePair<int, Point>>(tank.Name, new KeyValuePair<int, Point>(rows, point)); //Added in this way to get which row they are on
                pointList.Add(keyValuePair);

                Ellipse dumpValve = new Ellipse(); //Dump valves will probably have to improve the visuals of these, or change their position not really intuitive 
                dumpValve.Width = 10;
                dumpValve.Height = 10;
                dumpValve.Fill = Brushes.Black;
                dumpValve.StrokeThickness = 2;
                dumpValve.Stroke = Brushes.Black;
                dumpValve.Uid = "d_"+tank.Name;
                Canvas.SetLeft(dumpValve, time * distance - 10);
                Canvas.SetTop(dumpValve, fromTop + height / 2);
                dumpValves.Add(dumpValve);

                canvas.Children.Add(rectangle); //Draw the elements
                canvas.Children.Add(other);
                canvas.Children.Add(dumpValve);
                canvas.Children.Add(detailsExpander);

                if (tank is PasteurizationModule || tank is HomogenizationModule || tank is FreezingModule || tank is FlavoringHardeningPackingModule)
                {
                    TextBlock symbol = new TextBlock();
                    symbol.Text = "";
                    symbol.Width = 40;
                    symbol.Height = 100;
                    symbol.Name = "symbols_" + tank.Name;
                    symbol.FontSize = 20;
                    Canvas.SetLeft(symbol, time * distance + 35);
                    Canvas.SetTop(symbol, fromTop);
                    symbol.TextWrapping = TextWrapping.Wrap;
                    symbols.Add(symbol);
                    canvas.Children.Add(symbol);
                }

                time++;
            }
            int[] times = new int[rows + 1]; //Used to increment the length of which the lines are apart from eachother
            foreach(TankModule tank in tankList) //Create the connections
            {
                foreach(TankModule connected in tank.InFlowTanks) //This is a bit backward initial is the destination of the connection while target is the source
                {
                    KeyValuePair<int, Point> initialPair = pointList.Find(x => x.Key == tank.Name).Value; //The pairs are used to access the rows and points for the lines
                    KeyValuePair<int, Point> targetPair = pointList.Find(x => x.Key == connected.Name).Value;
                    int initialRow = initialPair.Key;
                    times[initialRow]++;
                    int targetRow = targetPair.Key;
                    if(initialRow != targetRow)
                    {
                        times[targetRow]++;
                    }
                    Polyline line = new Polyline(); //Used to visualize the connections between each tank
                    line.Stroke = Brushes.Black;
                    PointCollection points = new PointCollection(); //Used to specify the points of the line
                    Point initial = initialPair.Value;
                    Point target = targetPair.Value;
                    points.Add(initial);
                    Ellipse ellipse = new Ellipse(); //Used to visualize the valves
                    ellipse.Width = 10;
                    ellipse.Height = 10;
                    ellipse.Fill = Brushes.Black;
                    ellipse.StrokeThickness = 2;
                    ellipse.Stroke = Brushes.Black;
                    ellipse.Uid = tank.Name + "_" + connected.Name; //ID for the valves
                    connectedValves.Add(ellipse);
                    TextBlock label = new TextBlock(); //Label for which valve it is
                    label.Text = connected.Name + "->" + tank.Name + "\n";
                    label.Name = tank.Name + "_"  + connected.Name;
                    labels.Add(label);

                    if (initialRow == targetRow)
                    {
                        Point first = new Point();
                        first.Y = initial.Y + times[initialRow] * 10; //Make the lines look more seperate
                        first.X = initial.X;
                        Point second = new Point();
                        second.Y = first.Y;
                        second.X = target.X;
                        points.Add(first);
                        points.Add(second);

                        Canvas.SetLeft(ellipse, first.X + (second.X - first.X) / 2);
                        Canvas.SetTop(ellipse, first.Y - 5);
                        Canvas.SetLeft(label, first.X + (second.X - first.X) / 2 - 30);
                        Canvas.SetTop(label, first.Y - 40);
                    }
                    else //if the target is not in the same row it will use 4 points instead of 2
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
                        Canvas.SetLeft(label, third.X + 10);
                        Canvas.SetTop(label, second.Y + (third.Y - second.Y) / 2); ;
                    }
                    points.Add(target);
                    line.Points = points;
                    canvas.Children.Add(line);
                    canvas.Children.Add(ellipse);
                    canvas.Children.Add(label);
                }
            }
        }

        internal async Task UpdateVisuals()//Loops through the different elements and updates them
        {
            for (; ; )
            {
                foreach (Rectangle r in barList)
                {
                    r.Dispatcher.Invoke(() =>
                    {
                        string name = r.Uid;
                        TankModule current = tankList.Find(x => x.Name == name);
                        r.Height = 200 - 200 * current.LevelPercentage / 100;
                    });
                }
                foreach (Ellipse v in connectedValves) //White means open, black closed
                {

                    v.Dispatcher.Invoke(() =>
                    {
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
                foreach (Ellipse v in dumpValves)
                {

                    v.Dispatcher.Invoke(() =>
                    {
                        string name = v.Uid;
                        TankModule target = tankList.Find(x => x.Name == name.Split('_')[1]);
                        if (target.DumpValveOpen)
                        {
                            v.Fill = Brushes.White;
                        }
                        else
                        {
                            v.Fill = Brushes.Black;
                        }
                    });
                }
                foreach (Expander expander in detailsExpanders)
                {
                    expander.Dispatcher.Invoke(()=> {
                        string name = expander.Uid;
                        TextBlock content = new TextBlock();
                        content.Text = getTankInfo(name);
                        content.Padding = new Thickness(5, 0, 5, 0);
                        expander.Content = content;
                    });
                }
                foreach (TextBlock label in labels)
                {
                    label.Dispatcher.Invoke(() => {
                        TankModule tank = tankList.Find(x => x.Name == label.Name.Split('_')[0]);
                        if(tank != null)
                        {
                            TankModule connected = tank.InFlowTanks.Find(x => x.Name == label.Name.Split('_')[1]);
                            string msg = connected.Name + "->" + tank.Name + "\n";
                            msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "m3/s\n"; //Could also add the temperatures, will probably have to divide what each shows in other functions, as we should be able to select the details
                            label.Text = msg;
                        }
                    });
                }
                SymbolUpdate(); //Could divide the rest into functions, would probably be easier to modify later on
                await Task.Delay(1000);
            }
        }

        private void SymbolUpdate()
        {
            foreach (TextBlock textBlock in symbols)
            {
                textBlock.Dispatcher.Invoke(() => {
                    TankModule tank = tankList.Find(x => x.Name == textBlock.Name.Split('_')[1]);
                    switch(tank)
                    {
                        case PasteurizationModule p:
                            textBlock.Text = updatePasteurization(p);
                            break;
                        case HomogenizationModule h:
                            textBlock.Text = updateHomogenization(h);
                            break;
                        case FlavoringHardeningPackingModule fhp:
                            textBlock.Text = updateFlavoringHardeningPacking(fhp);
                            break;
                        case FreezingModule f:
                            textBlock.Text = updateFreezing(f);
                            break;
                        
                    }                  
                });
            }
        }

        private string updatePasteurization(PasteurizationModule temp)
        {
            string ret = "";
            if (temp.HeaterOn)
            {
                ret = "+";
                if (temp.CoolerOn)
                {
                    ret += "/-"; //Not really sure if this should be possible, as the result is NaN
                }
            }
            else if (temp.CoolerOn)
            {
                ret = "-";
            }
            else
            {
                ret = "";
            }
            return ret;
        }
        
        private string updateHomogenization(HomogenizationModule temp) //Present cooler and pressure in raw data instead
        {
            string ret = "";
            if(temp.HomogenizationOn)
            {
                ret = "H ";
            }
            if(temp.AgeingCoolingOn)
            {
                ret += "A-";
            }
            return ret;
        }

        private string updateFlavoringHardeningPacking(FlavoringHardeningPackingModule temp) //Represent Mix temp, cooler temp, package form (or with an image) in raw data
        {
            string ret = "";
            if (temp.StartFlavoring)
            {
                ret = "F ";
            }
            if (temp.StartHardening)
            {
                ret += "H ";
            }
            if (temp.StartPackaging)
            {
                ret += "P ";
            }
            if(temp.FinishBatch)
            {
                ret = "Fin";
            }
            return ret;
        }

        private string updateFreezing(FreezingModule temp) //Represent sending test values? Represent others via raw data
        {
            string ret = "";
            if (temp.FreezingOn)
            {
                ret = "-";
            }
            if (temp.DasherOn)
            {
                ret += "D";
            }
            if (temp.StartLiquidFlavoring) //Maybe do visually
            {
                ret += "LF";
            }
            return ret;
        }

        private string getTankInfo(string name)
        {
            //string msg = ""; //Old with all of the info
            //TankModule tank = tankList.Find(x => x.Name == name);
            //msg += "Name: " + tank.Name + "\n";
            //msg += "Level: " + Math.Round(tank.Level, 3) + " m \n";
            //msg += "Percent: " + Math.Round(tank.LevelPercentage, 3) + "%" + "\n";
            //msg += "Temp: " + Math.Round(tank.Temperature, 3) + "\n";
            //if(tank.InFlowTanks.Count > 0)
            //{
            //    msg += "InFlow from: ";
            //    foreach(var t in tank.InFlowTanks)
            //    {
            //        msg += t.Name + " ";
            //    }
            //    msg += "\n";
            //}
            //msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "m3/s\n"; 
            //msg += "InFow Temp: " + Math.Round(tank.InFlowTemp, 3) + "K\n";
            //msg += "OutFlow: " + Math.Round(tank.OutLetFlow, 3) + "K\n";
            //msg += "OutFlow Temp: " + Math.Round(tank.OutFlowTemp, 3) + "K\n";
            //msg += tank.Name + " Dmp. Valve: " + tank.DumpValveOpen + "\n";
            //msg += tank.Name + " Out Valve: " + tank.OutValveOpen + "\n";
            //msg += "\n";
            //return msg;

            string msg = "";
            TankModule tank = tankList.Find(x => x.Name == name);
            msg += "Level: " + Math.Round(tank.Level, 3) + "m\n";
            msg += "Percent: " + Math.Round(tank.LevelPercentage, 3) + "%" + "\n";
            msg += "Temp: " + Math.Round(tank.Temperature, 3) + "K\n";
            msg += "InFlow: " + Math.Round(tank.InletFlow, 3) + "m3/s\n";
            msg += "InFow Temp: " + Math.Round(tank.InFlowTemp, 3) + "K\n";
            msg += "OutFlow: " + Math.Round(tank.OutLetFlow, 3) + "m3/s\n";
            msg += "OutFlow Temp: " + Math.Round(tank.OutFlowTemp, 3) + "K\n";
            if (tank.DumpValveOpen)
            {
                msg += "Dump Valve: Open\n";
            }
            else
            {
                msg += "Dump Valve: Closed\n";
            }
            switch (tank)
            {
                case PasteurizationModule p: //Can add Thickness and conductivities if necessary
                    msg += "Heater temp: " + p.HeaterTemp + "K\n";
                    msg += "Cooler temp: " + p.CoolerTemp + "K\n";
                    break;
                case HomogenizationModule h:
                    msg += "Mix temp: " + h.MixTemperature + "K\n";
                    msg += "Particle size: " + h.ParticleSize + "K\n";
                    msg += "Stage 1 pressure: " + h.Stage1Pressure + " Pa\n"; //Don't know the unit, quessing pascal
                    msg += "Stage 2 pressure: " + h.Stage2Pressure + " Pa\n";
                    break;
                case FlavoringHardeningPackingModule fhp:
                    msg += "Mix temp: " + fhp.MixTemperature + "K\n";
                    msg += "Package type: " + fhp.PackageType + "\n";
                    msg += "Cooler temp: " + fhp.CoolerTemperature + "K\n";
                    break;
                case FreezingModule f:
                    msg += "Mix temp: " + f.MixTemperature + "K\n";
                    msg += "Freezer temp: " + f.FreezerTemp + "K\n";
                    msg += "Volume increase: " + f.Overrun + "%\n"; //Precentage of increase in volume after mix and freeze
                    msg += "Barrel rot. speed: " + f.BarrelRotationSpeed + " rad/s\n"; //Don't know which SI unit, quessing rad/s
                    msg += "Units: " + f.PasteurizationUnits + "\n"; //Should probably be visualized, probably ask customer
                    break;
            }
            return msg;
        }
    }

    
}
