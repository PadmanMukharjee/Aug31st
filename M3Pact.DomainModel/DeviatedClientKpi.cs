using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class DeviatedClientKpi
    {
        public DeviatedClientKpi()
        {
            MailRecepientsDetailsDayWise = new HashSet<MailRecepientsDetailsDayWise>();
        }

        public int DeviatedClientKpiid { get; set; }
        public DateTime SubmittedDate { get; set; }
        public DateTime CheckListDate { get; set; }
        public int ChecklistTypeId { get; set; }
        public int ClientId { get; set; }
        public string QuestionCode { get; set; }
        public string RecordStatus { get; set; }
        public string ExpectedResponse { get; set; }
        public string ActualResponse { get; set; }

        public CheckListType ChecklistType { get; set; }
        public Client Client { get; set; }
        public ICollection<MailRecepientsDetailsDayWise> MailRecepientsDetailsDayWise { get; set; }
    }
}
