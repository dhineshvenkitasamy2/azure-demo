using AzureDemo.NotificationHubs;
using Microsoft.Data.SqlClient.DataClassification;

namespace AzureDemo.Models
{
    public class NotificationModel
    {
        public string id { get; set; } = Guid.NewGuid().ToString();
        public string Content { get; set; } = String.Empty;
        public string? Tags { get; set; }
    }
}
