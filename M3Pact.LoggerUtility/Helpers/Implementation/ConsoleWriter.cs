using System;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    /// <summary>
    /// Implements Write method to writes to Console
    /// </summary>
    class ConsoleWriter : IOutputWriter
    {
        /// <summary>
        /// Writes the specified string value to the standard output stream
        /// </summary>
        /// <param name="message">Log message</param>
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
