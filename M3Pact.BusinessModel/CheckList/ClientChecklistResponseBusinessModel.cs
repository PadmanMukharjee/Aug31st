namespace M3Pact.BusinessModel.CheckList
{
    public class ClientChecklistResponseBusinessModel
    {
        public string ChecklistName { get; set; }
        public int? Questionid { get; set; }
        public string QuestionCode { get; set; }
        public string QuestionText { get; set; }
        public bool? IsKPI { get; set; }
        public bool? RequireFreeform { get; set; }
        public bool? ExpectedRespone { get; set; }
        public bool? ActualResponse { get; set; }
        public string ActualFreeForm { get; set; }
        public int? ClientCheckListMapID { get; set; }
        public int? CheckListAttributeMapID { get; set; }
    }
}
