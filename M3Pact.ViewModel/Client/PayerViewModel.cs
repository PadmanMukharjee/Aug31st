using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class PayerViewModel : BusinessBaseViewModel
    {
        public string PayerCode { get; set; }
        public string PayerName { get; set; }
        public string PayerDescription { get; set; }     
        public List<string> Clients { get; set; }
    }
}
