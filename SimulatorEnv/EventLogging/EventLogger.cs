using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics.Tracing;
using System.Linq;

namespace ABB.InSecTT.SimulatorEnv
{
    internal static class EventLogger
    {
        private static readonly string fPath = ConfigurationManager.AppSettings.Get("LogFilePath"); //change path in app.config if needed
        private static string fileName;
        private static string fName = $"SimulationEvents-{DateTime.Now:yyyy-MM-ddTHHmmss}.log"; //gets file name with ISO 8601 timestamp

        internal static void EventLogging(EventWrittenEventArgs eventArgs)
        {
            StringBuilder sb = new StringBuilder();
            fileName = Path.Combine(fPath, fName);
            sb.Append($"[{eventArgs.TimeStamp}] ");
            if (eventArgs.Payload.Count > 0)
            {
                var message = eventArgs.Message.ToString();
                var formattedMessage = string.Format(message, eventArgs.Payload.ToArray());
                sb.Append(formattedMessage);
            }        
            else if (eventArgs.Payload.Count == 0)
            {
                sb.Append(eventArgs.Message);
            }
            sb.AppendLine();
            File.AppendAllText(fileName, sb.ToString());
        }
    }

}

