using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel.DepositLog
{
    public class ExportDepositViewModel
    {
        public string ClientCode { get; set; }
        public List<DateTime> ExportMonths { get; set; }
    }
}
