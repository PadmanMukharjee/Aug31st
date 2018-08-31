using M3Pact.BusinessModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.Client
{
    public class ClientKPIAssignedDetails
    {
        public String ClientCode { get; set; }
        public int ChecklistId { get; set; }
        public string ChecklistType { get; set; }
        public DateTime ChecklistStartDate { get; set; }
        public DateTime ChecklistEndDate { get; set; }
        public DateTime ChecklistEffectiveDate { get; set; }
        public DateTime ChecklistQuestionAssignedStartDate { get; set; }
        public DateTime ChecklistQuestionAssignedEndDate{ get; set; }
        public DateTime ChecklistQuestionEffectiveDate { get; set; }
        public int KPIID { get; set; }
        public string KPIDescription { get; set; }
        public bool IsKPI { get; set; }
        public bool IsUnivarsal { get; set; }
        public bool CompanyStandard { get; set; }
        //public bool ClientStandard { get; set; }
        public bool? IsSLA { get; set; }
        public string QuestionCode { get; set; }
        public DateTime QuestionStartDate { get; set; }
        public DateTime QuestionEndDate { get; set; }
        public DateTime QuestionEffectiveDate { get; set; }
        public int ClientKPIMapID { get; set; }
        public DateTime KPIAssignedStartDate { get; set; }
        public DateTime KPIAssignedEndDate { get; set; }
    }

    public class ClientKPIDetails
    {
        public ClientKPIDetails()
        {
            clientKPIAssignedDetails = new List<ClientKPIAssignedDetails>();
            clientKPIAssignedUserDetails = new List<AllUsers>();
        }
        public List<ClientKPIAssignedDetails> clientKPIAssignedDetails { get; set; }
        public List<AllUsers> clientKPIAssignedUserDetails { get; set; }
    }
}
