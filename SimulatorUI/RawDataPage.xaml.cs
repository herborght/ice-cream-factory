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
        public ObservableCollection<FreezingModule> FreezingList { get; set; }
        public ObservableCollection<FlavoringHardeningPackingModule> FHPMList { get; set; }
        public ObservableCollection<HomogenizationModule> HomoList { get; set; }
        public ObservableCollection<PasteurizationModule> PastList { get; set; }

        public RawDataPage(List<TankModule> list)
        {
            TankList = new ObservableCollection<TankModule>(list);
            FreezingList = new ObservableCollection<FreezingModule>();
            FHPMList = new ObservableCollection<FlavoringHardeningPackingModule>();
            HomoList = new ObservableCollection<HomogenizationModule>();
            PastList = new ObservableCollection<PasteurizationModule>();
            foreach (TankModule module in list)
            {   
                if(Object.ReferenceEquals(module.GetType(), typeof(FreezingModule)))
                {
                    FreezingList.Add(module as FreezingModule);
                    TankList.Remove(module);
                } else if (Object.ReferenceEquals(module.GetType(), typeof(FlavoringHardeningPackingModule)))
                {
                    FHPMList.Add(module as FlavoringHardeningPackingModule);
                    TankList.Remove(module);
                } else if (Object.ReferenceEquals(module.GetType(), typeof(HomogenizationModule)))
                {
                    HomoList.Add(module as HomogenizationModule);
                    TankList.Remove(module);
                }
                else if (Object.ReferenceEquals(module.GetType(), typeof(HomogenizationModule)))
                {
                    PastList.Add(module as PasteurizationModule);
                    TankList.Remove(module);
                }

            }
            // Binding this instance as the datacontext for the view
            DataContext = this;
            InitializeComponent();
            Task.Run(() => UpdateLoop());
        }
        //Function for setting the column order in the DataGrid, similar functions are created for all tables
        private void DataGrid_FHPMordering(object sender, EventArgs e)
        {
            
            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                string itemname = item.Header.ToString();
                switch (itemname)
                {
                    case "Name":
                        item.DisplayIndex = 0;
                        break;
                    case "InletFlow":
                        item.DisplayIndex = 1;
                        break;
                    case "OutValveOpen":
                        item.DisplayIndex = 2;
                        break;
                    case "DumpValveOpen":
                        item.DisplayIndex = 3;
                        break;
                    case "InFlowTemp":
                        item.DisplayIndex = 4;
                        break;
                    case "OutFlowTemp":
                        item.DisplayIndex = 5;
                        break;
                    case "OutLetFlow":
                        item.DisplayIndex = 6;
                        break;
                    case "LevelPercentage":
                        item.DisplayIndex = 7;
                        break;
                    case "Temperature":
                        item.DisplayIndex = 8;
                        break;
                    case "Level":
                        item.DisplayIndex = 9;
                        break;
                    case "Height":
                        item.DisplayIndex = 10;
                        break;
                    case "BaseArea":
                        item.DisplayIndex = 11;
                        break;
                    case "OutletArea":
                        item.DisplayIndex = 12;
                        break;
                }
            }
        }
        //Function for setting the column order in the DataGrid, similar functions are created for all tables
        private void DataGrid_Freeordering(object sender, EventArgs e)
        {

            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                string itemname = item.Header.ToString();
                switch (itemname)
                {
                    case "Name":
                        item.DisplayIndex = 0;
                        break;
                    case "InletFlow":
                        item.DisplayIndex = 1;
                        break;
                    case "OutValveOpen":
                        item.DisplayIndex = 2;
                        break;
                    case "DumpValveOpen":
                        item.DisplayIndex = 3;
                        break;
                    case "InFlowTemp":
                        item.DisplayIndex = 4;
                        break;
                    case "OutFlowTemp":
                        item.DisplayIndex = 5;
                        break;
                    case "OutLetFlow":
                        item.DisplayIndex = 6;
                        break;
                    case "LevelPercentage":
                        item.DisplayIndex = 7;
                        break;
                    case "Temperature":
                        item.DisplayIndex = 8;
                        break;
                    case "Level":
                        item.DisplayIndex = 9;
                        break;
                    case "Height":
                        item.DisplayIndex = 10;
                        break;
                    case "BaseArea":
                        item.DisplayIndex = 11;
                        break;
                    case "OutletArea":
                        item.DisplayIndex = 12;
                        break;
                }
            }
        }
        //Function for setting the column order in the DataGrid, similar functions are created for all tables
        private void DataGrid_FREEordering(object sender, EventArgs e)
        {

            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                string itemname = item.Header.ToString();
                switch (itemname)
                {
                    case "Name":
                        item.DisplayIndex = 0;
                        break;
                    case "InletFlow":
                        item.DisplayIndex = 1;
                        break;
                    case "OutValveOpen":
                        item.DisplayIndex = 2;
                        break;
                    case "DumpValveOpen":
                        item.DisplayIndex = 3;
                        break;
                    case "InFlowTemp":
                        item.DisplayIndex = 4;
                        break;
                    case "OutFlowTemp":
                        item.DisplayIndex = 5;
                        break;
                    case "OutLetFlow":
                        item.DisplayIndex = 6;
                        break;
                    case "LevelPercentage":
                        item.DisplayIndex = 7;
                        break;
                    case "Temperature":
                        item.DisplayIndex = 8;
                        break;
                    case "Level":
                        item.DisplayIndex = 9;
                        break;
                    case "Height":
                        item.DisplayIndex = 10;
                        break;
                    case "BaseArea":
                        item.DisplayIndex = 11;
                        break;
                    case "OutletArea":
                        item.DisplayIndex = 12;
                        break;
                }
            }
        }
        private void DataGrid_HOMOordering(object sender, EventArgs e)
        {

            var grid = (DataGrid)sender;
            foreach (var item in grid.Columns)
            {
                string itemname = item.Header.ToString();
                switch (itemname)
                {
                    case "Name":
                        item.DisplayIndex = 0;
                        break;
                    case "InletFlow":
                        item.DisplayIndex = 1;
                        break;
                    case "OutValveOpen":
                        item.DisplayIndex = 2;
                        break;
                    case "DumpValveOpen":
                        item.DisplayIndex = 3;
                        break;
                    case "InFlowTemp":
                        item.DisplayIndex = 4;
                        break;
                    case "OutFlowTemp":
                        item.DisplayIndex = 5;
                        break;
                    case "OutLetFlow":
                        item.DisplayIndex = 6;
                        break;
                    case "LevelPercentage":
                        item.DisplayIndex = 7;
                        break;
                    case "Temperature":
                        item.DisplayIndex = 8;
                        break;
                    case "Level":
                        item.DisplayIndex = 9;
                        break;
                    case "Height":
                        item.DisplayIndex = 10;
                        break;
                    case "BaseArea":
                        item.DisplayIndex = 11;
                        break;
                    case "OutletArea":
                        item.DisplayIndex = 12;
                        break;
                }
            }
        }
        internal async Task UpdateLoop()
        {
            for (; ; )
            {
                await Task.Delay(1000);
            }
        }
    }
}
