using M3Pact.BusinessModel.Admin;
using M3Pact.Infrastructure.Common;
using System;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class ClientDetails : BusinessDTO
    {
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public decimal PercentageOfCash { get; set; }
        public int FlatFee { get; set; }
        public int NumberOfProviders { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public string ContractFilePath { get; set; }
        public int NoticePeriod { get; set; }
        public List<string> SendAlertsUsers { get; set; }
        public AllUsers RelationShipManager { get; set; }
        public AllUsers BillingManager { get; set; }
        public BusinessUnit BusinessUnitDetails { get; set; }
        public int MonthlyCheckList { get; set; }
        public Speciality Speciality { get; set; }
        public BusinessModel.Admin.System System { get; set; }
        public KeyValueModel Site { get; set; }
        //public UserLogin User { get; set; }
        public int WeeklyCheckList { get; set; }
        public List<ClientPayer> ClientPayer { get; set; }
        public string IsActive { get; set; }
        public DateTime? WeeklyChecklistEffectiveDate { get; set; }
        public DateTime? MonthlyChecklistEffectiveDate { get; set; }

    }
}
