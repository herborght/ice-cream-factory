using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class TankModule
    {
        public string Name { get; }
        public double InletFlow { get; set; }
        public bool OutValveOpen { get; set; }
        public bool DumpValveOpen { get; set; }
        public double InFlowTemp { get; set; }
        public double OutFlowTemp { get; set; }
        public double OutLetFlow { get; set; }
        public double LevelPercentage { get; set; }
        public double Temperature { get; set; }
        public double Level { get; set; }
        public double Height { get; set; }
        public double BaseArea { get; set; }
        public double OutletArea { get; set; }
        //public List<TankModule> InFlowTank; //will probably be needed later
        //public List<TankModule> OutFlowTank;

        public TankModule(string tankName)
        {
            Name = tankName;
        }
    }
}
