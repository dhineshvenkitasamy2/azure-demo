using AzureDemo.Models;
using AzureDemo.NotificationHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using Notification = AzureDemo.NotificationHubs.Notification;

namespace AzureDemo.Controllers
{
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/notifications")]
    public class PushNotificationsController : Controller
    {
        private NotificationHubProxy _notificationHubProxy;

        public PushNotificationsController(IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration)
        {
            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value);
        }

        [HttpGet("register")]
        public async Task<IActionResult> CreatePushRegistrationId()
        {
            var registrationId = await _notificationHubProxy.CreateRegistrationId();
            return Ok(registrationId);
        }
        [HttpDelete("unregister/{registrationId}")]
        public async Task<IActionResult> UnregisterFromNotifications(string registrationId)
        {
            await _notificationHubProxy.DeleteRegistration(registrationId);
            return Ok();
        }
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> RegisterForPushNotifications(string id, [FromBody] DeviceRegistration deviceUpdate)
        {
            HubResponse registrationResult = await _notificationHubProxy.RegisterForPushNotifications(id, deviceUpdate);

            if (registrationResult.CompletedWithSuccess)
                return Ok();

            return BadRequest("An error occurred while sending push notification: " + registrationResult.FormattedErrorMessages);
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Notification newNotification)
        {
            HubResponse<NotificationOutcome> pushDeliveryResult = await _notificationHubProxy.SendNotification(newNotification);

            if (pushDeliveryResult.CompletedWithSuccess)
                return Ok();

            return BadRequest("An error occurred while sending push notification: " + pushDeliveryResult.FormattedErrorMessages);
        }

    }
}
