import network
import picoweb
 
app = picoweb.WebApp(__name__)

@app.route("/")
def index(req, resp):
    #print(qs_parse(req.qs))
    yield from picoweb.start_response(resp)
    yield from resp.awrite("Hello world from picoweb running on the ESP32")


sta = network.WLAN(network.STA_IF)
app.run(debug=True, host = sta.ifconfig()[0])


# def qs_parse(qs): 
#     parameters = {}
#     ampersandSplit = qs.split("&")

#     for element in ampersandSplit:
#         equalSplit = element.split("=")
#         parameters[equalSplit[0]] = equalSplit[1]

#     return parameters