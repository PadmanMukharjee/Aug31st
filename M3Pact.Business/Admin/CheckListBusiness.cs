using M3Pact.BusinessModel;
using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.CheckList;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace M3Pact.Business.Admin
{
    public class CheckListBusiness : ICheckListBusiness
    {
        private readonly ICheckListRepository _checkListRepository;
        private ILogger _logger;
        public CheckListBusiness(ICheckListRepository checkListRepository, ILogger logger)
        {
            _checkListRepository = checkListRepository;
            _logger = logger;
        }

        /// <summary>
        /// to get all saved questions from the repository layer
        /// </summary>
        /// <returns>asynchronous list of business Question</returns>
        public async Task<List<Question>> GetQuestions(string checklistType)
        {
            try
            {
                return await _checkListRepository.GetQuestionsOfChecklistType(checklistType);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return new List<Question>();
            }
        }

        /// <summary>
        ///  to save a question through the repository layer call
        /// </summary>
        /// <param name="question"></param>
        /// <returns>asynchronous a business Question</returns>
        public async Task<Question> CreateQuestion(CheckListItemViewModel checkListItemViewModel)
        {
            if (checkListItemViewModel == default(CheckListItemViewModel))
            {
                return null;
            }
            try
            {
                return await _checkListRepository.CreateQuestion(BusinessMapper.MappingChecklistItemViewModelToBusinessModel(checkListItemViewModel));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        ///  to save a question through the repository layer call
        /// </summary>
        /// <param name="question"></param>
        /// <returns>asynchronous a business Question</returns>
        public async Task<Question> UpdateQuestion(CheckListItemViewModel checkListItemViewModel)
        {
            try
            {
                return await _checkListRepository.UpdateQuestion(BusinessMapper.MappingChecklistItemViewModelToBusinessModel(checkListItemViewModel));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }        

        /// <summary>
        ///  to save a check list through the repository layer call
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns>an asynchronous business checklist</returns>

        public async Task<CheckList> CreateCheckList(CheckList checkList)
        {
            try
            {
                return await _checkListRepository.CreateCheckListAsync(checkList);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get all the checklists
        /// </summary>
        /// <returns></returns>
        public List<AllChecklistViewModel> GetAllChecklists()
        {
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                List<AllChecklistViewModel> viewAllChecklists = new List<AllChecklistViewModel>();
                List<ViewChecklist> checklists = _checkListRepository.GetAllChecklists();
                if (checklists?.Count > 0)
                {

                    foreach (ViewChecklist checklist in checklists)
                    {
                        AllChecklistViewModel checklistViewModel = new AllChecklistViewModel();
                        checklistViewModel.Name = checklist.CheckListName;
                        checklistViewModel.Type = checklist.CheckListType.CheckListTypeName;
                        checklistViewModel.Id = checklist.checklistId;


                        foreach (ChecklistSite site in checklist.Sites)
                        {
                            SiteViewModel siteViewModel = new SiteViewModel()
                            {
                                SiteCode = site.SiteCode,
                                SiteName = site.SiteName
                            };

                            checklistViewModel.SelectedSites.Add(siteViewModel);
                        }

                        foreach (ChecklistSystem system in checklist.Systems)
                        {
                            SystemViewModel systemViewModel = new SystemViewModel()
                            {
                                SystemCode = system.SystemCode,
                                SystemName = system.SystemName
                            };

                            checklistViewModel.SelectedSystems.Add(systemViewModel);
                        }

                        foreach (ClientChecklist client in checklist.Clients)
                        {
                            ClientViewModel clientViewModel = new ClientViewModel
                            {
                                ClientCode = client.ClientCode,
                                Name = client.ClientName
                            };

                            checklistViewModel.SelectedClients.Add(clientViewModel);
                        }

                        viewAllChecklists.Add(checklistViewModel);
                    }
                }
                return viewAllChecklists;

            }
            catch (Exception ex)
            {
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.VIEW_ALL_CHECKLIST_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                throw ex;
            }
        }

        /// <summary>
        /// to get all checklistype from the repository layer
        /// </summary>
        /// <returns>asynchronous list of checklisttype</returns>
        public async Task<List<CheckListType>> GetCheckListTypes()
        {
            try
            {
                return await _checkListRepository.GetCheckListTypesAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return new List<CheckListType>();
            }
        }

        /// <summary>
        /// To get checklist by id from repository
        /// </summary>
        /// <param name="id"></param>
        /// <returns>an asynchronous checklist</returns>

        public async Task<CheckList> GetCheckListById(int id)
        {
            try
            {
                return await _checkListRepository.GetCheckListByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// to check client/s is/are available for a checklist with systemid
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool CheckClientDependencyOnDelteSystem(int systemId, int checkListId)
        {
            try
            {
                return _checkListRepository.CheckClientDependencyOnDelteSystem(systemId, checkListId);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// to check client/s is/are available for a checklist with siteId
        /// </summary>
        /// <param name="systemId"></param>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool CheckClientDependencyOnDelteSite(int systemId, int checkListId)
        {
            try
            {
                return _checkListRepository.CheckClientDependencyOnDelteSite(systemId, checkListId);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  to save a checklist through a repository call
        /// </summary>
        /// <param name="checkList"></param>
        /// <returns>an asynchronous checklist</returns>
        public async Task<CheckList> UpdateCheckList(CheckList checkList)
        {
            try
            {
                return await _checkListRepository.UpdateCheckListAsync(checkList);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// ro get checklists from repo by querying the checklist
        /// </summary>
        /// <param name="checkListQueryModel"></param>
        /// <returns></returns>
        public async Task<List<CheckList>> GetCheckListByQuery(CheckListQueryModel checkListQueryModel)
        {
            try
            {
                return await _checkListRepository.GetCheckListByQuery(checkListQueryModel);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return new List<CheckList>();
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
                return _checkListRepository.GetChecklistHeatMapQuestions();
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return new List<string>();
            }
        }


    }
}
