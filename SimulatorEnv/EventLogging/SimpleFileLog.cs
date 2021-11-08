using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using ABB.InSecTT.Common.MessageHandling;
using ABB.InSecTT.Common.Configuration;

namespace ABB.InSecTT.SimulatorEnv
{
    internal static class SimpleFileLog
    {
        private static readonly string fPath = ConfigurationManager.AppSettings.Get("LogFilePath"); //change path in app.config if needed
        private static string fileName;
        private static IParameterDataBase m_parameters;



        internal static void Initialize(IParameterDataBase parameters)
        {   
            string fName = "SimulationState.log";
            fileName = Path.Combine(fPath,fName);
            m_parameters = parameters;
            LogHeaders();
        }

        private static void LogHeaders()
        {
            StringBuilder sb = new StringBuilder();
            foreach(var key in m_parameters.ParameterKeys)
            {
                sb.Append(key);
                sb.Append(";");
            }
            sb.AppendLine();
            File.AppendAllText(fileName, sb.ToString());
        }

        public static void LogState()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var key in m_parameters.ParameterKeys)
            {
                sb.Append(m_parameters.GetParameter(key).AnalogValue);
                sb.Append(";");
            }
            sb.AppendLine();
            File.AppendAllText(fileName, sb.ToString());
        }
        

    }
}
