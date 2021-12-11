using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
namespace SimulatorUI
{
    public class TankModule : INotifyPropertyChanged
    {
        //EDITED BY DSD under:
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyname)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyname));
        }
        protected bool SetField<T>(ref T field, T value, string propertyname)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyname);
            return true;
        }
        public string Name { get; }
        private double inletflow; // created by Bendik
        public double InletFlow
        {
            get { return inletflow; }
            set { SetField(ref inletflow, value, "InLetFlow"); }
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
            get { return inflowtemp; }
            set { SetField(ref inflowtemp, value, "InFlowTemp"); }
        }
        private double outflowtemp;
        public double OutFlowTemp
        {
            get { return outflowtemp; }
            set { SetField(ref outflowtemp, value, "OutFlowTemp"); }
        }
        private double outletflow;
        public double OutLetFlow
        {
            get { return outletflow; }
            set { SetField(ref outletflow, value, "OutLetFlow"); }
        }
        private double levelpercentage;
        public double LevelPercentage
        {
            get { return levelpercentage; }
            set { SetField(ref levelpercentage, value, "LevelPercentage"); }
        }
        public double temperature;
        public double Temperature 
        {
            get { return temperature; }
            set { SetField(ref temperature, value, "Temperature"); }
        }
        public double level;
        public double Level
        {
            get { return level; }
            set { SetField(ref level, value, "Level"); }
        }
        private double height;
        public double Height
        {
            get { return height; }
            set { SetField(ref height, value, "Height"); }
        }
        private double basearea;
        public double BaseArea
        {
            get { return basearea; }
            set { SetField(ref basearea, value, "BaseArea"); }
        }
        public double outletarea;
        public double OutletArea
        {
            get { return outletarea; }
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
