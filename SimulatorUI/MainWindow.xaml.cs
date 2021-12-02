using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TankModule> tankList;
        Page currentPage; 

        public MainWindow(List<TankModule> list)
        {
            tankList = list;
            InitializeComponent();
            currentPage = new SimulationPage(tankList);
            _mainFrame.Content = currentPage;
        }

        private void SwitchView(object sender, RoutedEventArgs e)
        {
            if (currentPage is SimulationPage) {

                Page newPage = new RawDataPage(tankList);
                currentPage = newPage;
                Filter.Visibility = Visibility.Visible;
                _mainFrame.Content = newPage;
            }
            else
            {
                Page newPage = new SimulationPage(tankList);
                currentPage = newPage;
                Filter.Visibility = Visibility.Collapsed;
                _mainFrame.Content = newPage;
            }

        }

    }
}
