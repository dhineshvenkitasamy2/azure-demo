using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace AzureDemo.NotificationHubs
{
    public class DeviceRegistration
    {
        public int Platform { get; set; }
        public string Handle { get; set; } = String.Empty;  
        public string[] Tags { get; set; } = new string[0];
    }
}
