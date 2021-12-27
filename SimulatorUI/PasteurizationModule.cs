using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class PasteurizationModule : TankModule
    {
        public bool HeaterOn { get; set; }
        public bool CoolerOn { get; set; }
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
