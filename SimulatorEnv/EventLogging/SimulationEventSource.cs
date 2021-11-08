using System;
using System.Diagnostics.Tracing;
using System.Configuration;

namespace ABB.InSecTT.SimulatorEnv
{

    [EventSource(Name = "Simulation")]
    class SimulationEventSource : EventSource
    {
        public class Keywords
        {
            public const EventKeywords SimulationState = (EventKeywords)64;
            public const EventKeywords Messaging = (EventKeywords)128;
            public const EventKeywords Diagnostic = (EventKeywords)256;
            public const EventKeywords Perf = (EventKeywords)512;
        }
        public class Tasks
        {
            public const EventTask Execution = (EventTask)1;
            public const EventTask MQTTConnect = (EventTask)2;
        }
        [Event(1, Message = "Starting up", Level = EventLevel.Informational, Keywords = Keywords.Perf)]
        public void Startup() { WriteEvent(1); }
        [Event(2, Message = "Simulation execution starting", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void ExecutionStart() { WriteEvent(2); }
        [Event(3, Message = "Simulation execution stopping", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void ExecutionStop() { WriteEvent(3); }


        [Event(4, Message = "Connected to MQTT broker at {0}", Level = EventLevel.Informational, Keywords = Keywords.Messaging)]
        public void ConnectMQTT(string hostname) { WriteEvent(4, hostname); }
        [Event(5, Message = "Application Failure: {0}", Level = EventLevel.Verbose, Keywords = Keywords.Diagnostic)]
        public void Failure(string error) { WriteEvent(5, error); }
        [Event(6, Message = "{0}:{1}", Level = EventLevel.Informational, Keywords = Keywords.SimulationState)]
        
        public void SimulationState(string key, string value) { WriteEvent(6, key, value); }
        [Event(7, Message = "{0} has been executed", Level = EventLevel.Informational, Keywords = Keywords.Diagnostic)]
        public void Command(string command) { WriteEvent(7, command); }


        public static SimulationEventSource Log = new SimulationEventSource();


    }

}
