using System;

namespace M3Pact.ViewModel.Client
{
    public class ClientCloseMonthViewModel
    {
        public int Year { get; set; }
        public string MonthStatus { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReOpenDate { get; set; }

        public string ClientCode { get; set; }
        public MonthViewModel Month { get; set; }
    }
}
