using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class PasteurizationModule : TankModule
    {
        private bool heateron;
        public bool HeaterOn
        {
            get { return heateron; }
            set { SetField(ref heateron, value, "HeaterOn"); }
        }
        private bool cooleron;
        public bool CoolerOn
        {
            get { return cooleron; }
            set { SetField(ref cooleron, value, "CoolerOn"); }
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
