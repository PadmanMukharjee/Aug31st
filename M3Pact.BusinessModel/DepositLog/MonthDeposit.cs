namespace M3Pact.BusinessModel.DepositLog
{
    public class MonthDeposit
    {
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public string MonthStatus { get; set; }
        public long? Target { get; set; }
        public decimal? ActualDeposit { get; set; }
        public decimal? MetPercent { get; set; }
    }
}
