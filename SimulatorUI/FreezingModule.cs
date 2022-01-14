namespace SimulatorUI
{
    public class FreezingModule : TankModule
    {
        private bool freezingOn;
        private double particleSize;
        private double mixTemperature;
        private double overRun;
        private double pasteurizationUnits;
        private bool sendTestValues;
        private bool dasherOn;
        private bool startLiquidFlavoring;
        public bool FreezingOn
        {
            get { return freezingOn; }
            set { SetField(ref freezingOn, value, "FreezingOn"); }
        }
        public bool DasherOn
        {
            get { return dasherOn; }
            set { SetField(ref dasherOn, value, "DasherOn"); }
        }

        public bool StartLiquidFlavoring
        {
            get { return startLiquidFlavoring; }
            set { SetField(ref startLiquidFlavoring, value, "StartLiquidFlavoring"); }
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

        public double Overrun
        {
            get { return overRun; }
            set { SetField(ref overRun, value, "Overrun"); }
        }

        public double PasteurizationUnits
        {
            get { return pasteurizationUnits; }
            set { SetField(ref pasteurizationUnits, value, "PasteurizationUnits "); }
        }

        public double FreezerTemp { get; }
        public double BarrelRotationSpeed { get; }

        public bool SendTestValues
        {
            get { return sendTestValues; }
            set { SetField(ref sendTestValues, value, "SendTestValues"); }
        }
        public FreezingModule(string name, double freezerTemp, double barrelRotationSpeed) : base(name)
        {
            FreezerTemp = freezerTemp;
            BarrelRotationSpeed = barrelRotationSpeed;
        }
    }
}
