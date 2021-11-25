using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class Main
    {
        private IParameterDataBase m_parameters;
        private IEnumerable<IModule> m_modules;
        private List<TankModule> Tanks;

        public Main(IParameterDataBase parameters, IEnumerable<IModule> modules)
        {
            m_parameters = parameters;
            m_modules = modules;
            Tanks = initializeTanks();
        }

        public void Run()
        {
            var application = new System.Windows.Application();
            application.Run(new MainWindow());
        }

        private List<TankModule> initializeTanks()//alternative: just add modules and assign parameters later, will also be updated when access to config files has been fixed
        {
            List<TankModule> tankList = new List<TankModule>();
            foreach (var parameterKey in m_parameters.ParameterKeys)
            {
                if(!tankList.Exists(tank => tank.Name == parameterKey.Split('/')[0]))
                {
                    tankList.Add(new TankModule(parameterKey.Split('/')[0]));
                }

                var parameter = m_parameters.GetParameter(parameterKey);
                if (parameter.ValueType == ParameterType.Analog)
                {
                    switch(parameterKey.Split('/')[1])
                    {
                        case "Level": 
                            tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]).Level = parameter.AnalogValue;
                            break;
                        case "LevelPercentage":
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
            return tankList;
        }

        //other functions etc.
        //like creating the other objects from our planned classes
    }
}
