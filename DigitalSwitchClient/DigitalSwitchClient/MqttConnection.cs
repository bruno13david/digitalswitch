using System;
using System.Text;
using System.Threading;

namespace DigitalSwitchClient
{
    public class MqttConnection
    {
        private OpenNETCF.MQTT.MQTTClient mqttClient;
        private const string SERVER_ADDRESS = "cpbsb2.o2br.net";
        private const int SERVER_PORT = 1883;
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

                mqttClient.Connect(clientId);

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