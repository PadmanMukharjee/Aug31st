namespace Notification.Helpers.Interface
{
    /// <summary>
    /// Defines the interface for sending email
    /// </summary>
    public interface IEmailHelper
    {
        void Send(string message);
    }
}
