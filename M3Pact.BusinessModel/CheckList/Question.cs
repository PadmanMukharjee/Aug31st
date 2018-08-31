using M3Pact.BusinessModel.Admin;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class Question
    {
        public Question()
        {
            checkListType = new CheckListType();
            KPIDescription = new KPI();
        }
        public int QuestionId { get; set; }
        public string QuestionText { get; set; }
        public string QuestionCode { get; set; }
        public string RecordStatus { get; set; }
        public bool ExpectedResponse { get; set; }
        public bool IsKpi { get; set; }
        public bool IsUniversal { get; set; }
        public bool IsFreeform { get; set; }
        public CheckListType checkListType { get; set; }
        public KPI KPIDescription { get; set; }
    }
}
