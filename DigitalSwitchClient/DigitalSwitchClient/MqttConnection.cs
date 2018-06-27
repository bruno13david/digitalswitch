using System;
using System.Text;
using System.Threading;

namespace DigitalSwitchClient
{
    public class MqttConnection
    {
        private OpenNETCF.MQTT.MQTTClient mqttClient;
        private const string SERVER_ADDRESS = "m20.cloudmqtt.com";
        private const int SERVER_PORT = 10146;
        private const string USER = "";
        private const string PASSWORD = "";
        private string clientId;

        public MqttConnection()
        {
            clientId = "clientTest";
        }

        public bool Connect()
        {
            try
            {
                mqttClient = new OpenNETCF.MQTT.MQTTClient(SERVER_ADDRESS, SERVER_PORT);
                mqttClient.MessageReceived += (topic, qos, payload) =>
                {
                };

                mqttClient.Connect(clientId, USER, PASSWORD);

                var i = 0;
                while (!mqttClient.IsConnected)
                {
                    Thread.Sleep(1000);
                    if (i++ > 10)
                        break;
                }

                if (mqttClient.IsConnected)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public bool Publish(ItemUI itemUI)
        {
            try
            {
                if (mqttClient.IsConnected)
                {
                    mqttClient.Publish(itemUI.Topic, Encoding.UTF8.GetBytes(itemUI.GetCommand()), OpenNETCF.MQTT.QoS.FireAndForget, false);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}