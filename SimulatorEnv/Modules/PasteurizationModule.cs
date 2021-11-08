using ABB.InSecTT.Common.MessageHandling;
using System;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal class PasteurizationModule : TankModule
    {
        public PasteurizationModule(string name, double baseArea, double outletArea, double height, double heaterTemp, double coolerTemp, double thickness, double HTC, double CTC) : base(name, baseArea, outletArea, height)
        {
            HeaterTemp = heaterTemp;
            CoolerTemp = coolerTemp;
            ThicknessOfMaterial = thickness;
            HeaterThermalConductivity = HTC;
            CoolerThermalConductivity = CTC;
        }

        // Inputs:
        IParameter m_heaterOn;
        IParameter m_coolerOn;

        // Outputs:
        //IParameter m_temperature; // In SI-units (K)

        readonly double HeaterTemp = 373.15; //in SI-units (K)
        readonly double CoolerTemp = 273.15; //in SI-units (K)
        readonly double AreaOfContact = 0.01; //in SI-units (m2)
        readonly double HeaterThermalConductivity = 80; //derived from SI-units (W/m K)
        readonly double CoolerThermalConductivity = 80; //derived from SI-units (W/m K)
        readonly double ThicknessOfMaterial = 0.05; //in SI-units (m)

        public override void Execute(int mils, MixProperties state)
        {
            base.Execute(mils, state);
            double timeIncrement = mils / 1000.0;
            double weight = m_baseArea * CurrentLevel * state.Density;

            if (HeaterOn)
            {
                //Temperature change from convection, math from https://www.toppr.com/guides/physics-formulas/heat-transfer-formula/?__cf_chl_captcha_tk__=1da412761b3cd65e5db6aa18211806cf278c5799-1624524025-0-AQBYGg8BsRIJtKjEjam_i_CLYwKa4hcVxu9EQ6cju8HS5KS12gePytvFdBGehOyCaP73o0fDTRPNIhXd3PE-bK3Dxjm0RFEVV7-veWzpsEJR4g3Hud_DipLh-8RD5avbupjL_0mzvec9KETre_TSxuX5dOG0YJ6Rxe83wWOFiPoU-Y3vBwE_2RkflHbJBpFS64oUwSis1bBPoRgEl44tzibKGmWiZwi1lO9__OZfSQ4-7zucFJqNkSlJVtShy_euNXEiqMundWBy5RfoOy2nY_UXQ-7nTpF_JBHiVxz5eRbLdkzJA79uLEOTJJeqUJPGqCCBaMIXBCxcSko0NL8OLicNPvRN19lpRlNfTMN8kRUfSa55OTRPkPy0bkBMSxT9q0QfOiadersnQ3wbmnZbRWDkHn-WmcGnZBrPMghRpJ_R9tmDnX4pxkH-N7xh37H16KyY198tYLsrYCwXnHyGJ7SO1_qV9eNqgwBhyA7ov-ykjZ69c9w1rRG3EObArWxUdHHrEUfIMbM1DTBkLmz-WtVOqIfMWjzeGIAeMmtGN75uLIrFYamf6rwsZ0D38GvhJ5LIF_UIKNbZy7MRn8vjqUmwQpistAQ0pd2twFA9mn_JgImZlSoYYIiMIxiUdctC-l4RMRzvMR8gPX6Rgw3K5Sw_zzJSfctAY0uJI_-BqQxJ50196Rn3n51QV_B7WQ-3KSYat9zxJ7xFaQDGvePoFTFePT5a5v_FlZ1nbdK4Zq9o
                TankTemperature += HeaterThermalConductivity * AreaOfContact * (HeaterTemp - TankTemperature) * timeIncrement / ThicknessOfMaterial / weight * state.ThermalConductivity; 
            }
            if (CoolerOn)
            {
                TankTemperature += CoolerThermalConductivity * AreaOfContact * (CoolerTemp - TankTemperature) * timeIncrement / ThicknessOfMaterial / weight * state.ThermalConductivity;
            }

            if (TankTemperature > 323)
            {
                state.PasteurUnits += timeIncrement/60 *Math.Pow(1.216, (TankTemperature - 273.15) - 60);
            }

            //if (CurrentLevel > 0.15)
            //{
            //    TankTemperature = state.AmbientTemperature + (TankTemperature - state.AmbientTemperature) * Math.Pow(Math.E, -1 * timeIncrement * state.ThermalConductivity / weight); //Uses Newton Law of Cooling T(t) = Tenv + (T(0) -Tenv)e^(-tAh/mc) m/c is simplified to h as it is constant.
                //Temperature = TankTemperature;
            //}
        }

        public override void TieParameters(IParameterDataBase parameters) 
        {
            base.TieParameters(parameters);
            string temperature = string.Format("{0}/Temperature", this.Name);
            string heaterOn = string.Format("{0}/HeaterOn", this.Name);
            string coolerOn = string.Format("{0}/CoolerOn", this.Name);

            //m_temperature = parameters.GetParameter(temperature);
            m_heaterOn = parameters.GetParameter(heaterOn);
            m_coolerOn = parameters.GetParameter(coolerOn);
        }
        //protected double Temperature
        //{
        //    get { return m_temperature.AnalogValue; }
        //    set { m_temperature.AnalogValue = value; }
        //}

        protected bool HeaterOn
        {
            get { return m_heaterOn.DigitalValue; }
            set { m_heaterOn.DigitalValue = value; }
        }
        protected bool CoolerOn
        {
            get { return m_coolerOn.DigitalValue; }
            set { m_coolerOn.DigitalValue = value; }
        }
    }
}
