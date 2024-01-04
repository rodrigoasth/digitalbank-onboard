
namespace DigitalBank.Onboard.Api.Shared
{
    public class Notification
    {
        public string Message { get; }

        public Notification(string message)
        {
            Message = message;
        }
    }
}