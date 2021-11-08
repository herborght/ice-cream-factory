using System;
using System.IO;
using System.Diagnostics.Tracing;

namespace ABB.InSecTT.SimulatorEnv
{
    
    public class SimulationListener : EventListener
    {
        private string m_eventSourceName;
        public SimulationListener(string eventSourceName)
        {
            m_eventSourceName = eventSourceName;
        }
        protected override void OnEventSourceCreated(EventSource eventSource)
        {
            
            if (eventSource.Name == m_eventSourceName)
            {
                EnableEvents(eventSource, EventLevel.LogAlways);
            }
        }

        protected override void OnEventWritten(EventWrittenEventArgs eventData)
        {
            EventLogger.EventLogging(eventData);
        }
    }
}
