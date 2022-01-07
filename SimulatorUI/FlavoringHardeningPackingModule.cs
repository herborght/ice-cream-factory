using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class FlavoringHardeningPackingModule : TankModule
    {
        private double mixtemperature;
        public double MixTemperature
        {
            get { return mixtemperature; }
            set { SetField(ref mixtemperature, value, "MixTemperature"); }
        }
        public string PackageType { get; }
        public double CoolerTemperature { get; }
        private bool startflavoring;
        public bool StartFlavoring
        {
            get { return startflavoring; }
            set { SetField(ref startflavoring, value, "StartFlavoring"); }
        }
        private bool starthardening;
        public bool StartHardening
        {
            get { return starthardening; }
            set { SetField(ref starthardening, value, "StartHardening"); }
        }
        private bool startpackaging;
        public bool StartPackaging
        {
            get { return startpackaging; }
            set { SetField(ref startpackaging, value, "StartPackaging"); }
        }
        private bool finishbatch;
        public bool FinishBatch
        {
            get { return finishbatch; }
            set { SetField(ref finishbatch, value, "FinishBatch"); }
        }

        public FlavoringHardeningPackingModule(string name, string packageType, double coolerTemp) : base(name)
        {
            PackageType = packageType;
            CoolerTemperature = coolerTemp;
        }
    }
}
