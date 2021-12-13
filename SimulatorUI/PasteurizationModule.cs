using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class PasteurizationModule : TankModule
    {
        public bool HeaterOn { get; set; }
        public bool CoolerOn { get; set; }
        public double HeaterTemp { get; set; }
        public double CoolerTemp { get; set; }
        public double Thickness { get; set; }
        public double HeaterConductivity { get; set; }
        public double CoolerConductivity { get; set; }
        public PasteurizationModule(string name) : base(name)
        {

        }
    }
}
