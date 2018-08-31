using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Client
    {
        public Client()
        {
            ClientCheckListMap = new HashSet<ClientCheckListMap>();
            ClientCloseMonth = new HashSet<ClientCloseMonth>();
            ClientConfig = new HashSet<ClientConfig>();
            ClientConfigStepDetail = new HashSet<ClientConfigStepDetail>();
            ClientHeatMapItemScore = new HashSet<ClientHeatMapItemScore>();
            ClientHeatMapRisk = new HashSet<ClientHeatMapRisk>();
            ClientKpimap = new HashSet<ClientKpimap>();
            ClientPayer = new HashSet<ClientPayer>();
            ClientTarget = new HashSet<ClientTarget>();
            ClientUserNoticeAlerts = new HashSet<ClientUserNoticeAlerts>();
            DepositLogMonthlyDetails = new HashSet<DepositLogMonthlyDetails>();
            DeviatedClientKpi = new HashSet<DeviatedClientKpi>();
            M3metricClientKpiDaily = new HashSet<M3metricClientKpiDaily>();
            UserClientMap = new HashSet<UserClientMap>();
        }

        public int ClientId { get; set; }
        public string ClientCode { get; set; }
        public string Name { get; set; }
        public string Acronym { get; set; }
        public int? SpecialityId { get; set; }
        public int? BusinessUnitId { get; set; }
        public int? SystemId { get; set; }
        public int? SiteId { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public decimal? PercentageOfCash { get; set; }
        public int? FlatFee { get; set; }
        public int? NumberOfProviders { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public int? NoticePeriod { get; set; }
        public int? RelationShipManagerId { get; set; }
        public int? BillingManagerId { get; set; }
        public int? WeeklyCheckListId { get; set; }
        public int? MonthlyCheckListId { get; set; }
        public string ContractFilePath { get; set; }
        public string IsActive { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public UserLogin BillingManager { get; set; }
        public BusinessUnit BusinessUnit { get; set; }
        public CheckList MonthlyCheckList { get; set; }
        public UserLogin RelationShipManager { get; set; }
        public Site Site { get; set; }
        public Speciality Speciality { get; set; }
        public System System { get; set; }
        public CheckList WeeklyCheckList { get; set; }
        public ICollection<ClientCheckListMap> ClientCheckListMap { get; set; }
        public ICollection<ClientCloseMonth> ClientCloseMonth { get; set; }
        public ICollection<ClientConfig> ClientConfig { get; set; }
        public ICollection<ClientConfigStepDetail> ClientConfigStepDetail { get; set; }
        public ICollection<ClientHeatMapItemScore> ClientHeatMapItemScore { get; set; }
        public ICollection<ClientHeatMapRisk> ClientHeatMapRisk { get; set; }
        public ICollection<ClientKpimap> ClientKpimap { get; set; }
        public ICollection<ClientPayer> ClientPayer { get; set; }
        public ICollection<ClientTarget> ClientTarget { get; set; }
        public ICollection<ClientUserNoticeAlerts> ClientUserNoticeAlerts { get; set; }
        public ICollection<DepositLogMonthlyDetails> DepositLogMonthlyDetails { get; set; }
        public ICollection<DeviatedClientKpi> DeviatedClientKpi { get; set; }
        public ICollection<M3metricClientKpiDaily> M3metricClientKpiDaily { get; set; }
        public ICollection<UserClientMap> UserClientMap { get; set; }
    }
}
