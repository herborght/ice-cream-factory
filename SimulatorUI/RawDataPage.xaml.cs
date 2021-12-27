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
using System.Collections.ObjectModel;

namespace SimulatorUI
{

    public partial class RawDataPage : Page
    {
        public ObservableCollection<TankModule> TankList { get; set; }
        public RawDataPage(List<TankModule> list)
        {
            TankList = new ObservableCollection<TankModule>(list);
            // Binding this instance as the datacontext for the view
            DataContext = this; 
            InitializeComponent();
            Task.Run(() => updateLoop());   
        }

        // DSD Bendik - Raw data displayed in a grid with databinding
        public void DataGridCell_load(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell.Column.Header.ToString() == "InLetFlow")
            {
                ((TextBlock)cell.Content).Text = "Lol";
            }
        }
        internal async Task updateLoop()
        {
            for (; ; )
            {
                await Task.Delay(1000);
            }
        }
    }
}
