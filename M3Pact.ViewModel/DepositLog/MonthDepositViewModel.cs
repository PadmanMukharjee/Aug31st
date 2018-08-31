namespace M3Pact.ViewModel.DepositLog
{
    public class MonthDepositViewModel
    {
        public int MonthID { get; set; }
        public string MonthName { get; set; }
        public string MonthStatus { get; set; }
        public long? Target { get; set; }
        public decimal? ActualDeposit { get; set; }
        public string MetPercent { get; set; }
        public string MetPercentStatus { get; set; }   
    }
}
