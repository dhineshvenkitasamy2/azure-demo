using AzureDemo.Models;
using AzureDemo.NotificationHubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Notification = AzureDemo.NotificationHubs.Notification;

namespace AzureDemo.Controllers
{
    [EnableCors]
    [AllowAnonymous]
    [Produces("application/json")]
    [Route("api/notifications")]
    public class PushNotificationsController : Controller
    {
        private NotificationHubProxy _notificationHubProxy;
        private readonly ConnectIndividualDemoContext _context;

        public PushNotificationsController(IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration, ConnectIndividualDemoContext context)
        {
            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value);
            _context = context;
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
            return Ok(true);
        }
        [HttpPut("enable/{id}")]
        public async Task<IActionResult> RegisterForPushNotifications(string id, [FromBody] DeviceRegistration deviceUpdate)
        {
            HubResponse registrationResult = await _notificationHubProxy.RegisterForPushNotifications(id, deviceUpdate);

            if (registrationResult.CompletedWithSuccess)
                return Ok(true);

            return BadRequest("An error occurred while sending push notification: " + registrationResult.FormattedErrorMessages);
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] Notification newNotification)
        {
            HubResponse<NotificationOutcome> pushDeliveryResult = await _notificationHubProxy.SendNotification(newNotification);

            if (pushDeliveryResult.CompletedWithSuccess)
            {
                NotificationModel notificationModel = new NotificationModel();
                notificationModel.id=Guid.NewGuid().ToString();
                notificationModel.Content=newNotification.Content;
                if (newNotification.Tags != null)
                {
                    notificationModel.Tags = string.Join(",", newNotification.Tags);
                }
                
                _context.Add(notificationModel);
                await _context.SaveChangesAsync();
                return Ok();
            }
               

            return BadRequest("An error occurred while sending push notification: " + pushDeliveryResult.FormattedErrorMessages);
        }
        [HttpGet("getPushNotificationByUser")]
        public async Task<IActionResult> GetPushNotificationByUser([FromQuery] string userId)
        {
            if (_context != null)
            {
                var result = await _context.Notification.Where(notification => notification.Tags.Contains(userId) || String.IsNullOrWhiteSpace(notification.Tags)).ToListAsync();
                return Ok(result);
            }
            return BadRequest("An error occurred ");
        }
        [HttpGet("getAllPushNotification")]
        public async Task<IActionResult> GetAllPushNotification()
        {
            if (_context != null)
            {
                var result = await _context.Notification.ToListAsync();
                return Ok(result);
            }
            return BadRequest("An error occurred ");
        }
    }
}
