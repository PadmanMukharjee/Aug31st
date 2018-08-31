using System;

namespace M3Pact.ViewModel
{
    public class ClientTargetViewModel
    {
        public int? ClientId { get; set; }
        public int? MonthId { get; set; }
        public int? CalendarYear { get; set; }
        public bool? IsManualEntry { get; set; }
        public long? AnnualCharges { get; set; }
        public decimal? GrossCollectionRate { get; set; }
        public long? Charges { get; set; }
        public long? Payments { get; set; }
        public long? Revenue { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string ClientCode { get; set; }
        public string Month { get; set; }
    }
}
