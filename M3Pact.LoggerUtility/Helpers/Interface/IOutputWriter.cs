namespace M3Pact.LoggerUtility.Helpers
{
    /// <summary>
    /// Provides interface for classes that have write function
    /// </summary>
    public interface IOutputWriter
    {
        void Write(string message);
    }
}
