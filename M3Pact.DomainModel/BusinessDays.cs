using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class BusinessDays
    {
        public int BusinessDaysId { get; set; }
        public int Year { get; set; }
        public int? MonthId { get; set; }
        public int BusinessDays1 { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Month Month { get; set; }
    }
}
