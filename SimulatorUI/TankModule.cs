using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class TankModule
    {
        public string Name { get; }
        public double FillLevel { get; set; }
        public double InletFlow { get; set; }
        public bool OutValveOpen { get; set; }
        public bool DumpValveOpen { get; set; }
        public double InFlowTemp { get; set; }
        public double OutFlowTemp { get; set; }
        public double OutLetFlow { get; set; }
        public double LevelPercenatage { get; set; }
        public double Temperature { get; set; }
        public double Level { get; set; }
        //double height { get; }
        //double area { get; }
        //public List<TankModule> InFlowTank; //will probably be needed later
        //public List<TankModule> OutFlowTank;

        public TankModule(string tankName)
        {
            Name = tankName;
        }
    }
}
