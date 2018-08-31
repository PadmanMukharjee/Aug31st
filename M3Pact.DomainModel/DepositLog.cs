using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class DepositLog
    {
        public int DepositLogId { get; set; }
        public int? ClientPayerId { get; set; }
        public int? DepositDateId { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsM3feeExempt { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ClientPayer ClientPayer { get; set; }
        public DateDimension DepositDate { get; set; }
    }
}
