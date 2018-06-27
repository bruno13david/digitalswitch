using System;
using System.Text;
using System.Threading;

namespace DigitalSwitchClient
{
    public enum EnumLed
    {
        Led1 = 1,
        Led2 = 2,
        Led3 = 3,
        Led4 = 4
    }
    public class MqttConnection
    {
        private static OpenNETCF.MQTT.MQTTClient mqttClient;
        private const string SERVER_ADDRESS = "m20.cloudmqtt.com";
        private const int SERVER_PORT = 10146;
        private const string USER = "rjkpvoiu";
        private const string PASSWORD = "KVDqnlPGHSY5";
        private const string TOPIC_LED = "led";
        private string clientId;

        public MqttConnection()
        {
            clientId = "clientTest";
        }

        public async void Connect(Action<bool> callback)
        {
            try
            {
                mqttClient = new OpenNETCF.MQTT.MQTTClient(SERVER_ADDRESS, SERVER_PORT);
                mqttClient.MessageReceived += (topic, qos, payload) =>
                {
                };

                await mqttClient.ConnectAsync(clientId, USER, PASSWORD);

                var i = 0;
                while (!mqttClient.IsConnected)
                {
                    Thread.Sleep(1000);
                    if (i++ > 10)
                        break;
                }

                if (mqttClient.IsConnected)
                    callback?.Invoke(true);
                else
                    callback?.Invoke(false);
            }
            catch (Exception ex)
            {
                callback?.Invoke(false);
            }
        }
        public async void SetLedStatus(EnumLed led, bool status, Action<bool> callback)
        {
            try
            {
                if (mqttClient.IsConnected)
                {
                    await mqttClient.PublishAsync(TOPIC_LED, Encoding.UTF8.GetBytes(formatLedMessage(led, status)), OpenNETCF.MQTT.QoS.AssureDelivery, false);                    
                    callback?.Invoke(true);
                }
                else
                {
                    callback?.Invoke(false);
                }
            }
            catch (Exception)
            {
                callback?.Invoke(false);
            }
        }

        private string formatLedMessage(EnumLed led, bool status)
        {
            return $"{led.GetHashCode()}:{status.ToString()}";
        }
    }
}