using System;
using System.Collections.Generic;
using System.Text;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{

    /// <summary>
    /// Example of a module consisting of several simple modules to emulate 
    /// </summary>
    internal class ComplexModule : ModuleBase
    {
        TankModule t1;
        TankModule t2;
        TankModule t3;
        int secCounter;
        IParameterDataBase m_internalParameters;
        AnalogInputParameter t2InFlow;

        public ComplexModule(string name) : base(name)
        {
            t1 = new TankModule(string.Format("{0}/T1", name), 0.5, 0.01, 4);
            t2 = new TankModule(string.Format("{0}/T2", name), 0.5, 0.005, 4);
            t3 = new TankModule(string.Format("{0}/T3", name), 0.8, 0.005, 4);
            t2InFlow = new AnalogInputParameter();
        }

        private void SetupInternalParameters(IParameter t1InletFlowParam, IParameter t3LevelParameter, IParameter t3OutFlow)
        {
            // All outlet-valves open!
            DigitalInputParameter valves = new DigitalInputParameter();
            valves.DigitalValue = true;

            AnalogOutputParameter t1Flow = new AnalogOutputParameter();
            AnalogOutputParameter t2Flow = new AnalogOutputParameter();

            m_internalParameters = ParameterDataBase.FromDictionary(new Dictionary<string, IParameter>()
            {
                { string.Format("{0}/T1/Level", Name), new AnalogOutputParameter() } ,
                { string.Format("{0}/T1/InFlow", Name), t1InletFlowParam } ,
                { string.Format("{0}/T1/OpenOutlet", Name), valves },
                { string.Format("{0}/T1/OutFlow", Name), t1Flow },
                { string.Format("{0}/T1/LevelPercent", Name), new AnalogOutputParameter() },

                { string.Format("{0}/T2/Level", Name), new AnalogOutputParameter() } ,
                { string.Format("{0}/T2/InFlow", Name), t2InFlow } ,
                { string.Format("{0}/T2/OpenOutlet", Name), valves },
                { string.Format("{0}/T2/OutFlow", Name), t2Flow},
                { string.Format("{0}/T2/LevelPercent", Name), new AnalogOutputParameter() },

                { string.Format("{0}/T3/Level", Name), t3LevelParameter } ,
                { string.Format("{0}/T3/InFlow", Name), new CalculatedParameter(CalculatedParameter.AnalogParameterAdd, t1Flow, t2Flow) } ,
                { string.Format("{0}/T3/OpenOutlet", Name), valves },
                { string.Format("{0}/T3/OutFlow", Name), t3OutFlow },
                { string.Format("{0}/T3/LevelPercent", Name), new AnalogOutputParameter() },

            });
        }

        public override void Execute(int mils, MixProperties state)
        {
            t1.Execute(mils, state);
            t2.Execute(mils, state);
            t3.Execute(mils, state);

            secCounter += mils;
            int sec = secCounter / 1000;
            if ((sec % 50) < 10)
            {
                t2InFlow.AnalogValue = 0.05;
            }
            else
            {
                t2InFlow.AnalogValue = 0.0;
            }

        }

        public override void TieParameters(IParameterDataBase parameters)
        {
            string inletFlowParam = string.Format("{0}/InFlow", this.Name); // Input (control value)
            string levelParam = string.Format("{0}/Level", this.Name); // Output (controlled variable)
            string outputFlow = string.Format("{0}/OutFlow", this.Name);

            SetupInternalParameters(parameters.GetParameter(inletFlowParam), parameters.GetParameter(levelParam), parameters.GetParameter(outputFlow));
            t1.TieParameters(m_internalParameters);
            t2.TieParameters(m_internalParameters);
            t3.TieParameters(m_internalParameters);
        }
    }
}
