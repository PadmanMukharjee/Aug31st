namespace M3Pact.BusinessModel.BusinessModels
{
    public class DepositLogMonthlyTarget
    {
        public int BusinessDays { get; set; }

        public long? Payments { get; set; }
        public decimal? DepositLogPaymentsTillDate { get; set; }

    }
}
