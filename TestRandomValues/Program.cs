using System;
using ABB.InSecTT.Common;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.Control;
using SimulatorUI;
using ABB.InSecTT.SimulatorEnv;
using System.Threading.Tasks;

namespace TestRandomValues
{
    class Program
    {
        // DSD Joakim - Separate program.cs used for testing UI in isolation from simulation, with random values
        [STAThread]
        static void Main(string[] args)
        {
            IParameterDataBase parameters = ParameterDataBase.FromConfiguration("ConfigFiles/SimulatorConfigs/TankConfigSim.xml");
            Task.Run(() => ExecuteSimulation(parameters)); 
            var app2 = new Main(parameters, "ConfigFiles/SimulatorConfigs/TankConfigSim.xml");
            app2.Run();
        }

        static internal async Task ExecuteSimulation(IParameterDataBase parameters)
        {
            for (; ; )
            {
                updateParameters(parameters);
                await Task.Delay(10000);
            }
        }

        static private void updateParameters(IParameterDataBase parameters)
        {
            int rand = 0;
            Random random = new Random();
            int moduleNr = 0;
            string name;
            for (int i = 0; i < 5; i++)
            {
                rand = random.Next(1, 6);
                moduleNr = random.Next(1, 6);
                name = "T" + moduleNr.ToString();
                int val = random.Next(1);
                switch (rand)
                {
                    case 1:
                        double value = random.NextDouble();
                        ChangeParameter(name + "/Level:" + value, parameters);
                        ChangeParameter(name + "/LevelPercent:" + value/2 * 100, parameters);
                        break;
                    case 2:
                        ChangeParameter(name + "/InFlow:" + random.NextDouble().ToString(), parameters);
                        break;
                    case 4:
                        ChangeParameter(name + "/OutFlow:" + random.NextDouble().ToString(), parameters);
                        break;
                    case 5:

                        ChangeParameter(name + "/OpenOutlet:True", parameters);

                        break;
                    case 6:
                        if (val == 0)
                        {
                            ChangeParameter(name + "/OpenDumpValve:False", parameters);
                        }
                        else
                        {
                            ChangeParameter(name + "/OpenDumpValve:True", parameters);
                        }
                        break;
                }
            }
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
                    Console.WriteLine($"ChangeParameter {key}:{unparsedValue}");
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
                else
                {
                    Console.WriteLine($"Parameter not changed {key}:{unparsedValue}");
                }
            }
        }
    }
}
