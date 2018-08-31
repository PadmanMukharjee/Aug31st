using System;

namespace M3Pact.LoggerUtility.Helpers.Interface.Providers
{
    public interface IFrequencyBasedFileNameProviderFactory : IProviderFactory
    {
        DateTime GetPreviousDate();
        void SetPreviousDate(DateTime date);
        string GetPreviousFileName();
        void SetPreviousFileName(string fileName);
    }
}
