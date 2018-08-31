using M3Pact.BusinessModel.HeatMap;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Business.HeatMap;
using M3Pact.Infrastructure.Interfaces.Repository.HeatMap;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.HeatMap;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.HeatMap
{
    public class ClientsHeatMapBusiness : IClientsHeatMapBusiness
    {
        #region Internal Properties
        private IClientsHeatMapRepository _clientsHeatMapRepository;
        private IAllUsersBusiness _allUserBusiness;
        private ILogger _logger;
        #endregion Internal Properties

        #region Constructor
        public ClientsHeatMapBusiness(IClientsHeatMapRepository clientsHeatMapRepository, IAllUsersBusiness allUsersBusiness, ILogger logger)
        {
            _clientsHeatMapRepository = clientsHeatMapRepository;
            _allUserBusiness = allUsersBusiness;
            _logger = logger;
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// To get complete heat map filter data.
        /// </summary>
        /// <returns></returns>
        public HeatMapFiltersData GetHeatMapFilterData()
        {
            HeatMapFiltersData heatMapFiltersData = new HeatMapFiltersData();
            try
            {
                List<AllUsersViewModel> allUsersViewModel = _allUserBusiness.GetAllUsersDataAndClientsAssigned(true);
                List<string> userIds = new List<string>();
                heatMapFiltersData.allUsersViewModel = allUsersViewModel.FindAll(p => p.RecordStatus == DomainConstants.RecordStatusActive && p.SelectedClients.Count > 0);
                heatMapFiltersData.allUsersViewModel.ForEach(p => { userIds.Add(p.UserId); });
                heatMapFiltersData.systemList = GetSystems(userIds);
                heatMapFiltersData.businessUnitList = GetBusinessUnits(userIds);
                heatMapFiltersData.specialityList = GetSpecialities(userIds);
                heatMapFiltersData.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                heatMapFiltersData.Success = false;
                heatMapFiltersData.IsExceptionOccured = true;
                heatMapFiltersData.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return heatMapFiltersData;
        }



        /// <summary>
        /// Get Heat Map for clients
        /// </summary>
        /// <param name="heatMapRequest"></param>
        /// <returns></returns>
        public string GetClientsHeatMap(ClientsHeatMapRequestViewModel heatMapRequest)
        {
            String json = string.Empty;
            string heatMapItemsJson = string.Empty;
            string heatMapJson = string.Empty;
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                if (heatMapRequest != null && heatMapRequest.EmployeeID == null)
                {
                    heatMapRequest.EmployeeID = userContext.UserId;
                }
                ClientsHeatMapRequest request = BusinessMapper.MappingHeatMapViewModelToBusinessModel(heatMapRequest);
                HeatMapDetails heatMapDetails = _clientsHeatMapRepository.GetHeatMapForClients(request);
                if (heatMapDetails != null && heatMapDetails.ClientsHeatMapList != null && heatMapDetails.ClientsHeatMapList.Count > 0)
                {
                    ClientsHeatMapResponse[] clientsHeatMapItemsArray = heatMapDetails.ClientsHeatMapList.ToArray();
                    List<IDictionary<string, object>> pivotArray = clientsHeatMapItemsArray.ToPivotArray(item => item.HeatMapItemName,
                        item => item.ClientCode,
                        items => items.Any() ? items.First().HeatMapItemNameDetail : new HeatMapItemDetail());
                    foreach (IDictionary<string, object> ele in pivotArray)
                    {
                        int? riskFactorScore = null;
                        int? metricScore = null;
                        List<object> metricRiskFactors = ele.Where(x => x.Key != "ClientCode").Select(x => x.Value).ToList();
                        List<HeatMapItemDetail> heatmapItems = new List<HeatMapItemDetail>();
                        foreach (HeatMapItemDetail item in metricRiskFactors)
                        {
                            if (item.Type == "RiskFactor" && item.Score != null)
                            {
                                if (riskFactorScore == null)
                                {
                                    riskFactorScore = item.Score;
                                }
                                else
                                {
                                    riskFactorScore += item.Score;
                                }
                            }
                            if (item.Type == "Metric" && item.Score != null)
                            {
                                if (metricScore == null)
                                {
                                    metricScore = item.Score;
                                }
                                else
                                {
                                    metricScore += item.Score;
                                }
                            }
                        }

                        ClientsHeatMapResponse result = heatMapDetails.ClientsHeatMapList.First(c => c.ClientCode == (ele.ContainsKey("ClientCode") ? (string)ele["ClientCode"] : string.Empty));
                        if (result != null)
                        {
                            ele.Add("ChecklistMonthlyDate", result.ChecklistMonthlyDate != null ? result.ChecklistMonthlyDate.Value.ToString("MMM") : "N/A");
                            ele.Add("ChecklistWeeklyDate", result.ChecklistWeeklyDate != null ? result.ChecklistWeeklyDate.Value.ToString("dd MMM") : "N/A");
                            ele.Add("ClientName", result.ClientName);
                            ele.Add("LTM", result.Ltm);
                            ele.Add("MetricScore", metricScore);
                            ele.Add("RiskFactorScore", riskFactorScore);
                            ele.Add("Risk", result.Risk);
                            ele.Add("RiskPercent", result.RiskPercentage);
                            ele.Add("RiskPercentClass", result.RiskPercentage != null ? "risk_" + result.RiskPercentage : "");
                            ele.Add("SiteAcronym", result.SiteAcronym);
                            ele.Add("Specialty", result.Specialty);
                            ele.Add("BusinessUnitCode", result.BusinessUnitCode);
                            ele.Add("SystemCode", result.SystemCode);
                            string trendStatus = string.Empty;
                            if (result.Trend != null)
                            {
                                trendStatus = result.Trend == result.Risk ? "trend_equal" : (result.Trend < result.Risk ? "fa-caret-down red_Trend" : "fa-caret-up green_Trend");
                            }
                            ele.Add("Trend", trendStatus);
                        }
                    }
                    heatMapJson = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                }

                if (heatMapDetails != null && heatMapDetails.HeatMapItemTypeDetail != null && heatMapDetails.HeatMapItemTypeDetail.Count > 0)
                {
                    heatMapItemsJson = JsonConvert.SerializeObject(heatMapDetails.HeatMapItemTypeDetail);
                }
                json = JsonConvert.SerializeObject(new { heatmaps = heatMapJson, heatmapItems = heatMapItemsJson });
                return json;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }

        }

        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// To get the systems based on list of user-ids.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private List<BusinessModel.Admin.System> GetSystems(List<string> userIds)
        {
            List<BusinessModel.Admin.System> businessSystemList = null;
            List<DomainModel.DomainModels.System> repositorySystemList = _clientsHeatMapRepository.GetClientSystemsOfUser(userIds);
            if (repositorySystemList != null)
            {
                businessSystemList = new List<BusinessModel.Admin.System>();
                foreach (DomainModel.DomainModels.System system in repositorySystemList)
                {
                    BusinessModel.Admin.System businessSystem = new BusinessModel.Admin.System();
                    businessSystem.ID = system.SystemId;
                    businessSystem.SystemCode = system.SystemCode;
                    businessSystem.SystemName = system.SystemName;
                    businessSystem.SystemDescription = system.SystemDescription;
                    businessSystemList.Add(businessSystem);
                }
            }
            return businessSystemList;
        }

        /// <summary>
        /// To get the business units based on list of user-ids.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private List<BusinessModel.BusinessModels.BusinessUnit> GetBusinessUnits(List<string> userIds)
        {
            List<BusinessModel.BusinessModels.BusinessUnit> businessUnitListBusiness = null;
            List<BusinessUnit> repositoryBusinessUnitList = _clientsHeatMapRepository.GetClientBusinessUnitsOfUser(userIds);
            if (repositoryBusinessUnitList != null)
            {
                businessUnitListBusiness = new List<BusinessModel.BusinessModels.BusinessUnit>();
                foreach (BusinessUnit businessUnitRepository in repositoryBusinessUnitList)
                {
                    BusinessModel.BusinessModels.BusinessUnit businessUnit = new BusinessModel.BusinessModels.BusinessUnit();
                    businessUnit.ID = businessUnitRepository.BusinessUnitId;
                    businessUnit.BusinessUnitCode = businessUnitRepository.BusinessUnitCode;
                    businessUnit.BusinessUnitName = businessUnitRepository.BusinessUnitName;
                    businessUnit.BusinessUnitDescription = businessUnitRepository.BusinessUnitDescription;
                    businessUnitListBusiness.Add(businessUnit);
                }
            }
            return businessUnitListBusiness;
        }


        /// <summary>
        /// To get the specialities based on list of user-ids.
        /// </summary>
        /// <param name="userIds"></param>
        /// <returns></returns>
        private List<BusinessModel.BusinessModels.Speciality> GetSpecialities(List<string> userIds)
        {
            List<BusinessModel.BusinessModels.Speciality> specialityListBusiness = null;
            List<Speciality> repositorySpecialityList = _clientsHeatMapRepository.GetClientSpecialitiesOfUser(userIds);
            if (repositorySpecialityList != null)
            {
                specialityListBusiness = new List<BusinessModel.BusinessModels.Speciality>();
                foreach (Speciality specialityRepository in repositorySpecialityList)
                {
                    BusinessModel.BusinessModels.Speciality speciality = new BusinessModel.BusinessModels.Speciality();
                    speciality.ID = specialityRepository.SpecialityId;
                    speciality.SpecialityCode = specialityRepository.SpecialityCode;
                    speciality.SpecialityName = specialityRepository.SpecialityName;
                    speciality.SpecialityDescription = specialityRepository.SpecialityDescription;
                    specialityListBusiness.Add(speciality);
                }
            }
            return specialityListBusiness;
        }

        #endregion Private Methods

        #region Mapper Methods
        public ClientsHeatMapRequest ConstructModelFromClientsHeatMapRequestViewModel(ClientsHeatMapRequestViewModel requestViewModel)
        {
            ClientsHeatMapRequest request = new ClientsHeatMapRequest();
            if (requestViewModel != null)
            {
                request.BusinessUnitCode = requestViewModel.BusinessUnitCode;
                request.SpecialtyCode = requestViewModel.SpecialtyCode;
                request.SystemCode = requestViewModel.SystemCode;
                request.EmployeeID = requestViewModel.EmployeeID;
            }
            return request;
        }
        #endregion Mapper Methods

    }
}
