using M3Pact.LoggerUtility.Helpers.Interface;
using System;
using System.IO;

namespace M3Pact.LoggerUtility.Helpers.Implementation
{
    /// <summary>
    /// Wrapper for Path static class
    /// </summary>
    public class PathProvider : IPathProvider
    {
        /// <summary>
        /// Proxy for Path.Combine
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public string Combine(string directoryPath, string filePath)
        {
            return Path.Combine(directoryPath, filePath);
        }

        /// <summary>
        /// Returns current BaseDirectory
        /// </summary>
        /// <returns></returns>
        public string GetDirectoryPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Proxy for Path.GetFileName
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
    }
}
