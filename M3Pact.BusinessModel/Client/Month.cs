using System;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class Month
    {
        public Month()
        {
            BusinessDays = new HashSet<BusinessDays>();
            ClientTarget = new HashSet<ClientTargetModel>();
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
        public ICollection<ClientTargetModel> ClientTarget { get; set; }
    }
}