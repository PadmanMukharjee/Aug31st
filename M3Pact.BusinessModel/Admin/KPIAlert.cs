namespace M3Pact.BusinessModel.Admin
{
    public class KPIAlert
    {
        public int KPIAlertId { get; set; }
        public bool? SendAlert { get; set; }
        public string SendAlertTitle { get; set; }
        public bool? SendToRelationshipManager { get; set; }
        public bool? SendToBillingManager { get; set; }
        public bool? EscalateAlert { get; set; }
        public string EscalateAlertTitle { get; set; }
        public string EscalateTriggerTime { get; set; }
        public bool? IncludeKPITarget { get; set; }
        public bool? IncludeDeviationTarget { get; set; }
        public bool? IsSla { get; set; }
        public string RecordStatus { get; set; }
    }
}
