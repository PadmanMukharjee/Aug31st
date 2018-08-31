using System;

namespace M3Pact.BusinessModel.AlertAndEscalation
{
    public class DeviatedClientKPIBusiness
    {      
        public int DeviatedClientKpiid { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime CheckListDate { get; set; }
        public int ChecklistTypeId { get; set; }
        public int ClientId { get; set; }
        public string QuestionCode { get; set; }
        public string RecordStatus { get; set; }
        public bool? ExpectedResponse { get; set; }
        public bool? ActualResponse { get; set; }       
    }
}
