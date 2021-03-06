using System;
using System.Xml;
using System.Reflection;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ABB.InSecTT.Common;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Modules;
using System.Runtime.CompilerServices;
using ABB.InSecTT.SimulatorEnv.Calculations;

[assembly: AssemblyVersion("1.3.*")]
[assembly: InternalsVisibleTo("SimulationTests")]

namespace ABB.InSecTT.SimulatorEnv
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {

            VersionDisplay.VersionWrite();
            List<ModuleBase> modules = new List<ModuleBase>();
            var parameters = LoadModules(modules, args[0]);
            var tokenSource1 = new CancellationTokenSource();
            var token1 = tokenSource1.Token;
            ConfigureLogging(parameters);
            SimulationEventSource.Log.Startup();

            MenuHandler menuHandler = new MenuHandler(CreateCommands(parameters));
            ExecEngine execEngine = new ExecEngine();
            string host = ConfigurationManager.AppSettings.Get("BrokerAddress");
            int.TryParse(ConfigurationManager.AppSettings.Get("MsgCycleDelay"), out int msgCycleDelay);
            int.TryParse(ConfigurationManager.AppSettings.Get("ExecutionInterval"), out int executionInterval);
            double.TryParse(ConfigurationManager.AppSettings.Get("SpeedModifier"), out double speedModifier);
            MessageHandling messageHandler = new MessageHandling(parameters, msgCycleDelay, host);
            SimulationEventSource.Log.ConnectMQTT(host);
            MixProperties state = null;
            if (args.Length > 1)
                state = new MixProperties(args[1]);
            else
                state = new MixProperties();


            var p = Task.Run(() => execEngine.ExecuteSimulation(modules, parameters, executionInterval, speedModifier, state, token1), token1);
            var q = Task.Run(() => messageHandler.SendMessages(token1), token1);
            var r = Task.Run(() => menuHandler.HandleCommand());

            // DSD - Start UI
            RunApplication(parameters/*, modules*/, args[0]);

            r.Wait();
            SimulationEventSource.Log.ExecutionStop();

            try
            {
                tokenSource1.Cancel();
                p.Wait(1000);
                q.Wait(1000);
            }
            catch (System.OperationCanceledException)
            {
                SimulationEventSource.Log.ExecutionStop();
                Console.WriteLine("Quitting...");
            }
            finally
            {
                tokenSource1.Dispose();
            }

        }

        private static void ConfigureLogging(IParameterDataBase parameters)
        {
            string eventSourceName;
            SimulationListener simulationEventListener;
            switch (ConfigurationManager.AppSettings.Get("LoggingMethod"))
            {
                case "EventLog":
                    eventSourceName = ConfigurationManager.AppSettings.Get("EventSourceName");
                    simulationEventListener = new SimulationListener(eventSourceName);
                    break;
                case "SimpleLog":
                    SimpleFileLog.Initialize(parameters);
                    break;
                case "AllLogs":
                    eventSourceName = ConfigurationManager.AppSettings.Get("EventSourceName");
                    simulationEventListener = new SimulationListener(eventSourceName);
                    SimpleFileLog.Initialize(parameters);
                    break;
                case "None":
                    break;
                default:
                    SimulationEventSource.Log.Startup();
                    SimpleFileLog.Initialize(parameters);
                    break;

            }
        }

        private static void RunApplication(IParameterDataBase parameters/*, IEnumerable<IModule> modules*/, string configFilePath)
        {
            //var application = new System.Windows.Application();
            //application.Run(new SimulatorUITest.SimulationWindow(parameters, modules));

            // DSD Emil - SimulatorUI.Main is a replacement for standard WPF App.xaml
            var app2 = new SimulatorUI.Main(parameters/*, modules*/, configFilePath);
            app2.Run();
        }

        private static IEnumerable<ICmd> CreateCommands(IParameterDataBase parameters)
        {
            Action<string> displayParameters;
            Action<string> changeParameters;

            changeParameters = (s) => ChangeParameter(s, parameters);
            displayParameters = (t) => DisplayParameters(parameters);

            yield return new Command("d", "d = Display parameters", displayParameters);
            yield return new Command("s", "s <parameter>:<value> = change <parameter> to <value>", changeParameters);

            // DSD Course - Hardcoded "controller" behaviour
            Action<string> mimicControllerBehaviour;
            mimicControllerBehaviour = (m) => MimicControllerBehaviour(parameters);
            yield return new Command("m", "m = Mimic basic controller behaviour", mimicControllerBehaviour);

        }

        // DSD Emil - Hardcoded to mimic some controller behaviour for TankConfig
        private static void MimicControllerBehaviour(IParameterDataBase parameters)
        {
            // "open" t1 inflow by changes its inflow value
            ChangeParameter("T1/InFlow:0,075", parameters);

            // wait while t1 is filling, total wait time is [wait] amount of seconds
            int wait = 10;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }

            // "close" the t1 inflow by zeroing it, open the t1 outlet
            ChangeParameter("T1/InFlow:0,0", parameters);
            ChangeParameter("T1/OpenOutlet:true", parameters);

            wait = 10;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T1/OpenOutlet:false", parameters); // some changes for the pasteurization module
            ChangeParameter("T2/HeaterOn:True", parameters); //Heat for 10s
            wait = 10;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            wait = 20;
            ChangeParameter("T2/HeaterOn:False", parameters);
            ChangeParameter("T2/CoolerOn:True", parameters);//Cool for 20s
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T2/CoolerOn:False", parameters);
            ChangeParameter("T2/OpenOutlet:true", parameters); //Let it out

            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T2/OpenOutlet:false", parameters);
            wait = 10;
            ChangeParameter("T3/HomogenizationOn:True", parameters);
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T3/HomogenizationOn:False", parameters);
            ChangeParameter("T3/AgeingCoolingOn:True", parameters);
            wait = 20;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T3/AgeingCoolingOn:False", parameters);
            ChangeParameter("T3/OpenOutlet:True", parameters);
            wait = 20;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T3/OpenOutlet:False", parameters);
            ChangeParameter("T3/OpenDumpValves:True", parameters);
            wait = 20;

            ChangeParameter("T4/FreezingOn:True", parameters);
            ChangeParameter("T4/PasteurizationUnits:1,0", parameters);
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T4/DasherOn:True", parameters);
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T4/StartLiquidFlavoring:True", parameters);
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T4/DasherOn:False", parameters);
            ChangeParameter("T4/StartLiquidFlavoring:False", parameters);
            ChangeParameter("T4/FreezingOn:False", parameters);
            ChangeParameter("T4/OpenOutlet:True", parameters);
            wait = 20;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T4/OpenOutlet:False", parameters);
            ChangeParameter("T5/StartFlavoring:True", parameters);
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T5/StartFlavoring:False", parameters);
            wait = 10;
            ChangeParameter("T5/OpenOutlet:True", parameters);
            ChangeParameter("T5/StartPackaging:True", parameters);
            wait = 20;
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T5/StartPackaging:False", parameters);
            
            ChangeParameter("T5/StartHardening:True", parameters); //Temp turns into NaN here
            for (int i = 0; i < wait; i++)
            {
                Thread.Sleep(1000);
                DisplayParameters(parameters);
            }
            ChangeParameter("T5/StartHardening:False", parameters);
            ChangeParameter("T5/FinishedBatch:True", parameters);
            DisplayParameters(parameters);
        }

        private static void ChangeParameter(string unparsed, IParameterDataBase parameters)
        {

            string[] cmd = unparsed.Split(':');
            if (cmd.Length == 2)
            {
                string key = cmd[0];
                string unparsedValue = cmd[1];
                if (parameters.ContainsParameter(key))
                {
                    SimulationEventSource.Log.Command($"ChangeParameter {key}:{unparsedValue}");
                    if (bool.TryParse(unparsedValue, out bool result))
                    {
                        parameters.GetParameter(key).DigitalValue = result;
                        return;
                    }
                    else if (double.TryParse(unparsedValue, out double analogResult))
                    {
                        parameters.GetParameter(key).AnalogValue = analogResult;
                        return;
                    }
                }
            }

        }

        

        private static void DisplayParameters(IParameterDataBase parameters)
        {
            foreach (var parameterKey in parameters.ParameterKeys)
            {
                var parameter = parameters.GetParameter(parameterKey);
                if (parameter.ValueType == ParameterType.Analog)
                    Console.WriteLine("{0}:{1:F3}", parameterKey, parameter.AnalogValue);

                else
                    Console.WriteLine("{0}:{1}", parameterKey, parameter.DigitalValue);
            }
        }

        /// <summary>
        /// Loads the modules from a designated XML file
        /// </summary>
        /// <returns>List of parameters in sim-environment</returns>
        private static IParameterDataBase LoadModules(List<ModuleBase> modules, string configFilePath)
        {
            Console.WriteLine("Load Modules");
            var parameters = ParameterDataBase.FromConfiguration(configFilePath);
            Console.WriteLine("Reading config");


            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(configFilePath);
            XmlNodeList mods = xDoc.GetElementsByTagName("modules");

            //This loops creates each module in the XML file and sets their values to the attributes provided in the file.
            foreach (XmlNode mod in mods[0])
            {
                ModuleBase module;
                string name = mod.Attributes["name"].Value;

                double.TryParse(mod.Attributes["baseArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double baseArea);
                double.TryParse(mod.Attributes["outletArea"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double outletArea);
                double.TryParse(mod.Attributes["height"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double height);

                switch (mod.Attributes["type"].Value)
                {
                    case "SimEnv":
                        // DSD Emil - "module" for environment parameters, used for adding ambient temp to DB
                        module = new SimEnv(name);
                        break;
                    case "TankModule":
                        module = new TankModule(name, baseArea, outletArea, height);
                        break;

                    case "PasteurizationModule":
                        double.TryParse(mod.Attributes["heaterTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double heaterTemp);
                        double.TryParse(mod.Attributes["coolerTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double coolerTemp);
                        double.TryParse(mod.Attributes["thickness"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double thickness);
                        double.TryParse(mod.Attributes["HTC"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double HTC);
                        double.TryParse(mod.Attributes["CTC"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double CTC);
                        module = new PasteurizationModule(name, baseArea, outletArea, height, heaterTemp, coolerTemp, thickness, HTC, CTC);
                        break;

                    case "HomogenizationModule":
                        double.TryParse(mod.Attributes["stage1Pressure"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double stage1Pressure);
                        double.TryParse(mod.Attributes["stage2Pressure"].Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double stage2Pressure);
                        module = new HomogenizationModule(name, baseArea, outletArea, height, stage1Pressure, stage2Pressure);
                        break;

                    case "FlavoringPackagingModule":
                        double.TryParse(mod.Attributes["coolerTemperature"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double coolerTemperature);
                        string packagingType = mod.Attributes["packagingType"].Value;
                        module = new HardeningFlavoringPacking(name, baseArea, outletArea, height, packagingType, coolerTemperature);
                        break;

                    case "FreezingModule":
                        double.TryParse(mod.Attributes["freezerTemp"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double freezerTemp);
                        double.TryParse(mod.Attributes["barrelRotationSpeed"].Value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double barrelRotationSpeed);
                        module = new FreezingModule(name, baseArea, outletArea, height, freezerTemp, barrelRotationSpeed);
                        break;

                    default:
                        continue;
                }


                modules.Add(module);
                module.TieParameters(parameters);
            }
            return parameters;

        }
    }
}
