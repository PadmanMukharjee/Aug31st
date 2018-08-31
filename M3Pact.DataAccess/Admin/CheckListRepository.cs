using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.CheckList;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Repository.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace M3Pact.Repository.Admin
{
    public class CheckListRepository : ICheckListRepository
    {
        #region private Properties

        private M3PactContext _m3PactContext;
        private IConfiguration _Configuration { get; }
        private IKPIRepository _kpiRepository;

        #endregion private Properties

        #region Constructor
        public CheckListRepository(M3PactContext m3PactContext, IConfiguration configuration, IKPIRepository kpiRepository)
        {
            _m3PactContext = m3PactContext;
            _Configuration = configuration;
            _kpiRepository = kpiRepository;
        }
        #endregion Constructor


        #region public Methods
        /// <summary>
        /// get all questions from the questions table
        /// </summary>
        /// <returns>asynchronous list of business questions</returns>
        public async Task<List<BusinessModel.BusinessModels.Question>> GetQuestionsOfChecklistType(string checklistType)
        {
            try
            {
                List<Question> questions = new List<Question>();
                if (checklistType != "null")
                {
                    questions = await _m3PactContext.Question
                                        .Include(t => t.CheckListType)
                                        .Where(x => x.EndDate == DateTime.MaxValue.Date && x.CheckListType.CheckListTypeCode == checklistType).ToListAsync();
                }
                else
                {
                    questions = await _m3PactContext.Question
                                        .Include(t => t.CheckListType)
                                        .Where(x => x.EndDate == DateTime.MaxValue.Date).ToListAsync();
                }

                List<BusinessModel.BusinessModels.Question> questionsDto = new List<BusinessModel.BusinessModels.Question>();

                if (questions != null)
                {
                    foreach (DomainModel.DomainModels.Question question in questions)
                    {
                        BusinessModel.BusinessModels.Question questionDto = new BusinessModel.BusinessModels.Question
                        {
                            ExpectedResponse = question.ExpectedRespone,
                            IsFreeform = question.RequireFreeform,
                            IsKpi = question.IsKpi,
                            QuestionId = question.QuestionId,
                            QuestionText = question.QuestionText,
                            RecordStatus = question.RecordStatus,
                            IsUniversal = question.IsUniversal,
                            QuestionCode = question.QuestionCode,
                            checkListType = new BusinessModel.BusinessModels.CheckListType()
                            {
                                CheckListTypeCode = question.CheckListType.CheckListTypeCode,
                                CheckListTypeID = question.CheckListType.CheckListTypeId,
                                CheckListTypeName = question.CheckListType.CheckListTypeName
                            }
                        };
                        if (questionDto.IsKpi)
                        {
                            questionDto.KPIDescription = _kpiRepository.GetKpiBaesdOnQuestionCode(question.QuestionCode);
                        }
                        questionsDto.Add(questionDto);
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
        /// Creates a question in the question table
        /// </summary>
        /// <param name="question"></param>
        /// <returns>asynchronous saved question</returns>
        public async Task<BusinessModel.BusinessModels.Question> CreateQuestion(BusinessModel.BusinessModels.Question question)
        {
            if (question == default(BusinessModel.BusinessModels.Question) && question.QuestionId > 0 || string.IsNullOrWhiteSpace(question.QuestionText))
            {
                return null;
            }

            Question dbQuestion = MappingQuestion(question);

            try
            {
                _m3PactContext.Question.Add(dbQuestion);

                if (await _m3PactContext.SaveChangesAsync() > 0)
                {
                    question.QuestionId = dbQuestion.QuestionId;
                    if (dbQuestion.IsUniversal)
                    {
                        AddUniversalChecklistItemToAllChecklists(dbQuestion);
                    }
                    return question;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        /// <summary>
        /// Gets QuestionCode sequence 
        /// </summary>
        /// <returns></returns>
        public string GetQuestionCodeSequence()
        {
            string sequenceNumber = string.Empty;
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.questionGetNextValueSequence, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlConn.Open();
                        SqlDataReader dr = sqlCmd.ExecuteReader();
                        while (dr.Read())
                        {
                            sequenceNumber = Convert.ToString(dr["SequenceNumber"]);
                        }
                        sqlConn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sequenceNumber;
        }

        /// <summary>
        /// Update a question in the question table
        /// </summary>
        /// <param name="question"></param>
        /// <returns>asynchronous saved question</returns>
        public async Task<BusinessModel.BusinessModels.Question> UpdateQuestion(BusinessModel.BusinessModels.Question question)
        {
            try
            {
                if (question == default(BusinessModel.BusinessModels.Question) || question.QuestionId == default(int))
                {
                    return null;
                }
                Question dbQuestion = await _m3PactContext.Question
                    .Where(r => r.QuestionId == question.QuestionId && r.EndDate == DateTime.MaxValue.Date && r.RecordStatus == DomainConstants.RecordStatusActive)
                    .FirstOrDefaultAsync();

                if (dbQuestion == null)
                {
                    return null;
                }
                if (DateTime.Now.Date < dbQuestion.EffectiveDate)
                {
                    dbQuestion.EndDate = dbQuestion.StartDate;
                    dbQuestion.RecordStatus = DomainConstants.RecordStatusInactive;
                }
                else
                {
                    dbQuestion.EndDate = GetEffectiveDateBasedOnChecklistType(dbQuestion.CheckListTypeId.Value);
                }

                DateTime currentTime = DateTime.Now;
                UserContext userContext = UserHelper.getUserContext();
                dbQuestion.ModifiedDate = currentTime;
                dbQuestion.ModifiedBy = userContext.UserId;
                _m3PactContext.Question.Update(dbQuestion);

                question.QuestionCode = dbQuestion.QuestionCode;
                Question newQuestion = MappingQuestion(question, true, dbQuestion.IsKpi, dbQuestion.EndDate);

                _m3PactContext.Question.Add(newQuestion);

                if (await _m3PactContext.SaveChangesAsync() > 0)
                {
                    if (!dbQuestion.IsUniversal && newQuestion.IsUniversal)
                    {
                        AddUniversalChecklistItemToAllChecklists(dbQuestion);
                    }
                    return question;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        /// <summary>
        /// To create a checklist
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns>asynchronous created Checklist</returns>
        public async Task<BusinessModel.BusinessModels.CheckList> CreateCheckListAsync(BusinessModel.BusinessModels.CheckList checkList)
        {

            try
            {
                if (checkList == default(BusinessModel.BusinessModels.CheckList))
                {
                    return null;
                }

                if (checkList.CheckListId > 0 ||
                    checkList.CheckListType == default(KeyValueModel) ||
                    checkList.CheckListType.Key == "0" ||
                    checkList.Sites == default(List<KeyValueModel>) ||
                    checkList.Sites.Any(e => e.Key == "0") ||
                    checkList.Systems == default(List<KeyValueModel>) ||
                     checkList.Systems.Any(e => e.Key == "0" || string.IsNullOrWhiteSpace(checkList.CheckListName))
                    )
                {
                    return null;
                }

                DateTime currentDate = DateTime.Today;
                UserContext userContext = UserHelper.getUserContext();

                DomainModel.DomainModels.CheckList dbCheckList = new DomainModel.DomainModels.CheckList
                {
                    CheckListName = checkList.CheckListName.Trim(),
                    CheckListTypeId = Convert.ToInt32(checkList.CheckListType.Key),
                    RecordStatus = DomainConstants.RecordStatusActive,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId
                };

                _m3PactContext.CheckList.Add(dbCheckList);

                if (await _m3PactContext.SaveChangesAsync() > 0)
                {
                    checkList.CheckListId = dbCheckList.CheckListId;
                }
                else
                {
                    return null;
                }

                CheckListAttribute systemAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSystemAttributeCode);
                CheckListAttribute siteAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSiteAttributeCode);
                CheckListAttribute QuestionAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListQuestionAttributeCode);

                if (systemAttribute == null || siteAttribute == null)
                {
                    return null;
                }

                DateTime effectiveDate = GetEffectiveDateBasedOnChecklistType(Convert.ToInt32(checkList.CheckListType.Key));

                IEnumerable<DomainModel.DomainModels.CheckListAttributeMap> dbsiteChecklists = checkList.Sites.Select(t => new DomainModel.DomainModels.CheckListAttributeMap
                {
                    CheckListId = dbCheckList.CheckListId,
                    CheckListAttributeId = siteAttribute.CheckListAttributeId,
                    CheckListAttributeValueId = t.Key.ToString(),
                    RecordStatus = DomainConstants.RecordStatusActive,
                    EndDate = DateTime.MaxValue.Date,
                    StartDate = currentDate,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId,
                    EffectiveDate = effectiveDate
                });

                IEnumerable<DomainModel.DomainModels.CheckListAttributeMap> dbsystemChecklists = checkList.Systems.Select(t => new DomainModel.DomainModels.CheckListAttributeMap
                {
                    CheckListId = dbCheckList.CheckListId,
                    CheckListAttributeId = systemAttribute.CheckListAttributeId,
                    CheckListAttributeValueId = t.Key.ToString(),
                    EndDate = DateTime.MaxValue.Date,
                    StartDate = currentDate,
                    RecordStatus = DomainConstants.RecordStatusActive,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId,
                    EffectiveDate = effectiveDate
                });

                IEnumerable<DomainModel.DomainModels.CheckListAttributeMap> dbquestionChecklists = checkList.Questions.Select(t => new DomainModel.DomainModels.CheckListAttributeMap
                {
                    CheckListId = dbCheckList.CheckListId,
                    CheckListAttributeId = QuestionAttribute.CheckListAttributeId,
                    CheckListAttributeValueId = t.Key.ToString(),
                    EndDate = DateTime.MaxValue.Date,
                    StartDate = currentDate,
                    RecordStatus = DomainConstants.RecordStatusActive,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId,
                    EffectiveDate = effectiveDate
                });


                _m3PactContext.CheckListAttributeMap.AddRange(dbsiteChecklists);
                _m3PactContext.CheckListAttributeMap.AddRange(dbsystemChecklists);

                if (dbquestionChecklists.Any())
                {
                    _m3PactContext.CheckListAttributeMap.AddRange(dbquestionChecklists);
                }

                if (await _m3PactContext.SaveChangesAsync() > 0)
                {
                    return checkList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }


        /// <summary>
        /// To get all the Checklists
        /// </summary>
        /// <returns></returns>
        public List<ViewChecklist> GetAllChecklists()
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<ViewChecklist> viewAllChecklists = new List<ViewChecklist>();
                List<ChecklistSite> ChecklistSites = new List<ChecklistSite>();
                List<ChecklistSystem> ChecklistSystems = new List<ChecklistSystem>();
                List<ClientChecklist> ChecklistClients = new List<ClientChecklist>();

                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.GetAllChecklist, sqlConn))
                    {
                        sqlCmd.CommandType = CommandType.StoredProcedure;
                        sqlConn.Open();
                        SqlDataReader reader = sqlCmd.ExecuteReader();

                        while (reader.Read())  //First resultSet to get checklists
                        {
                            ViewChecklist checkList = new ViewChecklist();

                            if (reader["CheckListName"] != DBNull.Value)
                            {
                                checkList.CheckListName = reader["CheckListName"].ToString();
                            }
                            if (reader["CheckListID"] != DBNull.Value)
                            {
                                checkList.checklistId = (int)reader["CheckListID"];
                            }
                            if (reader["CheckListTypeCode"] != DBNull.Value)
                            {
                                if (reader["CheckListTypeCode"].ToString() == DomainConstants.WEEK)
                                {
                                    checkList.CheckListType.CheckListTypeName = DomainConstants.Weekly;
                                }
                                else if (reader["CheckListTypeCode"].ToString() == DomainConstants.MONTH)
                                {
                                    checkList.CheckListType.CheckListTypeName = DomainConstants.Monthly;
                                }
                                else
                                {
                                    checkList.CheckListType.CheckListTypeName = DomainConstants.M3Metrics;
                                }

                            }
                            viewAllChecklists.Add(checkList);
                        }
                        if (reader.NextResult())  // Second resultset to get Sites
                        {
                            while (reader.Read())
                            {

                                ChecklistSite checkListSite = new ChecklistSite();

                                if (reader["CheckListName"] != DBNull.Value)
                                {
                                    checkListSite.ChecklistName = reader["CheckListName"].ToString();
                                }
                                if (reader["SiteCode"] != DBNull.Value)
                                {
                                    checkListSite.SiteCode = reader["SiteCode"].ToString();
                                }
                                if (reader["SiteName"] != DBNull.Value)
                                {
                                    checkListSite.SiteName = reader["SiteName"].ToString();
                                }
                                ChecklistSites.Add(checkListSite);
                            }
                        }
                        if (reader.NextResult())  // Third resultset to get Systems
                        {
                            while (reader.Read())
                            {
                                // do something with third result set;
                                ChecklistSystem checklistSystem = new ChecklistSystem();

                                if (reader["CheckListName"] != DBNull.Value)
                                {
                                    checklistSystem.ChecklistName = reader["CheckListName"].ToString();
                                }
                                if (reader["SystemCode"] != DBNull.Value)
                                {
                                    checklistSystem.SystemCode = reader["SystemCode"].ToString();
                                }
                                if (reader["SystemName"] != DBNull.Value)
                                {
                                    checklistSystem.SystemName = reader["SystemName"].ToString();
                                }
                                ChecklistSystems.Add(checklistSystem);
                            }
                        }
                        if (reader.NextResult())   // Forth resultset to get Clients
                        {
                            while (reader.Read())
                            {
                                // do something with third result set;
                                ClientChecklist checklistClients = new ClientChecklist();

                                if (reader["CheckListName"] != DBNull.Value)
                                {
                                    checklistClients.ChecklistName = reader["CheckListName"].ToString();
                                }
                                if (reader["ClientCode"] != DBNull.Value)
                                {
                                    checklistClients.ClientCode = reader["ClientCode"].ToString();
                                }
                                if (reader["Name"] != DBNull.Value)
                                {
                                    checklistClients.ClientName = reader["Name"].ToString();
                                }
                                ChecklistClients.Add(checklistClients);
                            }
                        }
                    }
                    sqlConn.Close();
                }

                if (viewAllChecklists != null && viewAllChecklists.Any())
                {
                    foreach (ViewChecklist checklist in viewAllChecklists)
                    {
                        checklist.Sites = ChecklistSites?.Where(c => c.ChecklistName == checklist.CheckListName)?.ToList();
                        checklist.Systems = ChecklistSystems?.Where(c => c.ChecklistName == checklist.CheckListName)?.ToList();
                        checklist.Clients = ChecklistClients?.Where(c => c.ChecklistName == checklist.CheckListName)?.ToList();
                    }
                }

                return viewAllChecklists;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get active checklisttypes
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns></returns>
        public async Task<List<BusinessModel.BusinessModels.CheckListType>> GetCheckListTypesAsync()
        {
            try
            {
                List<CheckListType> result = await _m3PactContext.CheckListType.Where(r =>
                r.RecordStatus == DomainConstants.RecordStatusActive &&
                r.CheckListTypeCode != DomainConstants.M3).AsNoTracking().ToListAsync();

                return result.CheckListTypeMap();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get checklist by id from the db
        /// </summary>
        /// <param name="id"></param>
        /// <returns>asynchronous checklist if found</returns>
        public async Task<BusinessModel.BusinessModels.CheckList> GetCheckListByIdAsync(int id)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                CheckListAttribute systemAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSystemAttributeCode);
                CheckListAttribute siteAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSiteAttributeCode);
                CheckListAttribute questionAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListQuestionAttributeCode);

                CheckList result = await _m3PactContext.CheckList
                    .Include(e => e.CheckListAttributeMap)
                    .Include(e => e.CheckListType)
                    .Where(r =>
                    r.RecordStatus == DomainConstants.RecordStatusActive &&
                    r.CheckListId == id).FirstOrDefaultAsync();
                result.CheckListAttributeMap = result.CheckListAttributeMap.Where(t => t.RecordStatus == DomainConstants.RecordStatusActive).ToList();

                return result.CheckListMap(systemAttribute, siteAttribute, questionAttribute);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To check client exists for this checklist with systemid
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="checkListId"></param>
        /// <returns>true or false</returns>
        public bool CheckClientDependencyOnDelteSystem(int systemId, int checkListId)
        {
            try
            {
                return _m3PactContext.ClientCheckListMap.Include(r => r.Client).Where(e => e.CheckListId == checkListId && e.EndDate.Date >= DateTime.Now.Date && e.RecordStatus == DomainConstants.RecordStatusActive).AsNoTracking().Any(l => l.Client.SystemId == systemId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To check client exists for this checklist with siteid
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="checkListId"></param>
        /// <returns>true or false</returns>
        public bool CheckClientDependencyOnDelteSite(int siteId, int checkListId)
        {
            try
            {
                return _m3PactContext.ClientCheckListMap.Include(r => r.Client).Where(e => e.CheckListId == checkListId && e.EndDate.Date >= DateTime.Now.Date && e.RecordStatus == DomainConstants.RecordStatusActive).AsNoTracking().Any(l => l.Client.SiteId == siteId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// to update a checklist if it is valid
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns>asynchronous checklist if it was saved</returns>
        public async Task<BusinessModel.BusinessModels.CheckList> UpdateCheckListAsync(BusinessModel.BusinessModels.CheckList checkList)
        {
            if (checkList.CheckListId < 0 ||
             checkList.CheckListType == default(KeyValueModel) ||
                checkList.CheckListType.Key == "0" ||
                checkList.Sites == default(List<KeyValueModel>) ||
                checkList.Sites.Any(e => e.Key == "0") ||
                checkList.Systems == default(List<KeyValueModel>) ||
                 checkList.Systems.Any(e => e.Key == "0")
                 )
            {
                return null;
            }

            DateTime dateTime = DateTime.Now;
            UserContext userContext = UserHelper.getUserContext();
            DateTime effectiveDate = GetEffectiveDateBasedOnChecklistType(Convert.ToInt32(checkList.CheckListType.Key));

            if (!_m3PactContext.CheckList.Any(r => r.CheckListId == checkList.CheckListId && r.RecordStatus == DomainConstants.RecordStatusActive))
            {
                return null;
            }

            CheckListAttribute systemAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSystemAttributeCode);
            CheckListAttribute siteAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSiteAttributeCode);
            CheckListAttribute questionAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListQuestionAttributeCode);

            if (systemAttribute == null || siteAttribute == null)
            {
                return null;
            }

            List<CheckListAttributeMap> checkListAttributeMap = await _m3PactContext.CheckListAttributeMap
                .Where(e => e.CheckListId == checkList.CheckListId &&
                e.RecordStatus == DomainConstants.RecordStatusActive).ToListAsync();

            IEnumerable<CheckListAttributeMap> dbSystems = checkListAttributeMap.Where(e =>
            e.CheckListId == checkList.CheckListId &&
            e.CheckListAttributeId == systemAttribute.CheckListAttributeId &&
            e.EndDate.Date >= dateTime.Date && e.StartDate.Date <= dateTime.Date);

            IEnumerable<CheckListAttributeMap> newSystems = checkList.Systems.Where(t => !dbSystems.Any(b => b.CheckListAttributeValueId == t.Key.ToString())).Select(t =>
            new DomainModel.DomainModels.CheckListAttributeMap
            {
                CheckListId = checkList.CheckListId,
                CheckListAttributeId = systemAttribute.CheckListAttributeId,
                CheckListAttributeValueId = t.Key.ToString(),
                RecordStatus = DomainConstants.RecordStatusActive,
                EndDate = DateTime.MaxValue.Date,
                StartDate = dateTime.Date,
                CreatedDate = dateTime,
                ModifiedDate = dateTime,
                CreatedBy = userContext.UserId,
                ModifiedBy = userContext.UserId
            });

            IEnumerable<CheckListAttributeMap> removeSystems = dbSystems.Where(t => !checkList.Systems.Any(f => f.Key.ToString() == t.CheckListAttributeValueId)).Select(t =>
            {
                t.EndDate = dateTime.Date;
                t.RecordStatus = DomainConstants.RecordStatusInactive;
                return t;
            });

            if (removeSystems.Any(t => CheckClientDependencyOnDelteSystem(Convert.ToInt32(t.CheckListAttributeValueId), checkList.CheckListId)))
            {
                return null;
            };

            IEnumerable<CheckListAttributeMap> dbSites = checkListAttributeMap.Where(e =>
            e.CheckListId == checkList.CheckListId &&
            e.CheckListAttributeId == siteAttribute.CheckListAttributeId &&
            e.EndDate >= dateTime.Date && e.StartDate <= dateTime.Date);

            IEnumerable<CheckListAttributeMap> newSites = checkList.Sites.Where(t => !dbSites.Any(b => b.CheckListAttributeValueId == t.Key.ToString())).Select(t =>
            new DomainModel.DomainModels.CheckListAttributeMap
            {
                CheckListId = checkList.CheckListId,
                CheckListAttributeId = siteAttribute.CheckListAttributeId,
                CheckListAttributeValueId = t.Key.ToString(),
                RecordStatus = DomainConstants.RecordStatusActive,
                EndDate = DateTime.MaxValue.Date,
                StartDate = dateTime.Date,
                CreatedDate = dateTime,
                ModifiedDate = dateTime,
                CreatedBy = userContext.UserId,
                ModifiedBy = userContext.UserId
            });

            IEnumerable<CheckListAttributeMap> removeSites = dbSites.Where(t => !checkList.Sites.Any(f => f.Key.ToString() == t.CheckListAttributeValueId)).Select(t =>
            {
                t.EndDate = dateTime.Date;
                t.RecordStatus = DomainConstants.RecordStatusInactive;
                return t;
            });

            if (removeSites.Any(t => CheckClientDependencyOnDelteSite(Convert.ToInt32(t.CheckListAttributeValueId), checkList.CheckListId)))
            {
                return null;
            };

            IEnumerable<CheckListAttributeMap> dbQuestions = checkListAttributeMap.Where(e =>
            e.CheckListId == checkList.CheckListId &&
             e.CheckListAttributeId == questionAttribute.CheckListAttributeId
             && e.EndDate >= dateTime.Date && e.RecordStatus == DomainConstants.RecordStatusActive);

            IEnumerable<CheckListAttributeMap> newQuestions = checkList.Questions.Where(t => !dbQuestions.Any(b => b.CheckListAttributeValueId == t.Key.ToString())).Select(t =>
            new DomainModel.DomainModels.CheckListAttributeMap
            {
                CheckListId = checkList.CheckListId,
                CheckListAttributeId = questionAttribute.CheckListAttributeId,
                CheckListAttributeValueId = t.Key.ToString(),
                RecordStatus = DomainConstants.RecordStatusActive,
                EndDate = DateTime.MaxValue.Date,
                StartDate = dateTime.Date,
                EffectiveDate = effectiveDate,
                CreatedDate = dateTime,
                ModifiedDate = dateTime,
                CreatedBy = userContext.UserId,
                ModifiedBy = userContext.UserId
            });

            IEnumerable<CheckListAttributeMap> removeQuestions = dbQuestions.Where(t => !checkList.Questions.Any(f => f.Key.ToString() == t.CheckListAttributeValueId)).Select(t =>
            {
                t.EndDate = dateTime.Date;
                t.RecordStatus = DomainConstants.RecordStatusInactive;
                return t;
            });


            try
            {
                bool changesIncluded = false;

                if (newSystems.Any())
                {
                    _m3PactContext.CheckListAttributeMap.AddRange(newSystems);
                    changesIncluded = true;
                }
                if (removeSystems.Any())
                {
                    _m3PactContext.CheckListAttributeMap.UpdateRange(removeSystems);
                    changesIncluded = true;
                }

                if (newSites.Any())
                {
                    _m3PactContext.CheckListAttributeMap.AddRange(newSites);
                    changesIncluded = true;
                }
                if (removeSites.Any())
                {
                    _m3PactContext.CheckListAttributeMap.UpdateRange(removeSites);
                    changesIncluded = true;
                }

                if (newQuestions.Any())
                {
                    _m3PactContext.CheckListAttributeMap.AddRange(newQuestions);
                    changesIncluded = true;
                }
                if (removeQuestions.Any())
                {
                    _m3PactContext.CheckListAttributeMap.UpdateRange(removeQuestions);
                    changesIncluded = true;
                }

                if (!changesIncluded)
                {
                    return checkList;
                }

                if (await _m3PactContext.SaveChangesAsync() > 0)
                {
                    return checkList;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;

        }

        /// <summary>
        /// To get checklist
        /// </summary>
        /// <param name="checkListQueryModel"></param>
        /// <returns></returns>
        public async Task<List<BusinessModel.BusinessModels.CheckList>> GetCheckListByQuery(CheckListQueryModel checkListQueryModel)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                CheckListAttribute systemAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSystemAttributeCode);
                CheckListAttribute siteAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListSiteAttributeCode);
                CheckListAttribute questionAttribute = _m3PactContext.CheckListAttribute.FirstOrDefault(t => t.AttributeCode == DomainConstants.CheckListQuestionAttributeCode);
                CheckListType checkListType = _m3PactContext.CheckListType.FirstOrDefault(t => t.CheckListTypeCode == checkListQueryModel.CheckListTypeCode);

                if (systemAttribute == null || siteAttribute == null || checkListType == null)
                {
                    return null;
                }

                List<CheckList> dbChecklist = _m3PactContext.CheckList.Include(t => t.CheckListAttributeMap).Where(t =>
               t.CheckListTypeId == checkListType.CheckListTypeId &&
               t.RecordStatus == DomainConstants.RecordStatusActive &&
               t.CheckListAttributeMap.FirstOrDefault(e =>
               e.CheckListAttributeId == systemAttribute.CheckListAttributeId &&
               e.CheckListAttributeValueId == checkListQueryModel.SystemId.ToString() &&
                e.EndDate.Date != e.StartDate.Date &&
               e.EndDate.Date >= dateTime.Date && e.StartDate <= dateTime.Date && e.RecordStatus == DomainConstants.RecordStatusActive) != default(CheckListAttributeMap) &&
               t.CheckListAttributeMap.FirstOrDefault(e =>
                e.CheckListAttributeId == siteAttribute.CheckListAttributeId &&
               e.CheckListAttributeValueId == checkListQueryModel.SiteId.ToString() &&
                e.EndDate.Date != e.StartDate.Date &&
               e.EndDate.Date >= dateTime.Date && e.StartDate.Date <= dateTime.Date && e.RecordStatus == DomainConstants.RecordStatusActive) != default(CheckListAttributeMap)
            ).AsNoTracking().ToList();
                return dbChecklist.CheckListMap(systemAttribute, siteAttribute, questionAttribute);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// To add checklist item which is made as Universal to all the checklist  
        /// </summary>
        /// <param name="question"></param>
        public void AddUniversalChecklistItemToAllChecklists(Question question)
        {
            UserContext userContext = UserHelper.getUserContext();
            try
            {
                List<int> checklistIds = _m3PactContext.CheckListAttributeMap.Where(x => x.CheckList.CheckListTypeId == question.CheckListTypeId).Select(x => x.CheckListId).Distinct().ToList();
                int checklistAttribute = _m3PactContext.CheckListAttribute.Where(x => x.AttributeCode == "QUE").Select(x => x.CheckListAttributeId).FirstOrDefault();
                foreach (int checklistid in checklistIds)
                {
                    List<CheckListAttributeMap> checkListAttributeMaps = _m3PactContext.CheckListAttributeMap.Where(x => x.CheckListId == checklistid && x.RecordStatus == DomainConstants.RecordStatusActive)?.ToList();
                    CheckListAttributeMap checklistQuestion = checkListAttributeMaps.Where(x => x.CheckListAttributeValueId == question.QuestionCode && x.CheckListAttributeId == checklistAttribute
                                                                        && x.StartDate <= DateTime.Now.Date && x.EndDate >= DateTime.Now.Date
                                                                        )?.FirstOrDefault() ?? null;
                    if (checklistQuestion == null)
                    {
                        CheckListAttributeMap newCheckListAttributeMap = new CheckListAttributeMap();
                        newCheckListAttributeMap.CheckListId = checklistid;
                        newCheckListAttributeMap.CheckListAttributeId = checklistAttribute;
                        newCheckListAttributeMap.CheckListAttributeValueId = question.QuestionCode;
                        newCheckListAttributeMap.EffectiveDate = GetEffectiveDateBasedOnChecklistType(question.CheckListTypeId ?? 0);
                        newCheckListAttributeMap.RecordStatus = DomainConstants.RecordStatusActive;
                        newCheckListAttributeMap.StartDate = DateTime.Now.Date;
                        newCheckListAttributeMap.EndDate = DateTime.MaxValue.Date;
                        newCheckListAttributeMap.CreatedBy = userContext.UserId;
                        newCheckListAttributeMap.CreatedDate = DateTime.Now;
                        newCheckListAttributeMap.ModifiedBy = userContext.UserId;
                        newCheckListAttributeMap.ModifiedDate = DateTime.Now;
                        _m3PactContext.CheckListAttributeMap.Add(newCheckListAttributeMap);
                    }
                    else if (checklistQuestion.EndDate != DateTime.MaxValue.Date)
                    {
                        checklistQuestion.EndDate = DateTime.MaxValue.Date;
                        checklistQuestion.ModifiedBy = userContext.UserId;
                        checklistQuestion.ModifiedDate = DateTime.Now;
                        _m3PactContext.CheckListAttributeMap.Update(checklistQuestion);
                    }
                }
                _m3PactContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// To update the end date of Client KPI Map if Checklist item became Non KPI
        /// </summary>
        /// <param name="kpiId"></param>
        /// <param name="checklistType"></param>
        public void UpdateEndDateofClientKPI(int kpiId, int checklistType, DateTime endDate)
        {
            try
            {
                List<ClientKpimap> clientKpimaps = _m3PactContext.ClientKpimap.Where(ckm => ckm.Kpiid == kpiId)?.ToList();
                if (clientKpimaps != null && clientKpimaps.Count > 0)
                {
                    foreach (ClientKpimap clientkpiMap in clientKpimaps)
                    {
                        clientkpiMap.EndDate = endDate.Date;
                    }
                    _m3PactContext.UpdateRange(clientKpimaps);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get the checklist heat map questions id.
        /// </summary>
        /// <returns></returns>
        public List<string> GetChecklistHeatMapQuestions()
        {
            try
            {
                List<string> checklistHeatMapItems = (from h in _m3PactContext.HeatMapItem
                                                      join k in _m3PactContext.Kpi
                                                      on h.Kpiid equals k.Kpiid
                                                      join q in _m3PactContext.Question
                                                      on k.Measure equals q.QuestionCode
                                                      where h.RecordStatus == DomainConstants.RecordStatusActive
                                                      && k.RecordStatus == DomainConstants.RecordStatusActive
                                                      && q.RecordStatus == DomainConstants.RecordStatusActive
                                                      && q.StartDate != q.EndDate
                                                      && q.EffectiveDate <= DateTime.Today.Date
                                                      && q.EndDate > DateTime.Today.Date
                                                      && h.StartTime <= DateTime.Now
                                                      && h.EndTime > DateTime.Now
                                                      select q.QuestionCode
                                                      ).ToList();
                return checklistHeatMapItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion public Methods

        #region Private Methods
        /// <summary>
        /// EffectiveDate to be calculated based on ChecklistType
        /// </summary>
        /// <param name="checklistTypeID"></param>
        /// <returns></returns>
        private DateTime GetEffectiveDateBasedOnChecklistType(int checklistTypeID)
        {
            DateTime effectiveDate = DateTime.Today;
            if (checklistTypeID == DomainConstants.WeekChecklistTypeID)
            {
                effectiveDate = DateHelper.GetMondayOfEffectiveWeek(DateTime.Today);
            }
            else if (checklistTypeID == DomainConstants.MonthChecklistTypeID)
            {
                effectiveDate = DateHelper.GetFirstDayOfEffectiveMonth(DateTime.Today);
            }
            return effectiveDate;
        }

        private DomainModel.DomainModels.Question MappingQuestion(BusinessModel.BusinessModels.Question question, bool isOldQuestion = false, bool isExistingKPI = false, DateTime endDate = default(DateTime))
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                UserContext userContext = UserHelper.getUserContext();
                Question dbQuestion = new DomainModel.DomainModels.Question
                {
                    ExpectedRespone = question.ExpectedResponse,
                    RequireFreeform = question.IsFreeform,
                    IsKpi = question.IsKpi,
                    IsUniversal = question.IsUniversal,
                    RecordStatus = DomainConstants.RecordStatusActive,
                    QuestionText = question.QuestionText.Trim(),
                    CreatedDate = dateTime,
                    ModifiedDate = dateTime,
                    CreatedBy = userContext.UserId,
                    ModifiedBy = userContext.UserId,
                    StartDate = dateTime.Date,
                    EndDate = DateTime.MaxValue.Date,
                    CheckListTypeId = question.checkListType.CheckListTypeID,
                    EffectiveDate = GetEffectiveDateBasedOnChecklistType(question.checkListType.CheckListTypeID)
                };

                if (question.QuestionCode != null)
                {
                    dbQuestion.QuestionCode = question.QuestionCode;
                }
                else
                {
                    dbQuestion.QuestionCode = GetQuestionCodeSequence();
                }

                int kpiID = 0;

                kpiID = _m3PactContext.Kpi.Where(k => k.Measure == dbQuestion.QuestionCode).Select(k => k.Kpiid).FirstOrDefault();

                if (dbQuestion.IsKpi)
                {
                    KPI kpi = question.KPIDescription;
                    KPI kpiViewModel = new KPI
                    {
                        KPIID = kpiID,
                        KPIDescription = kpi.KPIDescription,
                        Measure = new Measure
                        {
                            MeasureCode = dbQuestion.QuestionCode
                        },
                        Source = new BusinessModel.BusinessModels.CheckListType
                        {
                            CheckListTypeID = question.checkListType.CheckListTypeID,
                            CheckListTypeCode = question.checkListType.CheckListTypeCode,
                            CheckListTypeName = question.checkListType.CheckListTypeName
                        },
                        KPIAlert = new KPIAlert
                        {
                            SendAlert = kpi.KPIAlert.SendAlert,
                            EscalateAlert = kpi.KPIAlert.EscalateAlert,
                            SendToRelationshipManager = kpi.KPIAlert.SendToRelationshipManager,
                            SendToBillingManager = kpi.KPIAlert.SendToBillingManager,
                            EscalateTriggerTime = kpi.KPIAlert.EscalateTriggerTime
                        },
                        IsHeatMapItem = kpi.IsHeatMapItem,
                        HeatMapScore = kpi.HeatMapScore,
                        RecordStatus = kpi.RecordStatus
                    };

                    bool kpiUpdated = _kpiRepository.SaveKPIs(kpiViewModel, question.IsUniversal, isOldQuestion);
                }
                if (isExistingKPI && !question.IsKpi)
                {
                    UpdateEndDateofClientKPI(kpiID, question.checkListType.CheckListTypeID, endDate);
                }
                return dbQuestion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Private Methods

    }
}
