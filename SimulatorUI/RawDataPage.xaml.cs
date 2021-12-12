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
            dataTable.ItemsSource = loadTable();
            Task.Run(() => updateLoop());
            
        }
        public List<displayModule> loadTable()
        {
            List<displayModule> modules = new List<displayModule>();
            foreach(TankModule tank in tankList)
            {
                modules.Add(new displayModule(tank));
            }
            return modules;
        }
        
        internal async Task updateLoop()
        {
            for (; ; )
            {
                foreach(displayModule mod in dataTable.ItemsSource)
                {
                    mod.updateVals();
                }
                dataTable.ItemsSource = null;
                dataTable.ItemsSource = loadTable();
                await Task.Delay(1000);
            }
        }
    }
}
