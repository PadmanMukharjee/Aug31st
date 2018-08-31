using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class M3metricClientKpiDaily
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int KpiId { get; set; }
        public bool IsDeviated { get; set; }
        public DateTime InsertedDate { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string AlertLevel { get; set; }
        public string ActualValue { get; set; }

        public Client Client { get; set; }
        public Kpi Kpi { get; set; }
    }
}
