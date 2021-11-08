using System;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal class TankModule : ModuleBase
    {

        public TankModule(string name, double baseArea, double outletArea, double height) : base(name)
        {
            m_baseArea = baseArea;
            m_outletArea = outletArea;
            m_height = height;
        }

        // Inputs:
        IParameter m_inletFlow; // in SI-units (m3/s)
        IParameter m_outValveOpen;
        IParameter m_dumpValveOpen;
        IParameter m_inFlowTemp;

        // State:
        IParameter m_TankTemperature;

        // Outputs:
        IParameter m_outFlowTemp;
        IParameter m_outletFlow; // in SI-units (m3/s)
        protected IParameter m_currLevel; // In SI-units (m)
        IParameter m_levelPercent; // in %

        // Constants
        protected readonly double m_baseArea = 0.5; // in SI-units (m2)
        protected readonly double m_outletArea = 0.005; // in SI-units (m2)
        readonly double c_c = 0.97; // Discharge coefficient = c_c * c_v = c_d - this is actually related to fluid viscosity and how sharp the edge of the aperture is.
        readonly double m_height; //in SI-units (m)

        public override void TieParameters(IParameterDataBase parameters)
        {
            string inletFlowParam = string.Format("{0}/InFlow", this.Name);
            string inFlowTemp = string.Format("{0}/InFlowTemp", this.Name);
            string outFlowTemp = string.Format("{0}/OutFlowTemp", this.Name);
            string outletOpenParam = string.Format("{0}/OpenOutlet", this.Name);
            string outletFlowParam = string.Format("{0}/OutFlow", this.Name);
            string levelParam = string.Format("{0}/Level", this.Name);
            string levelpercent = string.Format("{0}/LevelPercent", this.Name);
            string tankTemp = string.Format("{0}/Temperature", this.Name);
            string openDumpValve = string.Format("{0}/OpenDumpValve", this.Name);

            m_inletFlow = parameters.GetParameter(inletFlowParam);
            m_outValveOpen = parameters.GetParameter(outletOpenParam);
            m_dumpValveOpen = parameters.GetParameter(openDumpValve);

            m_outletFlow = parameters.GetParameter(outletFlowParam);
            m_currLevel = parameters.GetParameter(levelParam);
            m_levelPercent = parameters.GetParameter(levelpercent);

            m_inFlowTemp = parameters.GetParameter(inFlowTemp);
            m_outFlowTemp = parameters.GetParameter(outFlowTemp);
            m_TankTemperature = parameters.GetParameter(tankTemp);
        }

        protected double CurrentLevel
        {
            get { return m_currLevel.AnalogValue; }
            set { m_currLevel.AnalogValue = value; }
        }
        protected double Height
        {
            get { return m_height; }
        }

        protected double LevelPercent
        {
            get { return m_levelPercent.AnalogValue; }
            set { m_levelPercent.AnalogValue = value; }
        }

        protected double InletFlow
        {
            get { return m_inletFlow.AnalogValue; }
        }

        protected double OutletFlow
        {
            get { return m_outletFlow.AnalogValue; }
            set { m_outletFlow.AnalogValue = value; }
        }

        protected bool OutputValveOpen
        {
            get { return m_outValveOpen.DigitalValue; }
        }
        protected bool DumpValveOpen
        {
            get { return m_dumpValveOpen.DigitalValue; }
        }

        protected double TankTemperature 
        {
            get { return m_TankTemperature.AnalogValue; }
            set { m_TankTemperature.AnalogValue = value; }
        }

        protected double InFlowTemperature
        {
            get { return m_inFlowTemp.AnalogValue; }            
        }

        protected double OutFlowTemperature
        {
            set { m_outFlowTemp.AnalogValue = value; }
        }

        public override void Execute(int mils, MixProperties state)
        {            
            double timeIncrement = mils/ 1000.0;
            if(InletFlow>0 && InFlowTemperature == 0)
            {
                // If no in-flow temp is set, use ambient temp (since 0K is physically impossible....)
                m_inFlowTemp.AnalogValue = state.AmbientTemperature;
            }

            // Calculate state
            var oldLevel = CurrentLevel;
            CurrentLevel += timeIncrement * (InletFlow - OutletFlow) / m_baseArea;

            if (DumpValveOpen)
            {
                CurrentLevel += timeIncrement * (InletFlow - 0.05) / m_baseArea;
            }

            if (CurrentLevel < 0)
            {
                // Cannot be negative level...
                CurrentLevel = 0;
            }

            LevelPercent = CurrentLevel / m_height * 100;

            if (CurrentLevel > m_height) 
            {
                CurrentLevel = m_height; //Overflow "spills over" and is lost
                LevelPercent = 100;
            }

            // Temperature calculations:
            var surroundingTemp = state.AmbientTemperature;
            if (CurrentLevel > 0)
            {
                //First cooling - Uses Newton Law of Cooling T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) m/c is simplified to h as it is constant.
                var thermConductivity = state.ThermalConductivity;
                var density = state.Density;
                TankTemperature = surroundingTemp + (TankTemperature - surroundingTemp) * Math.Pow(Math.E, -1 * (mils / 1000.0) * (thermConductivity / density) * CurrentLevel * m_baseArea);

                // Secondly temp from mixing w. added inflow-material:
                TankTemperature = ((timeIncrement * InletFlow * InFlowTemperature) + TankTemperature * (oldLevel * m_baseArea)) / (timeIncrement * InletFlow + (oldLevel * m_baseArea));
            }
            else TankTemperature = surroundingTemp;

            OutFlowTemperature = TankTemperature;

            // Calc out-flow:
            if (OutputValveOpen && CurrentLevel > 0)
            {
                var viscosity = state == null ? 1: state.Viscosity;
                //Calculations from: https://www.engineeringtoolbox.com/flow-liquid-water-tank-d_1753.html
                OutletFlow = c_c * (double)viscosity * m_outletArea * Math.Sqrt(2 * Constants.G * CurrentLevel);

            }
            else
            {
                OutletFlow = 0;
            }

        }
    }
}
