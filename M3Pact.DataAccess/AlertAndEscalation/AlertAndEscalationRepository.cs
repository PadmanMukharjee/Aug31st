using M3Pact.BusinessModel.AlertAndEscalation;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Repository.AlertsAndEscalation;
using M3Pact.Infrastructure.Interfaces.Repository.Checklist;
using M3Pact.Repository.Checklist;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace M3Pact.Repository.AlertAndEscalation
{
    public class AlertAndEscalationRepository : IAlertAndEscalationRepository
    {
        #region private Properties
        private IConfiguration _Configuration { get; }
        private M3PactContext _m3PactContext;

        #endregion private Properties

        #region public Properties
        public List<int> deviatedClientKpiIds;
        #endregion public Properties

        #region Constructor

        public AlertAndEscalationRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3PactContext = m3PactContext;
            deviatedClientKpiIds = DeviatedClientIds();
            _Configuration = configuration;
        }
        #endregion Constructor


        #region public Methods

        /// <summary>
        /// Get the Deviated clientids one time and use wherever needed
        /// </summary>
        /// <returns></returns>
        public List<int> DeviatedClientIds()
        {
            try
            {
                DateTime Today = DateTime.Now.Date;
                List<int> clientIdsDeviated = _m3PactContext.DeviatedClientKpi.Where(d => d.SubmittedDate.Date == Today && d.RecordStatus == DomainConstants.RecordStatusActive).Select(d => d.ClientId).Distinct()?.ToList();
                return clientIdsDeviated;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get The billing and relationship managers of clients whose KPIs are deviated
        /// </summary>
        /// <param name="BillingManagers"></param>
        /// <param name="RelationshipManagers"></param>
        public void GetBillingAndRelationshipManagers(out List<int?> BillingManagers, out List<int?> RelationshipManagers)
        {
            try
            {
                DateTime Today = DateTime.Now.Date;
                List<int> clientIdsDeviated = DeviatedClientIds();
                List<int> clinetIds = _m3PactContext.Client.Select(c => c.ClientId).ToList();
                BillingManagers = _m3PactContext.Client.Where(c => clientIdsDeviated.Contains(c.ClientId)).Select(c => c.BillingManagerId)?.ToList();
                RelationshipManagers = _m3PactContext.Client.Where(c => clientIdsDeviated.Contains(c.ClientId)).Select(c => c.RelationShipManagerId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the alert recepients other than billing and relationship managers
        /// </summary>
        /// <returns></returns>
        public List<int> GetAlertRecepient()
        {
            try
            {
                return _m3PactContext.ClientKpiuserMap.Where(ck => ck.RecordStatus == DomainConstants.RecordStatusActive).Select(ck => ck.UserId).Distinct()?.ToList() ?? null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get the Deviated KPIs of Billing and relationship managers
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="managerType"></param>
        /// <returns></returns>
        public List<DeviatedClientKpi> ManagersDeviatedKPIs(int userId, string managerType)
        {
            try
            {
                DateTime Today = DateTime.Now.Date;
                List<int> clientIdsDeviated = deviatedClientKpiIds;
                List<Client> clientofManager = new List<Client>();
                clientofManager = _m3PactContext.Client.Where(c => c.BillingManagerId == userId || c.RelationShipManagerId == userId).ToList();
                List<int> clientIdsofBM = clientofManager.Where(c => clientIdsDeviated.Contains(c.ClientId)).Select(c => c.ClientId).ToList();
                return _m3PactContext.DeviatedClientKpi.Where(d => clientIdsofBM.Contains(d.ClientId) && d.RecordStatus == DomainConstants.RecordStatusActive && d.SubmittedDate.Date == Today).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the Deviated KPIs of Alert Recepients other than billing and relationship manager
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<DeviatedClientKpi> AlertRecepientDeviatedKPIs(int userId)
        {
            try
            {
                DateTime Today = DateTime.Now.Date;
                List<int> clientIdsDeviated = deviatedClientKpiIds;
                List<int> clientKpiMapIds = _m3PactContext.ClientKpiuserMap.Where(ck => ck.UserId == userId && ck.RecordStatus == DomainConstants.RecordStatusActive).Select(ck => ck.ClientKpimapId).ToList();
                List<int> clientIds = _m3PactContext.ClientKpimap.Where(ck => clientKpiMapIds.Contains(ck.ClientKpimapId)).Select(ck => ck.ClientId).Distinct()?.ToList();
                List<int> kpiIds = _m3PactContext.ClientKpimap.Where(ckm => clientKpiMapIds.Contains(ckm.ClientKpimapId)).Select(ckm => ckm.Kpiid).ToList();
                List<string> questionCodes = _m3PactContext.Kpi.Where(k => kpiIds.Contains(k.Kpiid)).Select(k => k.Measure).ToList();
                return _m3PactContext.DeviatedClientKpi.Where(d => questionCodes.Contains(d.QuestionCode) && clientIds.Contains(d.ClientId) && d.RecordStatus == DomainConstants.RecordStatusActive && d.SubmittedDate.Date == Today).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get the mail entities Which are needed for a grid which need to send in the mail
        /// </summary>
        /// <param name="deviatedClientKpis"></param>
        /// <param name="managerType"></param>
        /// <returns></returns>
        public List<MailEntity> FormMailEntities(List<DeviatedClientKpi> deviatedClientKpis, string managerType, int userId)
        {
            try
            {
                List<MailEntity> mailEntities = new List<MailEntity>();
                foreach (DeviatedClientKpi d in deviatedClientKpis)
                {
                    if (!IsMailAlreadySentForKPI(d, userId))
                    {
                        Kpi kpi = _m3PactContext.Kpi.Where(k => k.Measure == d.QuestionCode)?.FirstOrDefault() ?? new Kpi();
                        Kpialert kpialert = _m3PactContext.Kpialert.Where(k => k.Kpiid == kpi.Kpiid)?.FirstOrDefault() ?? new Kpialert();
                        if (kpialert.SendAlert == true && ((managerType == DomainConstants.BillingManager && (kpialert.SendToBillingManager == true || kpialert.SendToRelationshipManager == true)) ||
                            managerType == DomainConstants.AlertRecepient))
                        {
                            MailEntity mailEntity = new MailEntity();
                            mailEntity.Client = _m3PactContext.Client.Where(c => c.ClientId == d.ClientId).Select(c => c.ClientCode + " - " + c.Name).FirstOrDefault();
                            mailEntity.KPIType = _m3PactContext.CheckListType.Where(clt => clt.CheckListTypeId == d.ChecklistTypeId).Select(clt => clt.CheckListTypeName).FirstOrDefault();
                            if (mailEntity.KPIType == DomainConstants.M3Metrics)
                            {
                                mailEntity.KPIType = mailEntity.KPIType;
                            }
                            mailEntity.Response = d.ActualResponse;
                            mailEntity.ChecklistDate = d.CheckListDate;
                            mailEntity.KPIDescription = kpi.Kpidescription;
                            mailEntity.IsSLA = kpi.ClientKpimap.Where(c => c.ClientId == d.ClientId).Select(c => c.IsSla)?.FirstOrDefault();
                            mailEntity.Standard = kpi.ClientKpimap.Where(c => c.ClientId == d.ClientId).Select(c => c.ClientStandard)?.FirstOrDefault();
                            if (mailEntity.Standard == null)
                            {
                                mailEntity.Standard = d.ExpectedResponse;
                            }
                            mailEntities.Add(mailEntity);
                        }
                    }
                }
                return mailEntities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

        /// <summary>
        /// to get user's email and username
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        public void GetUserNameAndEmail(int userId, out string email, out string userName)
        {
            try
            {
                var user = _m3PactContext.UserLogin.Where(u => u.Id == userId).FirstOrDefault();
                userName = user != null ? user.FirstName + " " + user.LastName : "";
                email = user != null ? user.Email : "";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        ///  to save the mail recepients
        /// </summary>
        /// <param name="deviatedClientKpis"></param>
        /// <param name="userId"></param>
        /// <param name="alertType"></param>
        public void SaveMailRecepientDetails(List<DeviatedClientKpi> deviatedClientKpis, int userId, string alertType)
        {
            try
            {
                List<MailRecepientsDetailsDayWise> mailRecepients = new List<MailRecepientsDetailsDayWise>();
                foreach (DeviatedClientKpi dck in deviatedClientKpis)
                {
                    MailRecepientsDetailsDayWise mailRecepient = new MailRecepientsDetailsDayWise();
                    mailRecepient.DeviatedClientKpiid = dck.DeviatedClientKpiid;
                    mailRecepient.AlertType = alertType;
                    mailRecepient.SentDate = DateTime.Now;
                    mailRecepient.UserId = userId.ToString();
                    mailRecepients.Add(mailRecepient);
                }
                _m3PactContext.MailRecepientsDetailsDayWise.AddRange(mailRecepients);
                _m3PactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Maintain track of sent mails
        /// </summary>
        /// <param name="deviatedClientKpis"></param>
        /// <param name="userId"></param>
        /// <param name="alertType"></param>
        public void SaveMailRecepientDetails(List<int> deviatedClientKpis, string userId, string alertType)
        {
            try
            {
                List<MailRecepientsDetailsDayWise> mailRecepients = new List<MailRecepientsDetailsDayWise>();
                foreach (int deviatedClientKpi in deviatedClientKpis)
                {
                    MailRecepientsDetailsDayWise mailRecepient = new MailRecepientsDetailsDayWise();
                    mailRecepient.DeviatedClientKpiid = deviatedClientKpi;
                    mailRecepient.AlertType = alertType;
                    mailRecepient.SentDate = DateTime.Now.Date;
                    mailRecepient.UserId = userId;
                    mailRecepients.Add(mailRecepient);
                }
                _m3PactContext.MailRecepientsDetailsDayWise.AddRange(mailRecepients);
                _m3PactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsMailAlreadySentForKPI(DeviatedClientKpi deviatedClientKpi, int userId)
        {
            try
            {
                int id = _m3PactContext.MailRecepientsDetailsDayWise.Where(m => deviatedClientKpi.DeviatedClientKpiid == m.DeviatedClientKpiid && m.SentDate.Date == DateTime.Now.Date && m.UserId == userId.ToString()).
                      Select(m => m.Id).FirstOrDefault();

                return id > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Save the job run details when the job starts
        /// </summary>
        /// <returns></returns>
        public int SaveJobRun()
        {
            try
            {
                JobRun job = new JobRun();
                job.JobProcessGroupId = _m3PactContext.JobProcessGroup.Where(jp => jp.ProcessGroupCode == "AlertAndEscaltion").Select(jp => jp.JobProcessGroupId).FirstOrDefault();
                job.JobStatusId = _m3PactContext.JobStatus.Where(js => js.JobStatusCode == "Started").Select(js => js.JobStatusId).FirstOrDefault();
                job.StartTime = DateTime.Now;
                job.EndTime = DateTime.MaxValue;
                job.CreatedDate = DateTime.Now;
                job.CreatedBy = DomainConstants.Admin;
                job.ModifiedDate = DateTime.Now;
                job.ModifiedBy = DomainConstants.Admin;
                job.RecordStatus = DomainConstants.RecordStatusActive;
                _m3PactContext.JobRun.Add(job);
                _m3PactContext.SaveChanges();
                return job.JobRunId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// update the job status 
        /// </summary>
        /// <param name="jobRunId"></param>
        /// <param name="status"></param>
        public void UpdateJobRun(int jobRunId, string status)
        {
            try
            {
                JobRun job = _m3PactContext.JobRun.Where(jr => jr.JobRunId == jobRunId).FirstOrDefault();
                job.JobStatusId = _m3PactContext.JobStatus.Where(js => js.JobStatusCode == status).Select(js => js.JobStatusId).FirstOrDefault();
                job.EndTime = DateTime.Now;
                job.ModifiedDate = DateTime.Now;
                job.ModifiedBy = DomainConstants.Admin;
                _m3PactContext.JobRun.Update(job);
                _m3PactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Escalated Mail Details
        /// </summary>
        /// <returns></returns>
        public List<MailEntity> GetEscalatedMailDetails()
        {
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                List<MailEntity> mailEntities = new List<MailEntity>();
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString)) //TODO: Use from GenericHelper
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.EscalateAlertJob, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();                    
                        while (dr.Read())
                        {
                            MailEntity mailEntity = new MailEntity();
                            if (dr["Client"] != DBNull.Value)
                            {
                                mailEntity.Client = Convert.ToString(dr["Client"]);
                            }

                            if (dr["KPIDescription"] != DBNull.Value)
                            {
                                mailEntity.KPIDescription = Convert.ToString(dr["KPIDescription"]);
                            }

                            if (dr["KPIType"] != DBNull.Value)
                            {
                                mailEntity.KPIType = Convert.ToString(dr["KPIType"]);
                            }

                            if (dr["ActualResponse"] != DBNull.Value)
                            {
                                mailEntity.Response = dr["ActualResponse"].ToString();
                            }

                            if (dr["IsSLA"] != DBNull.Value)
                            {
                                mailEntity.IsSLA = Convert.ToBoolean(dr["IsSLA"]);
                            }

                            if (dr["Standard"] != DBNull.Value)
                            {
                                mailEntity.Standard = Convert.ToString(dr["Standard"]);
                            }

                            if (dr["ChecklistDate"] != DBNull.Value)
                            {
                                mailEntity.ChecklistDate = Convert.ToDateTime(dr["ChecklistDate"]);
                            }

                            if (dr["DeviatedSince"] != DBNull.Value)
                            {
                                mailEntity.DeviatedSince = Convert.ToString(dr["DeviatedSince"]);
                            }

                            if (dr["EmailAddress"] != DBNull.Value)
                            {
                                mailEntity.EscalatedRecipients = Convert.ToString(dr["EmailAddress"]);
                            }

                            if (dr["DeviatedClientKPIId"] != DBNull.Value)
                            {
                                mailEntity.DeviatedClientKPIId = Convert.ToInt32(dr["DeviatedClientKPIId"]);
                            }

                            if (dr["BillingManager"] != DBNull.Value)
                            {
                                mailEntity.BillingManager = Convert.ToString(dr["BillingManager"]);
                            }

                            if (dr["RelationshipManager"] != DBNull.Value)
                            {
                                mailEntity.RelationshipManager = Convert.ToString(dr["RelationshipManager"]);
                            }

                            mailEntities.Add(mailEntity);
                        }
                    }
                    sqlConn.Close();
                }
                return mailEntities;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// to Insert the Deviated m3Metric KPIS in DB
        /// </summary>
        public void InsertDeviatedMetricKPiAndHeatMapItemScore()
        {
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.MeridianConnection);
                int m3MetricChecklistTypeId = _m3PactContext.CheckListType.Where(cl => cl.CheckListTypeCode == "m3").Select(cl => cl.CheckListTypeId).FirstOrDefault();
                DataTable kpiConfig = KPiConfigfromM3metricDb();
                List<Client> clients = _m3PactContext.Client.Where(c => c.IsActive == DomainConstants.RecordStatusActive).ToList();

                IQueryable<Kpi> kpis = (from k in _m3PactContext.Kpi
                                        join clt in _m3PactContext.CheckListType
                                        on k.CheckListTypeId equals clt.CheckListTypeId
                                        where clt.CheckListTypeCode == DomainConstants.M3
                                        select k);

                IQueryable<Kpimeasure> kpiMeasure = (from km in _m3PactContext.Kpimeasure
                                                     join clt in _m3PactContext.CheckListType
                                                     on km.CheckListTypeId equals clt.CheckListTypeId
                                                     where clt.CheckListTypeCode == DomainConstants.M3
                                                     select km);

                DataTable dataTable = GetClientKPIDatafromM3MetricDB(DomainConstants.Daily);
                DataTable dataTableMonthly = GetClientKPIDatafromM3MetricDB(DomainConstants.Monthly);
                DataTable m3MetricDataDaily = GetClientKPIDatafromM3MetricDB(DomainConstants.M3Metrics);
                PerformM3MetricClientKPIDeviation(clients, m3MetricDataDaily, kpiConfig);
                //To Save heatmap scores
                SaveClientHeatMapScores(clients);

                foreach (Client client in clients)
                {
                    DataRow result = dataTable.Select("client='" + client.ClientCode + "'", "PostingYear Desc,PostingMonth Desc").FirstOrDefault();
                    List<Kpi> kpisDaily = (from k in kpis
                                           join ck in _m3PactContext.ClientKpimap
                                           on k.Kpiid equals ck.Kpiid
                                           join km in kpiMeasure
                                           on k.KpimeasureId equals km.KpimeasureId
                                           where ck.ClientId == client.ClientId
                                           && km.Measure == DomainConstants.Daily
                                           select k).ToList();

                    KPistoBeinDeviated(kpisDaily.ToList(), DomainConstants.Daily, kpiConfig, result, client.ClientId, m3MetricChecklistTypeId);

                    if (DateTime.Now.DayOfWeek.ToString() == "Monday")
                    {
                        List<Kpi> kpisWeekly = (from k in kpis
                                                join ck in _m3PactContext.ClientKpimap
                                                on k.Kpiid equals ck.Kpiid
                                                join km in kpiMeasure
                                                on k.KpimeasureId equals km.KpimeasureId
                                                where ck.ClientId == client.ClientId
                                                && km.Measure == DomainConstants.Weekly
                                                select k).ToList();
                        KPistoBeinDeviated(kpisWeekly.ToList(), DomainConstants.Weekly, kpiConfig, result, client.ClientId, m3MetricChecklistTypeId);
                    }

                    DataRow[] rows = dataTableMonthly.Select("client='" + client.ClientCode + "'");
                    GetPreviousMonthAndYear(out int previousYear, out int previousMonth);

                    DataRow monthlyData = rows.Select(r => r.Table.Select("PostingYear=" + previousYear + "AND PostingMonth=" + previousMonth).FirstOrDefault()).FirstOrDefault();
                    if (rows.Length > 1 && monthlyData != null)
                    {
                        List<Kpi> kpisMonthly = (from k in kpis
                                                 join ck in _m3PactContext.ClientKpimap
                                                 on k.Kpiid equals ck.Kpiid
                                                 join km in kpiMeasure
                                                 on k.KpimeasureId equals km.KpimeasureId
                                                 where ck.ClientId == client.ClientId
                                                 && km.Measure == DomainConstants.Monthly
                                                 select k).ToList();

                        KPistoBeinDeviated(kpisMonthly.ToList(), DomainConstants.Monthly, kpiConfig, monthlyData, client.ClientId, m3MetricChecklistTypeId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion public Methods

        #region private Methods

        /// <summary>
        /// Insert metric daily data to M3metricClientKpiDaily
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="m3MetricDataDaily"></param>
        /// <param name="kpiConfig"></param>
        private void PerformM3MetricClientKPIDeviation(List<Client> clients, DataTable m3MetricDataDaily, DataTable kpiConfig)
        {
            bool isDataExist = _m3PactContext.M3metricClientKpiDaily.Where(x => x.InsertedDate == DateTime.Now.Date).Any();
            if (!isDataExist)
            {
                List<M3metricClientKpiDaily> m3metricClientKpiDailyData = new List<M3metricClientKpiDaily>();
                List<Kpi> metricKPIs = _m3PactContext.Kpi.Include(h => h.CheckListType)
                                       .Where(h => h.RecordStatus == DomainConstants.RecordStatusActive && h.CheckListType.CheckListTypeCode == DomainConstants.M3).ToList();

                foreach (Client client in clients)
                {
                    DataRow m3MetricClientData = m3MetricDataDaily.Select("client='" + client.ClientCode + "'").FirstOrDefault();
                    List<Kpi> clientMetricKPIs = (from k in metricKPIs
                                                  join ck in _m3PactContext.ClientKpimap on k.Kpiid equals ck.Kpiid
                                                  where ck.ClientId == client.ClientId
                                                  select k).ToList();

                    foreach (Kpi metricKPI in clientMetricKPIs)
                    {
                        M3metricsQuestion m3MetricsQuestion = _m3PactContext.M3metricsQuestion.Where(m => m.M3metricsQuestionId.ToString() == metricKPI.Measure).FirstOrDefault();
                        if (m3MetricsQuestion != null)
                        {
                            string kpiTargetColumn = GetKPITargetColumn(kpiConfig, m3MetricsQuestion.M3metricsQuestionCode);
                            if (kpiTargetColumn != "" && m3MetricClientData != null)
                            {
                                string alertLevel = GetKPIAlertLevelWithOperatorForClient(client.ClientId, metricKPI);
                                string standardAlertValue = GetStandardAlertValueForClient(alertLevel);
                                string alertCompare = GetKPIAlertOperatorForClient(alertLevel);
                                string actualkpiValue = GetActualAlertValueForClient(m3MetricClientData, kpiTargetColumn);
                                bool isDeviation = IsKPIDeviated(standardAlertValue, alertCompare, actualkpiValue);

                                M3metricClientKpiDaily m3metricKpi = new M3metricClientKpiDaily();
                                m3metricKpi.ClientId = client.ClientId;
                                m3metricKpi.KpiId = metricKPI.Kpiid;
                                m3metricKpi.IsDeviated = isDeviation ? true : false;
                                m3metricKpi.RecordStatus = DomainConstants.RecordStatusActive;
                                m3metricKpi.InsertedDate = DateTime.Now.Date;
                                m3metricKpi.CreatedDate = DateTime.Now;
                                m3metricKpi.CreatedBy = DomainConstants.Admin;
                                m3metricKpi.ModifiedDate = DateTime.Now;
                                m3metricKpi.ModifiedBy = DomainConstants.Admin;
                                m3metricKpi.ActualValue = actualkpiValue;
                                m3metricKpi.AlertLevel = alertCompare + " " + standardAlertValue;
                                m3metricClientKpiDailyData.Add(m3metricKpi);
                            }
                        }
                    }
                }

                if (m3metricClientKpiDailyData != null && m3metricClientKpiDailyData.Count > 0)
                {
                    _m3PactContext.M3metricClientKpiDaily.AddRange(m3metricClientKpiDailyData);
                    _m3PactContext.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Save Heatmap Item scores and Risk Score
        /// </summary>
        /// <param name="clients"></param>
        private void SaveClientHeatMapScores(List<Client> clients)
        {            
            DateTime today = DateTime.Now.Date;
            bool isDataExist = _m3PactContext.ClientHeatMapRisk.Where(x => x.M3dailyDate == today).Any();
            if (!isDataExist)
            {
                List<ClientHeatMapItemScore> heatMapItemsScoresToSave = new List<ClientHeatMapItemScore>();
                List<ClientHeatMapRisk> heatMapRiskToSave = new List<ClientHeatMapRisk>();
                List<HeatMapItem> metricKPIsInHeatMap = GetM3MetricKPIsInHeatMap();
                IPendingChecklistRepository _pendingChecklistRepository = new PendingChecklistRepository(_m3PactContext, _Configuration);
                if (metricKPIsInHeatMap != null && metricKPIsInHeatMap.Count > 0 && metricKPIsInHeatMap.Count == 5)
                {
                    var heatMapItemKpis = (from metricData in _m3PactContext.M3metricClientKpiDaily
                                           join metricHeatMapData in metricKPIsInHeatMap on metricData.KpiId equals metricHeatMapData.Kpiid
                                           orderby metricData.ClientId ascending
                                           select new
                                           {
                                               metricData,
                                               metricHeatMapData
                                           }).Where(x => x.metricData.InsertedDate == today).ToList();

                    foreach (Client client in clients)
                    {
                        int? m3DailyScore = 0;
                        var clientHeatMapItemKpis = heatMapItemKpis.Where(x => x.metricData.ClientId == client.ClientId);
                        if (clientHeatMapItemKpis.Any())
                        {
                            foreach (var item in clientHeatMapItemKpis)
                            {
                                ClientHeatMapItemScore heatMapItemScore = new ClientHeatMapItemScore();
                                heatMapItemScore.Score = item.metricData.IsDeviated ? DomainConstants.HeatMapScore : 0;
                                heatMapItemScore.ClientId = item.metricData.ClientId;
                                heatMapItemScore.HeatMapItemId = item.metricHeatMapData.HeatMapItemId;
                                heatMapItemScore.HeatMapItemDate = today;
                                heatMapItemScore.RecordStatus = DomainConstants.RecordStatusActive;
                                heatMapItemScore.CreatedDate = DateTime.Now;
                                heatMapItemScore.CreatedBy = DomainConstants.Admin;
                                heatMapItemScore.ModifiedDate = DateTime.Now;
                                heatMapItemScore.ModifiedBy = DomainConstants.Admin;
                                heatMapItemsScoresToSave.Add(heatMapItemScore);

                                m3DailyScore += heatMapItemScore.Score;
                            }

                            ClientHeatMapRisk existingClientHeatMapRisk = new ClientHeatMapRisk();
                            ClientHeatMapRisk newClientHeatMapRisk = new ClientHeatMapRisk();
                            existingClientHeatMapRisk = _m3PactContext.ClientHeatMapRisk.Where(c => c.ClientId == client.ClientId).OrderByDescending(c => c.ClientHeatMapRiskId)?.FirstOrDefault();
                            int? risk = 0;
                            if (existingClientHeatMapRisk != null)
                            {
                                newClientHeatMapRisk.ChecklistWeeklyDate = existingClientHeatMapRisk.ChecklistWeeklyDate;
                                newClientHeatMapRisk.ChecklistMonthlyDate = existingClientHeatMapRisk.ChecklistMonthlyDate;
                                int existingWeeklyScore = 0;
                                int existingMonthlyScore = 0;

                                if (existingClientHeatMapRisk.ChecklistWeeklyDate != null)
                                {
                                    existingWeeklyScore = _pendingChecklistRepository.GetScore(existingClientHeatMapRisk.ClientHeatMapRiskId, DomainConstants.WEEK, "save").Value;
                                }
                                if (existingClientHeatMapRisk.ChecklistMonthlyDate != null)
                                {
                                    existingMonthlyScore = _pendingChecklistRepository.GetScore(existingClientHeatMapRisk.ClientHeatMapRiskId, DomainConstants.MONTH, "Save").Value;
                                }
                                risk += existingWeeklyScore + existingMonthlyScore + m3DailyScore;
                            }
                            else
                            {
                                risk = m3DailyScore;
                            }
                            newClientHeatMapRisk.RiskScore = risk;
                            newClientHeatMapRisk.M3dailyDate = today;
                            newClientHeatMapRisk.ClientId = client.ClientId;
                            newClientHeatMapRisk.CreatedBy = DomainConstants.Admin;
                            newClientHeatMapRisk.ModifiedBy = DomainConstants.Admin;
                            newClientHeatMapRisk.CreatedDate = DateTime.Now;
                            newClientHeatMapRisk.ModifiedDate = DateTime.Now;
                            newClientHeatMapRisk.EffectiveTime = DateTime.Now;
                            newClientHeatMapRisk.RecordStatus = DomainConstants.RecordStatusActive;
                            heatMapRiskToSave.Add(newClientHeatMapRisk);
                        }
                    }
                    if (heatMapItemsScoresToSave != null && heatMapItemsScoresToSave.Count > 0)
                    {
                        _m3PactContext.ClientHeatMapItemScore.AddRange(heatMapItemsScoresToSave);
                        if (heatMapRiskToSave != null && heatMapRiskToSave.Count > 0)
                        {
                            _m3PactContext.ClientHeatMapRisk.AddRange(heatMapRiskToSave);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get M3 Metric KPIs In HeatMap
        /// </summary>
        /// <returns></returns>
        private List<HeatMapItem> GetM3MetricKPIsInHeatMap()
        {
            return _m3PactContext.HeatMapItem
                    .Include(h => h.Kpi)
                    .Include(h => h.Kpi.CheckListType)
                    .Where(h => h.RecordStatus == DomainConstants.RecordStatusActive
                            && h.Kpi.CheckListType.CheckListTypeCode == DomainConstants.M3
                            && DateTime.Now >= h.StartTime && DateTime.Now <= h.EndTime).ToList();
        }

        /// <summary>
        /// Get the KPI_config table from m3metric DB
        /// </summary>
        /// <returns></returns>
        private DataTable KPiConfigfromM3metricDb()
        {
            int kpiNameId;
            int kpiTargetTableId;
            int kpiTargetColumnId;
            DataTable kpiConfig = new DataTable();
            string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.MeridianConnection);
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.Text;
                    sqlCmd.CommandText = "Select * from kpi_config";
                    sqlConn.Open();
                    SqlDataReader kpi = sqlCmd.ExecuteReader();
                    kpiConfig.Load(kpi);
                    DataColumnCollection columns = kpiConfig.Columns;
                    kpiNameId = columns["kpi_name"].Ordinal;
                    kpiTargetTableId = columns["kpi_target_table"].Ordinal;
                    kpiTargetColumnId = columns["kpi_target_column"].Ordinal;
                    sqlConn.Close();
                }
            }
            return kpiConfig;
        }

        /// <summary>
        /// to get all Clinets M3 metric data 
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private DataTable GetClientKPIDatafromM3MetricDB(string type)
        {
            DataTable dataTable = new DataTable();
            string query = "";
            string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.MeridianConnection);
            using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand sqlCmd = new SqlCommand())
                {
                    sqlCmd.Connection = sqlConn;
                    sqlCmd.CommandType = CommandType.Text;
                    SqlParameter sqlParameter = new SqlParameter();
                    if (type == DomainConstants.Monthly)
                    {
                        int currentMonth = DateTime.Now.Month;
                        int currentYear = DateTime.Now.Year;
                        int previousMonth = DateTime.Now.AddMonths(-1).Month;
                        int previousYear = DateTime.Now.Year;

                        if (previousMonth == 12)
                        {
                            previousYear = DateTime.Now.AddYears(-1).Year;
                        }

                        query = string.Format("Select * from ClientKPi "
                            + "where (PostingMonth ={0} or PostingMonth ={1})"
                            + " AND (PostingYear={2} or PostingYear={3})",
                            currentMonth, previousMonth, currentYear, previousYear);
                    }
                    else if (type == DomainConstants.M3Metrics)
                    {
                        query = "SELECT * FROM " +
                                "(SELECT *, ROW_NUMBER() OVER(PARTITION BY client ORDER BY client, PostingYear DESC, PostingMonth DESC) AS RowNum FROM ClientKPi) A " +
                                "WHERE RowNum = 1";
                    }
                    else
                    {
                        query = "Select * from ClientKPi";
                    }
                    sqlCmd.CommandText = query;
                    sqlConn.Open();
                    SqlDataReader kpi = sqlCmd.ExecuteReader();
                    dataTable.Load(kpi);
                    sqlConn.Close();
                }
            }
            return dataTable;
        }

        /// <summary>
        /// Check and insert into deviatedclient KPI table
        /// </summary>
        /// <param name="kpis"></param>
        /// <param name="metrictype"></param>
        /// <param name="kpiConfig"></param>
        /// <param name="m3MetricClientData"></param>
        /// <param name="clientId"></param>
        /// <param name="m3MetricChecklistTypeId"></param>
        private void KPistoBeinDeviated(List<Kpi> kpis, string metrictype, DataTable kpiConfig, DataRow m3MetricClientData, int clientId, int m3MetricChecklistTypeId)
        {
            foreach (Kpi k in kpis)
            {
                M3metricsQuestion m3MetricsQuestion = _m3PactContext.M3metricsQuestion.Where(m => m.M3metricsQuestionId.ToString() == k.Measure).FirstOrDefault();
                DateTime checklistDate = new DateTime();
                DeviatedClientKpi deviatedClientKpi = new DeviatedClientKpi();
                if (metrictype == DomainConstants.Monthly)
                {

                    GetPreviousMonthAndYear(out int previousYear, out int previousMonth);
                    deviatedClientKpi = _m3PactContext.DeviatedClientKpi.Where(d => d.ChecklistTypeId == m3MetricChecklistTypeId &&
                                                             d.CheckListDate.Month == previousMonth &&
                                                             d.CheckListDate.Year == previousYear
                                                             && d.ClientId == clientId
                                                             && d.QuestionCode == m3MetricsQuestion.M3metricsQuestionCode
                                                             && d.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault();
                    if (deviatedClientKpi != null)
                    {
                        break;
                    }
                    checklistDate = new DateTime(previousYear, previousMonth, 1).Date;
                }
                else
                {
                    deviatedClientKpi = _m3PactContext.DeviatedClientKpi.Where(d => d.ChecklistTypeId == m3MetricChecklistTypeId &&
                                                                d.CheckListDate == DateTime.Now.Date
                                                                && d.ClientId == clientId
                                                                && d.QuestionCode == m3MetricsQuestion.M3metricsQuestionId.ToString()
                                                                && d.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault();
                    if (deviatedClientKpi != null)
                    {
                        break;
                    }
                }
                string kpiTargetColumn = GetKPITargetColumn(kpiConfig, m3MetricsQuestion.M3metricsQuestionCode);
                if (kpiTargetColumn != "" && m3MetricClientData != null)
                {
                    string alertLevel = GetKPIAlertLevelWithOperatorForClient(clientId, k);
                    string standardAlertValue = GetStandardAlertValueForClient(alertLevel);
                    string alertCompare = GetKPIAlertOperatorForClient(alertLevel);
                    string actualkpiValue = GetActualAlertValueForClient(m3MetricClientData, kpiTargetColumn);
                    bool isDeviation = IsKPIDeviated(standardAlertValue, alertCompare, actualkpiValue);
                    if (isDeviation)
                    {
                        DeviatedKPI(k.Kpiid, clientId, standardAlertValue, actualkpiValue,
                           metrictype == DomainConstants.Monthly ? checklistDate : DateTime.Now.Date, m3MetricChecklistTypeId, m3MetricsQuestion.M3metricsQuestionId.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Get Client KPI's AlertLevel With Operator. (Ex: >=,5555)
        /// </summary>
        /// <param name="clientID"></param>
        /// <param name="kpi"></param>
        /// <returns></returns>
        private string GetKPIAlertLevelWithOperatorForClient(int clientID, Kpi kpi)
        {
            string clientSpecificAlert = _m3PactContext.ClientKpimap.Where(ck => ck.ClientId == clientID && ck.Kpiid == kpi.Kpiid).Select(ck => ck.ClientStandard).FirstOrDefault();
            string alert = (clientSpecificAlert != null && clientSpecificAlert != "") ? clientSpecificAlert : kpi.AlertLevel;
            return alert;
        }

        /// <summary>
        /// Get Alert Value. (Ex: 5555 from >=,5555)
        /// </summary>
        /// <param name="alertWithOperator"></param>
        /// <returns></returns>
        private string GetStandardAlertValueForClient(string alertWithOperator)
        {
            string standardAlert = alertWithOperator.Substring(alertWithOperator.IndexOf(',') + 1, alertWithOperator.Length - alertWithOperator.IndexOf(',') - 1);
            return standardAlert;
        }

        /// <summary>
        /// Get Alert Operator. (Ex: >= from >=,5555)
        /// </summary>
        /// <param name="alertWithOperator"></param>
        /// <returns></returns>
        private string GetKPIAlertOperatorForClient(string alertWithOperator)
        {
            string alertCompare = alertWithOperator.Substring(0, alertWithOperator.IndexOf(','));
            return alertCompare;
        }

        /// <summary>
        /// Get Actual KPI Value that is in M3Metrics Db
        /// </summary>
        /// <param name="m3MetricClientData"></param>
        /// <param name="kpiTargetColumn"></param>
        /// <returns></returns>
        private string GetActualAlertValueForClient(DataRow m3MetricClientData, string kpiTargetColumn)
        {
            string kpiValue = m3MetricClientData[kpiTargetColumn].ToString();
            return kpiValue;
        }

        /// <summary>
        /// Get KPI Column Name
        /// </summary>
        /// <param name="kpiConfigResult"></param>
        /// <returns></returns>
        private string GetKPITargetColumn(DataTable kpiConfig, string metricKPICode)
        {
            DataRow kpiNameRow = kpiConfig.Select("kpi_name='" + metricKPICode + "'").FirstOrDefault();
            string kpiTargetColumn = kpiNameRow?["kpi_target_column"]?.ToString() ?? "";
            return kpiTargetColumn;
        }

        /// <summary>
        /// Check for KPI Deviation
        /// </summary>
        /// <param name="standardAlert"></param>
        /// <param name="alertCompare"></param>
        /// <param name="actualKPIValue"></param>
        /// <returns></returns>
        private bool IsKPIDeviated(string standardAlert, string alertCompare, string actualKPIValue)
        {
            bool isDeviated = false;
            decimal kpiValue = Convert.ToDecimal(actualKPIValue);
            decimal alertValue = Convert.ToDecimal(standardAlert);
            Func<decimal, decimal, bool> op = GetCompareOperator(alertCompare);
            isDeviated = op(kpiValue, alertValue);
            return isDeviated;
        }

        /// <summary>
        /// DeviatedClientKPi Table mapping
        /// </summary>
        /// <param name="kpiId"></param>
        /// <param name="clientid"></param>
        /// <param name="expected"></param>
        /// <param name="Actual"></param>
        /// <param name="date"></param>
        /// <param name="checklistTypeId"></param>
        /// <param name="questionCode"></param>
        private void DeviatedKPI(int kpiId, int clientid, string expected, string Actual, DateTime date, int checklistTypeId, string questionCode)
        {
            DeviatedClientKpi deviatedClientKpi = new DeviatedClientKpi();
            deviatedClientKpi.ActualResponse = Actual;
            deviatedClientKpi.ExpectedResponse = expected;
            deviatedClientKpi.ClientId = clientid;
            deviatedClientKpi.SubmittedDate = DateTime.Now.Date;
            deviatedClientKpi.CheckListDate = date;
            deviatedClientKpi.ChecklistTypeId = checklistTypeId;
            deviatedClientKpi.RecordStatus = DomainConstants.RecordStatusActive;
            deviatedClientKpi.QuestionCode = questionCode;
            deviatedClientKpi.SubmittedDate = DateTime.Now.Date;
            _m3PactContext.DeviatedClientKpi.Add(deviatedClientKpi);
            _m3PactContext.SaveChanges();
        }

        /// <summary>
        /// Dynamic comparision
        /// </summary>
        /// <param name="oper"></param>
        /// <returns></returns>
        private Func<decimal, decimal, bool> GetCompareOperator(string oper)
        {
            Func<decimal, decimal, bool> op;
            switch (oper)
            {
                case "<=":
                    return op = (a, b) => a <= b;
                case ">=":
                    return op = (a, b) => a >= b;
                case ">":
                    return op = (a, b) => a > b;
                case "<":
                    return op = (a, b) => a < b;
                case "=":
                    return op = (a, b) => a == b;
                default:
                    return op = (a, b) => a == b;
            }
        }

        /// <summary>
        /// To get the previous month and year
        /// </summary>
        /// <param name="previousYear"></param>
        /// <param name="previousMonth"></param>
        private void GetPreviousMonthAndYear(out int previousYear, out int previousMonth)
        {
            previousMonth = DateTime.Now.AddMonths(-1).Month;
            previousYear = DateTime.Now.Year;

            if (previousMonth == 12)
            {
                previousYear = DateTime.Now.AddYears(-1).Year;
            }
        }
        #endregion private Methods
    }
}
