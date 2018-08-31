using M3Pact.BusinessModel.BusinessModels;
using System;

namespace M3Pact.BusinessModel.Client
{
    public class ClientCloseMonth
    {
        public int Year { get; set; }
        public string MonthStatus { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ReOpenDate { get; set; }

        public String ClientCode { get; set; }
        public Month Month { get; set; }
    }
}
