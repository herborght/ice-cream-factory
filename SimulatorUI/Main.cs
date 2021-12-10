using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Globalization;

namespace SimulatorUI
{
    public class Main
    {
        private IParameterDataBase m_parameters;
        private IEnumerable<IModule> m_modules;
        private List<TankModule> tankList;

        public Main(IParameterDataBase parameters, IEnumerable<IModule> modules, string configFilePath)
        {
            m_parameters = parameters;
            m_modules = modules;
            initializeTanks(configFilePath);
        }

        public void Run()
        {
            var application = new System.Windows.Application();
            Task.Run(() => ExecuteSimulation());
            application.Run(new MainWindow(tankList));

        }

        internal async Task ExecuteSimulation()
        {
            for (; ; )
            {
                updateTanks();
                await Task.Delay(1000);
            }
        }      

        private void initializeTanks(string configFilePath)
        {
            tankList = readConfig(configFilePath);

            updateTanks();
        }

        // DSD Emil - reads all modules from config and adds them to list
        private List<TankModule> readConfig(string configFilePath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configFilePath);
            XmlNode config = xDoc.LastChild.ChildNodes[0];

            var tankList = new List<TankModule>();

            foreach (XmlNode mod in config)
            {
                // Module properties
                string m_name = mod.Attributes["name"].Value;
                string m_type = mod.Attributes["type"].Value;             
            
                // Parse and convert values to use comma separator instead of decimal point separator
                double.TryParse(mod.Attributes["baseArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_baseArea);
                double.TryParse(mod.Attributes["outletArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_outletArea);
                double.TryParse(mod.Attributes["height"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_height);

                // To avoid adding simulator environment as a tank, skip the current iteration of the loop,
                // will need to be changed/extended when we expand for more module types.
                if (m_type.Equals("SimEnv"))
                {
                    continue;
                }                

                var tank = new TankModule(m_name);
                tank.BaseArea = m_baseArea;
                tank.OutletArea = m_outletArea;
                tank.Height = m_height;             

                foreach (XmlNode param in mod.ChildNodes)
                {
                    string p_name = param.InnerText;
                    string p_type = param.LocalName;
                    string from; // Used as InOutChaining source

                    switch (p_type)
                    {
                        case "AnalogInputParameter":
                        case "AnalogOutputParameter":
                        case "DigitalInputParameter":
                        case "DigitalOutputParameter":
                            break;
                        case "InOutChaining":
                            // Add the chaining source tank to InFlowTanks list
                            from = param.Attributes["from"].Value;
                            TankModule inFlowSourceTank = tankList.Find(t => t.Name.Equals(from.Split('/')[0]));

                            // First check if the inflowsource already exists in the list, to avoid duplicates
                            if(!tank.InFlowTanks.Exists(t => t.Name.Equals(inFlowSourceTank.Name)))
                            {
                                tank.InFlowTanks.Add(inFlowSourceTank);
                            }                         
                            break;
                        default:
                            continue;
                    }
                }
                tankList.Add(tank);
            }
            return tankList;
        }

        private void updateTanks()
        {
            foreach (var parameterKey in m_parameters.ParameterKeys)
            {
                if (!tankList.Exists(tank => tank.Name == parameterKey.Split('/')[0])) //if new tank modules can be added during runtime
                {
                    tankList.Add(new TankModule(parameterKey.Split('/')[0]));
                }
                var parameter = m_parameters.GetParameter(parameterKey);
                if (parameter.ValueType == ParameterType.Analog)
                {
                    switch (parameterKey.Split('/')[1])
                    {
                        case "Level":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).Level = parameter.AnalogValue;
                            break;
                        case "LevelPercent":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).LevelPercentage = parameter.AnalogValue;
                            break;
                        case "InFlow":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).InletFlow = parameter.AnalogValue;
                            break;
                        case "InFlowTemp":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).InFlowTemp = parameter.AnalogValue;
                            break;
                        case "Temperature":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).Temperature = parameter.AnalogValue;
                            break;
                        case "OutFlowTemp":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).OutFlowTemp = parameter.AnalogValue;
                            break;
                        case "OutFlow":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).OutLetFlow = parameter.AnalogValue;
                            break;
                    }
                }
                else
                {
                    switch (parameterKey.Split('/')[1])
                    {
                        case "OpenDumpValve":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).DumpValveOpen = parameter.DigitalValue;
                            break;
                        case "OpenOutlet":
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).OutValveOpen = parameter.DigitalValue;
                            break;
                    }
                }
            }
        }
        //other functions etc.
        //like creating the other objects from our planned classes
    }
}
