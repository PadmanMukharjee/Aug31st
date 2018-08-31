using System;

namespace M3Pact.ViewModel
{
    public class BaseViewModel
    {
        protected BaseViewModel()
        {
            Validation = new ValidationViewModel();
        }
        public ValidationViewModel Validation { get; set; }

        public string User { get; set; }

        public int UserID { get; set; }

        public DateTime CurrentTime
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
