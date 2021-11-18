using ABB.InSecTT.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class Main
    {
        private IParameterDataBase m_parameters;
        private IEnumerable<IModule> m_modules;

        public Main(IParameterDataBase parameters, IEnumerable<IModule> modules)
        {
            m_parameters = parameters;
            m_modules = modules;
        }

        public void Run()
        {
            var application = new System.Windows.Application();
            application.Run(new MainWindow());
        }

        //other functions etc.
        //like creating the other objects from our planned classes
    }
}
