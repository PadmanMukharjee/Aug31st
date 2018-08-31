using System;
using System.Collections.Generic;
using Notification.Messengers;
using Notification.Helpers;
using Notification.Enums;
using ConfigManager;

namespace Notification
{
    /// <summary>
    /// Contains utility functions for getting messengers from config file
    /// </summary>
    class MessengerSelector
    {
        /// <summary>
        /// Returns list of messenger objects based on targets mentioned in config file
        /// </summary>
        /// <returns>List of IMessenger objects</returns>
        public static List<IMessenger> GetMessengersFromConfig()
        {
            var messengers = new List<IMessenger>();

            var appConfig = new ConfigurationProvider();
            var notificationConfig = new ConfigurationProvider(appConfig.GetValue(Constants.ConfigFileName));
            List<string> messengersInConfig = notificationConfig.GetList();

            foreach (var messengerName in messengersInConfig)
            {
                messengers.Add(GetMessenger(messengerName));
            }
            return messengers;
        }

        /// <summary>
        /// utility function to get messenger object provided string value of  the messenger name
        /// </summary>
        /// <param name="messenger"></param>
        /// <returns>IMessenger object </returns>
        private static IMessenger GetMessenger(string messenger)
        {
            var messengerType = (MessengerType)Enum.Parse(typeof(MessengerType), messenger.ToUpper());
            switch (messengerType)
            {
                case MessengerType.EMAIL:
                    return new Messenger(new EmailHelper());

                default:
                    return null;
            }
        }
    }
}
