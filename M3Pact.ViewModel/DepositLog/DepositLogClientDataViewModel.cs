using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class DepositLogClientDataViewModel
    {
        public string ClientCode { get; set; }
        public DateTime Date { get; set; }
        public decimal? Total { get; set; }
        public List<PayerDataViewModel> Payers { get; set; }
        public bool IsMonthClosed { get; set; }
        public int SavedLastNumberOfBusinessDays { get; set; }
    }
}
