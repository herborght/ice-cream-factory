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
            //Init the table with header
            FlowDocument flowdoc = new FlowDocument();
            Table dataTable = new Table();
            dataTable.CellSpacing = 10;
            dataTable.Background = Brushes.White;
            int colNum = 6;
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
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("2004 Sales Project"))));
            currentRow.Cells[0].ColumnSpan = 6;
            // Add the second (header) row.
            dataTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = dataTable.RowGroups[0].Rows[1];

            // Global formatting for the header row.
            currentRow.FontSize = 18;
            currentRow.FontWeight = FontWeights.Bold;

            // Add cells with content to the second row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Product"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 1"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 2"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 3"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Quarter 4"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("TOTAL"))));
            // Add the third row.
            dataTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = dataTable.RowGroups[0].Rows[2];

            // Global formatting for the row.
            currentRow.FontSize = 12;
            currentRow.FontWeight = FontWeights.Normal;

            // Add cells with content to the third row.
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Widgets"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$50,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$55,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$60,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$65,000"))));
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("$230,000"))));

            // Bold the first cell.
            currentRow.Cells[0].FontWeight = FontWeights.Bold;

            dataTable.RowGroups[0].Rows.Add(new TableRow());
            currentRow = dataTable.RowGroups[0].Rows[3];

            // Global formatting for the footer row.
            currentRow.Background = Brushes.LightGray;
            currentRow.FontSize = 18;
            currentRow.FontWeight = System.Windows.FontWeights.Normal;

            // Add the header row with content,
            currentRow.Cells.Add(new TableCell(new Paragraph(new Run("Projected 2004 Revenue: $810,000"))));
            // and set the row to span all 6 columns.
            currentRow.Cells[0].ColumnSpan = 6;
            flowdoc.Blocks.Add(dataTable);
            this.Content = flowdoc;


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
                    RawDataTable.RowGroups[0].Rows.Add(new TableRow());
                    RawDataTable.CellSpacing = 50;
                    TableRow currentRow = RawDataTable.RowGroups[0].Rows[1];
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
