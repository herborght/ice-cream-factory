using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using M2Mqtt;
using M2Mqtt.Messages;
using System.Globalization;
using System.Threading.Tasks;
using System.Threading;
using ABB.InSecTT.Common.Configuration;

namespace ABB.InSecTT.Common.MessageHandling
{
    public class MessageHandling
    {        
        readonly string SendQueue = ConfigurationManager.AppSettings.Get("SendQueue");
        readonly string ReceiveQueue = ConfigurationManager.AppSettings.Get("ReceiveQueue");
        readonly int MsgCycleDelay;
        readonly string TopicFormat = "{0}/{1}";

        private string m_host;
        private MqttClient m_client;
        private IParameterDataBase m_parameters;        

        public MessageHandling(IParameterDataBase parameters, int msgCycleDelay, string brokerAddress)
        {
            m_host = brokerAddress;
            m_client = new MqttClient(m_host);
            string clientId = Guid.NewGuid().ToString();
            m_client.Connect(clientId);
            m_parameters = parameters;
            this.MsgCycleDelay = msgCycleDelay;
            SetupSubscriptions();
        }

        public void Disconnect()
        {
            m_client.Disconnect();
        }

        public void SetupSubscriptions()
        {
            m_client.MqttMsgPublishReceived += MqttMsgPublishReceived;
            List<string> topics = new List<string>();
            List<byte> qosArr = new List<byte>();

            foreach (var parameterKey in m_parameters.ParameterKeys)
            {
                if (m_parameters.GetParameter(parameterKey).IsInput)
                {
                    string topic = string.Format(TopicFormat, ReceiveQueue, parameterKey);
                    topics.Add(topic);
                    qosArr.Add(MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE);
                }
            }
 
            m_client.Subscribe(topics.ToArray(), qosArr.ToArray());
        }
        private void MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            // handle message received             
            var body = e.Message;
            var message = Encoding.UTF8.GetString(body);
            
            //Console.WriteLine(" {1} Received {0}", message, e.Topic);
            
            string topicDomain = string.Format(TopicFormat, ReceiveQueue, string.Empty);
            string parameterKey = e.Topic.Replace(topicDomain, string.Empty);

            ParseMessage(parameterKey, message);
        }

        private void ParseMessage(string parameterKey, string message) 
        {

            if (m_parameters.ContainsParameter(parameterKey))
            {
                var parameter = m_parameters.GetParameter(parameterKey);

                if (double.TryParse(message, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out double result))
                {
                    parameter.AnalogValue = result;
                }
                else if (bool.TryParse(message, out bool boolResult))
                {
                    parameter.DigitalValue = boolResult;
                }
                else
                {
                    Console.WriteLine("Unable to parse message {0}", message);
                }

            }
        }

        public async Task<long> SendMessages(CancellationToken token)
        {
            for (; ; )
            {
                if (token.IsCancellationRequested)
                {
                    //token.ThrowIfCancellationRequested();
                    break;
                }
                foreach (var parameterKey in m_parameters.ParameterKeys)
                {
                    var parameter = m_parameters.GetParameter(parameterKey);
                    if (parameter.IsOutput)
                    {
                        string message;

                        if (parameter.ValueType == ParameterType.Digital)
                        {
                            message = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:F3}", parameter.DigitalValue);
                        }
                        else
                        {
                            message = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:F3}", parameter.AnalogValue);
                        }
                        

                        string sendTopic = string.Format(TopicFormat, SendQueue, parameterKey);
                        m_client.Publish(sendTopic, Encoding.UTF8.GetBytes(message), MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE, true);
                        
                    }
                }

                await Task.Delay(MsgCycleDelay);
            }

            Disconnect();
            return 0;
        }
    }

}
