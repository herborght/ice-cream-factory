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
        List<Rectangle> tankLevelList; // List of the rectangles visualizing the tank level
        List<KeyValuePair<string, KeyValuePair<int, Point>>> pointList; // The points of connectections for the tanks
        List<Ellipse> connectedValves; // The visualization of the valves
        List<Ellipse> dumpValves;
        List<TextBlock> labels;
        List<Expander> detailsExpanders;
        List<TextBlock> symbols; //Could be replaced with images, for example pasteurization could use a snowflake and a flame
        Boolean valvef;
        List<Line> arrowshafts;
        List<Line> arrowheads;
        List<TextBlock> levelTextBlocks;
        List<TextBlock> tempTextBlocks;
        private double ambientTemp;
        TextBlock ambTempBlock;
        
        public SimulationPage(List<TankModule> list, double ambTemp)
        {
            tankList = list;
            ambientTemp = ambTemp;
            InitializeComponent();
            CreateTanks();
            Task.Run(() => UpdateVisuals());
        }
        public List<Expander> GetExpanders()
        {
            return this.detailsExpanders;
        }

        public void SetValve(bool check)
        {
            this.valvef = check;
        }
        public void PassData(bool data)
        {
            this.valvef = data;
        }

        // DSD Joakim - Create all the tanks
        private void CreateTanks()
        {
            int height = 200; // General height of the tank elements
            int time = 0; // How many tanks are in this row
            int fromTop = 20; // The height of the row
            int rows = 0; // Shows which row is the current
            int distance = 230; // distance + offset  between each tank
            int offset = 50; // To make room for lines etc
            tankLevelList = new List<Rectangle>();
            pointList = new List<KeyValuePair<string, KeyValuePair<int, Point>>>();
            connectedValves = new List<Ellipse>();
            dumpValves = new List<Ellipse>();
            labels = new List<TextBlock>();
            symbols = new List<TextBlock>();
            detailsExpanders = new List<Expander>();
            arrowshafts = new List<Line>();
            arrowheads = new List<Line>();
            levelTextBlocks = new List<TextBlock>();
            tempTextBlocks = new List<TextBlock>();

            foreach (TankModule tank in tankList) //Setup the shapes and connection points
            {
                if (time == 3)
                {
                    fromTop += 300;
                    time = 0;
                    rows++;
                }

                // The rectangle for visualizing the tank level
                Rectangle tankRectangle = new Rectangle
                {
                    Width = 75,
                    Height = height,
                    Fill = Brushes.Blue,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                Canvas.SetLeft(tankRectangle, time * distance + offset );
                Canvas.SetTop(tankRectangle, fromTop);

                // The rectangle showing how empty the tank is 
                Rectangle tankLevelRectangle = new Rectangle
                {
                    Uid = tank.Name,
                    Width = 75,
                    Height = 200,
                    Fill = Brushes.White,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black
                };
                Canvas.SetLeft(tankLevelRectangle, time * distance + offset );
                Canvas.SetTop(tankLevelRectangle, fromTop);
                tankLevelList.Add(tankLevelRectangle);

                // DSD Emil - Expander used for details dropdown
                TextBlock headerText = new TextBlock
                {
                    Text = "Name: " + tank.Name,
                    Margin = new Thickness(0, 0, 3, 0)
                };
                Expander detailsExpander = new Expander
                {
                    Uid = tank.Name,
                    Header = headerText,
                    Background = Brushes.White,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Thickness(2)
                };
                Canvas.SetZIndex(detailsExpander, 10); // Set z-index to draw ontop ofother elements (such as tank connections)
                Canvas.SetLeft(detailsExpander, time * distance + offset  + 80);
                Canvas.SetTop(detailsExpander, fromTop - 1); // yeah its stupid, but the expander box was visually a tiny bit under the top of the tank
                detailsExpanders.Add(detailsExpander);

                // The point at which the tank will have its connections
                Point exit = new Point
                {
                    X = time * distance + offset  + 37.5,
                    Y = fromTop + height
                };
                Point entry = new Point
                {
                    X = time * distance + offset ,
                    Y = fromTop + height / 2
                };
                KeyValuePair<string, KeyValuePair<int, Point>> exitPoint = new KeyValuePair<string, KeyValuePair<int, Point>>(tank.Name + "_exit", new KeyValuePair<int, Point>(rows, exit)); //Added in this way to get which row they are on
                KeyValuePair<string, KeyValuePair<int, Point>> entryPoint = new KeyValuePair<string, KeyValuePair<int, Point>>(tank.Name + "_entry", new KeyValuePair<int, Point>(rows, entry));
                pointList.Add(exitPoint);
                pointList.Add(entryPoint);

                // Dump valves will probably have to improve the visuals of these, or change their position not really intuitive 
                Ellipse dumpValve = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.Black,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black,
                    Uid = "d_" + tank.Name
                };
                Canvas.SetLeft(dumpValve, time * distance + offset  + 70); //just placed it on the other side
                Canvas.SetTop(dumpValve, fromTop + height / 2);
                dumpValves.Add(dumpValve);

                // DSD Emil - Lines used for creating arrows
                Line arrowshaft = new Line
                {
                    X1 = time * distance + offset  + 20,
                    Y1 = fromTop + 1,
                    X2 = time * distance + offset  + 20,
                    Y2 = fromTop + height,
                    StrokeThickness = 2,
                    Stroke = Brushes.Black,
                    Uid = "as_" + tank.Name
                };
                Canvas.SetZIndex(arrowshaft,10);
                arrowshafts.Add(arrowshaft);

                Line arrowhead = new Line
                {
                    X1 = time * distance + offset  + 20,
                    Y1 = fromTop + 7,
                    X2 = time * distance + offset  + 20,
                    Y2 = fromTop + 8,
                    StrokeThickness = 13,
                    Stroke = Brushes.Black,
                    StrokeStartLineCap = PenLineCap.Triangle,
                    Uid = "ah1_" + tank.Name
                };
                Canvas.SetZIndex(arrowhead, 11);
                arrowheads.Add(arrowhead);

                Line arrowhead2 = new Line
                {
                    X1 = time * distance + offset  + 20,
                    Y1 = fromTop + height - 8,
                    X2 = time * distance + offset  + 20,
                    Y2 = fromTop + height - 9,
                    StrokeThickness = 13,
                    Stroke = Brushes.Black,
                    StrokeStartLineCap = PenLineCap.Triangle,
                    Uid = "ah2_" + tank.Name
                };
                Canvas.SetZIndex(arrowhead2, 11);
                arrowheads.Add(arrowhead2);

                // DSD Emil - Text displaying the current level, beside the arrows
                TextBlock levelText = new TextBlock
                {
                    Text = "0%",
                    Width = 40,
                    Height = 20,
                    Uid = tank.Name
                };
                Canvas.SetLeft(levelText, time * distance + offset  + 30);
                Canvas.SetTop(levelText, fromTop + 100);
                levelTextBlocks.Add(levelText);

                // DSD Emil - Text displaying the current temp, at the top of the tank
                TextBlock tempText = new TextBlock
                {
                    Text = "0K",
                    Width = 71,
                    Height = 20,
                    Uid = tank.Name,
                    TextAlignment = TextAlignment.Right,
                    Padding = new Thickness(0, 0, 5, 0),
                };
                Canvas.SetLeft(tempText, time * distance + offset  + 2);
                Canvas.SetTop(tempText, fromTop + 2);
                Canvas.SetZIndex(tempText, 12);
                tempTextBlocks.Add(tempText);

                // DSD Emil - Textblock displaying simulatiom ambient temperature
                ambTempBlock = new TextBlock
                {
                    TextAlignment = TextAlignment.Right,
                    Padding = new Thickness(0, 0, 5, 0),
                };
                Canvas.SetRight(ambTempBlock, canvas.ActualWidth);

                // All elements to be drawn are added to the canvas
                canvas.Children.Add(tankRectangle);
                canvas.Children.Add(tankLevelRectangle);
                canvas.Children.Add(dumpValve);
                canvas.Children.Add(detailsExpander);
                canvas.Children.Add(arrowshaft);
                canvas.Children.Add(arrowhead);
                canvas.Children.Add(arrowhead2);
                canvas.Children.Add(levelText);
                canvas.Children.Add(tempText);
                canvas.Children.Add(ambTempBlock);

                if (tank is PasteurizationModule || tank is HomogenizationModule || tank is FreezingModule || tank is FlavoringHardeningPackingModule)
                {
                    TextBlock symbol = new TextBlock
                    {
                        Text = "",
                        Width = 40,
                        Height = 100,
                        Name = "symbols_" + tank.Name,
                        //FontSize = 20,
                        TextWrapping = TextWrapping.Wrap
                    };

                    Canvas.SetLeft(symbol, time * distance + offset  + 35);
                    Canvas.SetTop(symbol, fromTop + 10);
                    symbols.Add(symbol);
                    canvas.Children.Add(symbol);
                }

                time++;
            }
            int[] times = new int[rows + 1]; // Used to increment the length of which the lines are apart from eachother
            foreach (TankModule tank in tankList) // Create the connections
            {
                if(tank == tankList[0])
                {
                    KeyValuePair<int, Point> initialPair = pointList.Find(x => x.Key == tank.Name + "_entry").Value;
                    Point initial = initialPair.Value;
                    Point target = new Point
                    {
                        Y = initial.Y,
                        X = initial.X - 30
                    };
                    PointCollection points = new PointCollection();
                    points.Add(initial);
                    points.Add(target);
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black,
                        Uid = "entry" // ID for the valves
                    };
                    connectedValves.Add(ellipse);
                    TextBlock label = new TextBlock
                    {
                        Text = "Entry" + "\n",
                        Name = "entry"
                    };
                    labels.Add(label);

                    Canvas.SetLeft(ellipse, target.X - 5);
                    Canvas.SetTop(ellipse, target.Y - 5);
                    Canvas.SetLeft(label, target.X - 25);
                    Canvas.SetTop(label, target.Y + 10);
                    Polyline line = new Polyline
                    {
                        Stroke = Brushes.Black
                    };
                    line.Points = points;
                    canvas.Children.Add(line);
                    canvas.Children.Add(ellipse);
                    canvas.Children.Add(label);
                }

                else if (tank == tankList[tankList.Count - 1])
                {
                    KeyValuePair<int, Point> initialPair = pointList.Find(x => x.Key == tank.Name + "_exit").Value;
                    Point initial = initialPair.Value;
                    Point target = new Point
                    {
                        Y = initial.Y + 20,
                        X = initial.X
                    };
                    PointCollection points = new PointCollection();
                    points.Add(initial);
                    points.Add(target);
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black,
                        Uid = "exit" // ID for the valves
                    };
                    connectedValves.Add(ellipse);
                    TextBlock label = new TextBlock
                    {
                        Text = "Exit" + "\n",
                        Name = "exit"
                    };
                    labels.Add(label);

                    Canvas.SetLeft(ellipse, target.X - 5);
                    Canvas.SetTop(ellipse, target.Y - 5);
                    Canvas.SetLeft(label, target.X + 20);
                    Canvas.SetTop(label, target.Y - 10);
                    Polyline line = new Polyline
                    {
                        Stroke = Brushes.Black
                    };
                    line.Points = points;
                    canvas.Children.Add(line);
                    canvas.Children.Add(ellipse);
                    canvas.Children.Add(label);
                }

                // This is a bit backward initial is the destination of the connection while target is the source, which means the lines have to be drawn backwards
                foreach (TankModule connected in tank.InFlowTanks)
                {
                    // The pairs are used to access the rows and points for the lines
                    KeyValuePair<int, Point> initialPair = pointList.Find(x => x.Key == tank.Name+ "_entry").Value; 
                    KeyValuePair<int, Point> targetPair = pointList.Find(x => x.Key == connected.Name+"_exit").Value;
                    int initialRow = initialPair.Key;
                    int targetRow = targetPair.Key;

                    // Used to visualize the connections between each tank
                    Polyline line = new Polyline
                    {
                        Stroke = Brushes.Black
                    };

                    // Used to specify the points of the line
                    PointCollection points = new PointCollection(); 
                    Point initial = initialPair.Value;
                    Point target = targetPair.Value;
                    points.Add(initial);

                    // Used to visualize the valves
                    Ellipse ellipse = new Ellipse
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Brushes.Black,
                        StrokeThickness = 2,
                        Stroke = Brushes.Black,
                        Uid = tank.Name + "_" + connected.Name // ID for the valves
                    }; 
                    connectedValves.Add(ellipse);


                    // Label for which valve it is
                    TextBlock label = new TextBlock
                    {
                        Text = connected.Name + "->" + tank.Name + "\n",
                        Name = tank.Name + "_" + connected.Name
                    }; 
                    labels.Add(label);

                    if (initialRow == targetRow)
                    {
                        times[initialRow]++;
                        Point first = new Point
                        {
                            X = initial.X - 75,
                            Y = initial.Y // Make the lines look more seperate
                        };

                        Point second = new Point
                        {
                            Y = target.Y + 15 * times[targetRow],
                            X = first.X
                        };

                        Point third = new Point
                        {
                            Y = second.Y,
                            X = target.X
                        };

                        points.Add(first);
                        points.Add(second);
                        points.Add(third);

                        Canvas.SetLeft(ellipse, second.X - 5);
                        Canvas.SetTop(ellipse, second.Y - 5);
                        Canvas.SetLeft(label, second.X - 60);
                        Canvas.SetTop(label, second.Y - 50);
                    }
                    else // If the target is not in the same row it will use 4 points instead of 2
                    {
                        times[targetRow]++;
                        Point first = new Point
                        {
                            Y = initial.Y,
                            X = initial.X - 60
                        };
                        Point second = new Point
                        {
                            Y = target.Y + 15 * times[targetRow],
                            X = first.X
                        };
                        Point third = new Point
                        {
                            Y = second.Y,
                            X = target.X
                        };
                        points.Add(first);
                        points.Add(second);
                        points.Add(third);

                        Canvas.SetLeft(ellipse, second.X - 5);
                        Canvas.SetTop(ellipse, second.Y - 5);
                        Canvas.SetLeft(label, second.X + 5);
                        Canvas.SetTop(label, second.Y + 5);
                    }
                    points.Add(target);
                    line.Points = points;
                    canvas.Children.Add(line);
                    canvas.Children.Add(ellipse);
                    canvas.Children.Add(label);
                }
            }
        }

        // DSD Joakim - Loops through the different elements and updates them
        internal async Task UpdateVisuals()
        {
            for (; ; )
            {
                // Update rectangles visualizing the tank level
                foreach (Rectangle rectangle in tankLevelList)
                {
                    rectangle.Dispatcher.Invoke(() =>
                    {
                        string name = rectangle.Uid;
                        TankModule current = tankList.Find(x => x.Name == name);
                        rectangle.Height = 200 - 200 * current.LevelPercentage / 100;

                        // DSD Emil - Update the level indicator arrows here since their level is dependant on the current tank level
                        Line arrow = arrowshafts.Find(x => x.Uid.Split('_')[1] == name);
                        arrow.Y1 = arrow.Y2 - 200 + rectangle.Height;

                        List<Line> arrowheadlist = arrowheads.FindAll(x => x.Uid.Split('_')[1] == name);

                        Line arrowhead1 = arrowheadlist.Find(x => x.Uid.Split('_')[0] == "ah1");
                        Line arrowhead2 = arrowheadlist.Find(x => x.Uid.Split('_')[0] == "ah2");

                        TextBlock leveltext = levelTextBlocks.Find(x => x.Uid == name);
                        leveltext.Text = Math.Round(current.LevelPercentage, 3) + "%";
                        
                        if (rectangle.Height < 187)
                        {
                            arrowhead1.Visibility = Visibility.Visible;
                            arrowhead2.Visibility = Visibility.Visible;

                            arrowhead1.Y1 = arrow.Y1 + 6;
                            arrowhead1.Y2 = arrow.Y1 + 7;

                            if (rectangle.Height < 200 - leveltext.Height)
                            {
                                Canvas.SetTop(leveltext, (arrow.Y1 + arrow.Y2 - leveltext.Height) / 2);                             
                            }
                            else
                            {
                                Canvas.SetTop(leveltext, arrow.Y1 - leveltext.Height);
                            }                            
                        }
                        else
                        {
                            arrowhead1.Visibility = Visibility.Hidden;
                            arrowhead2.Visibility = Visibility.Hidden;

                            Canvas.SetTop(leveltext, arrow.Y1 - leveltext.Height);
                        }
                    });
                }

                // Update the valves, White means open, black closed
                foreach (Ellipse valve in connectedValves)
                {
                    valve.Dispatcher.Invoke(() =>
                    {
                        if(valve.Uid == "entry")
                        {
                            if(tankList[0].InletFlow > 0)
                            {
                                valve.Fill = Brushes.White;
                            }
                            else
                            {
                                valve.Fill = Brushes.Black;
                            }
                        }
                        else if (valve.Uid == "exit")
                        {
                            if (tankList[tankList.Count - 1].OutValveOpen)
                            {
                                valve.Fill = Brushes.White;
                            }
                            else
                            {
                                valve.Fill = Brushes.Black;
                            }
                        }
                        else
                        {
                            string name = valve.Uid;
                            TankModule target = tankList.Find(x => x.Name == name.Split('_')[0]);
                            TankModule source = target.InFlowTanks.Find(x => x.Name == name.Split('_')[1]);
                            if (source != null)
                            {
                                if (source.OutValveOpen)
                                {
                                    valve.Fill = Brushes.White;
                                }
                                else
                                {
                                    valve.Fill = Brushes.Black;
                                }
                            }
                        }

                    });
                }

                // Update dumpvalves, currently only black dots on the side of the tanks
                foreach (Ellipse valve in dumpValves)
                {
                    valve.Dispatcher.Invoke(() =>
                    {
                        string name = valve.Uid;
                        TankModule target = tankList.Find(x => x.Name == name.Split('_')[1]);
                        if (target.DumpValveOpen)
                        {
                            valve.Fill = Brushes.White;
                        }
                        else
                        {
                            valve.Fill = Brushes.Black;
                        }
                    });
                }

                // DSD Emil - Update expandable/collapsable container for displaying tank info
                foreach (Expander expander in detailsExpanders)
                {
                    expander.Dispatcher.Invoke(() =>
                    {
                        TextBlock content = new TextBlock
                        {
                            Text = GetTankInfo(expander.Uid),
                            Padding = new Thickness(5, 0, 5, 0)
                        };
                        expander.Content = content;
                    });
                }
                // Update labels for tank connections
                foreach (TextBlock label in labels)
                {
                    label.Dispatcher.Invoke(() =>
                    {
                        string msg = "";
                        if (label.Name == "entry")
                        {
                            msg = "Entry\n";
                            if (Tag != null)
                            {
                                valvef = (this.Tag as MainWindow).GetValvef();
                                if (valvef)
                                {
                                    msg += "InFlow: \n" + Math.Round(tankList[0].InletFlow, 3) + "m3/s\n"; 
                                }
                            }
                            
                        }
                        else if(label.Name == "exit")
                        {
                            msg = "Exit\n";
                            if (Tag != null)
                            {
                                valvef = (this.Tag as MainWindow).GetValvef();
                                if (valvef)
                                {
                                    msg += "OutFlow: \n" + Math.Round(tankList[tankList.Count - 1].OutLetFlow, 3) + "m3/s\n";
                                }
                            }
                            
                        }
                        else
                        {
                            TankModule tank = tankList.Find(x => x.Name == label.Name.Split('_')[0]);
                            if(tank != null)
                            {
                                TankModule connected = tank.InFlowTanks.Find(x => x.Name == label.Name.Split('_')[1]);
                                msg = connected.Name + "->" + tank.Name + "\n";
                                //Checks if valve flowrate is selected, and displays the flowrate if it is 
                                if (Tag != null)
                                {
                                    valvef = (this.Tag as MainWindow).GetValvef();
                                    if (valvef)
                                    {
                                        msg += "InFlow: \n" + Math.Round(tank.InletFlow, 3) + "m3/s\n"; //Could also add the temperatures, will probably have to divide what each shows in other functions, as we should be able to select the 
                                    }
                                }
                            }
                        }
                        label.Text = msg;
                    });
                }

                // DSD Emil - Update tank temperature text
                foreach (TextBlock temp in tempTextBlocks)
                {
                    temp.Dispatcher.Invoke(() =>
                    {
                        TankModule tank = tankList.Find(x => x.Name == temp.Uid);
                        temp.Text = tank.Temperature + "K";
                    });
                }

                ambTempBlock.Dispatcher.Invoke(() =>
                {
                    ambTempBlock.Text = "Ambient temp: " + ambientTemp.ToString() + "K";

                });

                SymbolUpdate();
                await Task.Delay(1000);
            }
        }

        // Update special symbols
        private void SymbolUpdate()
        {
            foreach (TextBlock textBlock in symbols)
            {
                textBlock.Dispatcher.Invoke(() => {
                    TankModule tank = tankList.Find(x => x.Name == textBlock.Name.Split('_')[1]);
                    switch(tank)
                    {
                        case PasteurizationModule p:
                            textBlock.Text = UpdatePasteurization(p);
                            break;
                        case HomogenizationModule h:
                            textBlock.Text = UpdateHomogenization(h);
                            break;
                        case FlavoringHardeningPackingModule fhp:
                            textBlock.Text = UpdateFlavoringHardeningPacking(fhp);
                            break;
                        case FreezingModule f:
                            textBlock.Text = UpdateFreezing(f);
                            break;
                        
                    }                  
                });
            }
        }

        private string UpdatePasteurization(PasteurizationModule temp)
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
        

        private string UpdateHomogenization(HomogenizationModule temp) //Present cooler and pressure in raw data instead
        {
            string ret = "";
            if(temp.HomogenizationOn)
            {
                ret = "Homogen ";
            }
            if(temp.AgeingCoolingOn)
            {
                ret += "Aging, -";
            }
            return ret;
        }

        private string UpdateFlavoringHardeningPacking(FlavoringHardeningPackingModule temp) //Represent Mix temp, cooler temp, package form (or with an image) in raw data
        {
            string ret = "";
            if (temp.StartFlavoring)
            {
                ret = "Flavor ";
            }
            if (temp.StartHardening)
            {
                ret += "Hard ";
            }
            if (temp.StartPackaging)
            {
                ret += "Pack ";
            }
            if(temp.FinishBatch)
            {
                ret = "Fin";
            }
            return ret;
        }

        private string UpdateFreezing(FreezingModule temp) //Represent sending test values? Represent others via raw data
        {
            string ret = "";
            if (temp.FreezingOn)
            {
                ret = "Freeze";
            }
            if (temp.DasherOn)
            {
                ret += "Dash";
            }
            if (temp.StartLiquidFlavoring) //Maybe do visually
            {
                ret += "L. Flavor";
            }
            return ret;
        }

        // DSD - Get the info from the tank to be displayed
        private string GetTankInfo(string name)
        {
            string msg = "";
            TankModule tank = tankList.Find(x => x.Name == name);
            msg += "Type: ";
            switch(tank)
            {
                case PasteurizationModule p:
                    msg += " P. Module\n";
                    break;
                case HomogenizationModule h:
                    msg += " H. Module\n";
                    break;
                case FlavoringHardeningPackingModule fhp:
                    msg += " F.H.P Module\n";
                    break;
                case FreezingModule f:
                    msg += " F. Module\n";
                    break;
                default:
                    msg += " Tank Module\n";
                    break;
            }
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
                case PasteurizationModule p: // Can add Thickness and conductivities if necessary
                    msg += "Heater temp: " + p.HeaterTemp + "K\n";
                    msg += "Cooler temp: " + p.CoolerTemp + "K\n";
                    break;
                case HomogenizationModule h:
                    msg += "Mix temp: " + h.MixTemperature + "K\n";
                    msg += "Particle size: " + h.ParticleSize + "K\n";
                    msg += "Stage 1 pressure: " + h.Stage1Pressure + " Pa\n"; // Don't know the unit, guessing pascal
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
                    msg += "Volume increase: " + f.Overrun + "%\n"; // Precentage of increase in volume after mix and freeze
                    msg += "Barrel rot. speed: " + f.BarrelRotationSpeed + " rad/s\n"; // Don't know which SI unit, guessing rad/s
                    msg += "Units: " + f.PasteurizationUnits + "\n"; // Should probably be visualized, probably ask customer
                    break;
            }

            return msg;
        }
        public void SetOpen()
        {
            foreach(Expander exp in detailsExpanders)
            {
                exp.IsExpanded = true;
            }
        }
        public void SetClosed()
        {
            foreach (Expander exp in detailsExpanders)
            {
                exp.IsExpanded = false;
            }
        }
    }
}
