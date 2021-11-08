using ABB.InSecTT.SimulatorEnv.Modules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.SimulatorEnv.Calculations;
using System.Configuration;

namespace ABB.InSecTT.SimulatorEnv
{
    public class ExecEngine
    {

        internal async Task ExecuteSimulation(List<ModuleBase> modules, IParameterDataBase parameters, double executionInterval, double speedModifier, MixProperties state, CancellationToken token)
        {
            Console.WriteLine("Starting Simulation execution");
            SimulationEventSource.Log.ExecutionStart();
            double msPerLap=executionInterval; //change the function call to "low" if you want to use a lower execution speed or "high" if you want to increae the execution speed. Entering anything else will use the default value.
            
            if (msPerLap < 5 )
                msPerLap = 5;

            if (msPerLap > 1000)
                msPerLap = 1000;

            if (msPerLap / speedModifier < 5)
                speedModifier = msPerLap / 5;

            for (; ; )
            {
                if (token.IsCancellationRequested)
                {                    
                    break;
                }

                foreach (var module in modules)
                    module.Execute((int)msPerLap, state);

                foreach (var parameterKey in parameters.ParameterKeys)
                {
                    var parameter = parameters.GetParameter(parameterKey);
                    if (parameter.ValueType == ParameterType.Analog)
                        
                        SimulationEventSource.Log.SimulationState(parameterKey, parameter.AnalogValue.ToString());
                    else
                        SimulationEventSource.Log.SimulationState(parameterKey, parameter.DigitalValue.ToString());

                }
                await Task.Delay((int)(msPerLap / speedModifier));
            }
            return;
        }

        

    }


}
