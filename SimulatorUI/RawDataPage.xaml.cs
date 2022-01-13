using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Controls;

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
                switch (module)
                {
                    case FreezingModule f:
                        FreezingList.Add(f);
                        TankList.Remove(f);
                        break;
                    case FlavoringHardeningPackingModule fhp:
                        FHPMList.Add(fhp);
                        TankList.Remove(fhp);
                        break;
                    case HomogenizationModule h:
                        HomoList.Add(h);
                        TankList.Remove(h);
                        break;
                    case PasteurizationModule p:
                        PastList.Add(p);
                        TankList.Remove(p);
                        break;
                }
            }
            // Binding this instance as the datacontext for the view
            DataContext = this;
            InitializeComponent();
            Task.Run(() => UpdateLoop());
        }

        // Function for setting the column order in the DataGrid 
        private void DataGridOrdering(object sender, EventArgs e)
        {
            DataGrid grid = (DataGrid)sender;
            foreach (DataGridColumn item in grid.Columns)
            {
                string itemName = item.Header.ToString();
                switch (itemName)
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
                    case "OutletFlow":
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
                    default:
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
