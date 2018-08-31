using M3Pact.LoggerUtility.Helpers.Interface;
using System.IO;
using System.Text;

namespace M3Pact.LoggerUtility.Helpers
{
    /// <summary>
    /// Implements Write method to write to File systems
    /// </summary>
    class FileWriter : IOutputWriter
    {
        private readonly IFileNameProvider _fileNameProvider;

        public FileWriter(IFileNameProvider fileNameProvider)
        {
            _fileNameProvider = fileNameProvider;
        }

        /// <summary>
        /// Writes the message to the file specified in Configuration file
        /// </summary>
        /// <param name="message"></param>
        public void Write(string message)
        {
            var fileName = _fileNameProvider.GetFileName();
            
            using (FileStream stream = File.Open(fileName, FileMode.Append))
            {
                using (StreamWriter streamWriter = new StreamWriter(stream))
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Flush();
                }
            }
        }
    }
}
