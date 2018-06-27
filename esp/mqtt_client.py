from umqtt.simple import MQTTClient
import leds_manager
import time

mq = MQTTClient("MqttServer","m20.cloudmqtt.com",10146,"","")

def public_message(message):
    mq.connect()    
    mq.publish(b"led",b"{0}".format(message))

def subscribe():    
    mq.connect()    
    mq.set_callback(subscribe_callback)
    mq.subscribe(b"led")
    print('subscribe topic led...')

    while True:
        if True:
            mq.wait_msg()
        else:
            mq.check_msg()
            time.sleep(1)

def subscribe_callback(topic, message):
    print('msg recebida')
    print(topic)
    if topic.decode("utf-8") == "led":
        treat_led_topic(message.decode("utf-8"))

def treat_led_topic(message):
    data = message.split(':')
    if len(data) == 2:
        if int(data[0]) == 1:
            leds_manager.setPin1(isTrue(data[1]))
        elif int(data[0]) == 2:
            leds_manager.setPin2(isTrue(data[1]))
        elif int(data[0]) == 3:
            leds_manager.setPin3(isTrue(data[1]))
        elif int(data[0]) == 4:
            leds_manager.setPin4(isTrue(data[1]))

def isTrue(st):
    return st == "True" 