using System;
using System.Collections.Generic;
using System.ComponentModel;
namespace SimulatorUI
{
    public class TankModule : INotifyPropertyChanged
    {
        // DSD Joakim 
        // Updated by Bendik with databinding used in rawdataview
        public event PropertyChangedEventHandler PropertyChanged;
        private double inletFlow;
        private bool outValveOpen;
        private bool dumpValveOpen;
        private double inFlowTemp;
        private double outFlowTemp;
        private double outletFlow;
        private double levelPercentage;
        private double level;
        private double temperature;
        private double height;
        private double baseArea;
        private double outletArea;
        public List<TankModule> InFlowTanks;

        public TankModule(string tankName)
        {
            Name = tankName;
            InFlowTanks = new List<TankModule>();
        }

        protected virtual void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        protected bool SetField<T>(ref T field, T value, string propertyname)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyname);
            return true;
        }

        public string Name { get; }

        public double InletFlow
        {
            get
            {
                if (inletFlow < 0.001 && inletFlow > 0)
                {
                    return 0.001;
                }
                return Math.Round(inletFlow, 3);
            }
            set
            {
                if (Math.Round(value, 3).Equals(0.000))
                {
                    SetField(ref inletFlow, value, "InLetFlow");
                }
                else
                {
                    SetField(ref inletFlow, Math.Round(value, 3), "InLetFlow");
                }

            }
        }

        public bool OutValveOpen
        {
            get { return outValveOpen; }
            set { SetField(ref outValveOpen, value, "OutValveOpen"); }
        }

        public bool DumpValveOpen
        {
            get { return dumpValveOpen; }
            set { SetField(ref dumpValveOpen, value, "DumpValveOpen"); }
        }

        public double InFlowTemp
        {
            get { return Math.Round(inFlowTemp, 2); }
            set { SetField(ref inFlowTemp, value, "InFlowTemp"); }
        }

        public double OutFlowTemp
        {
            get { return Math.Round(outFlowTemp, 2); }
            set { SetField(ref outFlowTemp, value, "OutFlowTemp"); }
        }

        public double OutletFlow
        {
            get
            {
                if (outletFlow < 0.001 && outletFlow > 0)
                {
                    return 0.001;
                }
                return Math.Round(outletFlow, 3);
            }
            set
            {
                if (Math.Round(value, 3).Equals(0.000))
                {
                    SetField(ref outletFlow, Math.Round(value, 3), "OutLetFlow");
                }
                else
                {
                    SetField(ref outletFlow, Math.Round(value, 3), "OutLetFlow");
                }
            }
        }

        public double LevelPercentage
        {
            get { return Math.Round(levelPercentage, 2); }
            set { SetField(ref levelPercentage, value, "LevelPercentage"); }
        }

        public double Temperature
        {
            get { return Math.Round(temperature, 2); }
            set { SetField(ref temperature, value, "Temperature"); }
        }

        public double Level
        {
            get { return Math.Round(level, 2); }
            set { SetField(ref level, value, "Level"); }
        }

        public double Height
        {
            get { return Math.Round(height, 2); }
            set { SetField(ref height, value, "Height"); }
        }

        public double BaseArea
        {
            get { return Math.Round(baseArea, 2); }
            set { SetField(ref baseArea, value, "BaseArea"); }
        }

        public double OutletArea
        {
            get { return Math.Round(outletArea, 2); }
            set { SetField(ref outletArea, value, "OutLetArea"); }
        }
    }
}
