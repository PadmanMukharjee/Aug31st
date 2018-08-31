using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel.Checklist
{
    public class SubmitChecklistResponse
    {
        public string clientCode { get; set; }
        public DateTime pendingDate { get; set; }
        public bool isSubmit { get; set; }
        public List<ClientChecklistResponseViewModel> clientChecklistResponse { get; set; }
    }
    public class ClientChecklistResponseViewModel
    {
        public string ChecklistName { get; set; }
        public string QuestionCode { get; set; }
        public int? Questionid { get; set; }
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
