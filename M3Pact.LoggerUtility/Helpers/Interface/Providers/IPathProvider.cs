namespace M3Pact.LoggerUtility.Helpers.Interface
{
    public interface IPathProvider
    {
        string GetDirectoryPath();
        string Combine(string directoryPath, string filePath);
        string GetFileName(string path);
    }
}