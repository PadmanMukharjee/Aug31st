using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientHeatMapRisk
    {
        public int ClientHeatMapRiskId { get; set; }
        public int ClientId { get; set; }
        public DateTime? M3dailyDate { get; set; }
        public DateTime? M3weeklyDate { get; set; }
        public DateTime? M3monthlyDate { get; set; }
        public DateTime? ChecklistWeeklyDate { get; set; }
        public DateTime? ChecklistMonthlyDate { get; set; }
        public int? RiskScore { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? EffectiveTime { get; set; }

        public Client Client { get; set; }
    }
}
