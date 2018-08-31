using M3Pact.BusinessModel;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Client;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Enums;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.ClientData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

namespace M3Pact.Repository.ClientData
{
    public class ClientDataRepository : IClientDataRepository
    {
        #region private Properties
        private IConfiguration _Configuration { get; }
        private M3PactContext _m3pactContext;        
        private readonly ICheckListRepository _checkListRepository;
        #endregion

        #region Constructor
        public ClientDataRepository(M3PactContext m3PactContext, IConfiguration configuration, ICheckListRepository checkListRepository)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
            _checkListRepository = checkListRepository;

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// To get the ClientData depending upon clientCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientDetails GetClientData(string clientCode)
        {
            ClientDetails clientDetails = new ClientDetails();
            try
            {
                DateTime utcTime = DateTime.UtcNow;
                clientDetails = _m3pactContext.Client
                    .Include(t => t.BusinessUnit)
                    .Include(t => t.Speciality)
                    .Include(t => t.System)
                    .Include(t => t.RelationShipManager)
                    .Include(t => t.BillingManager)
                    .Include(t => t.ClientCheckListMap)
                    .Where(r => r.ClientCode == clientCode)
                    .ToList()
                    .Select(g =>
                    {
                        DateTime? weeklyChecklistEffeciveDate = null;
                        DateTime? monthlyChecklistEffeciveDate = null;
                        int monthlyChecklist = 0;
                        int weeklyChecklist = 0;
                        g.ClientCheckListMap.Where(t => t.StartDate != t.EndDate && t.EndDate >= utcTime).ToList().ForEach(y =>
                         {

                             DomainModel.DomainModels.CheckList checklist = _m3pactContext.CheckList.Include(f => f.CheckListType).FirstOrDefault(n => n.CheckListId == y.CheckListId);
                             if (checklist.CheckListType.CheckListTypeCode == DomainConstants.MONTH)
                             {
                                 monthlyChecklist = checklist.CheckListId;
                                 monthlyChecklistEffeciveDate = y.EffectiveDate;
                             }
                             else if (checklist.CheckListType.CheckListTypeCode == DomainConstants.WEEK)
                             {
                                 weeklyChecklist = checklist.CheckListId;
                                 weeklyChecklistEffeciveDate = y.EffectiveDate;
                             }
                         });
                        return new BusinessModel.BusinessModels.ClientDetails()
                        {
                            Acronym = g.Acronym,
                            BusinessUnitDetails = new BusinessModel.BusinessModels.BusinessUnit()
                            {
                                BusinessUnitCode = g.BusinessUnit.BusinessUnitCode,
                                BusinessUnitName = g.BusinessUnit.BusinessUnitName,
                                ID = g.BusinessUnit.BusinessUnitId
                            },
                            ClientCode = g.ClientCode,
                            ContactEmail = g.ContactEmail,
                            ContactName = g.ContactName,
                            ContactPhone = g.ContactPhone,
                            ContractStartDate = g.ContractStartDate.Value,
                            ContractEndDate = g.ContractEndDate.Value,
                            FlatFee = g.FlatFee.HasValue ? g.FlatFee.Value : 0,
                            ContractFilePath = g.ContractFilePath,
                            Name = g.Name,
                            IsActive = g.IsActive,
                            NoticePeriod = g.NoticePeriod.HasValue ? g.NoticePeriod.Value : 0,
                            NumberOfProviders = g.NumberOfProviders.HasValue ? g.NumberOfProviders.Value : 0,
                            PercentageOfCash = g.PercentageOfCash.HasValue ? g.PercentageOfCash.Value : 0,
                            Speciality = new BusinessModel.BusinessModels.Speciality() { SpecialityCode = g.Speciality.SpecialityCode, SpecialityName = g.Speciality.SpecialityName, ID = g.Speciality.SpecialityId },
                            RelationShipManager = new BusinessModel.Admin.AllUsers() { Email = g.RelationShipManager?.Email },
                            BillingManager = new BusinessModel.Admin.AllUsers() { Email = g.BillingManager.Email },
                            System = new BusinessModel.Admin.System() { SystemCode = g.System.SystemCode, SystemName = g.System.SystemName, ID = g.System.SystemId },
                            Site = new KeyValueModel { Key = g.SiteId?.ToString() ?? "0", Value = string.Empty },
                            MonthlyCheckList = monthlyChecklist,
                            WeeklyCheckList = weeklyChecklist,
                            WeeklyChecklistEffectiveDate = weeklyChecklistEffeciveDate,
                            MonthlyChecklistEffectiveDate = monthlyChecklistEffeciveDate,
                            SendAlertsUsers = GetClientUsersForSendingAlerts(clientCode)
                        };
                    }).FirstOrDefault();
                return clientDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Repository method to get the client's monthly target data       
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<ClientTargetModel> GetClientTargetData(string clientCode, int year)
        {
            try
            {
                List<ClientTargetModel> clientTargetModels = (from ct in _m3pactContext.ClientTarget
                                                              join c in _m3pactContext.Client
                                                              on ct.ClientId equals c.ClientId
                                                              join m in _m3pactContext.Month
                                                              on ct.MonthId equals m.MonthId
                                                              where c.ClientCode == clientCode
                                                              && ct.CalendarYear == year
                                                              && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                              select new ClientTargetModel()
                                                              {
                                                                  CalendarYear = ct.CalendarYear,
                                                                  AnnualCharges = ct.AnnualCharges,
                                                                  GrossCollectionRate = ct.GrossCollectionRate,
                                                                  Charges = ct.Charges,
                                                                  Payments = ct.Payments,
                                                                  Revenue = ct.Revenue,
                                                                  IsManualEntry = ct.IsManualEntry,
                                                                  Month = new BusinessModel.BusinessModels.Month()
                                                                  {
                                                                      MonthName = m.MonthName
                                                                  },
                                                                  Client = new ClientDetails()
                                                                  {
                                                                      ClientCode = c.ClientCode
                                                                  },
                                                              }

                                                            ).ToList();
                return clientTargetModels;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Update and Save ClientData into database
        /// </summary>
        /// <param name="clientData"></param>
        public bool SaveClientData(ClientDetails clientData)
        {
            try
            {
                if (clientData.WeeklyCheckList == 0 || clientData.MonthlyCheckList == 0)
                {
                    return false;
                }

                BusinessModel.BusinessModels.CheckList monthlyChecklist = _checkListRepository.GetCheckListByIdAsync(clientData.MonthlyCheckList).Result;
                BusinessModel.BusinessModels.CheckList weeklyChecklist = _checkListRepository.GetCheckListByIdAsync(clientData.WeeklyCheckList).Result;

                if (monthlyChecklist == null || weeklyChecklist == null || monthlyChecklist.CheckListId == default(int) || weeklyChecklist.CheckListId == default(int))
                {
                    return false;
                }

                UserContext userContext = UserHelper.getUserContext();
                bool isRevenueChanged = false;
                Client existingClientDetails = (from c in _m3pactContext.Client
                                                join b in _m3pactContext.BusinessUnit
                                                on c.BusinessUnitId equals b.BusinessUnitId
                                                join s in _m3pactContext.Speciality
                                                on c.SpecialityId equals s.SpecialityId
                                                join sy in _m3pactContext.System
                                                on c.SystemId equals sy.SystemId
                                                where c.ClientCode == clientData.ClientCode
                                                select c).FirstOrDefault();
                DateTime currentDate = DateTime.Today.Date;
                M3Pact.DomainModel.DomainModels.CheckListType weeklyCheckListType = _m3pactContext.CheckListType.FirstOrDefault(t => t.CheckListTypeCode == DomainConstants.WEEK);
                M3Pact.DomainModel.DomainModels.CheckListType monthlyCheckListType = _m3pactContext.CheckListType.FirstOrDefault(t => t.CheckListTypeCode == DomainConstants.MONTH);

                if (existingClientDetails != null)
                {
                    if (existingClientDetails.PercentageOfCash != clientData.PercentageOfCash || existingClientDetails.FlatFee != clientData.FlatFee)
                    {
                        isRevenueChanged = true;
                    }
                    existingClientDetails.ClientCode = clientData.ClientCode;
                    existingClientDetails.Acronym = clientData.Acronym;
                    existingClientDetails.BusinessUnitId = _m3pactContext.BusinessUnit.Where(c => c.BusinessUnitCode == clientData.BusinessUnitDetails.BusinessUnitCode).First().BusinessUnitId;
                    existingClientDetails.ContactEmail = clientData.ContactEmail;
                    existingClientDetails.ContactName = clientData.ContactName;
                    existingClientDetails.ContactPhone = clientData.ContactPhone;
                    existingClientDetails.ContractStartDate = clientData.ContractStartDate;
                    existingClientDetails.ContractEndDate = clientData.ContractEndDate;
                    existingClientDetails.FlatFee = clientData.FlatFee;
                    existingClientDetails.Name = clientData.Name;
                    existingClientDetails.NoticePeriod = clientData.NoticePeriod;
                    existingClientDetails.NumberOfProviders = clientData.NumberOfProviders;
                    existingClientDetails.PercentageOfCash = clientData.PercentageOfCash;
                    existingClientDetails.ContractFilePath = clientData.ContractFilePath;
                    existingClientDetails.SpecialityId = _m3pactContext.Speciality.Where(c => c.SpecialityCode == clientData.Speciality.SpecialityCode && c.RecordStatus == DomainConstants.RecordStatusActive).First().SpecialityId;
                    existingClientDetails.SystemId = _m3pactContext.System.Where(c => c.SystemCode == clientData.System.SystemCode && c.RecordStatus == DomainConstants.RecordStatusActive).First().SystemId;
                    existingClientDetails.SiteId = Convert.ToInt32(clientData.Site.Key);
                    existingClientDetails.IsActive = clientData.IsActive;
                    existingClientDetails.CreatedBy = existingClientDetails.CreatedBy;
                    existingClientDetails.CreatedDate = existingClientDetails.CreatedDate;
                    existingClientDetails.ModifiedBy = userContext.UserId;
                    existingClientDetails.ModifiedDate = DateTime.Now;
                    existingClientDetails.RelationShipManagerId = _m3pactContext.UserLogin.Where(u => u.Email == clientData.RelationShipManager.Email && u.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().Id;  //Need to change the Managers and User
                    existingClientDetails.BillingManagerId = _m3pactContext.UserLogin.Where(u => u.Email == clientData.BillingManager.Email && u.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().Id;
                    AddOrUpdateSendAlertsUser(clientData);
                    List<ClientCheckListMap> dbchecklistMaps = new List<ClientCheckListMap>();
                    dbchecklistMaps = _m3pactContext.ClientCheckListMap.Include(r => r.CheckList).AsNoTracking().
                        Where(e => e.ClientId == existingClientDetails.ClientId &&
                        e.StartDate.Date != e.EndDate.Date &&
                        e.EndDate >= currentDate && e.StartDate <= currentDate).ToList();

                    //  Weekly Checklist updation scenarios

                    int assignedClientWeeklyChecklistId = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId && Convert.ToDateTime(t.EndDate) == DateTime.MaxValue.Date).Select(r => r.CheckListId).FirstOrDefault();
                    int currentClientWeeklyCheckListId = _m3pactContext.ClientCheckListMap.Include(r => r.CheckList).Include(r => r.CheckList.CheckListType).
                                                         Where(t => t.StartDate.Date != t.EndDate.Date && t.EffectiveDate < currentDate
                                                         && t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId && t.ClientId == existingClientDetails.ClientId)
                                                         .Select(g => g.CheckListId).FirstOrDefault();

                    WeeklyChecklistUpdateScenarios(assignedClientWeeklyChecklistId, currentClientWeeklyCheckListId, dbchecklistMaps, clientData, existingClientDetails, weeklyCheckListType, currentDate, userContext);

                    // Monthly Checklist updation scenarios.

                    int assignedClientMonthlyChecklistId = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId && Convert.ToDateTime(t.EndDate) == DateTime.MaxValue.Date).Select(r => r.CheckListId).FirstOrDefault();

                    int currentClientMonthlyCheckListId = _m3pactContext.ClientCheckListMap.Include(r => r.CheckList).Include(r => r.CheckList.CheckListType).
                    Where(t => t.StartDate.Date != t.EndDate.Date && t.EffectiveDate < currentDate
                    && t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId && t.ClientId == existingClientDetails.ClientId)
                   .Select(g => g.CheckListId).FirstOrDefault();

                    MonthlyChecklistUpdateScenarios(assignedClientMonthlyChecklistId, currentClientMonthlyCheckListId, dbchecklistMaps, clientData, existingClientDetails, monthlyCheckListType, currentDate, userContext);

                    _m3pactContext.Client.Update(existingClientDetails);

                    if (_m3pactContext.SaveChanges() > 0)
                    {
                        // Client KPI is updated when client checklist is updated.

                        if (assignedClientWeeklyChecklistId != clientData.WeeklyCheckList || assignedClientMonthlyChecklistId != clientData.MonthlyCheckList)
                        {
                            UpdateClientKPIsOnCheckListUpdate(existingClientDetails.ClientId, currentClientWeeklyCheckListId, clientData.WeeklyCheckList, assignedClientWeeklyChecklistId, DateHelper.GetMondayOfEffectiveWeek(DateTime.Today),
                                DateHelper.GetFirstDayOfEffectiveMonth(DateTime.Today), userContext.UserId, currentClientMonthlyCheckListId, clientData.MonthlyCheckList, assignedClientMonthlyChecklistId, DateTime.MaxValue.Date);
                        }
                    }
                    if (isRevenueChanged)
                    {
                        List<int?> calendarYears = (from c in _m3pactContext.Client
                                                    join ct in _m3pactContext.ClientTarget
                                                    on c.ClientId equals ct.ClientId
                                                    where c.ClientId == existingClientDetails.ClientId
                                                    && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                    select ct.CalendarYear)?.Distinct()?.ToList() ?? null;
                        if (calendarYears != null && calendarYears.Count > 0)
                        {
                            foreach (int year in calendarYears)
                            {
                                long? existingAnnualCharges = (from c in _m3pactContext.Client
                                                               join ct in _m3pactContext.ClientTarget
                                                               on c.ClientId equals ct.ClientId
                                                               where c.ClientId == existingClientDetails.ClientId
                                                               && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                               && ct.CalendarYear == year
                                                               select ct.AnnualCharges)?.Distinct()?.ToList()?.FirstOrDefault() ?? null;

                                decimal? existingGrossCollection = (from c in _m3pactContext.Client
                                                                    join ct in _m3pactContext.ClientTarget
                                                                    on c.ClientId equals ct.ClientId
                                                                    where c.ClientId == existingClientDetails.ClientId
                                                                    && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                                     && ct.CalendarYear == year
                                                                    select ct.GrossCollectionRate)?.Distinct()?.ToList()?.FirstOrDefault() ?? null;
                                ClientTargetModel clientTargetModel = new ClientTargetModel();
                                clientTargetModel.CalendarYear = year;
                                clientTargetModel.AnnualCharges = existingAnnualCharges;
                                clientTargetModel.GrossCollectionRate = existingGrossCollection;
                                clientTargetModel.Client = new ClientDetails() { ClientCode = existingClientDetails.ClientCode };
                                SaveClientTargetData(clientTargetModel);
                            }
                        }
                    }
                }
                else
                {
                    Client clientEntity = new Client();
                    clientEntity.ClientCode = clientData.ClientCode;
                    clientEntity.Acronym = clientData.Acronym;
                    clientEntity.BusinessUnitId = _m3pactContext.BusinessUnit.Where(c => c.BusinessUnitCode == clientData.BusinessUnitDetails.BusinessUnitCode && c.RecordStatus == DomainConstants.RecordStatusActive).First().BusinessUnitId;
                    clientEntity.ContactEmail = clientData.ContactEmail;
                    clientEntity.ContactName = clientData.ContactName;
                    clientEntity.ContactPhone = clientData.ContactPhone;
                    clientEntity.ContractStartDate = clientData.ContractStartDate;
                    clientEntity.ContractEndDate = clientData.ContractEndDate;
                    clientEntity.FlatFee = clientData.FlatFee;
                    clientEntity.Name = clientData.Name;
                    clientEntity.NoticePeriod = clientData.NoticePeriod;
                    clientEntity.NumberOfProviders = clientData.NumberOfProviders;
                    clientEntity.PercentageOfCash = clientData.PercentageOfCash;
                    clientEntity.ContractFilePath = clientData.ContractFilePath;
                    clientEntity.SpecialityId = _m3pactContext.Speciality.Where(c => c.SpecialityCode == clientData.Speciality.SpecialityCode && c.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault()?.SpecialityId;
                    clientEntity.SystemId = _m3pactContext.System.Where(c => c.SystemCode == clientData.System.SystemCode && c.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault()?.SystemId;
                    clientEntity.SiteId = Convert.ToInt32(clientData.Site.Key);
                    clientEntity.RelationShipManagerId = _m3pactContext.UserLogin.Where(u => u.Email == clientData.RelationShipManager.Email && u.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault()?.Id;
                    clientEntity.BillingManagerId = _m3pactContext.UserLogin.Where(u => u.Email == clientData.BillingManager.Email && u.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault()?.Id;
                    //clientEntity.WeeklyCheckListId
                    //clientEntity.MonthlyCheckListId
                    clientEntity.IsActive = clientData.IsActive;
                    clientEntity.CreatedBy = userContext.UserId;
                    clientEntity.CreatedDate = DateTime.Now;
                    clientEntity.ModifiedBy = userContext.UserId;
                    clientEntity.ModifiedDate = DateTime.Now;
                    clientEntity.RecordStatus = DomainConstants.RecordStatusActive;
                    _m3pactContext.Client.Add(clientEntity);
                    IQueryable<ClientCheckListMap> dbchecklistMaps = _m3pactContext.ClientCheckListMap.Include(r => r.CheckList).AsNoTracking().Where(e =>
                                          e.ClientId == clientEntity.ClientId &&
                                          e.StartDate != e.EndDate &&
                                          e.EndDate >= currentDate && e.StartDate <= currentDate);

                    if (!dbchecklistMaps.Any(t => t.CheckListId == clientData.WeeklyCheckList && t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId))
                    {
                        ClientCheckListMap checklistMap = new ClientCheckListMap()
                        {
                            CheckListId = clientData.WeeklyCheckList,
                            ClientId = clientEntity.ClientId,
                            StartDate = currentDate,
                            EndDate = DateTime.MaxValue.Date,
                            RecordStatus = DomainConstants.RecordStatusActive,
                            CreatedBy = userContext.UserId,
                            ModifiedBy = userContext.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        _m3pactContext.ClientCheckListMap.Add(checklistMap);

                        ClientCheckListMap existingMap = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId).FirstOrDefault();
                        if (existingMap != null)
                        {
                            existingMap.EndDate = currentDate;
                            existingMap.ModifiedDate = DateTime.Now;
                            existingMap.ModifiedBy = userContext.UserId;
                            _m3pactContext.ClientCheckListMap.Update(existingMap);
                        }
                    }

                    if (!dbchecklistMaps.Any(t => t.CheckListId == clientData.MonthlyCheckList && t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId))
                    {
                        ClientCheckListMap checklistMap = new ClientCheckListMap()
                        {
                            CheckListId = clientData.MonthlyCheckList,
                            ClientId = clientEntity.ClientId,
                            StartDate = currentDate,
                            EndDate = DateTime.MaxValue.Date,
                            RecordStatus = DomainConstants.RecordStatusActive,
                            CreatedBy = userContext.UserId,
                            ModifiedBy = userContext.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        _m3pactContext.ClientCheckListMap.Add(checklistMap);

                        ClientCheckListMap existingMap = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId).FirstOrDefault();
                        if (existingMap != null)
                        {
                            existingMap.EndDate = currentDate;
                            existingMap.ModifiedDate = DateTime.Now;
                            existingMap.ModifiedBy = userContext.UserId;
                            _m3pactContext.ClientCheckListMap.Update(existingMap);
                        }

                    }
                    if (_m3pactContext.SaveChanges() > 0)
                    {
                        AddUniversalKPIsToNewClient(clientEntity.ClientCode, userContext.UserId);
                        AddOrUpdateSendAlertsUser(clientData);
                        _m3pactContext.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Handling weekly checklist updation scenarios.
        /// </summary>
        /// <param name="dbchecklistMaps"></param>
        /// <param name="clientData"></param>
        /// <param name="existingClientDetails"></param>
        /// <param name="weeklyCheckListType"></param>
        /// <param name="currentDate"></param>
        /// <param name="userContext"></param>
        public void WeeklyChecklistUpdateScenarios(int assignedClientWeeklyChecklistId, int currentClientWeeklyCheckListId, List<ClientCheckListMap> dbchecklistMaps, ClientDetails clientData, Client existingClientDetails, M3Pact.DomainModel.DomainModels.CheckListType weeklyCheckListType, DateTime currentDate, UserContext userContext)
        {
            try
            {
                // Weekly CheckList
                // Checking if the client weekly checklist is updated or not.
                if (assignedClientWeeklyChecklistId != clientData.WeeklyCheckList)
                {
                    List<ClientCheckListMap> clientCheckListMapList = new List<ClientCheckListMap>();
                    clientCheckListMapList = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId).ToList();

                    // Checking if the current client checklist is same as the updated checklist.
                    if (currentClientWeeklyCheckListId == clientData.WeeklyCheckList)
                    {
                        foreach (ClientCheckListMap clientChecklistMap in clientCheckListMapList)
                        {
                            if (clientChecklistMap.StartDate != clientChecklistMap.EndDate && clientChecklistMap.EffectiveDate < currentDate)
                            {
                                clientChecklistMap.EndDate = DateTime.MaxValue.Date;
                                _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                            }
                            else
                            {
                                clientChecklistMap.EndDate = clientChecklistMap.StartDate;
                                clientChecklistMap.ModifiedDate = DateTime.Now;
                                clientChecklistMap.ModifiedBy = userContext.UserId;
                                _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                            }
                        }
                    }
                    else
                    {
                        ClientCheckListMap checklistMap = new ClientCheckListMap()
                        {
                            CheckListId = clientData.WeeklyCheckList,
                            ClientId = existingClientDetails.ClientId,
                            StartDate = currentDate,
                            EndDate = DateTime.MaxValue.Date,
                            EffectiveDate = DateHelper.GetMondayOfEffectiveWeek(DateTime.Today),
                            RecordStatus = DomainConstants.RecordStatusActive,
                            CreatedBy = userContext.UserId,
                            ModifiedBy = userContext.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        if (existingClientDetails.IsActive == DomainConstants.Pending)
                        {
                            checklistMap.EffectiveDate = null;
                        }
                        _m3pactContext.ClientCheckListMap.Add(checklistMap);
                        if (clientCheckListMapList.Count > 1)
                        {
                            foreach (ClientCheckListMap clientChecklistMap in clientCheckListMapList)
                            {
                                if (clientChecklistMap.EffectiveDate > currentDate)
                                {
                                    clientChecklistMap.EndDate = clientChecklistMap.StartDate;
                                    clientChecklistMap.ModifiedDate = DateTime.Now;
                                    clientChecklistMap.ModifiedBy = userContext.UserId;
                                    _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                                }
                            }
                        }
                        else
                        {
                            ClientCheckListMap existingMap = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == weeklyCheckListType.CheckListTypeId).FirstOrDefault();
                            if (existingMap != null)
                            {
                                if (currentClientWeeklyCheckListId == 0)
                                {
                                    existingMap.EndDate = existingMap.StartDate;
                                }
                                else
                                {
                                    existingMap.EndDate = DateHelper.GetMondayOfEffectiveWeek(DateTime.Today);
                                }
                                existingMap.ModifiedDate = DateTime.Now;
                                existingMap.ModifiedBy = userContext.UserId;
                                _m3pactContext.ClientCheckListMap.Update(existingMap);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Handling monthly checklist updation scenarios.
        /// </summary>
        /// <param name="dbchecklistMaps"></param>
        /// <param name="clientData"></param>
        /// <param name="existingClientDetails"></param>
        /// <param name="monthlyCheckListType"></param>
        /// <param name="currentDate"></param>
        /// <param name="userContext"></param>
        public void MonthlyChecklistUpdateScenarios(int assignedClientMonthlyChecklistId, int currentClientMonthlyCheckListId, List<ClientCheckListMap> dbchecklistMaps, ClientDetails clientData, Client existingClientDetails, M3Pact.DomainModel.DomainModels.CheckListType monthlyCheckListType, DateTime currentDate, UserContext userContext)
        {
            try
            {
                // Monthly CheckList
                // Checking if the client monthly checklist is updated or not.
                if (assignedClientMonthlyChecklistId != clientData.MonthlyCheckList)
                {
                    List<ClientCheckListMap> clientMonthlyCheckListMapList = new List<ClientCheckListMap>();
                    clientMonthlyCheckListMapList = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId).ToList();

                    // Checking if the current client checklist is same as the updated checklist.
                    if (currentClientMonthlyCheckListId == clientData.MonthlyCheckList)
                    {
                        foreach (ClientCheckListMap clientChecklistMap in clientMonthlyCheckListMapList)
                        {
                            if (clientChecklistMap.StartDate != clientChecklistMap.EndDate && clientChecklistMap.EffectiveDate < currentDate)
                            {
                                clientChecklistMap.EndDate = DateTime.MaxValue.Date;
                                _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                            }
                            else
                            {
                                clientChecklistMap.EndDate = clientChecklistMap.StartDate;
                                clientChecklistMap.ModifiedDate = DateTime.Now;
                                clientChecklistMap.ModifiedBy = userContext.UserId;
                                _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                            }
                        }
                    }
                    else
                    {
                        ClientCheckListMap checklistMap = new ClientCheckListMap()
                        {
                            CheckListId = clientData.MonthlyCheckList,
                            ClientId = existingClientDetails.ClientId,
                            StartDate = currentDate,
                            EndDate = DateTime.MaxValue.Date,
                            EffectiveDate = DateHelper.GetFirstDayOfEffectiveMonth(DateTime.Today),
                            RecordStatus = DomainConstants.RecordStatusActive,
                            CreatedBy = userContext.UserId,
                            ModifiedBy = userContext.UserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now
                        };
                        if (existingClientDetails.IsActive == DomainConstants.Pending)
                        {
                            checklistMap.EffectiveDate = null;
                        }
                        _m3pactContext.ClientCheckListMap.Add(checklistMap);
                        if (clientMonthlyCheckListMapList.Count > 1)
                        {
                            foreach (ClientCheckListMap clientChecklistMap in clientMonthlyCheckListMapList)
                            {
                                if (clientChecklistMap.EffectiveDate > currentDate)
                                {
                                    clientChecklistMap.EndDate = clientChecklistMap.StartDate;
                                    clientChecklistMap.ModifiedDate = DateTime.Now;
                                    clientChecklistMap.ModifiedBy = userContext.UserId;
                                    _m3pactContext.ClientCheckListMap.Update(clientChecklistMap);
                                }
                            }
                        }
                        else
                        {
                            ClientCheckListMap existingMap = dbchecklistMaps.Where(t => t.CheckList.CheckListTypeId == monthlyCheckListType.CheckListTypeId).FirstOrDefault();
                            if (existingMap != null)
                            {
                                if (currentClientMonthlyCheckListId == 0)
                                {
                                    existingMap.EndDate = existingMap.StartDate;
                                }
                                else
                                {
                                    existingMap.EndDate = DateHelper.GetFirstDayOfEffectiveMonth(DateTime.Today);
                                }
                                existingMap.ModifiedDate = DateTime.Now;
                                existingMap.ModifiedBy = userContext.UserId;
                                _m3pactContext.ClientCheckListMap.Update(existingMap);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Client KPI Effective date when client checklist is updated.
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="currentClientWeeklyCheckListId"></param>
        /// <param name="updatedClientWeeklyCheckListId"></param>
        /// <param name="assignedClientWeeklyCheckListId"></param>
        /// <param name="weeklyCheckListEffectiveDate"></param>
        /// <param name="monthlyCheckListEffectiveDate"></param>
        /// <param name="user"></param>
        /// <param name="currentClientMonthlyCheckListId"></param>
        /// <param name="updatedClientMonthlyCheckListId"></param>
        /// <param name="assignedClientMonthlyCheckListId"></param>
        /// <param name="maxDate"></param>
        /// <returns></returns>
        public bool UpdateClientKPIsOnCheckListUpdate(int clientId, int currentClientWeeklyCheckListId, int updatedClientWeeklyCheckListId, int assignedClientWeeklyCheckListId,
                                               DateTime weeklyCheckListEffectiveDate, DateTime monthlyCheckListEffectiveDate, string user, int currentClientMonthlyCheckListId,
                                                int updatedClientMonthlyCheckListId, int assignedClientMonthlyCheckListId, DateTime maxDate)
        {
            bool status = false;
            string result = null;
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.UpdateClientKPIEffectiveDateOnChecklistUpdate;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientId", clientId);
                        sqlCmd.Parameters.AddWithValue("@CurrentClientWeeklyCheckListId", currentClientWeeklyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@UpdatedClientWeeklyCheckListId", updatedClientWeeklyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@AssignedClientWeeklyCheckListId", assignedClientWeeklyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@WeeklyCheckListEffectiveDate", weeklyCheckListEffectiveDate);
                        sqlCmd.Parameters.AddWithValue("@MonthlyCheckListEffectiveDate", monthlyCheckListEffectiveDate);
                        sqlCmd.Parameters.AddWithValue("@User", user);
                        sqlCmd.Parameters.AddWithValue("@CurrentClientMonthlyCheckListId", currentClientMonthlyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@UpdatedClientMonthlyCheckListId", updatedClientMonthlyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@AssignedClientMonthlyCheckListId", assignedClientMonthlyCheckListId);
                        sqlCmd.Parameters.AddWithValue("@MaxDate", maxDate);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                result = (string)reader["Status"];
                            }
                        }
                        sqlConn.Close();
                    }
                }
                if (result == "SUCCESS")
                {
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To add the universal kpis to during new client creation.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool AddUniversalKPIsToNewClient(string clientCode, string user)
        {
            bool status = false;
            string result = null;
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.AddUniversalKPIsToNewClient;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@User", user);
                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                result = (string)dr["Status"];
                            }
                        }
                        sqlConn.Close();
                    }
                }
                if (result == "SUCCESS")
                {
                    status = true;
                }
                return status;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Repository method to save the client Target data through Quick method
        /// </summary>
        /// <param name="clientTarget"></param>
        /// <returns></returns>
        public List<ClientTargetModel> SaveClientTargetData(ClientTargetModel clientTarget)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = "SaveClientTargets";
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientTarget.Client.ClientCode);
                        sqlCmd.Parameters.AddWithValue("@CalendarYear", clientTarget.CalendarYear);
                        sqlCmd.Parameters.AddWithValue("@AnnualCharges", clientTarget.AnnualCharges);
                        sqlCmd.Parameters.AddWithValue("@GrossCollectionRate", clientTarget.GrossCollectionRate);
                        sqlCmd.Parameters.AddWithValue("@UserID", userContext.UserId);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        List<ClientTargetModel> clientTargets = new List<ClientTargetModel>();
                        if (reader.HasRows)
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ClientTargetModel ct = new ClientTargetModel();
                                    ct.AnnualCharges = Convert.ToInt64(reader["AnnualCharges"]);
                                    ct.CalendarYear = Convert.ToInt32(reader["CalendarYear"]);
                                    ct.Charges = Convert.ToInt64(reader["Charges"]);
                                    ct.Payments = Convert.ToInt64(reader["Payments"]);
                                    ct.Revenue = Convert.ToInt64(reader["Revenue"]);
                                    ct.Month = new BusinessModel.BusinessModels.Month();
                                    ct.Month.MonthId = Convert.ToInt32(reader["MonthID"]);
                                    ct.Month.MonthName = (from m in _m3pactContext.Month
                                                          where m.MonthId == ct.Month.MonthId
                                                          select m).FirstOrDefault().MonthName;
                                    ct.GrossCollectionRate = Convert.ToInt64(reader["GrossCollectionRate"]);
                                    clientTargets.Add(ct);

                                }
                                reader.NextResult();
                            }
                        }
                        return clientTargets;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Repository method to update the client Target data through Quick method
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public bool UpdateClientTargetData(int year)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = "UpdateClientTargets";
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@CalendarYear", year);
                        sqlCmd.Parameters.AddWithValue("@UserID", userContext.UserId);
                        sqlConn.Open();
                        sqlCmd.ExecuteNonQuery();
                        sqlConn.Close();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Client's All Steps' Status Details 
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<ClientStepStatusDetail> GetClientStepStausDetails(string clientCode)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                List<ClientStepStatusDetail> clientStepDetails = new List<ClientStepStatusDetail>();
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString)) //TODO: Use from GenericHelper
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetClientStepStatus, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@RoleCode", userContext.Role);
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        //TODO: Implement generic
                        while (dr.Read())
                        {
                            ClientStepStatusDetail clientStepDetail = new ClientStepStatusDetail();
                            if (dr["ClientConfigStepDetailID"] != DBNull.Value)
                            {
                                clientStepDetail.ID = Convert.ToInt32(dr["ClientConfigStepDetailID"]);
                            }

                            if (dr["ClientConfigStepID"] != DBNull.Value)
                            {
                                clientStepDetail.StepID = Convert.ToInt32(dr["ClientConfigStepID"]);
                            }

                            if (dr["ClientConfigStepStatusID"] != DBNull.Value)
                            {
                                clientStepDetail.StepStatusID = Convert.ToInt32(dr["ClientConfigStepStatusID"]);
                            }

                            if (dr["ClientConfigStepName"] != DBNull.Value)
                            {
                                clientStepDetail.StepName = dr["ClientConfigStepName"].ToString();
                            }

                            if (dr["CanView"] != DBNull.Value)
                            {
                                clientStepDetail.CanView = Convert.ToBoolean(dr["CanView"]);
                            }

                            if (dr["CanEdit"] != DBNull.Value)
                            {
                                clientStepDetail.CanEdit = Convert.ToBoolean(dr["CanEdit"]);
                            }
                            clientStepDetails.Add(clientStepDetail);
                        }
                    }
                    sqlConn.Close();
                }
                return clientStepDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Insert/Update row in ClientStepStatusDetail 
        /// </summary>
        /// <param name="clientStepDetailDTO"></param>
        /// <returns></returns>
        public bool SaveClientStepStatusDetail(ClientStepStatusDetail clientStepDetailDTO)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();

                bool isUpdate = false;
                ClientConfigStepDetail configStepDetailModel;
                ClientConfigStepDetail clientConfigStepDetail = (from CCSD in _m3pactContext.ClientConfigStepDetail
                                                                 join C in _m3pactContext.Client on CCSD.ClientId equals C.ClientId
                                                                 where C.ClientCode == clientStepDetailDTO.ClientCode && CCSD.ClientConfigStepId == clientStepDetailDTO.StepID
                                                                 select CCSD).FirstOrDefault();

                if (clientConfigStepDetail != null)
                {
                    isUpdate = true;
                    configStepDetailModel = clientConfigStepDetail;
                }
                else
                {
                    configStepDetailModel = new ClientConfigStepDetail();
                    configStepDetailModel.CreatedBy = userContext.UserId;
                    configStepDetailModel.CreatedDate = DateTime.UtcNow;
                    configStepDetailModel.RecordStatus = DomainConstants.RecordStatusActive;
                }

                configStepDetailModel.ClientConfigStepId = clientStepDetailDTO.StepID;
                configStepDetailModel.ClientConfigStepStatusId = clientStepDetailDTO.StepStatusID;
                configStepDetailModel.ClientId = _m3pactContext.Client.Where(c => c.ClientCode == clientStepDetailDTO.ClientCode).FirstOrDefault().ClientId;
                configStepDetailModel.ClientConfigStepId = clientStepDetailDTO.StepID;
                configStepDetailModel.ClientConfigStepId = clientStepDetailDTO.StepID;
                configStepDetailModel.ModifiedBy = userContext.UserId;
                configStepDetailModel.ModifiedDate = DateTime.UtcNow;

                if (isUpdate)
                {
                    _m3pactContext.Update(configStepDetailModel);
                }
                else
                {
                    _m3pactContext.ClientConfigStepDetail.Add(configStepDetailModel);
                }
                _m3pactContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Saving the Edited targets to Db by inactivating existing client target records
        /// </summary>
        /// <param name="manuallyEditedTargets"></param>
        public void SaveManuallyEditedTargetData(ManuallyEditedTargetsBusinessModel manuallyEditedTargets)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<ClientTarget> existingclientTargets = (from ct in _m3pactContext.ClientTarget
                                                            join c in _m3pactContext.Client
                                                            on ct.ClientId equals c.ClientId
                                                            join m in _m3pactContext.Month
                                                            on ct.MonthId equals m.MonthId
                                                            where c.ClientCode == manuallyEditedTargets.clientCode
                                                            && ct.CalendarYear == manuallyEditedTargets.year
                                                            && ct.RecordStatus == DomainConstants.RecordStatusActive
                                                            select ct).ToList();
                if (existingclientTargets != null && existingclientTargets.Count > 0)
                {
                    foreach (ClientTarget ct in existingclientTargets)
                    {
                        ct.RecordStatus = DomainConstants.RecordStatusInactive;
                        ct.ModifiedDate = DateTime.Now;
                        ct.ModifiedBy = userContext.UserId;
                    }
                }
                _m3pactContext.ClientTarget.UpdateRange(existingclientTargets);

                List<ClientTarget> clientTargets = new List<ClientTarget>();
                int clientid = (from c in _m3pactContext.Client
                                where c.ClientCode == manuallyEditedTargets.clientCode
                                select c.ClientId).ToList().FirstOrDefault();

                for (int i = 0; i < manuallyEditedTargets.charges.Count; i++)
                {
                    ClientTarget ct = new ClientTarget();
                    ct.ClientId = clientid;
                    ct.MonthId = GetMonthIdbyName(manuallyEditedTargets.charges[i].name);
                    ct.CalendarYear = manuallyEditedTargets.year;
                    ct.Charges = manuallyEditedTargets.charges[i].value;
                    ct.Payments = manuallyEditedTargets.payments[i].value;
                    ct.Revenue = manuallyEditedTargets.revenue[i].value;
                    ct.RecordStatus = DomainConstants.RecordStatusActive;
                    ct.AnnualCharges = existingclientTargets?[0]?.AnnualCharges ?? 0;
                    ct.GrossCollectionRate = existingclientTargets?[0]?.GrossCollectionRate ?? 0;
                    ct.CreatedBy = userContext.UserId;
                    ct.CreatedDate = DateTime.Now;
                    ct.ModifiedBy = userContext.UserId;
                    ct.ModifiedDate = DateTime.Now;
                    ct.IsManualEntry = false;
                    clientTargets.Add(ct);
                }
                _m3pactContext.ClientTarget.AddRange(clientTargets);
                _m3pactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the monthid based in name of month
        /// </summary>
        /// <param name="monthName"></param>
        /// <returns></returns>
        public int GetMonthIdbyName(string monthName)
        {
            try
            {
                DomainModel.DomainModels.Month month = (from m in _m3pactContext.Month
                                                        where m.MonthName.Contains(monthName)
                                                        && m.RecordStatus == DomainConstants.RecordStatusActive
                                                        select m
                                ).ToList().FirstOrDefault();
                return month.MonthId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the data of all client data.
        /// </summary>
        /// <returns></returns>
        public List<ClientsData> GetAllClientsData()
        {
            try
            {
                List<ClientsData> clientDataList = null;
                UserContext userContext = UserHelper.getUserContext();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetAllClientsData;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@UserId", userContext.UserId);
                        sqlCmd.Parameters.AddWithValue("@Month", DateTime.Today.Month);
                        sqlCmd.Parameters.AddWithValue("@Year", DateTime.Today.Year);
                        sqlCmd.Parameters.AddWithValue("@Role", userContext.Role);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            clientDataList = new List<ClientsData>();
                            while (reader.Read())
                            {
                                ClientsData client = new ClientsData();
                                client.ClientId = Convert.ToInt32(reader["ClientId"]);
                                client.ClientCode = Convert.ToString(reader["ClientCode"]);
                                client.ClientName = Convert.ToString(reader["ClientName"]);
                                client.Site = Convert.ToString(reader["Site"]);
                                client.BillingManager = Convert.ToString(reader["BillingManager"]);
                                client.RelationshipManager = Convert.ToString(reader["RelationshipManager"]);
                                if (reader["MTDDeposit"] != DBNull.Value)
                                {
                                    client.MTDDeposit = Convert.ToDecimal(reader["MTDDeposit"]);
                                }
                                if (reader["MTDTarget"] != DBNull.Value)
                                {
                                    client.MTDTarget = Convert.ToDecimal(reader["MTDTarget"]);
                                }
                                if (reader["ProjectedCash"] != DBNull.Value)
                                {
                                    client.ProjectedCash = Convert.ToDecimal(reader["ProjectedCash"]);
                                }
                                if (reader["MonthlyTarget"] != DBNull.Value)
                                {
                                    client.MonthlyTarget = Convert.ToDecimal(reader["MonthlyTarget"]);
                                }
                                if (reader["ActualM3Revenue"] != DBNull.Value)
                                {
                                    client.ActualM3Revenue = Convert.ToDecimal(reader["ActualM3Revenue"]);
                                }
                                if (reader["ForecastedM3Revenue"] != DBNull.Value)
                                {
                                    client.ForecastedM3Revenue = Convert.ToDecimal(reader["ForecastedM3Revenue"]);
                                }
                                client.Status = Convert.ToString(reader["Status"]);
                                clientDataList.Add(client);
                            }
                        }
                    }
                    sqlConn.Close();
                }
                return clientDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// To get the month status for the client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<BusinessModel.Client.ClientCloseMonth> GetClientCloseMonthData(string clientCode, int year)
        {
            try
            {
                return (from c in _m3pactContext.Client
                        join cm in _m3pactContext.ClientCloseMonth
                        on c.ClientId equals cm.ClientId
                        join m in _m3pactContext.Month
                        on cm.MonthId equals m.MonthId
                        where c.ClientCode == clientCode
                        && cm.Year == year
                        select new BusinessModel.Client.ClientCloseMonth()
                        {
                            ClientCode = c.ClientCode,
                            ClosedDate = cm.ClosedDate,
                            Month = new BusinessModel.BusinessModels.Month()
                            {
                                MonthCode = m.MonthCode,
                                MonthName = m.MonthName
                            },
                            MonthStatus = cm.MonthStatus,
                            ReOpenDate = cm.ReOpenDate,
                            Year = year
                        }
                          ).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Active Clients of A User
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ClientDetails> GetClientsByUser(string userID, string role)
        {
            try
            {
                List<ClientDetails> userClientList = new List<ClientDetails>();

                if (role == DomainConstants.Admin)
                {
                    return (from c in _m3pactContext.Client
                            where c.IsActive == DomainConstants.RecordStatusActive
                            select new ClientDetails()
                            {
                                ClientCode = c.ClientCode,
                                Name = c.Name
                            }
                    )?.OrderBy(d => d.Name)?.ToList();
                }
                else
                {
                    return (from c in _m3pactContext.Client
                            join uc in _m3pactContext.UserClientMap on
                            c.ClientId equals uc.ClientId
                            join ul in _m3pactContext.UserLogin on
                            uc.UserId equals ul.Id
                            where c.IsActive == DomainConstants.RecordStatusActive
                            && ul.RecordStatus == DomainConstants.RecordStatusActive
                            && uc.RecordStatus == DomainConstants.RecordStatusActive
                            && ul.UserId == userID
                            select new ClientDetails()
                            {
                                ClientCode = c.ClientCode,
                                Name = c.Name
                            }
                            )?.OrderBy(d => d.Name)?.ToList();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the user clients to show for logged in meridian user.
        /// </summary>
        /// <param name="clientsIDsOfLoggedInUser"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ClientDetails> GetUserClientsToShowForLoggedinMeridianUser(List<int> clientsIDsOfLoggedInUser, string userID)
        {
            try
            {
                List<Client> userClientList = new List<Client>();
                string roleCode = (from ul in _m3pactContext.UserLogin
                                   join r in _m3pactContext.Roles on ul.RoleId equals r.RoleId
                                   where ul.UserId == userID
                                   select r.RoleCode).ToList().FirstOrDefault();

                if (roleCode == RoleType.Admin.ToString())
                {
                    userClientList = _m3pactContext.Client.Include(c => c.System).Include(c => c.Speciality).Include(c => c.BusinessUnit).
                        Where(c => c.IsActive == DomainConstants.RecordStatusActive).ToList();
                }
                else
                {
                    userClientList = (from ucm in _m3pactContext.UserClientMap
                                      join ul in _m3pactContext.UserLogin on ucm.UserId equals ul.Id
                                      join c in _m3pactContext.Client on ucm.ClientId equals c.ClientId
                                      where ul.UserId == userID && clientsIDsOfLoggedInUser.Contains(ucm.ClientId)
                                      && c.IsActive == DomainConstants.RecordStatusActive && ucm.RecordStatus == DomainConstants.RecordStatusActive
                                      orderby c.Name
                                      select new Client
                                      {
                                          ClientCode = c.ClientCode,
                                          ClientId = c.ClientId,
                                          Name = c.Name,
                                          UserClientMap = c.UserClientMap,
                                          System = c.System,
                                          BusinessUnit = c.BusinessUnit,
                                          Speciality = c.Speciality
                                      }).ToList();

                }
                return ToClientDataList(userClientList);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Check if given clientCode exists in db or not
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public bool CheckForExistingClientCode(string clientCode)
        {
            try
            {
                bool isClientCodeExists = false;
                isClientCodeExists = (from C in _m3pactContext.Client
                                      where C.ClientCode == clientCode
                                      select C).Any();
                return isClientCodeExists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Activate A Client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="checklistEffectiveWeek"></param>
        /// <param name="checklistEffectiveMonth"></param>
        /// <returns></returns>
        public bool ActivateClient(string clientCode, DateTime checklistEffectiveWeek, DateTime checklistEffectiveMonth)
        {
            bool isSuccess = false;
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                Client clientData = _m3pactContext.Client.Where(c => c.ClientCode == clientCode).FirstOrDefault();
                if (clientData != null && clientData.IsActive == DomainConstants.Pending)
                {
                    isSuccess = UpdateChecklistEffectiveDatesWhenActivatingClient(clientData.ClientId, checklistEffectiveWeek, checklistEffectiveMonth);
                    if (isSuccess)
                    {
                        clientData.IsActive = DomainConstants.RecordStatusActive;
                        clientData.ModifiedBy = userContext.UserId;
                        clientData.ModifiedDate = DateTime.Now;
                        _m3pactContext.Client.Update(clientData);
                        _m3pactContext.SaveChanges();
                        isSuccess = true;
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Client Contract StartDate
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public DateTime GetClientContractStartDate(string clientCode)
        {
            try
            {
                DateTime contractStartDate = (from C in _m3pactContext.Client
                                              where C.ClientCode == clientCode
                                              select C.ContractStartDate).FirstOrDefault().Value;
                return contractStartDate.Date;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the history of the client based on start date and end date.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ClientHistory> GetClientHistory(string clientCode , DateTime startDate , DateTime endDate)
        {
            try
            {
                TimeSpan endDateTimeSpan = new TimeSpan(23, 59, 0);
                endDate = endDate.Date + endDateTimeSpan;
                List<ClientHistory> clientHistoryList = null;
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetClientHistoryData;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@StartDate", startDate);
                        sqlCmd.Parameters.AddWithValue("@EndDate", endDate);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            clientHistoryList = new List<ClientHistory>();
                            while (reader.Read())
                            {
                                ClientHistory clientHistory = new ClientHistory();
                                clientHistory.ModifiedOrAddedBy = Convert.ToString(reader["ModifiedBy"]);
                                clientHistory.ModifiedOrAddedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                                clientHistory.UpdatedOrAddedProperty = Convert.ToString(reader["ColumnProperty"]);
                                clientHistory.Action = Convert.ToString(reader["ActionName"]);
                                clientHistory.OldValue = Convert.ToString(reader["OldValue"]);
                                clientHistory.NewValue = Convert.ToString(reader["NewValue"]);
                                clientHistoryList.Add(clientHistory);
                            }
                        }
                    }
                }
                return clientHistoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the client created date and created user.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetClientCreationDetails(string clientCode)
        {
            try
            {
                Dictionary<string, object> clientDetails = new Dictionary<string, object>();
                Client client = _m3pactContext.Client.Where(c => c.ClientCode == clientCode).FirstOrDefault();
                DateTime clientFirstStepCreatedDate = _m3pactContext.ClientConfigStepDetail.Where(c => c.ClientId == client.ClientId && c.RecordStatus == DomainConstants.RecordStatusActive).OrderBy(c => c.CreatedDate).Select(c => c.CreatedDate).FirstOrDefault();
                if (client.IsActive != DomainConstants.RecordStatusPartial)
                {
                    clientDetails = new Dictionary<string, object>();
                    ClientConfigStepDetail clientConfigStepDetail = _m3pactContext.ClientConfigStepDetail.Where(c => c.ClientId == client.ClientId && c.RecordStatus == DomainConstants.RecordStatusActive).OrderByDescending(c => c.CreatedDate).FirstOrDefault();
                    string createdBy = _m3pactContext.UserLogin.Where(u => u.UserId == clientConfigStepDetail.CreatedBy).Select(u => u.LastName + ' ' + u.FirstName).FirstOrDefault();
                    clientDetails.Add(DomainConstants.CreatedDate, Convert.ToDateTime(clientConfigStepDetail.CreatedDate));
                    clientDetails.Add(DomainConstants.ClientFirstStepCreatedDate, Convert.ToDateTime(clientFirstStepCreatedDate));
                    clientDetails.Add(DomainConstants.CreatedBy, createdBy != null ? createdBy.ToString() : String.Empty);
                } 
                else
                {
                    clientDetails.Add(DomainConstants.ClientFirstStepCreatedDate, Convert.ToDateTime(clientFirstStepCreatedDate));
                }
                return clientDetails;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Update Checklist Effective dates when activating a client
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="checklistEffectiveWeek"></param>
        /// <param name="checklistEffectiveMonth"></param>
        /// <returns></returns>
        private bool UpdateChecklistEffectiveDatesWhenActivatingClient(int clientID, DateTime checklistEffectiveWeek, DateTime checklistEffectiveMonth)
        {
            bool isSuccess = false;
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<ClientCheckListMap> clientCheckListMaps = _m3pactContext.ClientCheckListMap
                                                                .Include(t => t.CheckList)
                                                                .Include(t => t.CheckList.CheckListType)
                                                                .Where(r => r.ClientId == clientID && r.EffectiveDate == null && r.StartDate != r.EndDate
                                                                    && r.StartDate <= DateTime.Today && r.EndDate >= DateTime.Today)
                                                                .ToList();

                if (clientCheckListMaps != null && clientCheckListMaps.Count > 0)
                {
                    List<ClientCheckListMap> clientCheckListMapData = new List<ClientCheckListMap>();

                    if (clientCheckListMaps.First().CheckList != null && clientCheckListMaps.First().CheckList.CheckListType != null)
                    {
                        ClientCheckListMap weeklyChecklist = clientCheckListMaps.Where(c => c.CheckList.CheckListType.CheckListTypeCode == DomainConstants.WEEK)?.FirstOrDefault();
                        if (weeklyChecklist != null)
                        {
                            weeklyChecklist.EffectiveDate = checklistEffectiveWeek;
                            weeklyChecklist.ModifiedDate = DateTime.Now;
                            weeklyChecklist.ModifiedBy = userContext.UserId;
                            clientCheckListMapData.Add(weeklyChecklist);
                        }

                        ClientCheckListMap monthlyChecklist = clientCheckListMaps.Where(c => c.CheckList.CheckListType.CheckListTypeCode == DomainConstants.MONTH)?.FirstOrDefault();
                        if (monthlyChecklist != null)
                        {
                            monthlyChecklist.EffectiveDate = checklistEffectiveMonth;
                            monthlyChecklist.ModifiedDate = DateTime.Now;
                            monthlyChecklist.ModifiedBy = userContext.UserId;
                            clientCheckListMapData.Add(monthlyChecklist);
                        }

                        _m3pactContext.ClientCheckListMap.UpdateRange(clientCheckListMapData);
                        _m3pactContext.SaveChanges();
                        isSuccess = true;
                    }
                }
                return isSuccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the client users for sending alerts
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        private List<string> GetClientUsersForSendingAlerts(string clientCode)
        {
            int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientCode).Select(c => c.ClientId).FirstOrDefault();
            List<string> userList = new List<string>();
            userList = _m3pactContext.ClientUserNoticeAlerts.Include(c => c.Client).Include(d => d.UserLogin)
                        .Where(c => c.ClientId == clientId && c.RecordStatus == DomainConstants.RecordStatusActive && c.UserLogin.RecordStatus == DomainConstants.RecordStatusActive)
                        .Select(c => c.UserLogin.Email).ToList();
            return userList;
        }

        /// <summary>
        /// Add or Update the users of send alerts.
        /// </summary>
        /// <param name="clientData"></param>
        private void AddOrUpdateSendAlertsUser(ClientDetails clientData)
        {
            if (clientData.SendAlertsUsers.Count > 0)
            {
                UserContext userContext = UserHelper.getUserContext();
                int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientData.ClientCode).Select(c => c.ClientId).FirstOrDefault();

                //All the Existing UserIds of a client
                List<ClientUserNoticeAlerts> existingClientUserNoticeAlerts = _m3pactContext.ClientUserNoticeAlerts.Include(DomainConstants.Client)
                                            .Where(c => c.Client.ClientId == clientId
                                                   && c.RecordStatus == DomainConstants.RecordStatusActive)?.ToList();

                List<int> existingClientUserNoticeAlertsIds = existingClientUserNoticeAlerts.Select(c => c.UserLoginId).ToList();

                //All the UserIds assigned
                List<int> userIds = _m3pactContext.UserLogin.Where(c => c.Email.Any(cu => clientData.SendAlertsUsers.Contains(c.Email))).Select(d => d.Id).ToList();

                //Newly Added UserIds
                IEnumerable<int> newlyAddedUserIds = userIds.Except(existingClientUserNoticeAlertsIds);
                IEnumerable<int> notChangedUserIds = userIds.Intersect(existingClientUserNoticeAlertsIds);
                if (notChangedUserIds != null)
                {
                    existingClientUserNoticeAlerts.Where(ec => !notChangedUserIds.Contains(ec.UserLoginId)).ToList()?
                                         .ForEach(c =>
                                         {
                                             c.RecordStatus = DomainConstants.RecordStatusInactive;
                                             c.ModifiedDate = DateTime.UtcNow;
                                             c.ModifiedBy = userContext.UserId;
                                         });
                    _m3pactContext.ClientUserNoticeAlerts.UpdateRange(existingClientUserNoticeAlerts);
                }
                foreach (int userId in newlyAddedUserIds)
                {
                    _m3pactContext.ClientUserNoticeAlerts.Add(new ClientUserNoticeAlerts()
                    {
                        ClientId = clientId,
                        UserLoginId = userId,
                        RecordStatus = DomainConstants.RecordStatusActive,
                        CreatedBy = userContext.UserId,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = userContext.UserId,
                        ModifiedDate = DateTime.Now,
                    });
                }
            }
        }

        #endregion Private Methods

        #region Mappers

        /// <summary>
        /// converts to Busieness.ClientsData from DomainModel.Client
        /// </summary>
        /// <param name="userClientList"></param>
        /// <returns></returns>
        private List<ClientDetails> ToClientDataList(List<Client> userClientList)
        {
            try
            {
                List<ClientDetails> clientsDataList = new List<ClientDetails>();
                foreach (Client client in userClientList)
                {
                    ClientDetails clientsData = new ClientDetails();

                    clientsData.ClientCode = client.ClientCode;
                    clientsData.Name = client.Name;
                    if (client.System != null)
                    {
                        clientsData.System = new BusinessModel.Admin.System();
                        clientsData.System.ID = client.System.SystemId;
                        clientsData.System.SystemCode = client.System.SystemCode;
                        clientsData.System.SystemName = client.System.SystemName;
                        clientsData.System.SystemDescription = client.System.SystemDescription;
                    }
                    if (client.BusinessUnit != null)
                    {
                        clientsData.BusinessUnitDetails = new BusinessModel.BusinessModels.BusinessUnit();
                        clientsData.BusinessUnitDetails.ID = client.BusinessUnit.BusinessUnitId;
                        clientsData.BusinessUnitDetails.BusinessUnitCode = client.BusinessUnit.BusinessUnitCode;
                        clientsData.BusinessUnitDetails.BusinessUnitName = client.BusinessUnit.BusinessUnitName;
                        clientsData.BusinessUnitDetails.BusinessUnitDescription = client.BusinessUnit.BusinessUnitDescription;
                    }
                    if (client.Speciality != null)
                    {
                        clientsData.Speciality = new BusinessModel.BusinessModels.Speciality();
                        clientsData.Speciality.ID = client.Speciality.SpecialityId;
                        clientsData.Speciality.SpecialityCode = client.Speciality.SpecialityCode;
                        clientsData.Speciality.SpecialityName = client.Speciality.SpecialityName;
                        clientsData.Speciality.SpecialityDescription = client.Speciality.SpecialityDescription;
                    }
                    clientsDataList.Add(clientsData);
                }
                return clientsDataList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Mappers
    }
}
