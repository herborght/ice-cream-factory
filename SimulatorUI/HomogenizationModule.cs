using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class HomogenizationModule : TankModule
    {
        private bool homogeniztionon;

        public bool HomogenizationOn
        {
            get { return homogeniztionon; }
            set { SetField(ref homogeniztionon, value, "HomogenizationOn"); } 
        }
        private bool ageingcoolingon;
        public bool AgeingCoolingOn
        {
            get { return ageingcoolingon; }
            set { SetField(ref ageingcoolingon, value, "AgeingCoolingOn"); }
        }
        private double particlesize;
        public double ParticleSize
        {
            get { return particlesize; }
            set { SetField(ref particlesize, value, "ParticleSize"); }
        }
        private double mixtemperature;
        public double MixTemperature
        {
            get { return mixtemperature; }
            set { SetField(ref mixtemperature, value, "MixTemperature"); }
        }
        public double Stage1Pressure { get; }
        public double Stage2Pressure { get; }
        public HomogenizationModule(string name, double stage1pressure, double stage2pressure) : base(name)
        {
            Stage1Pressure = stage1pressure;
            Stage2Pressure = stage2pressure;
        }
    }
}
