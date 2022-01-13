using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class FlavoringHardeningPackingModule : TankModule
    {
        private double mixTemperature;
        private bool startFlavoring;
        private bool startHardening;
        private bool startPackaging;
        private bool finishBatch;
        public double MixTemperature
        {
            get { return mixTemperature; }
            set { SetField(ref mixTemperature, value, "MixTemperature"); }
        }
        public string PackageType { get; }
        public double CoolerTemperature { get; }

        public bool StartFlavoring
        {
            get { return startFlavoring; }
            set { SetField(ref startFlavoring, value, "StartFlavoring"); }
        }

        public bool StartHardening
        {
            get { return startHardening; }
            set { SetField(ref startHardening, value, "StartHardening"); }
        }

        public bool StartPackaging
        {
            get { return startPackaging; }
            set { SetField(ref startPackaging, value, "StartPackaging"); }
        }

        public bool FinishBatch
        {
            get { return finishBatch; }
            set { SetField(ref finishBatch, value, "FinishBatch"); }
        }

        public FlavoringHardeningPackingModule(string name, string packageType, double coolerTemp) : base(name)
        {
            PackageType = packageType;
            CoolerTemperature = coolerTemp;
        }
    }
}
