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
            createTable();
            Task.Run(() => updateLoop());
            
        }
        public void createTable()
        {
            FlowDocument flowdoc = new FlowDocument();
            Table dataTable = new Table();
            dataTable.Name = "dataTable";
            dataTable.CellSpacing = 10;
            dataTable.Background = Brushes.White;
            int colNum = 10;
            for (int i = 0; i < colNum; i++)
            {
                dataTable.Columns.Add(new TableColumn());
                if (i % 2 == 0)
                    dataTable.Columns[i].Background = Brushes.Beige;
                else
                    dataTable.Columns[i].Background = Brushes.LightSteelBlue;
            }
            dataTable.RowGroups.Add(new TableRowGroup());
            dataTable.RowGroups[0].Rows.Add(new TableRow());
            TableRow currentRow = dataTable.RowGroups[0].Rows[0];
            currentRow.Background = Brushes.Silver;
            currentRow.FontSize = 40;
            currentRow.FontWeight = System.Windows.FontWeights.Bold;
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Raw Data View"))));
            currentRow.Cells[0].ColumnSpan = 6;

            // Second-header row.
            dataTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = dataTable.RowGroups[0].Rows[1];
            // Formatting for the header row.
            currentRow.FontSize = 14;
            currentRow.FontWeight = FontWeights.Bold;
            // Add col-description.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Name"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Level"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Percent"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Temp"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("InFlow"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("InFlowTemp"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("OutFlow"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("DmpValve"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("OutValve"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Inflow from"))));
            // Global formatting for the row.
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;
            //FILL Table
            string outflowfrom = "";
            int row = 2;
            foreach (TankModule tank in tankList)
            {
                outflowfrom = "None";
                String name = tank.Name;
                String level = Math.Round(tank.Level, 3).ToString();
                String Percent = Math.Round(tank.LevelPercentage, 3).ToString() + "%";
                String Temp = Math.Round(tank.Temperature, 3).ToString() + " K";
                String InFlow = Math.Round(tank.InletFlow, 3) + " m3/s";
                String InFlowTemp = Math.Round(tank.InFlowTemp, 3) + " K";
                String OutFlow = Math.Round(tank.OutLetFlow, 3) + " m3/s";
                String OutFlowTemp = Math.Round(tank.OutFlowTemp, 3) + " K";
                String DmpValve = tank.DumpValveOpen.ToString();
                String OutValve = tank.OutValveOpen.ToString();
                if (tank.InFlowTanks.Count > 0)
                {
                    outflowfrom = "";
                    foreach (var t in tank.InFlowTanks)
                    {
                        outflowfrom += t.Name + ", ";
                    }
                }


                dataTable.RowGroups[0].Rows.Add(new TableRow());
                currentRow = dataTable.RowGroups[0].Rows[row++];
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(name))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(level))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Percent))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Temp))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(InFlow))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(InFlowTemp))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(OutFlow))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DmpValve))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(OutValve))));
                currentRow.Cells.Add(new TableCell(new Paragraph(new Run(outflowfrom))));

                //Adds the initial flowdoc with table to the Page
                flowdoc.Blocks.Add(dataTable);
                this.Content = flowdoc;
            }
        }
        internal async Task updateLoop()
        {
            for (; ; )
            {

                string msg = "";
                foreach (TankModule tank in tankList)
                {
                    msg += "\n";
                    String name = tank.Name;
                    String level = Math.Round(tank.Level, 3).ToString();
                    String Percent = Math.Round(tank.LevelPercentage, 3).ToString() + "%";
                    String Temp = Math.Round(tank.Temperature, 3).ToString() + " K";
                    String InFlow = Math.Round(tank.InletFlow, 3) + " m3/s";
                    String InFlowTemp = Math.Round(tank.InFlowTemp, 3) + " K";
                    String OutFlow = Math.Round(tank.OutLetFlow, 3) + " m3/s";
                    String OutFlowTemp = Math.Round(tank.OutFlowTemp, 3) + " K";
                    String DmpValve = tank.DumpValveOpen.ToString();
                    String OutValve = tank.OutValveOpen.ToString();
                    if (tank.InFlowTanks.Count > 0)
                    {
                        msg += "InFlow from: ";
                        foreach (var t in tank.InFlowTanks)
                        {
                            msg += t.Name + " ";
                        }
                        msg += "\n";
                    }
                    
                    /*
                    dataTable.RowGroups[0].Rows.Add(new TableRow());
                    TableRow currentRow = dataTable.RowGroups[0].Rows[1];
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(name))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(level))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Percent))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(Temp))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(InFlow))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(InFlowTemp))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(OutFlow))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(DmpValve))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(OutValve))));
                    currentRow.Cells.Add(new TableCell(new Paragraph(new Run(name))));
                    currentRow.Cells[0].ColumnSpan = 10;
                    */
                }
                await Task.Delay(1000);
            }
        }
    }
}
