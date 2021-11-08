using System;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal class HomogenizationModule : TankModule
    {
        //private IParameter m_mixTemperature;
        private IParameter m_homogenizationOn;
        private IParameter m_ageingCoolingOn;
        private IParameter m_particleSize;
        private readonly double m_heatTransferCoef = 1.1; // these three variables should probably be read from a calculated constants class that calculates constants based on ingredients
        private double m_stage1Pressure;
        private double m_stage2Pressure;
        double m_processTime;
        public HomogenizationModule(string name, double baseArea, double outletArea, double height, double stage1Pressure, double stage2Pressure) : base(name, baseArea, outletArea, height)
        {
            m_stage1Pressure = stage1Pressure;
            m_stage2Pressure = stage2Pressure;
        }


        //TODO: This module should be rewritten to be internally represented by two tanks with the homogenization in between... And the homogenization must be time-dependent somehow.
        public override void Execute(int mils, MixProperties state)
        {

            base.Execute(mils, state);

            if (HomogenizationOn)
            {
                Homogenize(state);
            }

            if (AgeingCoolingOn)
            {
                AgeingCooling(mils, state);
            }

            //if (CurrentLevel > 0.15)
            //{
                //Uses Newton Law of Cooling T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) m/c is simplified to h as it is constant.
            //    TankTemperature = state.AmbientTemperature + (TankTemperature - state.AmbientTemperature) * Math.Pow(Math.E, -1 * mils / 1000.0 * state.ThermalConductivity / (double)state.Density * CurrentLevel * m_baseArea); 
                
            //}
        }

        /// <summary>
        /// This method homogenizes the icecream mix, it uses a calculation for change in temperature and particle size to output it to the next module. 
        /// </summary>
        private void Homogenize(MixProperties state)
        {
            state.Viscosity = 3.72; // Generic value of ice cream mix that hasn't been aged, value is in Pa*s, which is the SI unit for viscosity,https://www.researchgate.net/publication/233330405_The_Effect_of_Ageing_at_a_Low_Temperature_on_the_Rheological_Properties_of_Kahramanmaras-Type_Ice_Cream_Mix, value for 0 aging time and 1 rps used.
            double velocity = 120; // speed in m/s of which the fluid will have after homogenization, taken from https://www.uoguelph.ca/foodscience/book-page/homogenization-mechanism
            double dPressure = m_stage1Pressure - m_stage2Pressure;
            double pressureTemperatureConstant = 4e6;

            double outTemperature = dPressure / pressureTemperatureConstant;

            state.ParticleSize = 4.5 * state.Viscosity *  velocity / (state.Density) /  Constants.G; // Taken from: https://dairyprocessinghandbook.tetrapak.com/chapter/homogenizers
            TankTemperature += outTemperature;
            //Console.WriteLine("Homogenization completed");


        }
        /// <summary>
        /// This Method cools down the mix using Netwton's cooling law (T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) h/c is simplified to h in this case as it is constant) as well as ages the mix using a simple Delay. 
        /// </summary>
        private void AgeingCooling(int mils, MixProperties state)
        {
            double coolerTemperature = 277;
            var timeInc = mils / 1000;

            m_processTime += mils / 1000.0; //process state time variable
            

            double dTemperature;
            dTemperature = TankTemperature - coolerTemperature;

            if (dTemperature > 1 && AgeingCoolingOn)
            {
                double weight = m_baseArea * CurrentLevel * (double)state.Density;
                TankTemperature = coolerTemperature + dTemperature * Math.Pow(Math.E, -1 * timeInc / m_heatTransferCoef / weight * m_baseArea); //Newton's Law of Cooling  
            }

        }

        public override void TieParameters(IParameterDataBase parameters)
        {
            base.TieParameters(parameters);

            //special parameters for this module
            //string mixTemperatureOutParam = string.Format("{0}/MixTemperature", this.Name);
            string homogenizationOnParam = string.Format("{0}/HomogenizationOn", this.Name);
            string ageingCoolingOnParam = string.Format("{0}/AgeingCoolingOn", this.Name);
            string particleSizeParam = string.Format("{0}/ParticleSize", this.Name);

            //m_mixTemperature = parameters.GetParameter(mixTemperatureOutParam);
            m_homogenizationOn = parameters.GetParameter(homogenizationOnParam);
            m_ageingCoolingOn = parameters.GetParameter(ageingCoolingOnParam);
            m_particleSize = parameters.GetParameter(particleSizeParam);

        }
        protected bool HomogenizationOn
        {
            get { return m_homogenizationOn.DigitalValue; }
            set { m_homogenizationOn.DigitalValue = value; }
        }
        protected bool AgeingCoolingOn
        {
            get { return m_ageingCoolingOn.DigitalValue; }
            set { m_ageingCoolingOn.DigitalValue = value; }
        }
        //protected double MixTemperature
        //{
        //    get { return m_mixTemperature.AnalogValue; }
        //    set { m_mixTemperature.AnalogValue = value; }
        //}
        protected double ParticleSize
        {
            get { return m_particleSize.AnalogValue; }
            set { m_particleSize.AnalogValue = value; }
        }
    }
}
