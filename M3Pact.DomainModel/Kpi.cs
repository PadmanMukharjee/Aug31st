using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Kpi
    {
        public Kpi()
        {
            ClientKpimap = new HashSet<ClientKpimap>();
            HeatMapItem = new HashSet<HeatMapItem>();
            Kpialert = new HashSet<Kpialert>();
            M3metricClientKpiDaily = new HashSet<M3metricClientKpiDaily>();
        }

        public int Kpiid { get; set; }
        public string Kpidescription { get; set; }
        public int CheckListTypeId { get; set; }
        public string Measure { get; set; }
        public string Standard { get; set; }
        public string AlertLevel { get; set; }
        public int KpimeasureId { get; set; }
        public bool? IsHeatMapItem { get; set; }
        public int? HeatMapScore { get; set; }
        public bool? IsUniversal { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public CheckListType CheckListType { get; set; }
        public Kpimeasure Kpimeasure { get; set; }
        public ICollection<ClientKpimap> ClientKpimap { get; set; }
        public ICollection<HeatMapItem> HeatMapItem { get; set; }
        public ICollection<Kpialert> Kpialert { get; set; }
        public ICollection<M3metricClientKpiDaily> M3metricClientKpiDaily { get; set; }
    }
}
