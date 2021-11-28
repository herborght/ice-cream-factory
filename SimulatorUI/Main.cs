using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            readConfig(configFilePath);
            initializeTanks();
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

        private void readConfig(string configFilePath)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configFilePath);
            XmlNode config = xDoc.LastChild.ChildNodes[0];
            // var parameters = new ParameterDataBase(); 


            foreach (XmlNode mod in config)
            {
                // outer loop runs for every module in config
                Console.WriteLine("{0}", mod.Attributes["name"].Value);
                foreach (XmlNode param in mod.ChildNodes)
                {
                    // inner loop runs for every parameter in the current module
                    string name = param.InnerText;
                    string type = param.LocalName;
                    string from; // used as inoutchaining source

                    switch (type)
                    {
                        case "AnalogInputParameter":
                            // do stuff with parameter here, save in list etc
                            Console.WriteLine("   Parameter name: {0}", name);
                            Console.WriteLine("   Parameter type: {0}\n", type);
                            break;
                        case "AnalogOutputParameter":
                            // do stuff with parameter here, save in list etc
                            Console.WriteLine("   Parameter name: {0}", name);
                            Console.WriteLine("   Parameter type: {0}\n", type);
                            break;
                        case "DigitalInputParameter":
                            // do stuff with parameter here, save in list etc
                            Console.WriteLine("   Parameter name: {0}", name);
                            Console.WriteLine("   Parameter type: {0}\n", type);
                            break;
                        case "DigitalOutputParameter":
                            // do stuff with parameter here, save in list etc
                            Console.WriteLine("   Parameter name: {0}", name);
                            Console.WriteLine("   Parameter type: {0}\n", type);
                            break;
                        case "InOutChaining":
                            // do stuff with parameter here, save in list etc
                            from = param.Attributes["from"].Value;
                            Console.WriteLine("   Parameter name: {0}", name); //prints InFlow 
                            Console.WriteLine("   Parameter type: {0}", type); //prints InOutChaining 
                            Console.WriteLine("   Chaining source: {0}\n", from); //prints T1/OutFlow 
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        private void initializeTanks()//Will be extended later with config file
        {
            tankList = new List<TankModule>();
            foreach(IModule module in m_modules)
            {
                tankList.Add(new TankModule(module.Name));
            }
            updateTanks();
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
