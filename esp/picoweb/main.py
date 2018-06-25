import picoweb
import time
import network

sta = network.WLAN(network.STA_IF)
sta.connect('A', '1234567a')

time.sleep(7)

def qs_parse(qs): 
    parameters = {}
    ampersandSplit = qs.split("&")

    for element in ampersandSplit:
        equalSplit = element.split("=")
        parameters[equalSplit[0]] = equalSplit[1]

    return parameters

app = picoweb.WebApp(__name__)
 
@app.route("/")
def index(req, resp):
    print(qs_parse(req.qs))
    yield from picoweb.start_response(resp)
    yield from resp.awrite("Hello world from picoweb running on the ESP32")

app.run(debug=True, host = "192.168.43.249")
