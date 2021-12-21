using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    class FlavoringHardeningPackingModule : TankModule
    {
        public double MixTemperature { get; set; }
        public string PackageType { get; }
        public double CoolerTemperature { get; }
        public bool StartFlavoring { get; set; }
        public bool StartHardening { get; set; }
        public bool StartPackaging { get; set; }
        public bool FinishBatch { get; set; }

        public FlavoringHardeningPackingModule(string name, string packageType, double coolerTemp) : base(name)
        {
            PackageType = packageType;
            CoolerTemperature = coolerTemp;
        }
    }
}
