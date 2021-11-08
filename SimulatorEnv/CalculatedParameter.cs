using System;
using System.Collections.Generic;
using System.Text;
using ABB.InSecTT.Common.MessageHandling;

namespace ABB.InSecTT.Common.MessageHandling
{
    internal class CalculatedParameter: IParameter        
    {
        IParameter[] m_parameters;
        Func<IParameter[], IParameter> m_calculation;

        public CalculatedParameter(Func<IParameter[], IParameter> calculation, params IParameter[] parameters)
        {
            if (parameters.Length == 0)
                throw new ArgumentException("Empty parameters-array in calculated parameters");

            m_calculation = calculation;
            m_parameters = parameters;
        }

        private IParameter CalculatedParam { get { return m_calculation(m_parameters); } }

        public bool IsInput => CalculatedParam.IsInput;

        public bool IsOutput => CalculatedParam.IsOutput;

        public double AnalogValue { get => CalculatedParam.AnalogValue; set { } }
        public bool DigitalValue { get => CalculatedParam.DigitalValue; set { } }

        public ParameterType ValueType => CalculatedParam.ValueType;

        public static IParameter AnalogParameterAdd(IParameter[] parameters)
        {
            AnalogOutputParameter parameter = new AnalogOutputParameter();
            foreach (var par in parameters)
                parameter.AnalogValue += par.AnalogValue;

            return parameter;
        }

        public static IParameter InOutChaining(IParameter[] parameters)
        {
            return parameters[0];
        }


    }
}