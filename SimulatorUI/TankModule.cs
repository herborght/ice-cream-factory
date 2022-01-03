using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace SimulatorUI
{
    public class TankModule : INotifyPropertyChanged
    {
        // DSD Joakim 
        // Updated by Bendik with databinding used in rawdataview
        public event PropertyChangedEventHandler PropertyChanged;
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
        private double inletflow;
        public double InletFlow
        {
            get
            {
                if (inletflow < 0.001 && inletflow > 0)
                {
                    return 0.001;
                }
                return Math.Round(inletflow, 3);
            }
            set
            {
                if (Math.Round(value, 3).Equals(0.000))
                {
                    SetField(ref inletflow, value, "InLetFlow");
                }
                else
                {
                    SetField(ref inletflow, Math.Round(value, 3), "InLetFlow");
                }

            }
        }
        private bool outvalveopen;
        public bool OutValveOpen
        {
            get { return outvalveopen; }
            set { SetField(ref outvalveopen, value, "OutValveOpen"); }
        }
        private bool dumpvalveopen;
        public bool DumpValveOpen
        {
            get { return dumpvalveopen; }
            set { SetField(ref dumpvalveopen, value, "DumpValveOpen"); }
        }
        private double inflowtemp;
        public double InFlowTemp
        {
            get { return Math.Round(inflowtemp, 2); }
            set { SetField(ref inflowtemp, value, "InFlowTemp"); }
        }
        private double outflowtemp;
        public double OutFlowTemp
        {
            get { return Math.Round(outflowtemp, 2); }
            set { SetField(ref outflowtemp, value, "OutFlowTemp"); }
        }
        private double outletflow;
        public double OutLetFlow
        {
            get
            {
                if (outletflow < 0.001 && outletflow > 0)
                {
                    return 0.001;
                }
                return Math.Round(outletflow, 3);
            }
            set
            {
                if (Math.Round(value, 3).Equals(0.000))
                {
                    SetField(ref outletflow, Math.Round(value, 3), "OutLetFlow");
                }
                else
                {
                    SetField(ref outletflow, Math.Round(value, 3), "OutLetFlow");
                }
            }
        }
        private double levelpercentage;
        public double LevelPercentage
        {
            get { return Math.Round(levelpercentage, 2); }
            set { SetField(ref levelpercentage, value, "LevelPercentage"); }
        }
        public double temperature;
        public double Temperature
        {
            get { return Math.Round(temperature, 2); }
            set { SetField(ref temperature, value, "Temperature"); }
        }
        public double level;
        public double Level
        {
            get { return Math.Round(level, 2); }
            set { SetField(ref level, value, "Level"); }
        }
        private double height;
        public double Height
        {
            get { return Math.Round(height, 2); }
            set { SetField(ref height, value, "Height"); }
        }
        private double basearea;
        public double BaseArea
        {
            get { return Math.Round(basearea, 2); }
            set { SetField(ref basearea, value, "BaseArea"); }
        }
        public double outletarea;
        public double OutletArea
        {
            get { return Math.Round(outletarea, 2); }
            set { SetField(ref outletarea, value, "OutLetArea"); }
        }

        public List<TankModule> InFlowTanks;

        public TankModule(string tankName)
        {
            Name = tankName;
            InFlowTanks = new List<TankModule>();
        }
    }
}
