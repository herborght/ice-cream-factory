using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.InSecTT.Common
{
    public class Command : ICmd
    {

        public Command(string usage, string description, Action<string> action)
        {
            Usage = usage;
            CommandDescription = description;
            Execute = action;
        }


        public string Usage
        {
            get;
            private set;
        }

        public string CommandDescription
        {
            get;
            private set;
        }
        public Action<string> Execute
        {
            get;
            private set;
        }

    }
}
