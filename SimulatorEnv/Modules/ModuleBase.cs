using System;
using System.Collections.Generic;
using System.Text;
using ABB.InSecTT.Common.Configuration;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.SimulatorEnv.Calculations;

namespace ABB.InSecTT.SimulatorEnv.Modules
{
    internal abstract class ModuleBase : IModule
    {
        public ModuleBase(string name) 
        {
            this.Name = name;
        }
        public abstract void Execute(int mils, MixProperties state);

        public abstract void TieParameters(IParameterDataBase parameters);
        

        public string Name { get; private set; }

    }

}
