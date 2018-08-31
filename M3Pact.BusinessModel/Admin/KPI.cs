using M3Pact.BusinessModel.BusinessModels;
using System;

namespace M3Pact.BusinessModel.Admin
{
    public class KPI
    {
        public int KPIID { get; set; }
        public string KPIDescription { get; set; }
        public CheckListType Source { get; set; }
        public Measure Measure { get; set; }
        public string Standard { get; set; }
        public string AlertLevel { get; set; }
        public KPIMeasure KPIMeasure { get; set; }
        public bool? IsHeatMapItem { get; set; }
        public int? HeatMapScore { get; set; }
        public bool? IsUniversal { get; set; }
        public string RecordStatus { get; set; }
        public DateTime? EndDate { get; set; }
        public KPIAlert KPIAlert { get; set; }
    }
}
