namespace M3Pact.ViewModel
{
    public class ClientStepDetailViewModel
    {
        public int StepDetailID { get; set; }
        public int StepID { get; set; }
        public string StepName { get; set; }
        public int StepStatusID { get; set; }
        public string StepStatusName { get; set; }
        public string ClientCode { get; set; }
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
    }
}
