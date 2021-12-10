using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.SimulatorEnv.Calculations;
using ABB.InSecTT.Common.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    // DSD Emil - Special module used for simulation environment parameters such as ambient temp
    internal class SimEnv : ModuleBase
    {
        public SimEnv(string name) : base(name)
        {
        }

        IParameter m_ambientTemperature;

        public override void TieParameters(IParameterDataBase parameters)
        {
            string ambientTemp = string.Format("{0}/Temperature", this.Name);

            m_ambientTemperature = parameters.GetParameter(ambientTemp);
        }

        protected double AmbientTemperature
        {
            get { return m_ambientTemperature.AnalogValue; }
            set { m_ambientTemperature.AnalogValue = value; }
        }

        public override void Execute(int mils, MixProperties state)
        {
            AmbientTemperature = state.AmbientTemperature;
        }
    }
}
