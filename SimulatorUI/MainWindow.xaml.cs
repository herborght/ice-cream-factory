﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO.Compression;
using System.IO;
using System.Linq;
using System.Globalization;

namespace SimulatorUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<TankModule> tankList;
        public Page currentPage;

        public MainWindow(List<TankModule> list, string configName)
        {
            tankList = list;
            InitializeComponent();
            currentPage = new SimulationPage(tankList);
            _mainFrame.Content = currentPage;
            WindowTitle.Text = "Current Simulation: " + configName;
        }

        // DSD Yrjar - Switch the view displayed in the mainframe
        public void SwitchView(object sender, RoutedEventArgs e)
        {
            if (currentPage is SimulationPage)
            {
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

        // DSD Yrjar - Create a zipfile with all simulation data between the two chosen dates
        private void Download(object sender, RoutedEventArgs e)
        {
            var timeStamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            // Extracting the data from here
            string startPath = @"..\..\..\..\LogData\EventLogData";
            // Where the zipped file is sent
            string zipPath = @"..\..\..\..\ZippedLog\download" + timeStamp + ".zip";

            // Getting the selected dates
            DateTime? firstDate = fromDate.SelectedDate;
            DateTime? lastDate = toDate.SelectedDate;

            // Checking if the user has inputed values
            if (firstDate.HasValue && lastDate.HasValue)
            {
                // Initalizes a temporary subdirectory
                DirectoryInfo dir = new DirectoryInfo(startPath);
                DirectoryInfo subdir = dir.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";

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
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                subdir.Delete(true);
            }
            else if (firstDate.HasValue)
            {
                DirectoryInfo dir = new DirectoryInfo(startPath);
                DirectoryInfo subdir = dir.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";
                foreach (FileInfo file in dir.EnumerateFiles())
                {
                    if (file.CreationTime.Date >= firstDate)
                    {
                        File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                    }
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                subdir.Delete(true);
            }
            else if (lastDate.HasValue)
            {
                DirectoryInfo dir = new DirectoryInfo(startPath);
                DirectoryInfo subdir = dir.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";
                foreach (FileInfo file in dir.EnumerateFiles())
                {
                    if (file.CreationTime.Date >= firstDate && file.CreationTime.Date <= lastDate)
                    {
                        File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                    }
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                subdir.Delete(true);
            }
            else
            {
                ZipFile.CreateFromDirectory(startPath, zipPath);
            }
        }
    }
}
