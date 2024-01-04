
namespace DigitalBank.Onboard.Api.Shared
{
    public class NotificationContext
    {
        private readonly List<Notification> _notifications;

        public NotificationContext()
        {
            _notifications = new List<Notification>();
        }

        public void AddNotification(string message)
        {
            _notifications.Add(new Notification(message));
        }

        public bool HasNotifications => _notifications.Any();
        public IReadOnlyCollection<Notification> Notifications => _notifications.AsReadOnly();
    }
}