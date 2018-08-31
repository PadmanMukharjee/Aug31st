namespace M3Pact.ViewModel.Admin
{
    public class KPIViewModel
    {
        public int KPIID { get; set; }
        public string KPIDescription { get; set; }
        public CheckListTypeViewModel Source { get; set; }
        public MeasureViewModel Measure { get; set; }
        public string Standard { get; set; }
        public string AlertLevel {get;set;}
        public KPIMeasureViewModel KPIMeasure { get;set;}
        public bool? IsHeatMapItem { get;set;}
        public int? HeatMapScore {get;set;}
        public bool? IsUniversal { get; set; }
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
