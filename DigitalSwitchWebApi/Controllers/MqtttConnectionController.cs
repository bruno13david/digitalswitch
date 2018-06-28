using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Net;
using System.Text;

namespace DigitalSwitchWebApi.Controllers
{
    [Route("api/[controller]")]
    public class MqttConnectionController : Controller
    {
        [HttpPost]
        public void Post([FromBody]string value)
        {
            MqttClient client = new MqttClient("cpbsb2.o2br.net"); 
 
            string clientId = Guid.NewGuid().ToString(); 
            client.Connect(clientId);             
            string strValue = Convert.ToString(value); 

            client.Publish("led", Encoding.UTF8.GetBytes(strValue), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false); 
        }
    }
}