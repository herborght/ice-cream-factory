using ABB.InSecTT.Common.MessageHandling;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
namespace ABB.InSecTT.Common.Configuration
{
    public class ParameterDataBase : IParameterDataBase
    {
        private Dictionary<string, IParameter> m_parameters;

        private ParameterDataBase() : this(new Dictionary<string, IParameter>())
        {
            
        }

        private ParameterDataBase(Dictionary<string, IParameter> paramDict)
        {
            m_parameters = paramDict;
        }

        public IParameter GetParameter(string key)
        {
            return m_parameters[key];
        }

        private void SetParameter(string key, IParameter parameter)
        {
            m_parameters[key] = parameter;
        }

        public IEnumerable<string> ModuleNames
        {
            get {
                var names = new List<string>();
                foreach(var key in ParameterKeys)
                {
                    var mod = key.Split("/")[0];
                    if(!names.Contains(mod))
                    {
                        names.Add(mod);
                    }
                }
                return names;
            }
        }

        public IEnumerable<string> ParameterKeys
        {
            get { return m_parameters.Keys; }
        }

        public bool ContainsParameter(string key)
        {
            return m_parameters.ContainsKey(key);
        }

        public static IParameterDataBase FromDictionary(Dictionary<string, IParameter> parameterDict)
        {
            return new ParameterDataBase(parameterDict);
        }

        public static IParameterDataBase FromConfiguration(string fileName)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(fileName);
            XmlNode config = xDoc.LastChild.ChildNodes[0];
            var parameters = new ParameterDataBase();
            

            foreach (XmlNode mod in config)
            {
                foreach (XmlNode param in mod.ChildNodes)
                {
                    string name = param.InnerText;
                    string type = param.LocalName;
                    IParameter paramType;

                    switch (type)
                    {
                        case "AnalogInputParameter":
                            paramType = new AnalogInputParameter();
                            break;
                        case "AnalogOutputParameter":
                            paramType = new AnalogOutputParameter();
                            break;
                        case "DigitalInputParameter":
                            paramType = new DigitalInputParameter();
                            break;
                        case "DigitalOutputParameter":
                            paramType = new DigitalOutputParameter();
                            break;
                        case "InOutChaining":
                            paramType = new CalculatedParameter(CalculatedParameter.InOutChaining, parameters.GetParameter(param.Attributes["from"].Value));
                            break;
                        case "AnalogParameterAdd":
                            paramType = new CalculatedParameter(CalculatedParameter.AnalogParameterAdd,
                                                                parameters.GetParameter(param.Attributes["from1"].Value),
                                                                parameters.GetParameter(param.Attributes["from2"].Value));
                            break;
                        default:
                            continue;
                    }

                    parameters.SetParameter(mod.Attributes["name"].Value + "/" + name, paramType);
                }
            }

            return parameters;

        }
    }
}
