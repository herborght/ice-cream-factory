using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class FreezingModule : TankModule
    {
        public bool FreezingOn { get; set; }
        public bool DasherOn { get; set; }
        public bool StartLiquidFlavoring { get; set; }
        public double ParticleSize { get; set; }
        public double MixTemperature { get; set; }
        public double Overrun { get; set; }
        public double PasteurizationUnits { get; set; }
        public double FreezerTemp { get; }
        public double BarrelRotationSpeed { get; }
        public bool SendTestValues { get; set; }
        public FreezingModule(string name, double freezerTemp, double barrelRotationSpeed) : base(name)
        {
            FreezerTemp = freezerTemp;
            BarrelRotationSpeed = barrelRotationSpeed;
        }
    }
}
