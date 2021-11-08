using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using ABB.InSecTT.SimulatorEnv;
using System.Threading.Tasks;
using System.Threading;
using ABB.InSecTT.SimulatorEnv.Modules;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;
using System.IO;
using System.Reflection;

namespace SimulationTests
{
    [TestClass]
    public class SimulationExecTests
    {

        [TestMethod]
        public void CancelExecEngineTest()
        {
            var eng = new ExecEngine();
            var tokenSource1 = new CancellationTokenSource();
            var token = tokenSource1.Token;
            var modules = new List<ModuleBase>();
            var filename = GetSimPath();
            var state = new MixProperties(GetRecipePath());
            var parameters = ParameterDataBase.FromDictionary(new Dictionary<string, IParameter>() { });
            var p = Task.Run(() => eng.ExecuteSimulation(modules, parameters, 100, 1, state, token), token);
            Thread.Sleep(1000);            
            tokenSource1.Cancel();
            p.Wait();
            // If we got here, the task is successfully cancelled!
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ExecEngineWModuleTest()
        {
            var eng = new ExecEngine();
            var tokenSource1 = new CancellationTokenSource();
            var token = tokenSource1.Token;
            var parameters = CreateParams();
            var modules = CreateModules(parameters);
            var filename = GetSimPath();
            var state = new MixProperties(GetRecipePath());
            var p = Task.Run(() => eng.ExecuteSimulation(modules, parameters, 100, 1, state, token), token);
            Thread.Sleep(1000);
            tokenSource1.Cancel();
            p.Wait();
            // If we got here, the task is successfully cancelled!
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ExecEngineTankModuleTest1()
        {
            var eng = new ExecEngine();
            var tokenSource1 = new CancellationTokenSource();
            var token = tokenSource1.Token;
            var parameters = CreateParams();
            var modules = CreateModules(parameters);
            var filename = GetSimPath();
            var state = new MixProperties(GetRecipePath());

            parameters.GetParameter("Tank1/OpenOutlet").DigitalValue = false;
            parameters.GetParameter("Tank1/InFlow").AnalogValue = 0;
            parameters.GetParameter("Tank1/Level").AnalogValue = 1;

            var p = Task.Run(() => eng.ExecuteSimulation(modules, parameters, 100, 1, state, token), token);
            Thread.Sleep(1000);

            // No change in level with no in-flow and closed outlet
            Assert.AreEqual(parameters.GetParameter("Tank1/Level").AnalogValue, 1);

            parameters.GetParameter("Tank1/OpenOutlet").DigitalValue = true;
            Thread.Sleep(1000);

            // Level shoud decrease if outlet open and no inflow
            var lowLevel = parameters.GetParameter("Tank1/Level").AnalogValue;
            Assert.IsTrue(lowLevel < 1);

            parameters.GetParameter("Tank1/OpenOutlet").DigitalValue = false;
            parameters.GetParameter("Tank1/InFlow").AnalogValue = 0.01;

            Thread.Sleep(1000);

            // Level shoud increase if outlet closed and inflow > 0
            Assert.IsTrue(lowLevel < parameters.GetParameter("Tank1/Level").AnalogValue);

            tokenSource1.Cancel();
            p.Wait();
            // If we got here, the task is successfully cancelled!
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ExecEngineTankModuleInOutChainingTest()
        {
            var eng = new ExecEngine();
            var tokenSource1 = new CancellationTokenSource();
            var token = tokenSource1.Token;
            ParameterDataBase parameters = (ParameterDataBase)CreateParams2();

            parameters.GetParameter("Tank1/OpenOutlet").DigitalValue = true;
            parameters.GetParameter("Tank1/InFlow").AnalogValue = 0;
            parameters.GetParameter("Tank1/Level").AnalogValue = 1;

            var modules = CreateModules(parameters);
            var filename = GetSimPath();
            var state = new MixProperties(GetRecipePath());
            var p = Task.Run(() => eng.ExecuteSimulation(modules, parameters, 100, 1, state, token), token);
           
            Thread.Sleep(1000);

            //Tank2 Level should increase if outlet open on a previous tank
            var Tank2Level = parameters.GetParameter("Tank2/Level").AnalogValue;
            Assert.IsTrue(Tank2Level > 0);

            tokenSource1.Cancel();
            p.Wait();
            // If we got here, the task is successfully cancelled!
            Assert.IsTrue(true);
        }

        IParameterDataBase CreateParams()
        {
            var paramDict = new Dictionary<string, IParameter>() {
                {"Tank1/InFlow", new AnalogInputParameter() },                
                {"Tank1/OutFlow", new AnalogOutputParameter() },
                {"Tank1/InFlowTemp", new AnalogInputParameter() },
                {"Tank1/OutFlowTemp", new AnalogOutputParameter() },
                {"Tank1/Temperature", new AnalogOutputParameter() },
                {"Tank1/Level", new AnalogOutputParameter() },                
                {"Tank1/OpenOutlet", new DigitalInputParameter() },
                {"Tank1/LevelPercent", new AnalogOutputParameter() },
                {"Tank1/OpenDumpValve", new AnalogOutputParameter() },
            };
            return ParameterDataBase.FromDictionary(paramDict);
        }
        IParameterDataBase CreateParams2()
        {
            AnalogOutputParameter t1Flow = new AnalogOutputParameter();
            AnalogOutputParameter t1FlowTemp = new AnalogOutputParameter();
            var paramDict = new Dictionary<string, IParameter>() {
                {"Tank1/InFlow", new AnalogInputParameter() },
                {"Tank1/OutFlow", t1Flow },
                {"Tank1/Level", new AnalogOutputParameter() },
                {"Tank1/OpenOutlet", new DigitalInputParameter() },
                {"Tank1/LevelPercent", new AnalogOutputParameter() },
                {"Tank1/OpenDumpValve", new AnalogOutputParameter() },
                {"Tank1/InFlowTemp", new AnalogInputParameter() },
                {"Tank1/OutFlowTemp", t1FlowTemp },
                {"Tank1/Temperature", new AnalogOutputParameter() },
                {"Tank2/InFlow", new CalculatedParameter(CalculatedParameter.InOutChaining, t1Flow) },
                {"Tank2/OutFlow", new AnalogOutputParameter() },
                {"Tank2/Level", new AnalogOutputParameter() },
                {"Tank2/OpenOutlet", new DigitalInputParameter() },
                {"Tank2/LevelPercent", new AnalogOutputParameter() },
                {"Tank2/OpenDumpValve", new AnalogOutputParameter() },                
                {"Tank2/InFlowTemp",  new CalculatedParameter(CalculatedParameter.InOutChaining, t1FlowTemp) },
                {"Tank2/OutFlowTemp", new AnalogOutputParameter() },
                {"Tank2/Temperature", new AnalogOutputParameter() },
            };
            return ParameterDataBase.FromDictionary(paramDict);
        }

        List<ModuleBase> CreateModules(IParameterDataBase parameters)
        {            
            var modules = new List<ModuleBase>();

            foreach (var item in parameters.ParameterKeys)
            {
                var filter = modules.Where(p => String.Equals(p.Name, item.Split("/")[0], StringComparison.CurrentCulture)).ToList();
                
                if (filter.Count() == 0)
                {
                    modules.Add(new TankModule(item.Split("/")[0], 1, 0.01, 1));
                }
            }

            foreach (var item in modules)
            {
                item.TieParameters(parameters);
            }
            return modules;
        }

        string GetSimPath()
        {
            var sep = Path.DirectorySeparatorChar;
            return $"ConfigFiles{sep}SimulatorConfigs{sep}MultipleModulesConfigSim.xml";
        }
        string GetRecipePath()
        {
            var sep = Path.DirectorySeparatorChar;
            return $"IngredientRecipe{sep}IngredientRecipe.xml";
        }

        
    }

}
