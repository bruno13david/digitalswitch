from umqtt.simple import MQTTClient

def create_connection():
    mq = MQTTClient("MqttServer","",10146,"","")
    mq.connect()
    mq.publish(b"topico1",b"1")