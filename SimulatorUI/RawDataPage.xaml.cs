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
            DataContext = this; //Binding this instancde as the datacontext for the view
            InitializeComponent();
            Task.Run(() => updateLoop());   
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
