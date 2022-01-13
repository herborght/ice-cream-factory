using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class PasteurizationModule : TankModule
    {
        private bool heaterOn;
        private bool coolerOn;
        public bool HeaterOn
        {
            get { return heaterOn; }
            set { SetField(ref heaterOn, value, "HeaterOn"); }
        }

        public bool CoolerOn
        {
            get { return coolerOn; }
            set { SetField(ref coolerOn, value, "CoolerOn"); }
        }
        public double HeaterTemp { get; }
        public double CoolerTemp { get; }
        public double Thickness { get; }
        public double HeaterConductivity { get; }
        public double CoolerConductivity { get; }
        public PasteurizationModule(string name, double HTemp, double CTemp, double thickness, double HConductivity, double CConductivity) : base(name)
        {
            HeaterTemp = HTemp;
            CoolerTemp = CTemp;
            Thickness = thickness;
            HeaterConductivity = HConductivity;
            CoolerConductivity = CConductivity;
        }
    }
}
