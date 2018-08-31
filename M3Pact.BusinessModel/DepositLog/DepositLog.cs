using System;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class DepositLog
    {
        public DateTime? DepositDate { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsM3feeExempt { get; set; }
        public string RecordStatus { get; set; }
    }
}
