using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;
using ABB.InSecTT.SimulatorEnv.Modules;
using SimulatorUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ABB.InSecTT.SimulatorEnv
{
    class ExecuteVisuals
    {
        internal async Task ExecuteSimulation(List<ModuleBase> modules, IParameterDataBase parameters, double executionInterval, double speedModifier, MixProperties state, CancellationToken token, Main app)//Testing loop
        {
            Console.WriteLine("Starting Simulation execution");
            SimulationEventSource.Log.ExecutionStart();
            double msPerLap = executionInterval; //change the function call to "low" if you want to use a lower execution speed or "high" if you want to increae the execution speed. Entering anything else will use the default value.

            if (msPerLap < 500)
                msPerLap = 500;

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
                app.accessDatabase(parameters, modules);
                
                await Task.Delay((int)(msPerLap / speedModifier));
            }
            return;
        }
    }
}
