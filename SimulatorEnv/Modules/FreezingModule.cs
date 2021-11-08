using ABB.InSecTT.Common.MessageHandling;
using System;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal class FreezingModule : TankModule
    {

        public FreezingModule(string name, double baseArea, double outletArea, double height, double freezerTemp, double barrelRotationSpeed) : base(name, baseArea, outletArea, height)
        {
            m_FreezerTemp = freezerTemp;
            m_barrelRotationSpeed = barrelRotationSpeed;

        }

        // Inputs:
        private IParameter m_freezerOn;
        private IParameter m_dasherOn;
        
        private IParameter m_overrun;
        private IParameter m_startLiquidFlavoring;
        private IParameter m_pasteurizationUnits;
        private IParameter m_particleSize;
        private IParameter m_sendTestValues;
        private readonly double m_FreezerTemp = 253.15;
        private double m_barrelRotationSpeed;
        private double m_freezingTime;
        private double m_mass;
        private double m_volOfIceCream;
        private double m_volofMixUsed;
        private double m_originalDensity;
        private readonly double AreaOfContact = 0.01; //in SI-units (m2)
        private readonly double CoolerThermalConductivity = 80; //derived from SI-units (W/m K)
        private readonly double ThicknessOfMaterial = 0.05; //in SI-units (m)

        public override void Execute(int mils, MixProperties state)
        {

            base.Execute(mils, state);
            
            if (StartLiquidFlavoring)
            {
                LiquidFlavoring(state);
            }

            if (FreezerOn || DasherOn)
            {
                Freeze(mils, state);
            }

            /*
            if (CurrentLevel > 0.15)
            {
                //Uses Newton Law of Cooling T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) m/c is simplified to h as it is constant.
                TankTemperature = state.AmbientTemperature + (TankTemperature - state.AmbientTemperature) * Math.Pow(Math.E, -1 * mils / 1000.0 * state.ThermalConductivity / (double)state.Density * CurrentLevel * m_baseArea); 
                MixTemperature = TankTemperature;
            }
            */

            if (SendTestValues)
            {
                ParticleSize = (double)state.ParticleSize;
                PasteurizationUnits = (double)state.PasteurUnits;
            }
        }
        /// <summary>
        /// Adds Liquid flavoring to the mix and increases the level accordingly
        /// </summary>
        private void LiquidFlavoring(MixProperties state)
        {
            CurrentLevel *= 1 + ((double)state.LiquidFlavoring / 100);
            StartLiquidFlavoring = false;
        }

        /// <summary>
        /// this is the freezing method, it takes in the mixture at a certain temp and calculates the volume of icecream, its overrun, difference in temperature and the new density.
        /// </summary>
        private void Freeze(int mils, MixProperties state)
        {

            double timeIncrement = mils / 1000.0;
            if (FreezerOn)
            {
                m_mass = m_baseArea * CurrentLevel * (double)state.Density;
                TankTemperature += timeIncrement * CoolerThermalConductivity * AreaOfContact * (m_FreezerTemp - TankTemperature) * m_FreezerTemp * ThicknessOfMaterial / m_mass * state.ThermalConductivity;
            }

            if (DasherOn && TankTemperature < 268.15)
            {
                if (m_freezingTime == 0)
                {
                    m_volofMixUsed = m_mass / (double)state.Density;
                    m_originalDensity = state.Density;
                }

                m_freezingTime += mils / 1000.0; //// Time taken to freeze the mixture

                m_volOfIceCream = m_volofMixUsed * (1 + m_freezingTime * m_barrelRotationSpeed / 20000); // the divisions come from unit conversions
                Overrun = (m_volOfIceCream - m_volofMixUsed )/ m_volofMixUsed * 100; 
                CurrentLevel += m_volofMixUsed * (1 + (Overrun / 100)) / m_baseArea / 1000;
                state.Density = m_originalDensity * (1 - Overrun / 100);
            }


            
        }

        public override void TieParameters(IParameterDataBase parameters)
        {
            base.TieParameters(parameters);

            //special parameters for this module
            string startLiquidFlavoringParam = string.Format("{0}/StartLiquidFlavoring", this.Name);
            string freezerOnParam = string.Format("{0}/FreezingOn", this.Name);
            string dasherOnParam = string.Format("{0}/DasherOn", this.Name);            
            string overrunParam = string.Format("{0}/Overrun", this.Name);
            string sendTestValuesParam = string.Format("{0}/SendTestValues", this.Name);
            string particleSizeParam = string.Format("{0}/ParticleSize", this.Name);
            string pasteurizationUnitsParam = string.Format("{0}/PasteurizationUnits", this.Name);


            m_startLiquidFlavoring = parameters.GetParameter(startLiquidFlavoringParam);
            m_freezerOn = parameters.GetParameter(freezerOnParam);
            m_dasherOn = parameters.GetParameter(dasherOnParam);            
            m_overrun = parameters.GetParameter(overrunParam);
            m_pasteurizationUnits = parameters.GetParameter(pasteurizationUnitsParam);
            m_particleSize = parameters.GetParameter(particleSizeParam);
            m_sendTestValues = parameters.GetParameter(sendTestValuesParam);

        }

        private bool StartLiquidFlavoring
        {
            get { return m_startLiquidFlavoring.DigitalValue; }
            set { m_startLiquidFlavoring.DigitalValue = value; }
        }

        private bool FreezerOn
        {
            get { return m_freezerOn.DigitalValue; }
            set { m_freezerOn.DigitalValue = value; }

        }

        private bool DasherOn
        {
            get { return m_dasherOn.DigitalValue; }
            set { m_dasherOn.DigitalValue = value; }

        }
       

        private double Overrun
        {
            get { return m_overrun.AnalogValue; }
            set { m_overrun.AnalogValue = value; }
        }
        private double PasteurizationUnits
        {
            get { return m_pasteurizationUnits.AnalogValue; }
            set { m_pasteurizationUnits.AnalogValue = value; }

        }

        private double ParticleSize
        {
            get { return m_particleSize.AnalogValue; }
            set { m_particleSize.AnalogValue = value; }
        }

        private bool SendTestValues
        {
            get { return m_sendTestValues.DigitalValue; }
            set { m_sendTestValues.DigitalValue = value; }
        }

    }
}

