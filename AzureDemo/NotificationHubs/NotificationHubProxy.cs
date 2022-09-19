using AzureDemo.Models;
using Microsoft.Azure.NotificationHubs.Messaging;
using Microsoft.Azure.NotificationHubs;

namespace AzureDemo.NotificationHubs
{
    public class NotificationHubProxy
    {
        private NotificationHubConfiguration _configuration;
        private NotificationHubClient _hubClient;

        public NotificationHubProxy(NotificationHubConfiguration configuration)
        {
            _configuration = configuration;
            _hubClient = NotificationHubClient.CreateClientFromConnectionString(_configuration.ConnectionString, _configuration.HubName);
        }
        public async Task<string> CreateRegistrationId()
        {
            return await _hubClient.CreateRegistrationIdAsync();
        }
        public async Task DeleteRegistration(string registrationId)
        {
            await _hubClient.DeleteRegistrationAsync(registrationId);
        }

        public async Task<HubResponse> RegisterForPushNotifications(string id, DeviceRegistration deviceUpdate)
        {
            RegistrationDescription? registrationDescription = null;

            switch (deviceUpdate.Platform)
            {
                case (int)MobilePlatform.wns:
                    registrationDescription = new WindowsRegistrationDescription(deviceUpdate.Handle);
                    break;
                case (int)MobilePlatform.apns:
                    registrationDescription = new AppleRegistrationDescription(deviceUpdate.Handle);
                    break;
                case (int)MobilePlatform.gcm:
                    registrationDescription = new FcmRegistrationDescription(deviceUpdate.Handle);
                    break;
                default:
                    return new HubResponse().AddErrorMessage("Please provide correct platform notification service name.");
            }

            registrationDescription.RegistrationId = id;
            if (deviceUpdate.Tags != null)
                registrationDescription.Tags = new HashSet<string>(deviceUpdate.Tags);

            try
            {
                await _hubClient.CreateOrUpdateRegistrationAsync(registrationDescription);
                return new HubResponse();
            }
            catch (MessagingException)
            {
                return new HubResponse().AddErrorMessage("Registration failed because of HttpStatusCode.Gone. PLease register once again.");
            }
        }
        public async Task<HubResponse<NotificationOutcome>> SendNotification(Notification newNotification)
        {
            try
            {
                NotificationOutcome? outcome = null;

                switch (newNotification.Platform)
                {
                    case (int)MobilePlatform.wns:
                        if (newNotification.Tags == null)
                            outcome = await _hubClient.SendWindowsNativeNotificationAsync(newNotification.Content);
                        else
                            outcome = await _hubClient.SendWindowsNativeNotificationAsync(newNotification.Content, newNotification.Tags);
                        break;
                    case (int)MobilePlatform.apns:
                        if (newNotification.Tags == null)
                            outcome = await _hubClient.SendAppleNativeNotificationAsync(newNotification.Content);
                        else
                            outcome = await _hubClient.SendAppleNativeNotificationAsync(newNotification.Content, newNotification.Tags);
                        break;
                    case (int)MobilePlatform.gcm:
                        if (newNotification.Tags == null)
                            outcome = await _hubClient.SendFcmNativeNotificationAsync(newNotification.Content);
                        else
                            outcome = await _hubClient.SendFcmNativeNotificationAsync(newNotification.Content, newNotification.Tags);
                        break;
                }

                if (outcome != null)
                {
                    if (!((outcome.State == NotificationOutcomeState.Abandoned) ||
                        (outcome.State == NotificationOutcomeState.Unknown)))
                        return new HubResponse<NotificationOutcome>();
                }

                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage("Notification was not sent due to issue. Please send again.");
            }

            catch (MessagingException ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }

            catch (ArgumentException ex)
            {
                return new HubResponse<NotificationOutcome>().SetAsFailureResponse().AddErrorMessage(ex.Message);
            }
        }
    }
}
