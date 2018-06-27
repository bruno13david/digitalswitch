from machine import Pin

pin1 = Pin(5, Pin.OUT)
pin2 = Pin(4, Pin.OUT)
pin3 = Pin(0, Pin.OUT)
pin4 = Pin(2, Pin.OUT)

def setPin1(enable):
    if enable:
        pin1.off()
    else:
        pin1.on()

def setPin2(enable):
    if enable:
        pin2.off()
    else:
        pin2.on()

def setPin3(enable):
    if enable:
        pin3.off()
    else:
        pin3.on()

def setPin4(enable):
    if enable:
        pin4.off()
    else:
        pin4.on()