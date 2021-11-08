using ABB.InSecTT.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SimulatorUITest
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class SimulationWindow : Window
    {
        private IParameterDataBase m_parameters;
        private IEnumerable<IModule> m_modules;

        public SimulationWindow(IParameterDataBase parameters, IEnumerable<IModule> modules)
        {
            m_modules = modules;
            m_parameters = parameters;

            InitializeComponent();            
        }
    }
}
