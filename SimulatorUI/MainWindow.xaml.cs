using System;
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
        Page currentPage;
        private static int counter;

        public MainWindow(List<TankModule> list)
        {
            tankList = list;
            InitializeComponent();
            currentPage = new SimulationPage(tankList);
            _mainFrame.Content = currentPage;
            counter = 0;
        }

        private void SwitchView(object sender, RoutedEventArgs e)
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
        private void Download(object sender, RoutedEventArgs e)
        {
            // Extracting the data from here
            string startPath = @"..\..\..\..\LogData\EventLogData";
            // Where the zipped file is sent
            string zipPath = @"..\..\..\..\ZippedLog\download" + counter + ".zip";
            counter++;

            // Getting the selected dates
            DateTime? firstDate = fromDate.SelectedDate;
            DateTime? lastDate = toDate.SelectedDate;

            // Checking if the user has inputed values
            if (firstDate.HasValue && lastDate.HasValue)
            {
                // Initalizes a temporary subdirectory
                DirectoryInfo di = new DirectoryInfo(startPath);
                DirectoryInfo sdi = di.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";

                // Iterate over the files in startpath
                foreach (FileInfo file in di.EnumerateFiles())
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
                sdi.Delete(true);
            } else if (firstDate.HasValue) {
                DirectoryInfo di = new DirectoryInfo(startPath);
                DirectoryInfo sdi = di.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    if (file.CreationTime.Date >= firstDate)
                    {
                        File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                    }
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                sdi.Delete(true);

            }
            else if (lastDate.HasValue) {
                DirectoryInfo di = new DirectoryInfo(startPath);
                DirectoryInfo sdi = di.CreateSubdirectory("subdir");
                string targetPath = @"..\..\..\..\LogData\EventLogData\subdir";
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    if (file.CreationTime.Date >= firstDate && file.CreationTime.Date <= lastDate)
                    {
                        File.Copy(Path.Combine(startPath, file.Name), Path.Combine(targetPath, file.Name), true);
                    }
                }
                ZipFile.CreateFromDirectory(targetPath, zipPath);
                sdi.Delete(true);
            }
            else
            {
                ZipFile.CreateFromDirectory(startPath, zipPath);

            }

        }
    }
}
