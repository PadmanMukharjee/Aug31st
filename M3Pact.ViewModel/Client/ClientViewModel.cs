using M3Pact.Infrastructure.Common;
using M3Pact.ViewModel.Admin;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel
{
    public class ClientViewModel
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
        public int WeeklyChecklist { get; set; }
        public int MonthlyChecklist { get; set; }
        public DateTime ContractStartDate { get; set; }
        public DateTime ContractEndDate { get; set; }
        public int NoticePeriod { get; set; }
        public List<string> SendAlertsUsers { get; set; }
        public AllUsersViewModel RelationShipManager { get; set; }
        public AllUsersViewModel BillingManager { get; set; }
        public IFormFile Contract { get; set; }
        public string ContractFilePath { get; set; }
        public string IsActive { get; set; }
        public BusinessUnitViewModel BusinessUnit { get; set; }
        public SpecialityViewModel Speciality { get; set; }
        public SystemViewModel System { get; set; }
        public KeyValueModel Site { get; set; }
        public ICollection<ClientPayerDepositViewModel> ClientPayer { get; set; }
        public bool IsNewClient { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ContractFileName { get; set; }

        public DateTime? WeeklyChecklistEffectiveDate { get; set; }
        public DateTime? MonthlyChecklistEffectiveDate { get; set; }

    }
}
