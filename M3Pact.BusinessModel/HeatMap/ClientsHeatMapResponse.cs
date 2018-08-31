using System;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.HeatMap
{
    public class ClientsHeatMapResponse
    {
        public ClientsHeatMapResponse()
        {
            HeatMapItemNameDetail = new HeatMapItemDetail();
        }
        public string SiteAcronym { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public string Specialty { get; set; }
        public string BusinessUnitCode { get; set; }
        public string SystemCode { get; set; }
        public decimal? Ltm { get; set; }
        public string HeatMapItemName { get; set; }
        public HeatMapItemDetail HeatMapItemNameDetail { get; set; }
        public DateTime? ChecklistWeeklyDate { get; set; }
        public DateTime? ChecklistMonthlyDate { get; set; }
        public int Risk { get; set; }
        public int? RiskPercentage { get; set; }
        public int? Trend { get; set; }
    }

    public class HeatMapItemDetail
    {
        public int? Score { get; set; }
        public string Type { get; set; }
        public string ActualValue { get; set; }
        public string AlertLevel { get; set; }
    }

    public class HeatMapItemTypeDetail
    {
        public string HeatMapItemName { get; set; }
        public string HeatMapItemType { get; set; }
        public string HeatMapItemMeasureType { get; set; }
    }

    public class HeatMapDetails
    {
        public HeatMapDetails()
        {
            ClientsHeatMapList = new List<ClientsHeatMapResponse>();
            HeatMapItemTypeDetail = new List<HeatMapItemTypeDetail>();
        }
        public List<ClientsHeatMapResponse> ClientsHeatMapList { get; set; }
        public List<HeatMapItemTypeDetail> HeatMapItemTypeDetail { get; set; }
    }
}
