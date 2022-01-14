using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TankModule> tankList;
        public Page currentPage;
        private double ambientTemp;

        public MainWindow(List<TankModule> list, string configName, double ambTemp)
        {
            tankList = list;
            ambientTemp = ambTemp;
            InitializeComponent();

            currentPage = new SimulationPage(tankList, ambientTemp);
            _mainFrame.Content = currentPage;
            WindowTitle.Text = "Current Simulation: " + configName;
        }

        public void AmbientTempCheckboxChanged(object sender, RoutedEventArgs e)
        {
            if (ambientTemperature != null && currentPage is SimulationPage)
            {
                if (ambientTemperature.IsChecked == true)
                {
                    (currentPage as SimulationPage).AmbientTempVisibility(true);
                }
                else
                {
                    (currentPage as SimulationPage).AmbientTempVisibility(false);
                }
            }
        }

        // Simulate the UI "crashing" or "freezing" to test that the simulator can run undisrupted
        private void FaultInjection(object sender, RoutedEventArgs e)
        {
            for (; ; )
            {

            }
        }

        private void Frame_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (currentPage is SimulationPage)
            {
                (e.Content as SimulationPage).Tag = this;
            }
            else
            {
                (e.Content as RawDataPage).Tag = this;
            }
        }

        public void ShowImportant(object sender, RoutedEventArgs e)
        {
            if (showImportant.IsChecked == true && currentPage is SimulationPage)
            {
                (currentPage as SimulationPage).SetClosed();
            }
        }

        public void ShowAll(object sender, RoutedEventArgs e)
        {
            if (showAll.IsChecked == true && currentPage is SimulationPage)
            {
                (currentPage as SimulationPage).SetOpen();
            }
        }

        public bool GetValvef()
        {
            // Returns the value for valve flowrate
            return valveFlowrate.IsChecked ?? false;
        }

        public bool GetAmbTemp()
        {
            return ambientTemperature.IsChecked ?? false;
        }

        // DSD Yrjar - Switch the view displayed in the mainframe
        public void SwitchView(object sender, RoutedEventArgs e)
        {
            if (currentPage is SimulationPage)
            {
                Page newPage = new RawDataPage(tankList);
                currentPage = newPage;
                filter.Visibility = Visibility.Visible;
                barOne.Visibility = Visibility.Collapsed;
                barTwo.Visibility = Visibility.Collapsed;
                showOptions.Visibility = Visibility.Collapsed;
                checkBoxes.Visibility = Visibility.Collapsed;
                _mainFrame.Content = newPage;
                switchViewButton.Content = "View simulation";
            }
            else
            {
                Page newPage = new SimulationPage(tankList, ambientTemp);
                currentPage = newPage;
                if (showAll.IsChecked ?? false)
                {
                    foreach (Expander ex in (currentPage as SimulationPage).GetExpanders())
                    {
                        ex.IsExpanded = true;
                    }
                }
                filter.Visibility = Visibility.Collapsed;
                barOne.Visibility = Visibility.Visible;
                barTwo.Visibility = Visibility.Visible;
                showOptions.Visibility = Visibility.Visible;
                checkBoxes.Visibility = Visibility.Visible;
                _mainFrame.Content = newPage;
                switchViewButton.Content = "View raw data";
            }
        }

        // DSD Yrjar - Create a zipfile with all simulation data between the two chosen dates
        // This download function currently creates a zipfile of the simulation events files
        // Done this way because the CSV file from SimpleLog only contains the simulation parameter names, and no actual data
        private void Download(object sender, RoutedEventArgs e)
        {
            // DSD Emil - Create a dialog box for choosing download location and file name
            SaveFileDialog dialog = new SaveFileDialog
            {
                DefaultExt = ".zip",
                Filter = "Zip file (*.zip)|*.zip"
            };

            // When user has pressed the OK button in the dialog window, proceed with creating the zipfile
            if (dialog.ShowDialog() == true)
            {
                // Extracting the data from here
                string startPath = @"..\..\..\..\LogData\EventLogData";

                // Getting the selected dates
                DateTime? firstDate = fromDate.SelectedDate;
                DateTime? lastDate = toDate.SelectedDate;

                // Initalizes a temporary subdirectory
                DirectoryInfo dir = new DirectoryInfo(startPath);
                DirectoryInfo subdir = dir.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";

                // Checking if the user has inputed values
                if (firstDate.HasValue && lastDate.HasValue)
                {
                    // Iterate over the files in startpath
                    foreach (FileInfo file in dir.EnumerateFiles())
                    {
                        // Filter the relevant files
                        if (file.CreationTime.Date >= firstDate && file.CreationTime.Date <= lastDate)
                        {
                            // Copy the file into the subdir
                            File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                        }
                    }
                    // Creates the zipfile and deletes the subdir
                    ZipFile.CreateFromDirectory(targetPath, dialog.FileName);
                    subdir.Delete(true);
                }
                else if (firstDate.HasValue)
                {
                    foreach (FileInfo file in dir.EnumerateFiles())
                    {
                        if (file.CreationTime.Date >= firstDate)
                        {
                            File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                        }
                    }
                    ZipFile.CreateFromDirectory(targetPath, dialog.FileName);
                    subdir.Delete(true);
                }
                else if (lastDate.HasValue)
                {
                    foreach (FileInfo file in dir.EnumerateFiles())
                    {
                        if (file.CreationTime.Date <= lastDate)
                        {
                            File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                        }
                    }
                    ZipFile.CreateFromDirectory(targetPath, dialog.FileName);
                    subdir.Delete(true);
                }
                else
                {
                    subdir.Delete(true);
                    ZipFile.CreateFromDirectory(startPath, dialog.FileName);
                }
            }
        }
    }
}
