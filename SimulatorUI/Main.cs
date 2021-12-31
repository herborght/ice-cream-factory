﻿using ABB.InSecTT.Common.Configuration;
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
        private List<TankModule> tankList;

        public Main(IParameterDataBase parameters, string configFilePath)
        {
            m_parameters = parameters;
            initializeTanks(configFilePath);
        }

        public void Run()
        {
            var application = new System.Windows.Application();
            Task.Run(() => ExecuteSimulation());
            application.Run(new MainWindow(tankList));
        }

        // DSD Joakim - Update loop for values
        internal async Task ExecuteSimulation()
        {
            for (; ; )
            {
                updateTanks();
                await Task.Delay(1000);
            }
        }

        // DSD Joakim - Initializing the tanks
        private void initializeTanks(string configFilePath)
        {
            tankList = readConfig(configFilePath);

            updateTanks();
        }

        // DSD Emil - Reads all modules from config and adds them to list
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

                // To avoid adding simulator environment as a tank, skip the current iteration of the loop
                if (m_type == "SimEnv")
                {
                    continue;
                }

                TankModule tank;

                switch (m_type)
                {
                    case "TankModule":
                        tank = new TankModule(m_name);
                        break;
                    case "PasteurizationModule":
                        double.TryParse(mod.Attributes["heaterTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_HeaterTemp);
                        double.TryParse(mod.Attributes["coolerTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_CoolerTemp);
                        double.TryParse(mod.Attributes["thickness"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_Thickness);
                        double.TryParse(mod.Attributes["HTC"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_HTC);
                        double.TryParse(mod.Attributes["CTC"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_CTC);
                        tank = new PasteurizationModule(m_name, m_HeaterTemp, m_CoolerTemp, m_Thickness, m_HTC, m_CTC);
                        break;
                    case "HomogenizationModule":
                        double.TryParse(mod.Attributes["stage1Pressure"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_S1Pressure);
                        double.TryParse(mod.Attributes["stage2Pressure"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_S2Pressure);
                        tank = new HomogenizationModule(m_name, m_S1Pressure, m_S2Pressure);
                        break;
                    case "FreezingModule":
                        double.TryParse(mod.Attributes["freezerTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_FreezerTemp);
                        double.TryParse(mod.Attributes["barrelRotationSpeed"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_BRSpeed);
                        tank = new FreezingModule(m_name, m_FreezerTemp, m_BRSpeed);
                        break;
                    case "FlavoringPackagingModule":
                        string m_PType = mod.Attributes["packagingType"].Value;
                        double.TryParse(mod.Attributes["coolerTemperature"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double m_CoolerTemp2);
                        tank = new FlavoringHardeningPackingModule(m_name, m_PType, m_CoolerTemp2);
                        break;
                    default:
                        throw new NotImplementedException();
                }

                tank.BaseArea = m_baseArea;
                tank.OutletArea = m_outletArea;
                tank.Height = m_height;

                foreach (XmlNode param in mod.ChildNodes)
                {
                    string p_name = param.InnerText;
                    string p_type = param.LocalName;
                    // Used as InOutChaining source
                    string from;

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
                            if (!tank.InFlowTanks.Exists(t => t.Name.Equals(inFlowSourceTank.Name)))
                            {
                                tank.InFlowTanks.Add(inFlowSourceTank);
                            }
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                tankList.Add(tank);
            }
            return tankList;
        }

        // DSD Joakim - Main update function, goes thorugh different functions depending on the type,
        // as all tanks are tank modules all will go through updateBase
        private void updateTanks()
        {
            foreach (var parameterKey in m_parameters.ParameterKeys)
            {
                if (parameterKey.Split('/')[0] == "SimEnv") // DSD Emil - SimEnv is not a tank, it only exists in the DB for visualization purposes.
                {
                    //add code for updating displayed ambient temp here, currently empty since amb temp is not visualized yet
                    continue;
                }
                var current = tankList.Find(tank => tank.Name == parameterKey.Split('/')[0]);
                updateBase(parameterKey, current);

                switch (current)
                {
                    case PasteurizationModule p:
                        updatePasteurizationTank(parameterKey, p);
                        break;
                    case HomogenizationModule h:
                        updateHomogenizationTank(parameterKey, h);
                        break;
                    case FreezingModule f:
                        updateFreezingModule(parameterKey, f);
                        break;
                    case FlavoringHardeningPackingModule fhp:
                        updateFlavoringPackagingModule(parameterKey, fhp);
                        break;
                    default:
                        break;
                }
            }
        }

        // DSD Joakim - Update the basic values each tank has
        private void updateBase(string parameterKey, TankModule current)
        {
            var parameter = m_parameters.GetParameter(parameterKey);
            if (parameter.ValueType == ParameterType.Analog)
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "Level":
                        current.Level = parameter.AnalogValue;
                        break;
                    case "LevelPercent":
                        current.LevelPercentage = parameter.AnalogValue;
                        break;
                    case "InFlow":
                        current.InletFlow = parameter.AnalogValue;
                        break;
                    case "InFlowTemp":
                        current.InFlowTemp = parameter.AnalogValue;
                        break;
                    case "Temperature":
                        current.Temperature = parameter.AnalogValue;
                        break;
                    case "OutFlowTemp":
                        current.OutFlowTemp = parameter.AnalogValue;
                        break;
                    case "OutFlow":
                        current.OutLetFlow = parameter.AnalogValue;
                        break;
                }
            }
            else
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "OpenDumpValve":
                        current.DumpValveOpen = parameter.DigitalValue;
                        break;
                    case "OpenOutlet":
                        current.OutValveOpen = parameter.DigitalValue;
                        break;
                }
            }
        }

        // DSD Joakim - Update the dynamic values of the pasteurization tank
        private void updatePasteurizationTank(string parameterKey, PasteurizationModule current)
        {
            var parameter = m_parameters.GetParameter(parameterKey);
            if (parameter.ValueType == ParameterType.Digital)
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "HeaterOn":
                        current.HeaterOn = parameter.DigitalValue;
                        break;
                    case "CoolerOn":
                        current.CoolerOn = parameter.DigitalValue;
                        break;
                }
            }
        }

        // DSD Joakim - Update the dynamic values of the homogenization tank
        private void updateHomogenizationTank(string parameterKey, HomogenizationModule current)
        {
            var parameter = m_parameters.GetParameter(parameterKey);
            if (parameter.ValueType == ParameterType.Digital)
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "HomogenizationOn":
                        current.HomogenizationOn = parameter.DigitalValue;
                        break;
                    case "AgeingCoolingOn":
                        current.AgeingCoolingOn = parameter.DigitalValue;
                        break;
                }
            }
            else
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "ParticleSize":
                        current.ParticleSize = parameter.AnalogValue;
                        break;
                    case "MixTemperature":
                        current.MixTemperature = parameter.AnalogValue;
                        break;
                }
            }
        }

        // DSD Joakim - Update the dynamic values of the freezing module
        private void updateFreezingModule(string parameterKey, FreezingModule current)
        {
            var parameter = m_parameters.GetParameter(parameterKey);
            if (parameter.ValueType == ParameterType.Analog)
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "ParticleSize":
                        current.ParticleSize = parameter.AnalogValue;
                        break;
                    case "MixTemperature":
                        current.MixTemperature = parameter.AnalogValue;
                        break;
                    case "Overrun":
                        current.Overrun = parameter.AnalogValue;
                        break;
                    case "PasteurizationUnits":
                        current.PasteurizationUnits = parameter.AnalogValue;
                        break;
                }
            }
            else
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "FreezingOn":
                        current.FreezingOn = parameter.DigitalValue;
                        break;
                    case "DasherOn":
                        current.DasherOn = parameter.DigitalValue;
                        break;
                    case "StartLiquidFlavoring":
                        current.StartLiquidFlavoring = parameter.DigitalValue;
                        break;
                    case "SendTestValues":
                        current.SendTestValues = parameter.DigitalValue;
                        break;

                }
            }
        }

        // DSD Joakim - Update the dynamic values of the flavoringhardeningpackaging module
        private void updateFlavoringPackagingModule(string parameterKey, FlavoringHardeningPackingModule current)
        {
            var parameter = m_parameters.GetParameter(parameterKey);
            if (parameter.ValueType == ParameterType.Analog)
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "MixTemperature":
                        current.MixTemperature = parameter.AnalogValue;
                        break;
                }
            }
            else
            {
                switch (parameterKey.Split('/')[1])
                {
                    case "StartFlavoring":
                        current.StartFlavoring = parameter.DigitalValue;
                        break;
                    case "StartHardening":
                        current.StartHardening = parameter.DigitalValue;
                        break;
                    case "StartPackaging":
                        current.StartPackaging = parameter.DigitalValue;
                        break;
                    case "FinishBatch":
                        current.FinishBatch = parameter.DigitalValue;
                        break;

                }
            }
        }
    }
}
