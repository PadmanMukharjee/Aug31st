using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class MailRecepientsDetailsDayWise
    {
        public int Id { get; set; }
        public int DeviatedClientKpiid { get; set; }
        public string UserId { get; set; }
        public DateTime SentDate { get; set; }
        public string AlertType { get; set; }

        public DeviatedClientKpi DeviatedClientKpi { get; set; }
    }
}
