using M3Pact.LoggerUtility.Helpers.Interface;
using System;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    /// <summary>
    /// Defines wrapper class for DateTime object
    /// </summary>
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
