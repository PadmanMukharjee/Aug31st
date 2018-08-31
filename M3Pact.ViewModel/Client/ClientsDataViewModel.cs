namespace M3Pact.ViewModel.Client
{
    public class ClientsDataViewModel
    {
        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string Site { get; set; }
        public string BillingManager { get; set; }
        public string RelationshipManager { get; set; }
        public decimal? MTDDeposit { get; set; }
        public decimal? MTDTarget { get; set; }
        public decimal? ProjectedCash { get; set; }
        public decimal? MonthlyTarget { get; set; }
        public decimal? ActualM3Revenue { get; set; }
        public decimal? ForecastedM3Revenue { get; set; }
        public string Status { get; set; }

    }
}
