namespace M3Pact.ViewModel
{
    public class DepositLogProjectionViewModel
    {
        public long? Payments { get; set; }
        public decimal? DepositLogAmount { get; set; }

        public int NumberOfLastWorkingDaysOrWeeks { get; set; }
    }
}
