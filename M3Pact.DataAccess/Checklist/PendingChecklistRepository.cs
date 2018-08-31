using M3Pact.BusinessModel;
using M3Pact.BusinessModel.CheckList;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Checklist;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace M3Pact.Repository.Checklist
{
    public class PendingChecklistRepository : IPendingChecklistRepository
    {
        #region private Properties
        private IConfiguration _Configuration { get; }
        private M3PactContext _m3pactContext;
        private UserContext userContext;
        #endregion private Properties

        #region Constructor
        public PendingChecklistRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
            userContext = UserHelper.getUserContext();
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// To get the weekly pending checklists
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<DateTime> GetWeeklyPendingChecklist(string clientCode)
        {
            try
            {
                List<DateTime> PendingChecklist = new List<DateTime>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string procedure = DomainConstants.GetWeeklyPendingChecklist;
                    using (SqlCommand sqlCmd = new SqlCommand(procedure, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PendingChecklist.Add(Convert.ToDateTime(reader["Checklistdate"]));
                                }
                                reader.NextResult();
                            }
                        }
                    }
                }
                return PendingChecklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the monthly Pending checklists
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<DateTime> GetMonthlyPendingChecklist(string clientCode)
        {
            try
            {
                List<DateTime> PendingChecklist = new List<DateTime>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string procedure = DomainConstants.GetMonthlyPendingChecklist;
                    using (SqlCommand sqlCmd = new SqlCommand(procedure, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PendingChecklist.Add(Convert.ToDateTime(reader["Checklistdate"]));
                                }
                                reader.NextResult();
                            }
                        }
                    }
                }
                return PendingChecklist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the Weeklist Questions both monthly and weekly
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="pendingChecklistDate"></param>
        /// <param name="checklistType"></param>
        /// <returns></returns>
        public List<ClientChecklistResponseBusinessModel> GetWeeklyPendingChecklistQuestions(string clientCode, DateTime pendingChecklistDate, string checklistType)
        {
            try
            {
                List<ClientChecklistResponseBusinessModel> clientChecklistResponses = new List<ClientChecklistResponseBusinessModel>();
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, "M3PactConnection");
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string procedure = DomainConstants.GetPendingChecklists;
                    using (SqlCommand sqlCmd = new SqlCommand(procedure, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);
                        sqlCmd.Parameters.AddWithValue("@PendingDate", pendingChecklistDate.Date);
                        sqlCmd.Parameters.AddWithValue("@checklistType", checklistType);
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    ClientChecklistResponseBusinessModel clientChecklistResponse = new ClientChecklistResponseBusinessModel();
                                    clientChecklistResponse.ActualFreeForm = reader["ActualFreeForm"]?.ToString() ?? null;
                                    if (reader["ActualResponse"] != DBNull.Value)
                                    {
                                        clientChecklistResponse.ActualResponse = Convert.ToBoolean(reader["ActualResponse"]);
                                    }
                                    else
                                    {
                                        clientChecklistResponse.ActualResponse = null;
                                    }
                                    clientChecklistResponse.CheckListAttributeMapID = Convert.ToInt32(reader["CheckListAttributeMapID"]);
                                    clientChecklistResponse.ChecklistName = reader["ChecklistName"].ToString();
                                    clientChecklistResponse.ClientCheckListMapID = Convert.ToInt32(reader["ClientCheckListMapID"]);
                                    clientChecklistResponse.ExpectedRespone = Convert.ToBoolean(reader["ExpectedRespone"]);
                                    clientChecklistResponse.IsKPI = Convert.ToBoolean(reader["IsKPI"]);
                                    clientChecklistResponse.QuestionText = reader["QuestionText"].ToString();
                                    clientChecklistResponse.Questionid = Convert.ToInt32(reader["Questionid"]);
                                    clientChecklistResponse.RequireFreeform = Convert.ToBoolean(reader["RequireFreeform"]);
                                    clientChecklistResponse.QuestionCode = reader["QuestionCode"].ToString();
                                    clientChecklistResponses.Add(clientChecklistResponse);
                                }
                                reader.NextResult();
                            }
                        }
                    }
                    return clientChecklistResponses;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Save or Submit Checklist Response both weekly and monthly
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="pendingDate"></param>
        /// <param name="isSubmit"></param>
        /// <param name="clientChecklistResponse"></param>
        /// <returns></returns>
        public bool SaveOrSubmitChecklistResponse(string clientCode, DateTime pendingDate, bool isSubmit, List<ClientChecklistResponseBusinessModel> clientChecklistResponse)
        {
            try
            {
                int? clientChecklistMapId = clientChecklistResponse[0].ClientCheckListMapID;

                ClientCheckListStatusDetail clientCheckListStatusDetail = (from csd in _m3pactContext.ClientCheckListStatusDetail
                                                                           where csd.ClientCheckListMapId == clientChecklistMapId
                                                                           && csd.CheckListEffectiveDate == pendingDate
                                                                           select csd).Distinct().FirstOrDefault();
                int checklistStatusId = 0;
                string checklistStatus = string.Empty;
                if (clientCheckListStatusDetail != null)
                {
                    checklistStatusId = clientCheckListStatusDetail.ClientCheckListStatusDetailId;
                    checklistStatus = clientCheckListStatusDetail.ChecklistStatus;
                }

                if (checklistStatusId > 0 && checklistStatus != DomainConstants.Completed && checklistStatus != string.Empty)
                {
                    List<ClientCheckListQuestionResponse> clientCheckListQuestionResponses = (from cqr in _m3pactContext.ClientCheckListQuestionResponse
                                                                                              where cqr.ClientCheckListMapId == clientChecklistMapId
                                                                                              && cqr.ClientCheckListStatusDetailId == checklistStatusId
                                                                                              select cqr).ToList();
                    List<int> checklistAttributeMapids = clientChecklistResponse.Select(c => c.CheckListAttributeMapID ?? 0).ToList();
                    List<ClientCheckListQuestionResponse> newclientCheckListQuestionResponses = new List<ClientCheckListQuestionResponse>();

                    foreach (ClientChecklistResponseBusinessModel crb in clientChecklistResponse)
                    {
                        ClientCheckListQuestionResponse existingCQR = clientCheckListQuestionResponses.Where(c => c.CheckListAttributeMapId == crb.CheckListAttributeMapID).FirstOrDefault();
                        if (existingCQR != null)
                        {
                            existingCQR.ExpectedResponse = crb.ActualResponse;
                            existingCQR.FreeFormResponse = crb.ActualFreeForm;
                            existingCQR.ModifiedBy = userContext.UserId;
                            existingCQR.ModifiedDate = DateTime.Now;
                        }
                        else
                        {
                            ClientCheckListQuestionResponse ncqr = new ClientCheckListQuestionResponse();
                            ncqr.ClientCheckListStatusDetailId = checklistStatusId;
                            ncqr.ClientCheckListMapId = clientChecklistMapId ?? 0;
                            ncqr.CheckListAttributeMapId = crb.CheckListAttributeMapID ?? 0;
                            ncqr.ExpectedResponse = crb.ActualResponse;
                            ncqr.FreeFormResponse = crb.ActualFreeForm;
                            ncqr.RecordStatus = DomainConstants.RecordStatusActive;
                            ncqr.CreatedBy = userContext.UserId;
                            ncqr.CreatedDate = DateTime.Now;
                            ncqr.ModifiedBy = userContext.UserId;
                            ncqr.ModifiedDate = DateTime.Now;
                            newclientCheckListQuestionResponses.Add(ncqr);
                        }

                    }
                    if (isSubmit)
                    {
                        clientCheckListStatusDetail.ChecklistStatus = DomainConstants.Completed;
                        clientCheckListStatusDetail.ModifiedBy = userContext.UserId;
                        clientCheckListStatusDetail.ModifiedDate = DateTime.Now;
                        _m3pactContext.Update(clientCheckListStatusDetail);
                    }
                    _m3pactContext.UpdateRange(clientCheckListQuestionResponses);
                    _m3pactContext.AddRange(newclientCheckListQuestionResponses);
                    _m3pactContext.SaveChanges();
                }
                else if (checklistStatus == string.Empty)
                {
                    clientCheckListStatusDetail = new ClientCheckListStatusDetail();
                    clientCheckListStatusDetail.ChecklistStatus = isSubmit == true ? DomainConstants.Completed : DomainConstants.Pending;
                    clientCheckListStatusDetail.ClientCheckListMapId = clientChecklistMapId ?? 0;
                    clientCheckListStatusDetail.CheckListEffectiveDate = pendingDate;
                    clientCheckListStatusDetail.RecordStatus = DomainConstants.RecordStatusActive;
                    clientCheckListStatusDetail.CreatedDate = DateTime.Now;
                    clientCheckListStatusDetail.CreatedBy = userContext.UserId;
                    clientCheckListStatusDetail.ModifiedDate = DateTime.Now;
                    clientCheckListStatusDetail.ModifiedBy = userContext.UserId;

                    _m3pactContext.ClientCheckListStatusDetail.Add(clientCheckListStatusDetail);
                    _m3pactContext.SaveChanges();
                    int id = clientCheckListStatusDetail.ClientCheckListStatusDetailId;

                    List<ClientCheckListQuestionResponse> clientCheckListQuestionResponses = new List<ClientCheckListQuestionResponse>();
                    foreach (ClientChecklistResponseBusinessModel crb in clientChecklistResponse)
                    {
                        ClientCheckListQuestionResponse cqr = new ClientCheckListQuestionResponse();
                        cqr.ClientCheckListStatusDetailId = id;
                        cqr.ClientCheckListMapId = clientChecklistMapId ?? 0;
                        cqr.CheckListAttributeMapId = crb.CheckListAttributeMapID ?? 0;
                        cqr.ExpectedResponse = crb.ActualResponse;
                        cqr.FreeFormResponse = crb.ActualFreeForm;
                        cqr.RecordStatus = DomainConstants.RecordStatusActive;
                        cqr.CreatedBy = userContext.UserId;
                        cqr.CreatedDate = DateTime.Now;
                        cqr.ModifiedBy = userContext.UserId;
                        cqr.ModifiedDate = DateTime.Now;
                        clientCheckListQuestionResponses.Add(cqr);
                    }
                    _m3pactContext.ClientCheckListQuestionResponse.AddRange(clientCheckListQuestionResponses);
                    _m3pactContext.SaveChanges();
                }
                else
                {
                    return false;
                }
                if (isSubmit)
                {
                    int checklistTypeId = _m3pactContext.ClientCheckListMap.Where(clm => clm.ClientCheckListMapId == clientChecklistMapId).Select(clm => clm.CheckList.CheckListType.CheckListTypeId).FirstOrDefault();
                    InsertDeviatedKPI(clientCode, pendingDate, checklistTypeId, clientChecklistResponse);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get checklists for a date range
        /// </summary>
        /// <returns></returns>
        public List<ChecklistDataResponse> GetChecklistsForADateRange(ChecklistDataRequest checklistDataRequest)
        {
            try
            {
                List<ChecklistDataResponse> completedChecklistData = (from ccqr in _m3pactContext.ClientCheckListQuestionResponse
                                                                      join ccm in _m3pactContext.ClientCheckListMap on ccqr.ClientCheckListMapId equals ccm.ClientCheckListMapId
                                                                      join ccsd in _m3pactContext.ClientCheckListStatusDetail on ccqr.ClientCheckListStatusDetailId equals ccsd.ClientCheckListStatusDetailId
                                                                      join cam in _m3pactContext.CheckListAttributeMap on ccqr.CheckListAttributeMapId equals cam.CheckListAttributeMapId
                                                                      join q in _m3pactContext.Question on cam.CheckListAttributeValueId equals q.QuestionCode
                                                                      join c in _m3pactContext.Client on ccm.ClientId equals c.ClientId
                                                                      join cl in _m3pactContext.CheckList on ccm.CheckListId equals cl.CheckListId
                                                                      join ct in _m3pactContext.CheckListType on cl.CheckListTypeId equals ct.CheckListTypeId
                                                                      where c.ClientCode == checklistDataRequest.ClientCode
                                                                            && cam.CheckListAttributeId == DomainConstants.CheckListQuestionAttributeId
                                                                            && ct.CheckListTypeCode == checklistDataRequest.ChecklistType
                                                                            && ccsd.ChecklistStatus == DomainConstants.ChecklistCompleted
                                                                            && q.StartDate != q.EndDate
                                                                            && q.RecordStatus == DomainConstants.RecordStatusActive
                                                                      orderby cl.CheckListName, ccsd.CheckListEffectiveDate ascending
                                                                      select new ChecklistDataResponse()
                                                                      {
                                                                          EffectiveDate = ccsd.CheckListEffectiveDate.ToString("MM/dd/yyyy"),
                                                                          QuestionText = q.QuestionText,
                                                                          ChecklistName = cl.CheckListName,
                                                                          RowSelector = cl.CheckListId + "-" + q.QuestionId,
                                                                          QuestionEndDate = q.EndDate,
                                                                          QuestionStartDate = q.StartDate,
                                                                          QuestionEffectiveDate = q.EffectiveDate,
                                                                          Answer = new AnswerResponse
                                                                          {
                                                                              SubmittedResponse = ccqr.ExpectedResponse != null ? ccqr.ExpectedResponse == true ? BusinessConstants.YES : BusinessConstants.NO : null,
                                                                              FreeformResponse = ccqr.FreeFormResponse
                                                                          }
                                                                      }).Where(x => DateTime.Parse(x.EffectiveDate) >= checklistDataRequest.FromDate && DateTime.Parse(x.EffectiveDate) <= checklistDataRequest.ToDate
                                                                         && DateTime.Parse(x.EffectiveDate) <= DateTime.Today
                                                                         && DateTime.Parse(x.EffectiveDate) >= x.QuestionStartDate && DateTime.Parse(x.EffectiveDate) <= x.QuestionEndDate
                                                                         && DateTime.Parse(x.EffectiveDate) >= x.QuestionEffectiveDate
                                                                     ).ToList();
                return completedChecklistData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Open checklist
        /// </summary>
        /// <param name="checklistDataRequest"></param>
        /// <returns></returns>
        public bool OpenChecklist(ChecklistDataRequest checklistDataRequest)
        {
            try
            {
                ClientCheckListStatusDetail clientCheckListStatusDetail = (from csd in _m3pactContext.ClientCheckListStatusDetail
                                                                           join ccm in _m3pactContext.ClientCheckListMap on csd.ClientCheckListMapId equals ccm.ClientCheckListMapId
                                                                           join c in _m3pactContext.Client on ccm.ClientId equals c.ClientId
                                                                           join cl in _m3pactContext.CheckList on ccm.CheckListId equals cl.CheckListId
                                                                           join ct in _m3pactContext.CheckListType on cl.CheckListTypeId equals ct.CheckListTypeId
                                                                           where csd.CheckListEffectiveDate == checklistDataRequest.FromDate
                                                                                && c.ClientCode == checklistDataRequest.ClientCode
                                                                                && ct.CheckListTypeCode == checklistDataRequest.ChecklistType
                                                                           select csd).Distinct().FirstOrDefault();

                if (clientCheckListStatusDetail != null && clientCheckListStatusDetail.ClientCheckListStatusDetailId > 0
                    && clientCheckListStatusDetail.ChecklistStatus == DomainConstants.ChecklistCompleted)
                {
                    clientCheckListStatusDetail.ChecklistStatus = DomainConstants.ChecklistReopen;
                    clientCheckListStatusDetail.ModifiedDate = DateTime.Now;
                    clientCheckListStatusDetail.ModifiedBy = userContext.UserId;

                    _m3pactContext.Update(clientCheckListStatusDetail);
                    _m3pactContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Saving the deviated KPIs for each client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="pendingDate"></param>
        /// <param name="checklistTypeId"></param>
        /// <param name="clientChecklistResponse"></param>
        public void InsertDeviatedKPI(string clientCode, DateTime pendingDate, int checklistTypeId, List<ClientChecklistResponseBusinessModel> clientChecklistResponse)
        {
            try
            {
                DateTime Today = DateTime.Now.Date;
                int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientCode && c.IsActive == DomainConstants.RecordStatusActive).Select(c => c.ClientId).FirstOrDefault();
                //To Save heatmap scores
                SaveHeatMapScores(clientId, pendingDate, checklistTypeId, clientChecklistResponse);
                List<int> clientKpimaps = _m3pactContext.ClientKpimap.Where(ck => ck.ClientId == clientId && ck.StartDate <= pendingDate && ck.EndDate >= pendingDate).Select(ck => ck.Kpiid).ToList();
                List<Kpi> kpis = _m3pactContext.Kpi.Where(k => clientKpimaps.Contains(k.Kpiid)).ToList();
                List<string> questionCodes = kpis.Where(k => k.CheckListTypeId != 3).Select(k => k.Measure).ToList();
                clientChecklistResponse = clientChecklistResponse.Where(cr => questionCodes.Contains(cr.QuestionCode)).ToList();
                List<DeviatedClientKpi> deviatedClientKpis = new List<DeviatedClientKpi>();
                List<DeviatedClientKpi> existingDeviatedClientKPis = new List<DeviatedClientKpi>();
                foreach (ClientChecklistResponseBusinessModel crb in clientChecklistResponse)
                {
                    if (crb.IsKPI == true)
                    {
                        DeviatedClientKpi deviatedClientKpi = _m3pactContext.DeviatedClientKpi.Where(d => d.ClientId == clientId && d.QuestionCode == crb.QuestionCode
                            && d.CheckListDate == pendingDate.Date && d.RecordStatus == DomainConstants.RecordStatusActive
                         )?.FirstOrDefault() ?? null;
                        if (deviatedClientKpi != null)
                        {
                            if ((deviatedClientKpi.SubmittedDate == Today && crb.ActualResponse == crb.ExpectedRespone) || deviatedClientKpi.SubmittedDate != Today)
                            {
                                deviatedClientKpi.RecordStatus = DomainConstants.RecordStatusInactive;
                            }
                            existingDeviatedClientKPis.Add(deviatedClientKpi);
                        }

                        if (crb.ActualResponse != crb.ExpectedRespone && deviatedClientKpi == null)
                        {
                            DeviatedClientKpi newdeviatedClientKpi = new DeviatedClientKpi();
                            newdeviatedClientKpi.CheckListDate = pendingDate.Date;
                            newdeviatedClientKpi.SubmittedDate = DateTime.Now.Date;
                            newdeviatedClientKpi.QuestionCode = crb.QuestionCode;
                            newdeviatedClientKpi.ChecklistTypeId = checklistTypeId;
                            newdeviatedClientKpi.ClientId = clientId;
                            newdeviatedClientKpi.RecordStatus = DomainConstants.RecordStatusActive;
                            newdeviatedClientKpi.ActualResponse = crb.ActualResponse == true ? "Yes" : "No";
                            newdeviatedClientKpi.ExpectedResponse = crb.ExpectedRespone == true ? "Yes" : "No";
                            deviatedClientKpis.Add(newdeviatedClientKpi);
                        }

                    }
                }
                _m3pactContext.DeviatedClientKpi.UpdateRange(existingDeviatedClientKPis);
                _m3pactContext.DeviatedClientKpi.AddRange(deviatedClientKpis);
                _m3pactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Weekly/Monthly Effective Date
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="checklistTypeCode"></param>
        /// <returns></returns>
        public DateTime GetClientChecklistTypeEffectiveFrom(string clientCode, string checklistTypeCode)
        {
            try
            {
                DateTime? startDate = _m3pactContext.ClientCheckListMap
                                        .Include(t => t.CheckList)
                                        .Include(t => t.CheckList.CheckListType)
                                        .Include(t => t.Client)
                                        .Where(x => x.Client.ClientCode == clientCode && x.CheckList.CheckListType.CheckListTypeCode == checklistTypeCode
                                            && x.StartDate != x.EndDate)
                                        .OrderBy(x => x.EffectiveDate).FirstOrDefault()?.EffectiveDate;
                return startDate != null ? startDate.Value : DateTime.MaxValue;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Client ChecklistType Data ie., Weekly & Monthly Effective Dates
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<ChecklistDataRequest> GetClientChecklistTypeData(string clientCode)
        {
            try
            {
                List<ChecklistDataRequest> checklistTypeData = new List<ChecklistDataRequest>();
                ChecklistDataRequest weeklyChecklistData = new ChecklistDataRequest();
                weeklyChecklistData.ClientCode = clientCode;
                weeklyChecklistData.ChecklistType = DomainConstants.WEEK;
                weeklyChecklistData.FromDate = GetClientChecklistTypeEffectiveFrom(clientCode, weeklyChecklistData.ChecklistType);
                checklistTypeData.Add(weeklyChecklistData);

                ChecklistDataRequest monthlyChecklistData = new ChecklistDataRequest();
                monthlyChecklistData.ClientCode = clientCode;
                monthlyChecklistData.ChecklistType = DomainConstants.MONTH;
                monthlyChecklistData.FromDate = GetClientChecklistTypeEffectiveFrom(clientCode, monthlyChecklistData.ChecklistType);
                checklistTypeData.Add(monthlyChecklistData);

                return checklistTypeData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To save heatmap scores for Weekly and monthly checklists
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pendingDate"></param>
        /// <param name="checklistTypeId"></param>
        /// <param name="clientChecklistResponse"></param>
        public void SaveHeatMapScores(int clientId, DateTime pendingDate, int checklistTypeId, List<ClientChecklistResponseBusinessModel> clientChecklistResponse)
        {
            try
            {
                //Update if already scores are present in HeatMapItemScore table
                List<ClientHeatMapItemScore> heatMapScoresToUpdate = GetHeatMapScoresToUpdate(pendingDate, clientId, checklistTypeId);

                if (heatMapScoresToUpdate?.Count > 0)
                {
                    foreach (ClientHeatMapItemScore itemScore in heatMapScoresToUpdate)
                    {
                        ClientChecklistResponseBusinessModel question = clientChecklistResponse.Where(c => c.QuestionCode == itemScore.HeatMapItem.Kpi.Measure).FirstOrDefault();

                        if (question.ExpectedRespone != question.ActualResponse)
                        {
                            itemScore.Score = DomainConstants.HeatMapScore;
                        }
                        else
                        {
                            itemScore.Score = 0;
                        }
                        itemScore.ModifiedDate = DateTime.Now;
                        itemScore.ModifiedBy = userContext.UserId;
                        _m3pactContext.ClientHeatMapItemScore.Update(itemScore);

                    }
                    //Need to update data accordingly in ClientHeatMapRisk
                    UpdateClientChecklistRisk(clientId, pendingDate, checklistTypeId, heatMapScoresToUpdate);

                }
                else
                {
                    //To save Heatmap scores into HeatMapItemScore table
                    List<HeatMapItem> heatMapItems = GetHeatMapWithType(checklistTypeId);
                    if (heatMapItems?.Count > 0)
                    {
                        List<ClientHeatMapItemScore> heatMapItemsToSave = MapHeatMapItemScores(clientId, pendingDate, clientChecklistResponse, heatMapItems);
                        _m3pactContext.ClientHeatMapItemScore.AddRange(heatMapItemsToSave);
                        //Need to update data accordingly in ClientHeatMapRisk
                        SaveClientChecklistRisk(clientId, pendingDate, checklistTypeId, heatMapItemsToSave.Sum(c => c.Score).Value);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To map heatmapscores which need to inserted
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pendingDate"></param>
        /// <param name="clientChecklistResponse"></param>
        /// <param name="heatMapItems"></param>
        /// <returns></returns>
        public List<ClientHeatMapItemScore> MapHeatMapItemScores(int clientId, DateTime pendingDate, List<ClientChecklistResponseBusinessModel> clientChecklistResponse, List<HeatMapItem> heatMapItems)
        {
            try
            {
                List<ClientHeatMapItemScore> heatMapItemsToSave = new List<ClientHeatMapItemScore>();
                foreach (HeatMapItem item in heatMapItems)
                {
                    ClientChecklistResponseBusinessModel question = clientChecklistResponse.Where(c => c.QuestionCode == item.Kpi.Measure).FirstOrDefault();

                    if (question != null)
                    {
                        ClientHeatMapItemScore heatMapItemScore = new ClientHeatMapItemScore();
                        if (question.ExpectedRespone != question.ActualResponse)
                        {
                            heatMapItemScore.Score = DomainConstants.HeatMapScore;
                        }
                        else
                        {
                            heatMapItemScore.Score = 0;
                        }
                        heatMapItemScore.ClientId = clientId;
                        heatMapItemScore.HeatMapItemId = item.HeatMapItemId;
                        heatMapItemScore.HeatMapItemDate = pendingDate;
                        heatMapItemScore.RecordStatus = DomainConstants.RecordStatusActive;
                        heatMapItemScore.CreatedDate = DateTime.Now;
                        heatMapItemScore.CreatedBy = userContext.UserId;
                        heatMapItemScore.ModifiedDate = DateTime.Now;
                        heatMapItemScore.ModifiedBy = userContext.UserId;
                        heatMapItemsToSave.Add(heatMapItemScore);
                    }
                }

                return heatMapItemsToSave;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To save HeatMap Score on first submit
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pendingDate"></param>
        /// <param name="checklistTypeId"></param>
        /// <param name="newScore"></param>
        private void SaveClientChecklistRisk(int clientId, DateTime pendingDate, int checklistTypeId, int newScore)
        {
            try
            {
                string checklistType = _m3pactContext.CheckListType.Where(c => c.CheckListTypeId == checklistTypeId && c.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().CheckListTypeCode;
                ClientHeatMapRisk existingRiskScore = new ClientHeatMapRisk();
                ClientHeatMapRisk newRiskScore = new ClientHeatMapRisk();

                existingRiskScore = _m3pactContext.ClientHeatMapRisk.Where(c => c.ClientId == clientId).OrderByDescending(c => c.ClientHeatMapRiskId)?.FirstOrDefault();
                if (existingRiskScore != null)
                {
                    if (checklistType == DomainConstants.WEEK)
                    {
                        if (existingRiskScore.ChecklistWeeklyDate != null)
                        {
                            int existingWeeklyscore = GetScore(existingRiskScore.ClientHeatMapRiskId, DomainConstants.WEEK, "save").Value;

                            newRiskScore.RiskScore = (existingRiskScore.RiskScore - existingWeeklyscore) + newScore;
                        }
                        else
                        {
                            newRiskScore.RiskScore = existingRiskScore.RiskScore + newScore;
                        }
                        newRiskScore.ChecklistWeeklyDate = pendingDate.Date;
                        newRiskScore.ChecklistMonthlyDate = existingRiskScore.ChecklistMonthlyDate;
                    }
                    else if (checklistType == DomainConstants.MONTH)
                    {
                        if (existingRiskScore.ChecklistMonthlyDate != null)
                        {
                            int existingWeeklyscore = GetScore(existingRiskScore.ClientHeatMapRiskId, DomainConstants.MONTH, "Save").Value;

                            newRiskScore.RiskScore = (existingRiskScore.RiskScore - existingWeeklyscore) + newScore;
                        }
                        else
                        {
                            newRiskScore.RiskScore = existingRiskScore.RiskScore + newScore;
                        }
                        newRiskScore.ChecklistMonthlyDate = pendingDate.Date;
                        newRiskScore.ChecklistWeeklyDate = existingRiskScore.ChecklistWeeklyDate;
                    }

                    newRiskScore.M3dailyDate = existingRiskScore.M3dailyDate;
                    newRiskScore.M3weeklyDate = existingRiskScore.M3weeklyDate;
                    newRiskScore.M3monthlyDate = existingRiskScore.M3monthlyDate;
                }
                else
                {
                    if (checklistType == DomainConstants.WEEK)
                    {
                        newRiskScore.ChecklistWeeklyDate = pendingDate.Date;
                    }
                    else if (checklistType == DomainConstants.MONTH)
                    {
                        newRiskScore.ChecklistMonthlyDate = pendingDate.Date;
                    }
                    newRiskScore.RiskScore = newScore;
                }
                newRiskScore.ClientId = clientId;
                newRiskScore.CreatedBy = userContext.UserId;
                newRiskScore.ModifiedBy = userContext.UserId;
                newRiskScore.CreatedDate = DateTime.Now;
                newRiskScore.ModifiedDate = DateTime.Now;
                newRiskScore.EffectiveTime = DateTime.Now;
                newRiskScore.RecordStatus = DomainConstants.RecordStatusActive;
                _m3pactContext.ClientHeatMapRisk.Add(newRiskScore);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update existing ClientHeatMapRisk
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="pendingDate"></param>
        /// <param name="checklistTypeId"></param>
        private void UpdateClientChecklistRisk(int clientId, DateTime pendingDate, int checklistTypeId, List<ClientHeatMapItemScore> updatedClientHeatMapItemScore)
        {
            try
            {
                string checklistType = _m3pactContext.CheckListType.Where(c => c.CheckListTypeId == checklistTypeId && c.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().CheckListTypeCode;
                List<ClientHeatMapRisk> existingRiskScores = new List<ClientHeatMapRisk>();
                if (checklistType == DomainConstants.WEEK)
                {
                    existingRiskScores = _m3pactContext.ClientHeatMapRisk.Where(c => c.ClientId == clientId && c.ChecklistWeeklyDate == pendingDate.Date)?.ToList();
                }
                else if (checklistType == DomainConstants.MONTH)
                {
                    existingRiskScores = _m3pactContext.ClientHeatMapRisk.Where(c => c.ClientId == clientId && c.ChecklistMonthlyDate == pendingDate.Date).ToList();
                }

                if (existingRiskScores?.Count > 0)
                {

                    foreach (ClientHeatMapRisk heatMapRisk in existingRiskScores)
                    {
                        int riskScore = 0;
                        if (checklistType != DomainConstants.MONTH && heatMapRisk.ChecklistMonthlyDate != null)
                        {
                            riskScore += GetScore(heatMapRisk.ClientHeatMapRiskId, DomainConstants.MONTH).Value;
                        }
                        if (checklistType != DomainConstants.WEEK && heatMapRisk.ChecklistWeeklyDate != null)
                        {
                            riskScore += GetScore(heatMapRisk.ClientHeatMapRiskId, DomainConstants.WEEK).Value;
                        }
                        if (heatMapRisk.M3dailyDate != null)
                        {
                            riskScore += GetScore(heatMapRisk.ClientHeatMapRiskId, DomainConstants.M3).Value;
                        }

                        heatMapRisk.RiskScore = riskScore + updatedClientHeatMapItemScore.Where(c => heatMapRisk.EffectiveTime >= c.HeatMapItem.StartTime && heatMapRisk.EffectiveTime <= c.HeatMapItem.EndTime)?.Sum(c => c.Score);
                        heatMapRisk.ModifiedBy = userContext.UserId;
                        heatMapRisk.ModifiedDate = DateTime.Now;
                        _m3pactContext.ClientHeatMapRisk.Update(heatMapRisk);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To getScores Depending on ClientHeatMapRiskId and type
        /// </summary>
        /// <param name="ClientHeatMapRiskId"></param>
        /// <param name="checklistMonthlyDate"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public int? GetScore(int ClientHeatMapRiskId, string type, string action = DomainConstants.Update)
        {
            try
            {
                int checklistId = _m3pactContext.CheckListType.Where(c => c.CheckListTypeCode == type).FirstOrDefault().CheckListTypeId;
                switch (type)
                {
                    case DomainConstants.WEEK:
                        var weekHeatmapData = (from cr in _m3pactContext.ClientHeatMapRisk
                                               join hs in _m3pactContext.ClientHeatMapItemScore on new { date = cr.ChecklistWeeklyDate.Value, cr.ClientId } equals new { date = hs.HeatMapItemDate, hs.ClientId }
                                               join hi in _m3pactContext.HeatMapItem on hs.HeatMapItemId equals hi.HeatMapItemId
                                               join kpi in _m3pactContext.Kpi on hi.Kpiid equals kpi.Kpiid
                                               select new
                                               {
                                                   cr,
                                                   hs,
                                                   hi,
                                                   kpi
                                               })?.ToList().Where(c => c.cr.ClientHeatMapRiskId == ClientHeatMapRiskId && c.kpi.CheckListTypeId == checklistId
                                                                    && c.cr.RecordStatus == DomainConstants.RecordStatusActive);
                        //&& c.cr.EffectiveTime >= c.hi.StartTime && c.cr.EffectiveTime <= c.hi.EndTime);

                        if (action == DomainConstants.Update)
                        {
                            return weekHeatmapData?.Where(c => c.cr.EffectiveTime >= c.hi.StartTime && c.cr.EffectiveTime <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }
                        else
                        {
                            return weekHeatmapData?.Where(c => DateTime.Now >= c.hi.StartTime && DateTime.Now <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }

                    case DomainConstants.MONTH:
                        var monthHeatmapData = (from cr in _m3pactContext.ClientHeatMapRisk
                                                join hs in _m3pactContext.ClientHeatMapItemScore on new { date = cr.ChecklistMonthlyDate.Value, cr.ClientId } equals new { date = hs.HeatMapItemDate, hs.ClientId }
                                                join hi in _m3pactContext.HeatMapItem on hs.HeatMapItemId equals hi.HeatMapItemId
                                                join kpi in _m3pactContext.Kpi on hi.Kpiid equals kpi.Kpiid
                                                select new
                                                {
                                                    cr,
                                                    hs,
                                                    hi,
                                                    kpi
                                                })?.ToList().Where(c => c.cr.ClientHeatMapRiskId == ClientHeatMapRiskId && c.kpi.CheckListTypeId == checklistId
                                                                    && c.cr.RecordStatus == DomainConstants.RecordStatusActive);

                        if (action == DomainConstants.Update)
                        {
                            return monthHeatmapData?.Where(c => c.cr.EffectiveTime >= c.hi.StartTime && c.cr.EffectiveTime <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }
                        else
                        {
                            return monthHeatmapData?.Where(c => DateTime.Now >= c.hi.StartTime && DateTime.Now <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }

                    case DomainConstants.M3:
                        var m3dailyHeatmapData = (from cr in _m3pactContext.ClientHeatMapRisk
                                                  join hs in _m3pactContext.ClientHeatMapItemScore on new { date = cr.M3dailyDate.Value, cr.ClientId } equals new { date = hs.HeatMapItemDate, hs.ClientId }
                                                  join hi in _m3pactContext.HeatMapItem on hs.HeatMapItemId equals hi.HeatMapItemId
                                                  join k in _m3pactContext.Kpi on hi.Kpiid equals k.Kpiid
                                                  select new
                                                  {
                                                      cr,
                                                      hs,
                                                      hi,
                                                      k
                                                  })?.ToList().Where(c => c.cr.ClientHeatMapRiskId == ClientHeatMapRiskId && c.k.CheckListTypeId == checklistId
                                                                        && c.cr.RecordStatus == DomainConstants.RecordStatusActive);

                        if (action == DomainConstants.Update)
                        {
                            return m3dailyHeatmapData?.Where(c => c.cr.EffectiveTime >= c.hi.StartTime && c.cr.EffectiveTime <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }
                        else
                        {
                            return m3dailyHeatmapData?.Where(c => DateTime.Now >= c.hi.StartTime && DateTime.Now <= c.hi.EndTime)?.Sum(c => c.hs.Score);
                        }

                    default: return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To get already assigned scores for the submitted checklist
        /// </summary>
        /// <param name="submittedDate"></param>
        /// <param name="clientId"></param>
        /// <param name="checklistTypeId"></param>
        /// <returns></returns>
        public List<ClientHeatMapItemScore> GetHeatMapScoresToUpdate(DateTime submittedDate, int clientId, int checklistTypeId)
        {
            try
            {
                return _m3pactContext.ClientHeatMapItemScore.Include(c => c.HeatMapItem).Include(c => c.HeatMapItem.Kpi)
               .Where(c => c.HeatMapItemDate == submittedDate && c.ClientId == clientId && c.HeatMapItem.Kpi.CheckListTypeId == checklistTypeId && c.RecordStatus == DomainConstants.RecordStatusActive)?.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get HeatMaps depending on ChecklistTypes
        /// </summary>
        public List<HeatMapItem> GetHeatMapWithType(int checklistTypeId)
        {
            try
            {
                return _m3pactContext.HeatMapItem.Include(h => h.Kpi)
                                             .Where(h => h.RecordStatus == DomainConstants.RecordStatusActive && h.Kpi.CheckListTypeId == checklistTypeId
                                             && DateTime.Now >= h.StartTime && DateTime.Now <= h.EndTime).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To get last submitted checklist date
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public DateTime? GetLastSubmittedChecklist(int clientId, int typeId)
        {
            try
            {
                return (from c in _m3pactContext.Client
                        join clm in _m3pactContext.ClientCheckListMap
                        on c.ClientId equals clm.ClientId
                        join cl in _m3pactContext.CheckList
                        on clm.CheckListId equals cl.CheckListId
                        join cds in _m3pactContext.ClientCheckListStatusDetail
                        on clm.ClientCheckListMapId equals cds.ClientCheckListMapId
                        where
                        c.ClientId == clientId
                        && clm.RecordStatus == DomainConstants.RecordStatusActive
                        && cl.RecordStatus == DomainConstants.RecordStatusActive
                        && cds.RecordStatus == DomainConstants.RecordStatusActive
                        && cds.ChecklistStatus != DomainConstants.ChecklistPending
                        && cl.CheckListTypeId == typeId

                        select cds
                          ).OrderByDescending(c => c.ClientCheckListStatusDetailId)?.FirstOrDefault()?.CheckListEffectiveDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
