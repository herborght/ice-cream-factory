using ABB.InSecTT.Common.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.InSecTT.Common.Configuration
{
    public interface IParameterDataBase
    {
        //void SetParameter(string key, IParameter parameter);

        IParameter GetParameter(string key);

        IEnumerable<string> ParameterKeys { get; }

        IEnumerable<string> ModuleNames { get; }

        bool ContainsParameter(string key);
    }
}
