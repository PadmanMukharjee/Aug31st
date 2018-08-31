namespace M3Pact.BusinessModel.BusinessModels
{
    public class DepositLogSimpleBusinessDays
    {
        public int BusinessDays { get; set; }

        public long? Payments { get; set; }
        public decimal? DepositLogPaymentsOnWeekDays { get; set; }
        public decimal? DepositLogPaymentsOnWeekEnds { get; set; }

    }
}
