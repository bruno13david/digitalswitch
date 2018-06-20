using System.Net.Http;

namespace DigitalSwitchClient
{
    public class Connection
    {
        private const string URL = "";
        private const string OPERATION_STATUS = "";
        private const string OPERATION_SET_STATUS = "";

        public void GetStatus(string lamp)
        {
            HttpClient client = new HttpClient();
            var result = client.GetAsync(URL);
        }
    }
}