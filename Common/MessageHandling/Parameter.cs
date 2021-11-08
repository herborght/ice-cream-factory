

namespace ABB.InSecTT.Common.MessageHandling
{
    public class Parameter : IParameter
    {       

        public Parameter(ParameterType type) :
            this(type, false, false)
        { }

        public Parameter(Parameter targetParameter) :
            this(targetParameter.ValueType, false, false, targetParameter)
        { }

        protected Parameter(ParameterType type, bool isInput, bool isOutput)
            : this(type, isInput, isOutput, null)
        { }

        protected Parameter(ParameterType type, bool isInput, bool isOutput, Parameter targetParameter)
        {
            IsInput = isInput;
            IsOutput = isOutput;
            m_targetParameter = targetParameter;
            ValueType = type; 
        }

        public bool IsInput
        {
            get; private set;
        }

        public bool IsOutput
        {
            get; private set;
        }

        public double AnalogValue
        {
            get { return m_analogValue; }
            set
            {
                m_analogValue = value;
                if (m_targetParameter != null)
                    m_targetParameter.AnalogValue = value;
            }
        }

        public bool DigitalValue
        {
            get { return m_digitalValue; }
            set
            {
                m_digitalValue = value;
                if (m_targetParameter != null)
                    m_targetParameter.DigitalValue = value;
            }
        }

        public ParameterType ValueType
        {
            get;
            private set;
        }

        Parameter m_targetParameter;
        double m_analogValue;
        bool m_digitalValue;

    }


    public class DigitalOutputParameter : Parameter
    {
        public DigitalOutputParameter() : this(null) { }

        public DigitalOutputParameter(Parameter targetParameter) : base(ParameterType.Digital, false, true, targetParameter) { }
    }

    public class DigitalInputParameter : Parameter
    {
        public DigitalInputParameter() : this(null) { }

        public DigitalInputParameter(Parameter targetParameter) : base(ParameterType.Digital, true, false, targetParameter) { }
    }

    public class AnalogOutputParameter : Parameter
    {
        public AnalogOutputParameter() : this(null) { }

        public AnalogOutputParameter(Parameter targetParameter) : base(ParameterType.Analog, false, true, targetParameter) { }
    }

    public class AnalogInputParameter : Parameter
    {
        public AnalogInputParameter() : this(null) { }

        public AnalogInputParameter(Parameter targetParameter) : base(ParameterType.Analog, true, false, targetParameter) { }
    }

}
