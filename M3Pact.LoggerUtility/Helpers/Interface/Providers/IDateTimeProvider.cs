using System;

namespace M3Pact.LoggerUtility.Helpers.Interface
{
    /// <summary>
    /// Defines interface for wrapper class around DateTime object
    /// </summary>
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
