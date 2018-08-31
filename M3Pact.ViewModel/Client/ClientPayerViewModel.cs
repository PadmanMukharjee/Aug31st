namespace M3Pact.ViewModel.Client
{
    public class ClientPayerViewModel : PayerViewModel
    {
        public string ClientCode { get; set; }
        public bool? IsM3FeeExempt { get; set; }
        public bool IsEditable { get; set; }
        public bool CanDelete { get; set; }
    }
}
