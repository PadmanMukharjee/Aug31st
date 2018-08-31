namespace M3Pact.BusinessModel.BusinessModels
{
    public class DepositLogMTD
    {
        public int? ClientId { get; set; }
        public string MonthCode { get; set; }
        public long? AnnualCharges { get; set; }
        public decimal? GrossCollectionRate { get; set; }
        public long? Payments { get; set; }
        public int BusinessDays { get; set; }
        public decimal? TotalDepositAmount { get; set; }
     }
}
