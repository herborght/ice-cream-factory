using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO.Compression;

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
        private void Download(object sender, RoutedEventArgs e)
        {

            string startPath = @"..\..\..\..\LogData";
            string zipPath = @"..\..\..\..\ZippedLog\test2.zip";

            ZipFile.CreateFromDirectory(startPath, zipPath);

            //ZipFile.ExtractToDirectory(zipPath, extractPath);
            //Add functionality to download
            DateTime? firstDate = fromDate.SelectedDate;
            DateTime? lastDate = toDate.SelectedDate;
            if (firstDate.HasValue && lastDate.HasValue)
            {
                string formattedFirstDate = firstDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string formattedLastDate = lastDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            }
        }

    }
}
