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

        // DSD Bendik - Raw data displayed in a grid with databinding
        public void DataGridCell_load(object sender, RoutedEventArgs e)
        {
            DataGridCell cell = sender as DataGridCell;
            if (cell.Column.Header.ToString() == "InLetFlow")
            {
                ((TextBlock)cell.Content).Text = "Lol";
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
