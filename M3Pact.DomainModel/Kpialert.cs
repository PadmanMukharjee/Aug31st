using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Kpialert
    {
        public int KpialertId { get; set; }
        public int Kpiid { get; set; }
        public bool? SendAlert { get; set; }
        public string SendAlertTitle { get; set; }
        public bool? SendToRelationshipManager { get; set; }
        public bool? SendToBillingManager { get; set; }
        public bool? EscalateAlert { get; set; }
        public string EscalateAlertTitle { get; set; }
        public string EscalateTriggerTime { get; set; }
        public bool? IncludeKpitarget { get; set; }
        public bool? IncludeDeviationTarget { get; set; }
        public bool? IsSla { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Kpi Kpi { get; set; }
    }
}
