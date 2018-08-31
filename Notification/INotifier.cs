namespace Notification
{
    /// <summary>
    /// Defines interface for sending notification
    /// </summary>
    public interface INotifier
    {
        void Notify(string message);
    }
}
