using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class ValidationViewModel
    {
        public ValidationViewModel()
        {
            ErrorMessages = new List<string>();
            WarningMessages = new List<string>();
            InfoMessages = new List<string>();
            RateMessages = new List<string>();
            Success = true;
        }

        /// <summary>
        /// Can be Business Rule Error message or Exception Error message
        /// </summary>
        public List<string> ErrorMessages { get; set; }

        public List<string> WarningMessages { get; set; }

        public List<string> InfoMessages { get; set; }

        public List<string> RateMessages { get; set; }

        public string SuccessMessage { get; set; }

        public bool Success { get; set; }

        public bool IsExceptionOccured { get; set; }

        public string AdditionalInfo { get; set; }
    }
}
