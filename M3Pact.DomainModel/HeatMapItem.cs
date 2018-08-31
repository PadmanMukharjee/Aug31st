using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class HeatMapItem
    {
        public HeatMapItem()
        {
            ClientHeatMapItemScore = new HashSet<ClientHeatMapItemScore>();
        }

        public int HeatMapItemId { get; set; }
        public int Kpiid { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public Kpi Kpi { get; set; }
        public ICollection<ClientHeatMapItemScore> ClientHeatMapItemScore { get; set; }
    }
}
