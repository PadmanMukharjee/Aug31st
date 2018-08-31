using System.Collections.Generic;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class ClientPayer
    {
        public ClientPayer()
        {
            Payer = new Payer();
            DepositLog = new List<BusinessModels.DepositLog>();
        }

        public bool? IsM3feeExempt { get; set; }
        public Payer Payer { get; set; }
        public List<DepositLog> DepositLog { get; set; }
        public string RecordStatus { get; set; }
        public bool IsEditable { get; set; }
        public int ID { get; set; }
        public string ClientCode { get; set; }
        public bool CanDelete { get; set; }
    }
}
