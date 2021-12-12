using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class HomogenizationModule : TankModule
    {
        public bool HomogenizationOn { get; set; }
        public bool AgeingCoolingOn { get; set; }
        public double ParticleSize { get; set; }
        public double MixTemperature { get; set; }
        public double Stage1Pressure { get; }
        public double Stage2Pressure { get; }
        public HomogenizationModule(string name, double stage1, double stage2) : base(name)
        {
            Stage1Pressure = stage1;
            Stage2Pressure = stage2;
        }
    }
}
