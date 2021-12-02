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
            
            /*
            tankList = new List<TankModule>();
            foreach(IModule module in m_modules)
            {
                tankList.Add(new TankModule(module.Name));
            }
            */
            updateTanks();
        }
        private List<TankModule> readConfig(string configFilePath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configFilePath);
            XmlNode config = xDoc.LastChild.ChildNodes[0];

            var tankList = new List<TankModule>();

            foreach (XmlNode mod in config)
            {// Outer loop runs for every module in config
                // Module properties
                string m_name = mod.Attributes["name"].Value;
                string m_type = mod.Attributes["type"].Value;             
            
                // Parse and convert values to use comma separator instead of decimal point separator
                double.TryParse(mod.Attributes["baseArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_baseArea);
                double.TryParse(mod.Attributes["outletArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_outletArea);
                double.TryParse(mod.Attributes["height"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_height);

                var tank = new TankModule(m_name);
                tank.BaseArea = m_baseArea;
                tank.OutletArea = m_outletArea;
                tank.Height = m_height;             

                /*
                Console.WriteLine("Tank name: {0}", m_name);
                Console.WriteLine(" baseArea: {0}", m_baseArea);
                Console.WriteLine(" outletArea: {0}", m_outletArea);
                Console.WriteLine(" height: {0}", m_height);
                Console.WriteLine(" type: {0}\n", m_type);
                */

                foreach (XmlNode param in mod.ChildNodes)
                {// Inner loop runs for every parameter in the current module

                    string p_name = param.InnerText;
                    string p_type = param.LocalName;
                    string from; // Used as InOutChaining source

                    switch (p_type)
                    {
                        case "AnalogInputParameter":
                        case "AnalogOutputParameter":
                        case "DigitalInputParameter":
                        case "DigitalOutputParameter":
                            // Do stuff with parameter here
                            /*
                            Console.WriteLine("   Parameter name: {0}", p_name);
                            Console.WriteLine("   Parameter type: {0}\n", p_type);
                            */
                            break;
                        case "InOutChaining":
                            // Do stuff with parameter here
                            from = param.Attributes["from"].Value;
                            /*
                            Console.WriteLine("   Parameter name: {0}", p_name);
                            Console.WriteLine("   Parameter type: {0}", p_type);
                            Console.WriteLine("   Chaining source: {0}\n", from);
                            */
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
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).LevelPercenatage = parameter.AnalogValue;
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
