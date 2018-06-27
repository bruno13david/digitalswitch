from machine import Pin
import ujson
import network

internalLed = Pin(2, Pin.OUT) 
sta_if = network.WLAN(network.STA_IF)

def do_connect():  
    network_found_list = sta_if.scan()
    network_config_list = get_network_config()
    
    for network_found in network_found_list:    
        list_result = [x for x in network_config_list if x['ssid'] == network_found[0].decode("utf-8")]                
        if len(list_result) > 0:
            net = list_result[0]
            pass

    if net:
        connect_in_network(net)            
        print('network config:', sta_if.ifconfig())

        if sta_if.isconnected():
            import webrepl
            webrepl.start()
            internalLed.off()

            import mqtt_client
            mqtt_client.subscribe()
        else:
            internalLed.on()

def get_network_config():
    json_f = open('init.json', 'r')
    return ujson.loads(json_f.read())

def connect_in_network(net):
    if not sta_if.isconnected():
        print('connecting to network...')            
        sta_if.active(True)

        sta_if.connect(net["ssid"], net["password"])      
        sta_if.ifconfig((net["ip"], net["mask"], net["gateway"], net["dns"]))

        while not sta_if.isconnected():
            pass
    

