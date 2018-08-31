using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.CheckList;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Checklist;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class HeatMapRepository : IHeatMapRepository
    {
        #region private Properties

        private M3PactContext _m3PactContext;
        private IPendingChecklistRepository _pendingChecklistRepository;
        private UserContext userContext;
        #endregion private Properties


        #region Constructor
        public HeatMapRepository(M3PactContext m3PactContext, IPendingChecklistRepository pendingChecklistRepository)
        {
            _m3PactContext = m3PactContext;
            _pendingChecklistRepository = pendingChecklistRepository;
            userContext = UserHelper.getUserContext();
        }
        #endregion Constructor

        #region public Methods

        /// <summary>
        /// To get the KPIs for heat map items
        /// </summary>
        /// <returns></returns>
        public List<HeatMapBusinessModel> GetKpisforHeatMap()
        {
            try
            {
                List<string> questionstobeHeatMap = GetChecklistItemsforHeatMap();
                List<HeatMapBusinessModel> kpis = (from k in _m3PactContext.Kpi
                                                   join qc in questionstobeHeatMap
                                                   on k.Measure equals qc
                                                   join clt in _m3PactContext.CheckListType
                                                   on k.CheckListTypeId equals clt.CheckListTypeId
                                                   where k.RecordStatus == DomainConstants.RecordStatusActive
                                                   && (clt.CheckListTypeCode == DomainConstants.WEEK || clt.CheckListTypeCode == DomainConstants.MONTH)
                                                   select new HeatMapBusinessModel()
                                                   {
                                                       KpiId = k.Kpiid,
                                                       KpiDescription = k.Kpidescription,
                                                       ChecklistType = clt.CheckListTypeCode
                                                   }).ToList();

                kpis.AddRange((from k in _m3PactContext.Kpi
                               join clt in _m3PactContext.CheckListType
                               on k.CheckListTypeId equals clt.CheckListTypeId
                               where k.IsHeatMapItem == true && k.IsUniversal == true
                               && clt.CheckListTypeCode == DomainConstants.M3
                               && k.RecordStatus == DomainConstants.RecordStatusActive
                               select new HeatMapBusinessModel()
                               {
                                   KpiId = k.Kpiid,
                                   KpiDescription = k.Kpidescription,
                                   ChecklistType = clt.CheckListTypeCode
                               }).ToList());
                return kpis;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To get the existing Heat Map items
        /// </summary>
        /// <returns></returns>
        public List<HeatMapBusinessModel> GetHeatMapItems()
        {
            try
            {
                List<string> questionstobeHeatMap = GetChecklistItemsforHeatMap();
                List<HeatMapBusinessModel> kpis = (from h in _m3PactContext.HeatMapItem
                                                   join k in _m3PactContext.Kpi
                                                   on h.Kpiid equals k.Kpiid
                                                   join qc in questionstobeHeatMap
                                                   on k.Measure equals qc
                                                   join clt in _m3PactContext.CheckListType
                                                   on k.CheckListTypeId equals clt.CheckListTypeId
                                                   where k.RecordStatus == DomainConstants.RecordStatusActive
                                                   && h.RecordStatus == DomainConstants.RecordStatusActive
                                                   && h.StartTime <= DateTime.Now && h.EndTime > DateTime.Now
                                                   && (clt.CheckListTypeCode == DomainConstants.WEEK || clt.CheckListTypeCode == DomainConstants.MONTH)
                                                   select new HeatMapBusinessModel()
                                                   {
                                                       KpiId = k.Kpiid,
                                                       KpiDescription = k.Kpidescription,
                                                       ChecklistType = clt.CheckListTypeCode
                                                   }).ToList();

                kpis.AddRange((from h in _m3PactContext.HeatMapItem
                               join k in _m3PactContext.Kpi
                               on h.Kpiid equals k.Kpiid
                               join clt in _m3PactContext.CheckListType
                               on k.CheckListTypeId equals clt.CheckListTypeId
                               where k.IsHeatMapItem == true && k.IsUniversal == true
                               && clt.CheckListTypeCode == DomainConstants.M3
                               && k.RecordStatus == DomainConstants.RecordStatusActive
                               && h.StartTime <= DateTime.Now && h.EndTime > DateTime.Now
                               select new HeatMapBusinessModel()
                               {
                                   KpiId = k.Kpiid,
                                   KpiDescription = k.Kpidescription,
                                   ChecklistType = clt.CheckListTypeCode
                               }).ToList());
                return kpis;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To save the updated heat map items
        /// </summary>
        /// <param name="heatMapBusinessModels"></param>
        /// <returns></returns>
        public bool SaveHeatMapItems(List<HeatMapBusinessModel> heatMapBusinessModels)
        {
            try
            {
                List<HeatMapItem> existingHeatMapItems = (from h in _m3PactContext.HeatMapItem
                                                          where h.RecordStatus == DomainConstants.RecordStatusActive
                                                          && h.StartTime <= DateTime.Now && h.EndTime > DateTime.Now
                                                          select h).ToList();
                List<HeatMapItem> updatedHeatMapItems = new List<HeatMapItem>();
                heatMapBusinessModels.ForEach(uhm =>
                {
                    HeatMapItem heatMapItem = _m3PactContext.HeatMapItem.Where(h => h.Kpiid == uhm.KpiId && h.RecordStatus == DomainConstants.RecordStatusActive && h.StartTime <= DateTime.Now && h.EndTime > DateTime.Now).FirstOrDefault();
                    if (heatMapItem != null)
                    {
                        updatedHeatMapItems.Add(heatMapItem);
                    }
                });
                List<HeatMapItem> unchangedHeatMapItems = updatedHeatMapItems.Intersect(existingHeatMapItems).ToList();
                List<int> unchangedKPIIds = unchangedHeatMapItems.Select(u => u.Kpiid).ToList();
                List<HeatMapItem> heatMapstoInactive = existingHeatMapItems.Except(unchangedHeatMapItems).ToList();
                List<int> kpistoInsert = heatMapBusinessModels.Where(i => !unchangedKPIIds.Contains(i.KpiId)).Select(i => i.KpiId).ToList();

                if (heatMapstoInactive?.Count > 0 || kpistoInsert?.Count > 0)
                {
                    heatMapstoInactive.ForEach(hm =>
                    {
                        hm.EndTime = DateTime.Now;
                        hm.ModifiedDate = DateTime.Now;
                        hm.ModifiedBy = userContext.UserId;
                    });
                    _m3PactContext.HeatMapItem.UpdateRange(heatMapstoInactive);

                    List<HeatMapItem> heatMapstoInsert = new List<HeatMapItem>();

                    kpistoInsert.ForEach(kpi =>
                    {
                        HeatMapItem heatMapItem = new HeatMapItem();
                        heatMapItem.Kpiid = kpi;
                        heatMapItem.CreatedBy = userContext.UserId;
                        heatMapItem.CreatedDate = DateTime.Now;
                        heatMapItem.ModifiedBy = userContext.UserId;
                        heatMapItem.ModifiedDate = DateTime.Now;
                        heatMapItem.RecordStatus = DomainConstants.RecordStatusActive;
                        heatMapItem.StartTime = DateTime.Now;
                        heatMapItem.EndTime = DateTime.MaxValue.Date;
                        heatMapstoInsert.Add(heatMapItem);
                    });

                    _m3PactContext.HeatMapItem.AddRange(heatMapstoInsert);
                    _m3PactContext.SaveChanges();
                    RecalculateRiskScores();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion public Methods

        #region private Methods
        private List<string> GetChecklistItemsforHeatMap()
        {
            List<Question> kpiQuestions = (from q in _m3PactContext.Question
                                           join clt in _m3PactContext.CheckListType
                                           on q.CheckListTypeId equals clt.CheckListTypeId
                                           where q.IsKpi == true && q.IsUniversal == true
                                           && q.RecordStatus == DomainConstants.RecordStatusActive
                                           && q.StartDate <= DateTime.Now.Date
                                           && q.EndDate > DateTime.Now.Date
                                           select q).Distinct().ToList();

            List<string> questionCodes = kpiQuestions.Select(q => q.QuestionCode).Distinct().ToList();

            List<string> questionstobeHeatMap = new List<string>();

            foreach (string question in questionCodes)
            {
                List<Question> questions = kpiQuestions.Where(q => q.QuestionCode == question && q.StartDate <= DateTime.Now.Date
                  && DateTime.Now < q.EndDate && q.RecordStatus == DomainConstants.RecordStatusActive).OrderBy(q => q.EndDate).ToList();
                if (questions.Count == 2)
                {
                    if (questions[0].EffectiveDate <= DateTime.Now.Date && questions[1].EndDate == DateTime.MaxValue.Date)
                    {
                        questionstobeHeatMap.Add(question);
                    }
                }
                else if (questions.Count == 1)
                {
                    if (questions[0].EffectiveDate <= DateTime.Now.Date && questions[0].EndDate == DateTime.MaxValue.Date)
                    {
                        questionstobeHeatMap.Add(question);
                    }
                }
            }
            return questionstobeHeatMap;
        }


        /// <summary>
        /// To calculate riskscores for each client on changing heatmap items
        /// </summary>
        private void RecalculateRiskScores()
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<Client> clientIds = _m3PactContext.Client?.Where(c => c.IsActive == DomainConstants.RecordStatusActive).ToList();
                if (clientIds?.Count > 0)
                {
                    int weeklyChecklistTypeId = _m3PactContext.CheckListType.Where(c => c.CheckListTypeCode == DomainConstants.WEEK && c.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().CheckListTypeId;
                    int monthlyChecklistTypeId = _m3PactContext.CheckListType.Where(c => c.CheckListTypeCode == DomainConstants.MONTH && c.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().CheckListTypeId;
                    int metricTypeId = _m3PactContext.CheckListType.Where(c => c.CheckListTypeCode == DomainConstants.M3 && c.RecordStatus == DomainConstants.RecordStatusActive).FirstOrDefault().CheckListTypeId;

                    List<HeatMapItem> weeklyHeatMapItems = _pendingChecklistRepository.GetHeatMapWithType(weeklyChecklistTypeId);
                    List<HeatMapItem> monthlyHeatMapItems = _pendingChecklistRepository.GetHeatMapWithType(monthlyChecklistTypeId);
                    List<HeatMapItem> metricHeatMapItems = _pendingChecklistRepository.GetHeatMapWithType(metricTypeId);

                    foreach (Client client in clientIds)
                    {
                        ClientHeatMapRisk existingRiskScore = _m3PactContext.ClientHeatMapRisk.Where(c => c.ClientId == client.ClientId).OrderByDescending(c => c.ClientHeatMapRiskId)?.FirstOrDefault();

                        if (existingRiskScore != null)
                        {
                            //To Calculate Metric scores and insert into item table and update Riskscore table
                            if (existingRiskScore.M3dailyDate != null)
                            {
                                List<ClientHeatMapItemScore> metricHeatmapScores = _pendingChecklistRepository.GetHeatMapScoresToUpdate(existingRiskScore.M3dailyDate.Value, client.ClientId, metricTypeId);
                                List<HeatMapItem> metricItemsToAdd = metricHeatMapItems.Where(c => !metricHeatmapScores.Select(d => d.HeatMapItemId).Contains(c.HeatMapItemId))?.ToList();
                                if (metricItemsToAdd?.Count > 0)
                                {
                                    int existingMetricscore = _pendingChecklistRepository.GetScore(existingRiskScore.ClientHeatMapRiskId, DomainConstants.M3).Value;

                                    var heatMapItemKpisResponse = (from metricData in _m3PactContext.M3metricClientKpiDaily
                                                                   join metricHeatMapData in metricItemsToAdd on metricData.KpiId equals metricHeatMapData.Kpiid
                                                                   select new
                                                                   {
                                                                       metricData,
                                                                       metricHeatMapData
                                                                   }).Where(x => x.metricData.InsertedDate == existingRiskScore.M3dailyDate && x.metricData.ClientId == client.ClientId).ToList();

                                    List<ClientHeatMapItemScore> metricHeatMapItemsToSave = new List<ClientHeatMapItemScore>();
                                    if (heatMapItemKpisResponse.Any())
                                    {
                                        foreach (var item in heatMapItemKpisResponse)
                                        {
                                            ClientHeatMapItemScore heatMapItemScore = new ClientHeatMapItemScore();
                                            heatMapItemScore.Score = item.metricData.IsDeviated ? DomainConstants.HeatMapScore : 0;
                                            heatMapItemScore.ClientId = item.metricData.ClientId;
                                            heatMapItemScore.HeatMapItemId = item.metricHeatMapData.HeatMapItemId;
                                            heatMapItemScore.HeatMapItemDate = existingRiskScore.M3dailyDate.Value;
                                            heatMapItemScore.RecordStatus = DomainConstants.RecordStatusActive;
                                            heatMapItemScore.CreatedDate = DateTime.Now;
                                            heatMapItemScore.CreatedBy = DomainConstants.Admin;
                                            heatMapItemScore.ModifiedDate = DateTime.Now;
                                            heatMapItemScore.ModifiedBy = DomainConstants.Admin;
                                            metricHeatMapItemsToSave.Add(heatMapItemScore);
                                        }
                                        _m3PactContext.ClientHeatMapItemScore.AddRange(metricHeatMapItemsToSave);

                                        metricHeatmapScores = metricHeatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                        metricHeatmapScores.AddRange(metricHeatMapItemsToSave);
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingMetricscore) + metricHeatmapScores.Sum(c => c.Score);
                                    }

                                }
                            }


                            //To Calculate Weekly changed scores and insert nto item table and update Riskscore table
                            if (existingRiskScore.ChecklistWeeklyDate != null)
                            {
                                List<ClientHeatMapItemScore> heatmapScores = _pendingChecklistRepository.GetHeatMapScoresToUpdate(existingRiskScore.ChecklistWeeklyDate.Value, client.ClientId, weeklyChecklistTypeId);
                                int existingWeeklyscore = _pendingChecklistRepository.GetScore(existingRiskScore.ClientHeatMapRiskId, DomainConstants.WEEK).Value;
                                if (weeklyHeatMapItems?.Count > 0)
                                {
                                    List<HeatMapItem> itemToAdd = weeklyHeatMapItems.Where(c => !heatmapScores.Select(d => d.HeatMapItemId).Contains(c.HeatMapItemId))?.ToList();
                                    if (itemToAdd?.Count > 0)
                                    {
                                        List<ClientChecklistResponseBusinessModel> submittedResponse = _pendingChecklistRepository.GetWeeklyPendingChecklistQuestions(client.ClientCode, existingRiskScore.ChecklistWeeklyDate.Value, DomainConstants.WEEK);

                                        List<ClientHeatMapItemScore> heatMapItemsToSave = _pendingChecklistRepository.MapHeatMapItemScores(client.ClientId, existingRiskScore.ChecklistWeeklyDate.Value, submittedResponse, itemToAdd);
                                        _m3PactContext.ClientHeatMapItemScore.AddRange(heatMapItemsToSave);

                                        heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                        heatmapScores.AddRange(heatMapItemsToSave);
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingWeeklyscore) + heatmapScores.Sum(c => c.Score);
                                    }
                                    else
                                    {
                                        heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingWeeklyscore) + heatmapScores.Sum(c => c.Score);
                                    }
                                }
                                else
                                {
                                    existingRiskScore.ChecklistWeeklyDate = null;
                                    heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                    existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingWeeklyscore) + heatmapScores.Sum(c => c.Score);
                                }

                            }
                            else if (weeklyHeatMapItems?.Count > 0)
                            {
                                DateTime? checklistSubmittedDate = _pendingChecklistRepository.GetLastSubmittedChecklist(client.ClientId, weeklyChecklistTypeId);

                                if (checklistSubmittedDate != null && checklistSubmittedDate != default(DateTime))
                                {

                                    List<ClientChecklistResponseBusinessModel> submittedResponse = _pendingChecklistRepository.GetWeeklyPendingChecklistQuestions(client.ClientCode, checklistSubmittedDate.Value, DomainConstants.WEEK);

                                    List<ClientHeatMapItemScore> heatMapItemsToSave = _pendingChecklistRepository.MapHeatMapItemScores(client.ClientId, checklistSubmittedDate.Value, submittedResponse, weeklyHeatMapItems);

                                    if (heatMapItemsToSave?.Count > 0)
                                    {
                                        _m3PactContext.ClientHeatMapItemScore.AddRange(heatMapItemsToSave);
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore) + heatMapItemsToSave?.Sum(c => c.Score);
                                        existingRiskScore.ChecklistWeeklyDate = checklistSubmittedDate;

                                    }
                                }

                            }


                            // //To Calculate Weekly changed scores and insert into item table and update Riskscore table
                            if (existingRiskScore.ChecklistMonthlyDate != null)
                            {
                                List<ClientHeatMapItemScore> heatmapScores = _pendingChecklistRepository.GetHeatMapScoresToUpdate(existingRiskScore.ChecklistMonthlyDate.Value, client.ClientId, monthlyChecklistTypeId);
                                int existingMonthlyscore = _pendingChecklistRepository.GetScore(existingRiskScore.ClientHeatMapRiskId, DomainConstants.MONTH).Value;
                                if (monthlyHeatMapItems?.Count > 0)
                                {
                                    List<HeatMapItem> itemsToAdd = monthlyHeatMapItems.Where(c => !heatmapScores.Select(d => d.HeatMapItemId).Contains(c.HeatMapItemId))?.ToList();
                                    if (itemsToAdd?.Count > 0)
                                    {
                                        List<ClientChecklistResponseBusinessModel> submittedResponse = _pendingChecklistRepository.GetWeeklyPendingChecklistQuestions(client.ClientCode, existingRiskScore.ChecklistMonthlyDate.Value, DomainConstants.MONTH);

                                        List<ClientHeatMapItemScore> heatMapItemsToSave = _pendingChecklistRepository.MapHeatMapItemScores(client.ClientId, existingRiskScore.ChecklistMonthlyDate.Value, submittedResponse, itemsToAdd);
                                        _m3PactContext.ClientHeatMapItemScore.AddRange(heatMapItemsToSave);

                                        heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                        heatmapScores.AddRange(heatMapItemsToSave);
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingMonthlyscore) + heatmapScores.Sum(c => c.Score);
                                    }
                                    else
                                    {
                                        heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingMonthlyscore) + heatmapScores.Sum(c => c.Score);
                                    }
                                }
                                else
                                {
                                    existingRiskScore.ChecklistMonthlyDate = null;
                                    heatmapScores = heatmapScores.Where(c => c.HeatMapItem.StartTime <= DateTime.Now && c.HeatMapItem.EndTime >= DateTime.Now)?.ToList();//check
                                    existingRiskScore.RiskScore = (existingRiskScore.RiskScore - existingMonthlyscore) + heatmapScores.Sum(c => c.Score);
                                }

                            }
                            else if (monthlyHeatMapItems?.Count > 0)
                            {
                                DateTime? checklistSubmittedDate = _pendingChecklistRepository.GetLastSubmittedChecklist(client.ClientId, monthlyChecklistTypeId);

                                if (checklistSubmittedDate != null && checklistSubmittedDate != default(DateTime))
                                {

                                    List<ClientChecklistResponseBusinessModel> submittedResponse = _pendingChecklistRepository.GetWeeklyPendingChecklistQuestions(client.ClientCode, checklistSubmittedDate.Value, DomainConstants.MONTH);

                                    List<ClientHeatMapItemScore> heatMapItemsToSave = _pendingChecklistRepository.MapHeatMapItemScores(client.ClientId, checklistSubmittedDate.Value, submittedResponse, monthlyHeatMapItems);

                                    if (heatMapItemsToSave?.Count > 0)
                                    {
                                        _m3PactContext.ClientHeatMapItemScore.AddRange(heatMapItemsToSave);
                                        existingRiskScore.RiskScore = (existingRiskScore.RiskScore) + heatMapItemsToSave?.Sum(c => c.Score);
                                        existingRiskScore.ChecklistMonthlyDate = checklistSubmittedDate;

                                    }
                                }

                            }

                            existingRiskScore.ModifiedBy = userContext.UserId;
                            existingRiskScore.ModifiedDate = DateTime.Now;
                            existingRiskScore.EffectiveTime = DateTime.Now;

                            _m3PactContext.ClientHeatMapRisk.Update(existingRiskScore);
                        }

                    }
                    _m3PactContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion private Methods
    }
}
