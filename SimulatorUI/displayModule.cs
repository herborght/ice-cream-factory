using System;
using System.Collections.Generic;
using System.Text;

namespace SimulatorUI
{
    public class displayModule
    {
        public TankModule lockedTank;
        public String Name { get; set; }
        public String Level { get; set; }
        public String Percent { get; set; }
        public String Temperature { get; set; }
        public String InFlow { get; set; }
        public String InFlowTemp { get; set; }
        public String OutFlow { get; set; }
        public String OutFlowTemp { get; set; }
        public String DmpValve { get; set; }
        public String OutValve { get; set; }
        public String Outflowfrom { get; set; }


        public void updateVals()
        {
            this.Outflowfrom = "None";
            this.Name = this.lockedTank.Name;
            this.Level = Math.Round(lockedTank.Level, 3).ToString();
            this.Percent = Math.Round(lockedTank.LevelPercentage, 3).ToString() + "%";
            this.Temperature = Math.Round(lockedTank.Temperature, 3).ToString() + " K";
            this.InFlow = Math.Round(lockedTank.InletFlow, 3) + " m3/s";
            this.InFlowTemp = Math.Round(lockedTank.InFlowTemp, 3) + " K";
            this.OutFlow = Math.Round(lockedTank.OutLetFlow, 3) + " m3/s";
            this.OutFlowTemp = Math.Round(lockedTank.OutFlowTemp, 3) + " K";
            this.DmpValve = "";
            this.OutValve = "";
            if (this.lockedTank.DumpValveOpen)
            {
                this.DmpValve = "Open";
            }
            else
            {
                this.DmpValve = "Closed";
            }
            if (this.lockedTank.OutValveOpen)
            {
                this.OutValve = "Open";
            }
            else
            {
                this.OutValve = "Closed";
            }
            if (this.lockedTank.InFlowTanks.Count > 0)
            {
                this.Outflowfrom = "";
                for (int i = 0; i < this.lockedTank.InFlowTanks.Count - 1; i++)
                {
                    this.Outflowfrom += this.lockedTank.InFlowTanks[i].Name + ", ";
                }
                this.Outflowfrom += this.lockedTank.InFlowTanks[^1].Name;
            }
        }
        public displayModule(TankModule tank)
        {
            this.lockedTank = tank;
            this.Outflowfrom = "None";
            this.Name = tank.Name;
            this.Level = Math.Round(tank.Level, 3).ToString();
            this.Percent = Math.Round(tank.LevelPercentage, 3).ToString() + "%";
            this.Temperature = Math.Round(tank.Temperature, 3).ToString() + " K";
            this.InFlow = Math.Round(tank.InletFlow, 3) + " m3/s";
            this.InFlowTemp = Math.Round(tank.InFlowTemp, 3) + " K";
            this.OutFlow = Math.Round(tank.OutLetFlow, 3) + " m3/s";
            this.OutFlowTemp = Math.Round(tank.OutFlowTemp, 3) + " K";
            this.DmpValve = "";
            this.OutValve = "";
            if (tank.DumpValveOpen)
            {
                this.DmpValve = "Open";
            }
            else
            {
                this.DmpValve = "Closed";
            }
            if (tank.OutValveOpen)
            {
                this.OutValve = "Open";
            }
            else
            {
                this.OutValve = "Closed";
            }
            if (tank.InFlowTanks.Count > 0)
            {
                this.Outflowfrom = "";
                for (int i = 0; i < tank.InFlowTanks.Count - 1; i++)
                {
                    this.Outflowfrom += tank.InFlowTanks[i].Name + ", ";
                }
                this.Outflowfrom += tank.InFlowTanks[^1].Name;
            }

        }
    }
}
