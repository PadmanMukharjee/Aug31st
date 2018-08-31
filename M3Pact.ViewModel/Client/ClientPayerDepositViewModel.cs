using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class ClientPayerDepositViewModel :PayerViewModel
    {
        //public string ClientCode { get; set; }
        public List<DepositLogViewModel> DepositLogs { get; set; }
    }
}
