using Notification.Helpers.Interface;

namespace Notification.Messengers
{
    /// <summary>
    /// Email class used to send email
    /// </summary>
    public class Messenger : IMessenger
    {
        private readonly IEmailHelper emailHelper;
        /// <summary>
        /// Constructor takes an object of IEmailHelper as parameter and returns email object
        /// </summary>
        public Messenger(IEmailHelper helper)
        {
            emailHelper = helper;
        }
        /// <summary>
        /// Send method on email object will send email
        /// </summary>
        public void Send(string message)
        {
            emailHelper.Send(message);
        }
    }
}