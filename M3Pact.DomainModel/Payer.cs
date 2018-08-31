using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Payer
    {
        public Payer()
        {
            ClientPayer = new HashSet<ClientPayer>();
        }

        public int PayerId { get; set; }
        public string PayerCode { get; set; }
        public string PayerName { get; set; }
        public string PayerDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public ICollection<ClientPayer> ClientPayer { get; set; }
    }
}
