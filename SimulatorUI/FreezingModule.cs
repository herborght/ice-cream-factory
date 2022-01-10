using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class FreezingModule : TankModule
    {
        private bool freezingon;
        public bool FreezingOn 
        {
            get { return freezingon; }
            set { SetField(ref freezingon, value, "FreezingOn"); } 
        }
        private bool dasheron;
        public bool DasherOn
        {
            get { return dasheron; }
            set { SetField(ref dasheron, value, "DasherOn"); }
        }
        private bool startliquidflavoring;
        public bool StartLiquidFlavoring
        {
            get { return startliquidflavoring; }
            set { SetField(ref startliquidflavoring, value, "StartLiquidFlavoring"); }
        }
        public double particlesize;
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
        private double overrun;
        public double Overrun
        {
            get { return overrun; }
            set { SetField(ref overrun, value, "Overrun"); }
        }
        private double pasteurizationunits;
        public double PasteurizationUnits
        {
            get { return pasteurizationunits; }
            set { SetField(ref pasteurizationunits, value, "PasteurizationUnits "); }
        }
        
        public double FreezerTemp { get; }
        public double BarrelRotationSpeed { get; }
        private bool sendtestvalues;
        public bool SendTestValues
        {
            get { return sendtestvalues; }
            set { SetField(ref sendtestvalues, value, "SendTestValues"); }
        }
        public FreezingModule(string name, double freezerTemp, double barrelRotationSpeed) : base(name)
        {
            FreezerTemp = freezerTemp;
            BarrelRotationSpeed = barrelRotationSpeed;
        }
    }
}
