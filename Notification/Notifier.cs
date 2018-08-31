using System.Collections.Generic;
using Notification.Messengers;

namespace Notification
{
    /// <summary>
    /// Used to create Notifier object and call Notify() method on it.
    /// </summary>
    public class Notifier : INotifier
    {
        private List<IMessenger> messengers;

        /// <summary>
        /// Constructor gets list of messengers like email, sms etc from configuration file and creates a list of messenger objects.
        /// </summary>
        public Notifier()
        {
            messengers = MessengerSelector.GetMessengersFromConfig();
        }

        /// <summary>
        /// Notify will call Send method on all messenger objects.
        /// </summary>
        public void Notify(string message)
        {
            foreach (IMessenger messenger in messengers)
            {
                messenger.Send(message);
            }
        }
    }
}
