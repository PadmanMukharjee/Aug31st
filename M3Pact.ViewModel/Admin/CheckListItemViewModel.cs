namespace M3Pact.ViewModel.Admin
{
    public class CheckListItemViewModel
    {
        public int QuestionID { get; set; }
        public string Question { get; set; }
        public bool ExpectedResponse { get; set; }
        public bool Kpi { get; set; }
        public bool Universal { get; set; }
        public bool Freeform { get; set; }
        public string QuestionCode { get; set; }
        public CheckListTypeViewModel checkListType { get; set; }
        public KPIViewModel KPIDescription { get; set; }
        public string RecordStatus { get; set; }
    }
}
