namespace SimulatorUI
{
    public class HomogenizationModule : TankModule
    {
        private bool homogenizationOn;
        private bool ageingCoolingOn;
        private double particleSize;
        private double mixTemperature;

        public bool HomogenizationOn
        {
            get { return homogenizationOn; }
            set { SetField(ref homogenizationOn, value, "HomogenizationOn"); }
        }

        public bool AgeingCoolingOn
        {
            get { return ageingCoolingOn; }
            set { SetField(ref ageingCoolingOn, value, "AgeingCoolingOn"); }
        }

        public double ParticleSize
        {
            get { return particleSize; }
            set { SetField(ref particleSize, value, "ParticleSize"); }
        }

        public double MixTemperature
        {
            get { return mixTemperature; }
            set { SetField(ref mixTemperature, value, "MixTemperature"); }
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
