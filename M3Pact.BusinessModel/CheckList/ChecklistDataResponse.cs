using System;

namespace M3Pact.BusinessModel
{
    public class ChecklistDataResponse
    {
        public string ChecklistName { get; set; }
        public string QuestionText { get; set; }
        public string EffectiveDate { get; set; }
        public AnswerResponse Answer { get; set; }
        public string RowSelector { get; set; }
        public DateTime QuestionStartDate { get; set; }
        public DateTime QuestionEndDate { get; set; }
        public DateTime QuestionEffectiveDate { get; set; }
    }

    public class AnswerResponse
    {
        public string SubmittedResponse { get; set; }
        public string FreeformResponse { get; set; }
    }
}
