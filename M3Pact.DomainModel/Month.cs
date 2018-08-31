using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Month
    {
        public Month()
        {
            BusinessDays = new HashSet<BusinessDays>();
            ClientCloseMonth = new HashSet<ClientCloseMonth>();
            ClientTarget = new HashSet<ClientTarget>();
            DepositLogMonthlyDetails = new HashSet<DepositLogMonthlyDetails>();
        }

        public int MonthId { get; set; }
        public string MonthCode { get; set; }
        public string MonthName { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<BusinessDays> BusinessDays { get; set; }
        public ICollection<ClientCloseMonth> ClientCloseMonth { get; set; }
        public ICollection<ClientTarget> ClientTarget { get; set; }
        public ICollection<DepositLogMonthlyDetails> DepositLogMonthlyDetails { get; set; }
    }
}
