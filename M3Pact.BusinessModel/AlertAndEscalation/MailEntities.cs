using System;

namespace M3Pact.BusinessModel.AlertAndEscalation
{
    public class MailEntity
    {
        public string Client { get; set; }
        public int DeviatedClientKPIId { get; set; }
        public int KPIId { get; set; }
        public string KPIDescription { get; set; }
        public string KPIType { get; set; }
        public string Response { get; set; }
        public bool? IsSLA { get; set; }
        public string Standard { get; set; }
        public DateTime ChecklistDate { get; set; }
        public string DeviatedSince { get; set; }
        public string BillingManager { get; set; }
        public string RelationshipManager { get; set; }
        public string EscalatedRecipients { get; set; }
    }
}
