using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientPayer
    {
        public ClientPayer()
        {
            DepositLog = new HashSet<DepositLog>();
        }

        public int ClientPayerId { get; set; }
        public int? ClientId { get; set; }
        public int? PayerId { get; set; }
        public bool? IsM3feeExempt { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public Client Client { get; set; }
        public Payer Payer { get; set; }
        public ICollection<DepositLog> DepositLog { get; set; }
    }
}
