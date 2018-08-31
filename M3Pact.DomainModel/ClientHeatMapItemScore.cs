using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ClientHeatMapItemScore
    {
        public int ClientHeatMapItemScoreId { get; set; }
        public int HeatMapItemId { get; set; }
        public int ClientId { get; set; }
        public DateTime HeatMapItemDate { get; set; }
        public int? Score { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Client Client { get; set; }
        public HeatMapItem HeatMapItem { get; set; }
    }
}
