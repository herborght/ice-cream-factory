using System;

namespace ABB.InSecTT.Common
{
    public interface ICmd
    {
        public string Usage { get; }
        public string CommandDescription { get; }
        public Action<string> Execute { get; }

    }


}