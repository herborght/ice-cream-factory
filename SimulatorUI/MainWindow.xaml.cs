using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Globalization;
using Microsoft.Win32;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TankModule> tankList;
        public Page currentPage; 
        private static int counter;
        public CheckBox valveCheckBox;
        public bool important; //used to close all expanders in simulationPage
        private double ambientTemp;

        public MainWindow(List<TankModule> list, string configName, double ambTemp)
        {
            tankList = list;
            ambientTemp = ambTemp;
            InitializeComponent();

            TextBlock test = new TextBlock();
            currentPage = new SimulationPage(tankList, ambientTemp);
            _mainFrame.Content = currentPage;
            WindowTitle.Text = "Current Simulation: " + configName;
        }
        public void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            
        }
        public void AmbientTempCheckboxChanged(object sender, RoutedEventArgs e)
        {
            if(AmbientTemp != null && currentPage is SimulationPage)
            {
                if (AmbientTemp.IsChecked == true)
                {
                    (currentPage as SimulationPage).AmbientTempVisibility(true);
                }
                else
                {
                    (currentPage as SimulationPage).AmbientTempVisibility(false);
                }
            }
        }
        private void FaultInjection(object sender, RoutedEventArgs e)
        {
            for(; ; )
            {
                
            }
        }
        private void Frame_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
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
        public void ShowImportat(object sender, RoutedEventArgs e)
        {
            if (showimportant.IsChecked == true && currentPage is SimulationPage)
            {
                (currentPage as SimulationPage).SetClosed();
            }
        }
        public void ShowAll(object sender, RoutedEventArgs e)
        {
            if (showall.IsChecked == true && currentPage is SimulationPage)
            {
                (currentPage as SimulationPage).SetOpen();
            }
        }
        public bool GetValvef()
        {   
            //returns the value for valve flowrate
            return valveflowrate.IsChecked ?? false;
        }

        public bool GetAmbTemp()
        {
            return AmbientTemp.IsChecked ?? false;
        }


        // DSD Yrjar - Switch the view displayed in the mainframe
        public void SwitchView(object sender, RoutedEventArgs e)
        {
            if (currentPage is SimulationPage)
            {
                Page newPage = new RawDataPage(tankList);
                currentPage = newPage;
                Filter.Visibility = Visibility.Visible;
                barone.Visibility = Visibility.Collapsed;
                bartwo.Visibility = Visibility.Collapsed;
                showoptions.Visibility = Visibility.Collapsed;
                checkboxes.Visibility = Visibility.Collapsed;
                _mainFrame.Content = newPage;
                SwitchViewButton.Content = "View simulation";
            }
            else
            {
                Page newPage = new SimulationPage(tankList, ambientTemp);
                currentPage = newPage;
                if (showall.IsChecked ?? false)
                {
                    foreach (Expander ex in (currentPage as SimulationPage).GetExpanders())
                    {
                        ex.IsExpanded = true;
                    }
                }
                Filter.Visibility = Visibility.Collapsed;
                barone.Visibility = Visibility.Visible;
                bartwo.Visibility = Visibility.Visible;
                showoptions.Visibility = Visibility.Visible;
                checkboxes.Visibility = Visibility.Visible;
                _mainFrame.Content = newPage;
                SwitchViewButton.Content = "View raw data";
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
