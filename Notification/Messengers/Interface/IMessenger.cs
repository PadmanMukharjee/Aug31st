namespace Notification.Messengers
{
    /// <summary>
    /// Defines interface for sending message
    /// </summary>
    public interface IMessenger
    {
        void Send(string message);
    }
}
