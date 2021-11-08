using System;
using System.Collections.Generic;
using System.Text;

namespace ABB.InSecTT.Common.MessageHandling
{
    public enum ParameterType
    {
        Digital,
        Analog
    }

    public interface IParameter
    {
        public bool IsInput
        {
            get;
        }

        public bool IsOutput
        {
            get;
        }

        public double AnalogValue
        {
            get;
            set;
        }

        public bool DigitalValue
        {
            get;
            set;
        }

        public ParameterType ValueType
        {
            get;
        }
    }

}
