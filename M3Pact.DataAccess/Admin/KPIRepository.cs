using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.Client;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class KPIRepository : IKPIRepository
    {
        #region private Properties
        private M3PactContext _m3pactContext;
        private IConfiguration _Configuration { get; }
        #endregion private Properties

        #region Constructor
        public KPIRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
        }
        #endregion Constructor

        #region public Methods

        /// <summary>
        /// Getting checkListTypes
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.BusinessModels.CheckListType> GetCheckListTypes()
        {
            try
            {
                List<BusinessModel.BusinessModels.CheckListType> checkListTypeDto = new List<BusinessModel.BusinessModels.CheckListType>();
                IEnumerable<DomainModel.DomainModels.CheckListType> checklistTypes = _m3pactContext.CheckListType.Where(x => x.RecordStatus == DomainConstants.RecordStatusActive);

                if (checklistTypes != null && checklistTypes.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.CheckListType item in checklistTypes)
                    {
                        BusinessModel.BusinessModels.CheckListType checklistType = new BusinessModel.BusinessModels.CheckListType();
                        checklistType.CheckListTypeID = item.CheckListTypeId;
                        checklistType.CheckListTypeCode = item.CheckListTypeCode;
                        checklistType.CheckListTypeName = item.CheckListTypeName;
                        checklistType.RecordStatus = item.RecordStatus;
                        checkListTypeDto.Add(checklistType);
                    }
                }
                return checkListTypeDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// getting KPI question based on checkListType
        /// </summary>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public IEnumerable<BusinessModel.BusinessModels.Question> GetKPIQuestionBasedOnCheckListType(string checkListTypeCode)
        {
            try
            {
                int checkListTypeId = _m3pactContext.CheckListType.Where(c => c.CheckListTypeCode == checkListTypeCode && c.RecordStatus == DomainConstants.RecordStatusActive).Select(c => c.CheckListTypeId).FirstOrDefault();
                List<BusinessModel.BusinessModels.Question> questionsDto = new List<BusinessModel.BusinessModels.Question>();

                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    string sql = DomainConstants.GetKPIQuestionBasedOnCheckListType;
                    using (SqlCommand sqlCmd = new SqlCommand(sql, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@CheckListTypeId", checkListTypeId);

                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                BusinessModel.BusinessModels.Question question = new BusinessModel.BusinessModels.Question();
                                question.QuestionId = (int)dr["QuestionID"];
                                question.QuestionText = (string)dr["QuestionText"];
                                question.ExpectedResponse = (bool)dr["ExpectedRespone"];
                                question.IsKpi = (bool)dr["IsKPI"];
                                question.IsUniversal = (bool)dr["IsUniversal"];
                                question.IsFreeform = (bool)dr["RequireFreeform"];
                                questionsDto.Add(question);
                            }
                        }
                    }
                    sqlConn.Close();
                }

                return questionsDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saving KPI Details
        /// </summary>
        /// <param name="kpi"></param>
        /// <returns></returns>
        public bool SaveKPIs(KPI kpiDto, bool? isQuestionUniversal = null, bool? isOldQuestion = null, bool? existingKPI = null)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                bool isUpdate = false;
                bool? existingKpiUniversal = null;
                Kpi kpiModel;
                Kpi kpi = _m3pactContext.Kpi.FirstOrDefault(x => x.Kpiid == kpiDto.KPIID);
                if (kpi != null)
                {
                    kpiModel = kpi;
                    isUpdate = true;
                    existingKpiUniversal = kpi.IsUniversal;
                    Kpialert kpiAlertModel = _m3pactContext.Kpialert.FirstOrDefault(x => x.Kpiid == kpi.Kpiid);
                    kpiAlertModel.SendAlert = kpiDto.KPIAlert.SendAlert;
                    kpiAlertModel.SendAlertTitle = kpiDto.KPIAlert.SendAlertTitle;
                    kpiAlertModel.SendToRelationshipManager = kpiDto.KPIAlert.SendToRelationshipManager;
                    kpiAlertModel.SendToBillingManager = kpiDto.KPIAlert.SendToBillingManager;
                    kpiAlertModel.EscalateAlert = kpiDto.KPIAlert.EscalateAlert;
                    kpiAlertModel.EscalateAlertTitle = kpiDto.KPIAlert.EscalateAlertTitle;
                    kpiAlertModel.EscalateTriggerTime = kpiDto.KPIAlert.EscalateTriggerTime;
                    kpiAlertModel.IncludeKpitarget = kpiDto.KPIAlert.IncludeKPITarget;
                    kpiAlertModel.IncludeDeviationTarget = kpiDto.KPIAlert.IncludeDeviationTarget;
                    kpiAlertModel.IsSla = kpiDto.KPIAlert.IsSla;
                    kpiAlertModel.RecordStatus = DomainConstants.RecordStatusActive;
                    kpiAlertModel.ModifiedBy = userContext.UserId;
                    kpiAlertModel.ModifiedDate = DateTime.Now;
                    _m3pactContext.Update(kpiAlertModel);
                }
                else
                {
                    kpiModel = new Kpi();
                    kpiModel.CreatedDate = DateTime.Now;
                    kpiModel.CreatedBy = userContext.UserId;
                    kpiModel.Kpialert.Add(new Kpialert()
                    {
                        SendAlert = kpiDto.KPIAlert.SendAlert,
                        SendAlertTitle = kpiDto.KPIAlert.SendAlertTitle,
                        SendToRelationshipManager = kpiDto.KPIAlert.SendToRelationshipManager,
                        SendToBillingManager = kpiDto.KPIAlert.SendToBillingManager,
                        EscalateAlert = kpiDto.KPIAlert.EscalateAlert,
                        EscalateAlertTitle = kpiDto.KPIAlert.EscalateAlertTitle,
                        EscalateTriggerTime = kpiDto.KPIAlert.EscalateTriggerTime,
                        IncludeKpitarget = kpiDto.KPIAlert.IncludeKPITarget,
                        IncludeDeviationTarget = kpiDto.KPIAlert.IncludeDeviationTarget,
                        IsSla = kpiDto.KPIAlert.IsSla,
                        RecordStatus = DomainConstants.RecordStatusActive,
                        CreatedBy = userContext.UserId,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = userContext.UserId,
                        ModifiedDate = DateTime.Now
                    });
                }
                kpiModel.ModifiedDate = DateTime.Now;
                kpiModel.ModifiedBy = userContext.UserId;
                kpiModel.Kpidescription = kpiDto.KPIDescription;
                kpiModel.CheckListTypeId = kpiDto.Source.CheckListTypeID;
                kpiModel.Standard = kpiDto.Standard;
                kpiModel.AlertLevel = kpiDto.AlertLevel;
                kpiModel.IsHeatMapItem = kpiDto.IsHeatMapItem;
                kpiModel.HeatMapScore = kpiDto.HeatMapScore;
                kpiModel.IsUniversal = kpiDto.IsUniversal;
                kpiModel.RecordStatus = DomainConstants.RecordStatusActive;
                if (kpiModel.CheckListTypeId == DomainConstants.CheckListTypeId)
                {
                    kpiModel.Measure = kpiDto.Measure.MeasureId.ToString();
                    kpiModel.KpimeasureId = kpiDto.KPIMeasure.KpimeasureId;
                }
                else
                {
                    kpiModel.Measure = kpiDto.Measure.MeasureCode;
                    kpiModel.KpimeasureId = GetMeasureBasedOnClientTypeID(kpiModel.CheckListTypeId).FirstOrDefault().KpimeasureId;
                }
                if (isUpdate)
                {

                    if ((isOldQuestion == true && isQuestionUniversal == true) || (existingKpiUniversal != null && !existingKpiUniversal.Value && kpiModel.IsUniversal != null && kpiModel.IsUniversal.Value))
                    {
                        List<int> mappedClientIdsWithMaxDate = _m3pactContext.ClientKpimap.Where(k => k.Kpiid == kpi.Kpiid && k.StartDate <= DateTime.Now && k.EndDate == DateTime.MaxValue.Date).Select(k => k.ClientId)?.ToList();

                        List<ClientKpimap> mappedClientKpiMapIds = _m3pactContext.ClientKpimap.Where(k => k.Kpiid == kpi.Kpiid && k.StartDate <= DateTime.Now && k.EndDate != DateTime.MaxValue.Date)?.ToList();

                        List<int> mappedClientIdsWithLessthanMaxDate = mappedClientKpiMapIds.Select(k => k.ClientId)?.ToList();

                        List<int> clientIds = _m3pactContext.Client.Select(c => c.ClientId).ToList();

                        List<int> newClientIdsToMap = clientIds.Except(mappedClientIdsWithLessthanMaxDate).Except(mappedClientIdsWithMaxDate)?.ToList();

                        mappedClientKpiMapIds.ForEach(k => { k.EndDate = DateTime.MaxValue.Date; k.ModifiedBy = userContext.UserId; k.ModifiedDate = DateTime.Now; });

                        _m3pactContext.ClientKpimap.UpdateRange(mappedClientKpiMapIds);
                        AssignClientKPIMap(userContext, kpiModel, newClientIdsToMap);

                    }

                    _m3pactContext.Update(kpiModel);
                }
                else
                {
                    if ((isQuestionUniversal == true) || (kpiModel.IsUniversal != null && kpiModel.IsUniversal.Value))
                    {
                        List<int> clientIds = _m3pactContext.Client.Select(c => c.ClientId).ToList();
                        AssignClientKPIMap(userContext, kpiModel, clientIds);
                    }
                    _m3pactContext.Kpi.Add(kpiModel);
                }
                _m3pactContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// Getting M3Metrics questions
        /// </summary>
        /// <returns></returns>
        public List<BusinessModel.Admin.M3metricsQuestion> GetM3metricsQuestion()
        {
            try
            {
                List<BusinessModel.Admin.M3metricsQuestion> questionsDto = new List<BusinessModel.Admin.M3metricsQuestion>();
                IEnumerable<DomainModel.DomainModels.M3metricsQuestion> questions = _m3pactContext.M3metricsQuestion.Where(x => x.RecordStatus == DomainConstants.RecordStatusActive);

                if (questions != null && questions.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.M3metricsQuestion item in questions)
                    {
                        BusinessModel.Admin.M3metricsQuestion question = new BusinessModel.Admin.M3metricsQuestion();
                        question.M3metricsQuestionCode = item.M3metricsQuestionCode;
                        question.M3metricsQuestionId = item.M3metricsQuestionId;
                        question.M3metricsQuestionText = item.M3metricsQuestionText;
                        question.M3metricsUnit = item.M3metricsUnit;
                        question.RecordStatus = item.RecordStatus;
                        questionsDto.Add(question);
                    }
                }
                return questionsDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting measure based in checkList Type
        /// </summary>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public List<KPIMeasure> GetMeasureBasedOnClientTypeID(int checkListTypeId)
        {
            try
            {
                List<BusinessModel.Admin.KPIMeasure> measuresDto = new List<BusinessModel.Admin.KPIMeasure>();
                IEnumerable<DomainModel.DomainModels.Kpimeasure> measures = _m3pactContext.Kpimeasure.Where(x => x.CheckListTypeId == checkListTypeId && x.RecordStatus == DomainConstants.RecordStatusActive);

                if (measures != null && measures.Count() > 0)
                {
                    foreach (DomainModel.DomainModels.Kpimeasure item in measures)
                    {
                        BusinessModel.Admin.KPIMeasure measure = new BusinessModel.Admin.KPIMeasure();
                        measure.KpimeasureId = item.KpimeasureId;
                        measure.CheckListTypeId = item.CheckListTypeId;
                        measure.Measure = item.Measure;
                        measure.RecordStatus = item.RecordStatus;
                        measuresDto.Add(measure);
                    }
                }
                return measuresDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting all KPIs
        /// </summary>
        /// <returns></returns>
        public List<KPI> GetAllKPIs()
        {
            try
            {
                List<KPI> kpisDto = new List<KPI>();
                IEnumerable<Kpi> kpis = _m3pactContext.Kpi.Where(x => x.RecordStatus == DomainConstants.RecordStatusActive);
                if (kpis != null && kpis.Count() > 0)
                {
                    foreach (Kpi item in kpis)
                    {
                        KPI kpi = MapKpiDetails(item);
                        kpisDto.Add(kpi);
                    }
                }
                return kpisDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<KPI> GetM3MetricsKPIs()
        {
            try
            {
                List<KPI> kpisDto = new List<KPI>();
                IEnumerable<Kpi> kpis;                

                kpis = _m3pactContext.Kpi.Where(x => x.RecordStatus == DomainConstants.RecordStatusActive && x.CheckListTypeId == 3);
                if (kpis != null && kpis.Count() > 0)
                {
                    foreach (Kpi item in kpis)
                    {
                        KPI kpi = MapKpiDetails(item);
                        kpisDto.Add(kpi);
                    }
                }
                return kpisDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Getting kpi based on Question code
        /// </summary>
        /// <param name="questionCode"></param>
        /// <returns></returns>
        public KPI GetKpiBaesdOnQuestionCode(string questionCode)
        {
            try
            {
                Kpi kpis = _m3pactContext.Kpi.FirstOrDefault(x => x.Measure == questionCode);
                KPI kpiData = new KPI();
                if (kpis != null)
                {
                    kpiData = MapKpiDetails(kpis);
                }
                return kpiData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Getting KPI based on ID
        /// </summary>
        /// <param name="KPIId"></param>
        /// <returns></returns>
        public KPI GetKPIById(int KPIId)
        {
            try
            {
                Kpi kpis = _m3pactContext.Kpi.FirstOrDefault(x => x.Kpiid == KPIId);
                return MapKpiDetails(kpis);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Mapping KPI domain model to KPI business model 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>

        /// <summary>
        /// Getting KPIId if KPI desciprion is created for that question
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="checkListTypeId"></param>
        /// <returns></returns>
        public int GetKPIIdBasedonQuestion(int questionId, string checkListTypeCode)
        {
            try
            {
                int checkListTypeId = _m3pactContext.CheckListType.Where(c => c.CheckListTypeCode == checkListTypeCode && c.RecordStatus == DomainConstants.RecordStatusActive).Select(c => c.CheckListTypeId).FirstOrDefault();
                Kpi kpi = _m3pactContext.Kpi.FirstOrDefault(x => x.Measure == questionId.ToString() && x.CheckListTypeId == checkListTypeId);
                if (kpi != null)
                {
                    return kpi.Kpiid;
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get KPI questions to assign for client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<KPI> GetKPIQuestionsForClient(string clientCode)
        {
            try
            {
                List<KPI> KPIDto = new List<KPI>();

                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetkPIQuestionsForClient, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);

                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                KPI kpi = new KPI();
                                kpi.KPIID = (int)dr["KPIID"];
                                kpi.KPIDescription = (string)dr["KPIDescription"];
                                kpi.Source = new BusinessModel.BusinessModels.CheckListType();
                                kpi.Source.CheckListTypeCode = (string)dr["CheckListTypeCode"];
                                kpi.IsUniversal = (bool)dr["IsUniversal"];
                                kpi.AlertLevel = (string)dr["Company Standard"];
                                if (dr["EndDate"] != DBNull.Value)
                                {
                                    kpi.EndDate = (DateTime)dr["EndDate"];
                                }
                                if (dr["Measure"] != DBNull.Value)
                                {
                                    Measure measure = new Measure();
                                    kpi.Measure = MapM3MetricQuestions((string)dr["Measure"], kpi.Source.CheckListTypeCode);
                                }
                                KPIDto.Add(kpi);
                            }
                        }
                    }
                    sqlConn.Close();
                }

                return KPIDto?.OrderBy(c => c.KPIDescription).ToList(); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To Save KPI for a client
        /// </summary>
        /// <param name="clientKPISetup"></param>
        /// <returns></returns>
        public bool SaveKPIForClient(ClientKPISetup clientKPISetup)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();

                int clientId = _m3pactContext.Client.Where(c => c.ClientCode == clientKPISetup.ClientCode).FirstOrDefault().ClientId;

                List<ClientKpimap> clientKPIQuestions = new List<ClientKpimap>();

                List<ClientKpimap> existingKPIForClient = _m3pactContext.ClientKpimap.Include(k => k.ClientKpiuserMap).Where(c => c.ClientId == clientId && c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now && c.StartDate != c.EndDate).ToList();



                foreach (KPISetup kpiQuestion in clientKPISetup.KPIQuestions)
                {

                    if (kpiQuestion.ClientKPIMapId != 0)
                    {
                        ClientKpimap changedKPI = existingKPIForClient.Where(c => c.ClientKpimapId == kpiQuestion.ClientKPIMapId).FirstOrDefault();

                        changedKPI.ClientStandard = kpiQuestion.ClientStandard;
                        changedKPI.Kpiid = kpiQuestion.Kpi.KPIID;
                        changedKPI.IsSla = kpiQuestion.Sla;
                        changedKPI.ModifiedBy = userContext.UserId;
                        changedKPI.ModifiedDate = DateTime.Now;

                        List<int> existingClientUserIds = changedKPI.ClientKpiuserMap.Where(cku => cku.RecordStatus == DomainConstants.RecordStatusActive).Select(c => c.UserId).ToList();

                        List<int> selectedUsers = kpiQuestion.SendTo.Select(c => c.ID).ToList();

                        IEnumerable<int> newlyAddedUserIds = selectedUsers.Except(existingClientUserIds);
                        IEnumerable<int> notChangedUserIds = selectedUsers.Intersect(existingClientUserIds);

                        if (notChangedUserIds != null)
                        {
                            changedKPI.ClientKpiuserMap.Where(ec => !notChangedUserIds.Contains(ec.UserId)).ToList()?
                                                 .ForEach(c =>
                                                 {
                                                     c.RecordStatus = DomainConstants.RecordStatusInactive;
                                                     c.ModifiedDate = DateTime.Now;
                                                     c.ModifiedBy = userContext.UserId;
                                                 });
                        }

                        foreach (int userId in newlyAddedUserIds)
                        {
                            changedKPI.ClientKpiuserMap.Add(new ClientKpiuserMap()
                            {
                                UserId = userId,
                                CreatedBy = userContext.UserId,
                                CreatedDate = DateTime.Now,
                                ModifiedBy = userContext.UserId,
                                ModifiedDate = DateTime.Now,
                                RecordStatus = DomainConstants.RecordStatusActive
                            });

                        }

                        _m3pactContext.ClientKpimap.Update(changedKPI);
                    }
                    else
                    {

                        ClientKpimap clientKpimap = new ClientKpimap();
                        clientKpimap.ClientId = clientId;
                        clientKpimap.ClientStandard = kpiQuestion.ClientStandard;
                        clientKpimap.Kpiid = kpiQuestion.Kpi.KPIID;
                        clientKpimap.IsSla = kpiQuestion.Sla;
                        clientKpimap.StartDate = DateTime.Now.Date;
                        clientKpimap.EndDate = DateTime.MaxValue.Date;

                        clientKpimap.CreatedBy = userContext.UserId;
                        clientKpimap.CreatedDate = DateTime.Now;
                        clientKpimap.ModifiedBy = userContext.UserId;
                        clientKpimap.ModifiedDate = DateTime.Now;
                        clientKpimap.RecordStatus = DomainConstants.RecordStatusActive;

                        List<ClientKpiuserMap> clientUsers = new List<ClientKpiuserMap>();
                        foreach (AllUsers user in kpiQuestion.SendTo)
                        {
                            clientUsers.Add(new ClientKpiuserMap()
                            {
                                UserId = user.ID,
                                CreatedBy = userContext.UserId,
                                CreatedDate = DateTime.Now,
                                ModifiedBy = userContext.UserId,
                                ModifiedDate = DateTime.Now,
                                RecordStatus = DomainConstants.RecordStatusActive

                            });

                        }
                        clientKpimap.ClientKpiuserMap = clientUsers;
                        _m3pactContext.ClientKpiuserMap.AddRange(clientUsers);
                        _m3pactContext.ClientKpimap.Add(clientKpimap);
                    }
                }
                return _m3pactContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// To get assigned kpis for client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientKPISetup GetClientAssignedM3KPIs(string clientCode)
        {
            try
            {
                List<ClientKpimap> clientAssignedKPIs = new List<ClientKpimap>();
                List<ClientKpiuserMap> clientKpiuserMap = new List<ClientKpiuserMap>();

                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetAssignedM3KPIsForClient, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);

                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();

                        while (reader.Read())  //First resultSet to get kpis
                        {
                            ClientKpimap clientKpimap = new ClientKpimap();
                            clientKpimap.Kpi = new Kpi();
                            clientKpimap.Kpi.CheckListType = new DomainModel.DomainModels.CheckListType();

                            if (reader["ClientKpimapId"] != DBNull.Value)
                            {
                                clientKpimap.ClientKpimapId = (int)reader["ClientKpimapId"];
                            }
                            if (reader["KPIID"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.Kpiid = (int)reader["KPIID"];
                            }
                            if (reader["KPIDescription"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.Kpidescription = reader["KPIDescription"].ToString();
                            }
                            if (reader["CheckListTypeCode"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.CheckListType.CheckListTypeCode = reader["CheckListTypeCode"].ToString();
                            }
                            if (reader["IsUniversal"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.IsUniversal = (bool)reader["IsUniversal"];
                            }
                            if (reader["Company Standard"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.AlertLevel = reader["Company Standard"].ToString() == "1" ? "YES" : reader["Company Standard"].ToString() == "0" ? "NO" : reader["Company Standard"].ToString();
                            }
                            if (reader["Client Standard"] != DBNull.Value)
                            {
                                clientKpimap.ClientStandard = reader["Client Standard"]?.ToString();
                            }
                            if (reader["IsSLA"] != DBNull.Value)
                            {
                                clientKpimap.IsSla = (bool)reader["IsSLA"];
                            }
                            if (reader["KpiMeasureId"] != DBNull.Value)
                            {
                                clientKpimap.Kpi.Measure = reader["KpiMeasureId"].ToString();
                            }
                            clientAssignedKPIs.Add(clientKpimap);
                        }
                        if (reader.NextResult())  // Second resultset to get clientkpiusers
                        {
                            while (reader.Read())
                            {
                                ClientKpiuserMap kpiUser = new ClientKpiuserMap();
                                kpiUser.User = new UserLogin();
                                kpiUser.User.Role = new DomainModel.DomainModels.Roles();

                                if (reader["ClientKPIMapID"] != DBNull.Value)
                                {
                                    kpiUser.ClientKpimapId = (int)reader["ClientKPIMapID"];
                                }
                                if (reader["ID"] != DBNull.Value)
                                {
                                    kpiUser.User.Id = (int)reader["ID"];
                                }
                                if (reader["Email"] != DBNull.Value)
                                {
                                    kpiUser.User.Email = reader["Email"].ToString();
                                }
                                if (reader["FirstName"] != DBNull.Value)
                                {
                                    kpiUser.User.FirstName = reader["FirstName"].ToString();
                                }
                                if (reader["IsMeridianUser"] != DBNull.Value)
                                {
                                    kpiUser.User.IsMeridianUser = (bool)reader["IsMeridianUser"];
                                }
                                if (reader["LastName"] != DBNull.Value)
                                {
                                    kpiUser.User.LastName = reader["LastName"].ToString();
                                }
                                if (reader["MobileNumber"] != DBNull.Value)
                                {
                                    kpiUser.User.MobileNumber = reader["MobileNumber"].ToString();
                                }
                                if (reader["RoleCode"] != DBNull.Value)
                                {
                                    kpiUser.User.Role.RoleCode = reader["RoleCode"].ToString();
                                }
                                if (reader["UserName"] != DBNull.Value)
                                {
                                    kpiUser.User.UserName = reader["UserName"].ToString();
                                }
                                if (reader["UserID"] != DBNull.Value)
                                {
                                    kpiUser.User.UserId = reader["UserID"].ToString();
                                }

                                clientKpiuserMap.Add(kpiUser);
                            }
                        }
                    }

                    sqlConn.Close();
                }

                foreach (ClientKpimap clientKpi in clientAssignedKPIs)
                {
                    clientKpi.ClientKpiuserMap = clientKpiuserMap.Where(c => c.ClientKpimapId == clientKpi.ClientKpimapId)?.ToList();
                }

                ClientKPISetup clientAssignedKPIsDto = new ClientKPISetup();
                clientAssignedKPIsDto.ClientCode = clientCode;

                if (clientAssignedKPIs != null)
                {
                    foreach (ClientKpimap clientKpi in clientAssignedKPIs)
                    {
                        KPISetup clientkPIDto = new KPISetup();
                        clientkPIDto.Kpi.Source = new BusinessModel.BusinessModels.CheckListType();

                        clientkPIDto.ClientStandard = clientKpi.ClientStandard;
                        clientkPIDto.Sla = clientKpi.IsSla;
                        clientkPIDto.ClientKPIMapId = clientKpi.ClientKpimapId;

                        clientkPIDto.Kpi.AlertLevel = clientKpi.Kpi.AlertLevel;
                        clientkPIDto.Kpi.IsUniversal = clientKpi.Kpi.IsUniversal;
                        clientkPIDto.Kpi.KPIDescription = clientKpi.Kpi.Kpidescription;
                        clientkPIDto.Kpi.KPIID = clientKpi.Kpi.Kpiid;
                        clientkPIDto.Kpi.Measure = MapM3MetricQuestions(clientKpi.Kpi.Measure, clientKpi.Kpi.CheckListType.CheckListTypeCode);
                        clientkPIDto.Kpi.Source.CheckListTypeCode = clientKpi.Kpi.CheckListType.CheckListTypeCode;

                        if (clientKpi.ClientKpiuserMap != null)
                        {
                            foreach (ClientKpiuserMap clientKpiuser in clientKpi.ClientKpiuserMap)
                            {
                                AllUsers clientKPIUserDto = new AllUsers();

                                clientKPIUserDto.ID = clientKpiuser.User.Id;
                                clientKPIUserDto.Email = clientKpiuser.User.Email;
                                clientKPIUserDto.FirstName = clientKpiuser.User.FirstName;
                                clientKPIUserDto.IsMeridianUser = clientKpiuser.User.IsMeridianUser;
                                clientKPIUserDto.LastName = clientKpiuser.User.LastName;
                                clientKPIUserDto.MobileNumber = clientKpiuser.User.MobileNumber;
                                clientKPIUserDto.RoleName = clientKpiuser.User.Role.RoleCode;
                                clientKPIUserDto.UserName = clientKpiuser.User.UserName;
                                clientKPIUserDto.UserId = clientKpiuser.User.UserId;

                                clientkPIDto.SendTo.Add(clientKPIUserDto);
                            }
                        }
                        clientAssignedKPIsDto.KPIQuestions.Add(clientkPIDto);
                    }
                    clientAssignedKPIsDto.KPIQuestions = clientAssignedKPIsDto.KPIQuestions?.OrderBy(c => c.Kpi.KPIDescription)?.ToList();
                }
                return clientAssignedKPIsDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get all the assigned weekly and Monthly KPIs
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientKPIDetails GetClientAssignedWeeklyMonthlyKPIs(string clientCode)
        {
            try
            {
                ClientKPIDetails clientKPIDetails = new ClientKPIDetails();
                List<ClientKPIAssignedDetails> clientAssignedKPIs = new List<ClientKPIAssignedDetails>();
                List<AllUsers> clientKpiuserMap = new List<AllUsers>();

                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetClientAssignedWeeklyMonthlyKPIs, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlCmd.Parameters.AddWithValue("@ClientCode", clientCode);

                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();

                        while (reader.Read())  //First resultSet to get kpis
                        {
                            ClientKPIAssignedDetails clientKpimap = new ClientKPIAssignedDetails();

                            if (reader["ClientCode"] != DBNull.Value)
                            {
                                clientKpimap.ClientCode = reader["ClientCode"].ToString();
                            }
                            if (reader["ChecklistId"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistId = (int)reader["ChecklistId"];
                            }
                            if (reader["ChecklistType"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistType = reader["ChecklistType"].ToString();
                            }
                            if (reader["ChecklistStartDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistStartDate = (DateTime)reader["ChecklistStartDate"];
                            }
                            if (reader["ChecklistEndDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistEndDate = (DateTime)reader["ChecklistEndDate"];
                            }
                            if (reader["ChecklistEffectiveDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistEffectiveDate = (DateTime)reader["ChecklistEffectiveDate"];
                            }
                            if (reader["ChecklistQuestionAssignedStartDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistQuestionAssignedStartDate = (DateTime)reader["ChecklistQuestionAssignedStartDate"];
                            }
                            if (reader["ChecklistQuestionAssignedEndDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistQuestionAssignedEndDate = (DateTime)reader["ChecklistQuestionAssignedEndDate"];
                            }
                            if (reader["ChecklistQuestionEffectiveDate"] != DBNull.Value)
                            {
                                clientKpimap.ChecklistQuestionEffectiveDate = (DateTime)reader["ChecklistQuestionEffectiveDate"];
                            }

                            if (reader["KPIID"] != DBNull.Value)
                            {
                                clientKpimap.KPIID = (int)reader["KPIID"];
                            }
                            if (reader["KPIDescription"] != DBNull.Value)
                            {
                                clientKpimap.KPIDescription = reader["KPIDescription"].ToString();
                            }
                            if (reader["IsKPI"] != DBNull.Value)
                            {
                                clientKpimap.IsKPI = (bool)reader["IsKPI"];
                            }
                            if (reader["IsUnivarsal"] != DBNull.Value)
                            {
                                clientKpimap.IsUnivarsal = (bool)reader["IsUnivarsal"];
                            }
                            if (reader["CompanyStandard"] != DBNull.Value)
                            {
                                clientKpimap.CompanyStandard = (bool)reader["CompanyStandard"];
                            }
                            if (reader["IsSLA"] != DBNull.Value)
                            {
                                clientKpimap.IsSLA = (bool)reader["IsSLA"];
                            }
                            if (reader["QuestionCode"] != DBNull.Value)
                            {
                                clientKpimap.QuestionCode = (string)reader["QuestionCode"];
                            }
                            if (reader["QuestionStartDate"] != DBNull.Value)
                            {
                                clientKpimap.QuestionStartDate = (DateTime)reader["QuestionStartDate"];
                            }
                            if (reader["QuestionEndDate"] != DBNull.Value)
                            {
                                clientKpimap.QuestionEndDate = (DateTime)reader["QuestionEndDate"];
                            }
                            if (reader["QuestionEffectiveDate"] != DBNull.Value)
                            {
                                clientKpimap.QuestionEffectiveDate = (DateTime)reader["QuestionEffectiveDate"];
                            }
                            if (reader["ClientKPIMapID"] != DBNull.Value)
                            {
                                clientKpimap.ClientKPIMapID = (int)reader["ClientKPIMapID"];
                            }
                            if (reader["KPIAssignedStartDate"] != DBNull.Value)
                            {
                                clientKpimap.KPIAssignedStartDate = (DateTime)reader["KPIAssignedStartDate"];
                            }
                            if (reader["KPIAssignedEndDate"] != DBNull.Value)
                            {
                                clientKpimap.KPIAssignedEndDate = (DateTime)reader["KPIAssignedEndDate"];
                            }
                            clientAssignedKPIs.Add(clientKpimap);
                        }
                        if (reader.NextResult())  // Second resultset to get clientkpiusers
                        {
                            while (reader.Read())
                            {
                                AllUsers kpiUser = new AllUsers();

                                if (reader["ClientKPIMapID"] != DBNull.Value)
                                {
                                    kpiUser.ClientKPIMapId = (int)reader["ClientKPIMapID"];
                                }
                                if (reader["ID"] != DBNull.Value)
                                {
                                    kpiUser.ID = (int)reader["ID"];
                                }
                                if (reader["Email"] != DBNull.Value)
                                {
                                    kpiUser.Email = reader["Email"].ToString();
                                }
                                if (reader["FirstName"] != DBNull.Value)
                                {
                                    kpiUser.FirstName = reader["FirstName"].ToString();
                                }
                                if (reader["IsMeridianUser"] != DBNull.Value)
                                {
                                    kpiUser.IsMeridianUser = (bool)reader["IsMeridianUser"];
                                }
                                if (reader["LastName"] != DBNull.Value)
                                {
                                    kpiUser.LastName = reader["LastName"].ToString();
                                }
                                if (reader["MobileNumber"] != DBNull.Value)
                                {
                                    kpiUser.MobileNumber = reader["MobileNumber"].ToString();
                                }
                                if (reader["RoleCode"] != DBNull.Value)
                                {
                                    kpiUser.RoleName = reader["RoleCode"].ToString();
                                }
                                if (reader["UserName"] != DBNull.Value)
                                {
                                    kpiUser.UserName = reader["UserName"].ToString();
                                }
                                if (reader["UserID"] != DBNull.Value)
                                {
                                    kpiUser.UserId = reader["UserID"].ToString();
                                }

                                clientKpiuserMap.Add(kpiUser);
                            }
                        }
                        clientKPIDetails.clientKPIAssignedDetails = clientAssignedKPIs;
                        clientKPIDetails.clientKPIAssignedUserDetails = clientKpiuserMap;
                    }

                    sqlConn.Close();
                }
                return clientKPIDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the kpi id's of heat map items.
        /// </summary>
        /// <returns></returns>
        public List<int> GetKpiHeatMapItems()
        {
            try
            {
                List<int> heatMapItemList = (from h in _m3pactContext.HeatMapItem
                                             join k in _m3pactContext.Kpi
                                             on h.Kpiid equals k.Kpiid
                                             join c in _m3pactContext.CheckListType
                                             on k.CheckListTypeId equals c.CheckListTypeId
                                             where h.RecordStatus == DomainConstants.RecordStatusActive
                                             && k.RecordStatus == DomainConstants.RecordStatusActive
                                             && c.RecordStatus == DomainConstants.RecordStatusActive
                                             && c.CheckListTypeCode == DomainConstants.M3
                                             && h.StartTime <= DateTime.Now
                                             && h.EndTime > DateTime.Now
                                             select h.Kpiid
                                            ).ToList();
                return heatMapItemList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion public Methods

        #region Private Methods
        /// <summary>
        /// To Map ClientKpIs
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="kpiModel"></param>
        /// <param name="clientIds"></param>
        private static void AssignClientKPIMap(UserContext userContext, Kpi kpiModel, List<int> clientIds)
        {
            List<ClientKpimap> clientKpimaps = new List<ClientKpimap>();
            foreach (int clientId in clientIds)
            {
                ClientKpimap clientkpi = new ClientKpimap()
                {
                    ClientId = clientId,
                    Kpiid = kpiModel.Kpiid,
                    StartDate = DateTime.Now.Date,
                    EndDate = DateTime.MaxValue.Date,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId,
                    RecordStatus = DomainConstants.RecordStatusActive

                };
                if (kpiModel.IsUniversal != null)
                {
                    clientkpi.ClientStandard = string.Empty;
                    clientkpi.IsSla = false;
                }

                clientKpimaps.Add(clientkpi);
            }
            kpiModel.ClientKpimap = clientKpimaps;
        }

        private KPI MapKpiDetails(Kpi item)
        {
            KPI kpi = new KPI();
            kpi.KPIID = item.Kpiid;
            kpi.KPIDescription = item.Kpidescription;

            kpi.Source = new BusinessModel.BusinessModels.CheckListType();
            DomainModel.DomainModels.CheckListType source = _m3pactContext.CheckListType.FirstOrDefault(x => x.CheckListTypeId == item.CheckListTypeId);
            kpi.Source.CheckListTypeID = source.CheckListTypeId;
            kpi.Source.CheckListTypeName = source.CheckListTypeName;
            kpi.Source.CheckListTypeCode = source.CheckListTypeCode;
            kpi.Source.RecordStatus = source.RecordStatus;
            kpi.Measure = new Measure();
            if (source.CheckListTypeId == 3)
            {
                kpi.Standard = item.Standard;
                kpi.IsUniversal = item.IsUniversal;
                DomainModel.DomainModels.M3metricsQuestion question = _m3pactContext.M3metricsQuestion.FirstOrDefault(x => x.M3metricsQuestionId == Convert.ToInt32(item.Measure));
                if (question != null)
                {
                    kpi.Measure.MeasureId = question.M3metricsQuestionId;
                    kpi.Measure.MeasureText = question.M3metricsQuestionText;
                    kpi.Measure.MeasureCode = question.M3metricsQuestionCode;
                    kpi.Measure.MeasureUnit = question.M3metricsUnit;
                }
            }
            else
            {
                DomainModel.DomainModels.Question question = _m3pactContext.Question.FirstOrDefault(x => x.QuestionCode == item.Measure && x.EndDate == DateTime.MaxValue.Date);
                if (question != null)
                {
                    kpi.Measure.MeasureId = question.QuestionId;
                    kpi.Measure.MeasureText = question.QuestionText;
                    kpi.Measure.MeasureCode = question.QuestionCode;
                }
            }
            kpi.AlertLevel = item.AlertLevel;

            kpi.KPIMeasure = new KPIMeasure();
            Kpimeasure kpiMeasure = _m3pactContext.Kpimeasure.FirstOrDefault(x => x.KpimeasureId == item.KpimeasureId);
            if (kpiMeasure != null)
            {
                kpi.KPIMeasure.KpimeasureId = kpiMeasure.KpimeasureId;
                kpi.KPIMeasure.Measure = kpiMeasure.Measure;
                kpi.KPIMeasure.CheckListTypeId = kpiMeasure.CheckListTypeId;
                kpi.KPIMeasure.RecordStatus = kpiMeasure.RecordStatus;
            }

            kpi.IsHeatMapItem = item.IsHeatMapItem;
            kpi.HeatMapScore = item.HeatMapScore;
            kpi.RecordStatus = item.RecordStatus;

            kpi.KPIAlert = new KPIAlert();
            Kpialert kpialert = _m3pactContext.Kpialert.FirstOrDefault(x => x.Kpiid == item.Kpiid);
            if (kpialert != null)
            {
                kpi.KPIAlert.KPIAlertId = kpialert.KpialertId;
                kpi.KPIAlert.SendAlert = kpialert.SendAlert;
                kpi.KPIAlert.SendAlertTitle = kpialert.SendAlertTitle;
                kpi.KPIAlert.SendToBillingManager = kpialert.SendToBillingManager;
                kpi.KPIAlert.SendToRelationshipManager = kpialert.SendToRelationshipManager;
                kpi.KPIAlert.EscalateAlert = kpialert.EscalateAlert;
                kpi.KPIAlert.EscalateAlertTitle = kpialert.EscalateAlertTitle;
                kpi.KPIAlert.EscalateTriggerTime = kpialert.EscalateTriggerTime;
                kpi.KPIAlert.IncludeKPITarget = kpialert.IncludeKpitarget;
                kpi.KPIAlert.IncludeDeviationTarget = kpialert.IncludeDeviationTarget;
                kpi.KPIAlert.IsSla = kpialert.IsSla;
            }

            return kpi;
        }

        /// <summary>
        /// Mapping M3MetricsQuestion with Measure business model.
        /// </summary>
        /// <param name="measureId"></param>
        /// <param name="checkListTypeCode"></param>
        /// <returns></returns>
        private Measure MapM3MetricQuestions(string measureId, string checkListTypeCode)
        {
            Measure measure = null;
            if (checkListTypeCode == DomainConstants.M3)
            {
                DomainModel.DomainModels.M3metricsQuestion m3Metricquestion = new DomainModel.DomainModels.M3metricsQuestion();
                m3Metricquestion = _m3pactContext.M3metricsQuestion.Where(p => p.M3metricsQuestionId.ToString() == measureId).SingleOrDefault();
                if (m3Metricquestion != null)
                {
                    measure = new Measure();
                    measure.MeasureCode = m3Metricquestion.M3metricsQuestionCode;
                    measure.MeasureUnit = m3Metricquestion.M3metricsUnit;
                }
            }
            return measure;
        }
        #endregion
    }
}
