using M3Pact.BusinessModel;
using M3Pact.BusinessModel.CheckList;
using M3Pact.BusinessModel.Mapper;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.Checklist;
using M3Pact.Infrastructure.Interfaces.Repository.Checklist;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Checklist;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using M3Pact.LoggerUtility;

namespace M3Pact.Business.Checklist
{
    public class PendingChecklistBusiness : IPendingChecklistBusiness
    {
        #region properties
        private IPendingChecklistRepository _pendingChecklistRepository;
        private ILogger _logger;
        #endregion properties

        #region constructor
        public PendingChecklistBusiness(IPendingChecklistRepository pendingChecklistRepository, ILogger logger)
        {
            _pendingChecklistRepository = pendingChecklistRepository;
            _logger = logger;
        }
        #endregion constructor

        #region public methods
        /// <summary>
        /// To get the Weekly Pending checklists
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<DateTime> GetWeeklyPendingChecklist(string clientCode)
        {
            try
            {
                return _pendingChecklistRepository.GetWeeklyPendingChecklist(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
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
                return _pendingChecklistRepository.GetMonthlyPendingChecklist(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the Weeklist Questions both monthly and weekly
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="pendingChecklistDate"></param>
        /// <param name="checklistType"></param>
        /// <returns></returns>
        public List<ClientChecklistResponseViewModel> GetWeeklyPendingChecklistQuestions(string clientCode, DateTime pendingChecklistDate, string checklistType)
        {
            try
            {
                List<ClientChecklistResponseViewModel> clientChecklistResponseViewModels = new List<ClientChecklistResponseViewModel>();
                List<ClientChecklistResponseBusinessModel> clientChecklistResponseBusiness = _pendingChecklistRepository.GetWeeklyPendingChecklistQuestions(clientCode, pendingChecklistDate, checklistType);
                if (clientChecklistResponseBusiness != null && clientChecklistResponseBusiness.Count > 0)
                {
                    foreach (ClientChecklistResponseBusinessModel crb in clientChecklistResponseBusiness)
                    {
                        ClientChecklistResponseViewModel cvm = new ClientChecklistResponseViewModel();
                        cvm.ActualFreeForm = crb.ActualFreeForm;
                        cvm.QuestionCode = crb.QuestionCode;
                        cvm.ActualResponse = crb.ActualResponse;
                        cvm.CheckListAttributeMapID = crb.CheckListAttributeMapID;
                        cvm.ChecklistName = crb.ChecklistName;
                        cvm.ClientCheckListMapID = crb.ClientCheckListMapID;
                        cvm.ExpectedRespone = crb.ExpectedRespone;
                        cvm.IsKPI = crb.IsKPI;
                        cvm.Questionid = crb.Questionid;
                        cvm.QuestionText = crb.QuestionText;
                        cvm.RequireFreeform = crb.RequireFreeform;
                        clientChecklistResponseViewModels.Add(cvm);
                    }
                }
                return clientChecklistResponseViewModels;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
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
        public bool SaveOrSubmitChecklistResponse(string clientCode, DateTime pendingDate, bool isSubmit, List<ClientChecklistResponseViewModel> clientChecklistResponse)
        {
            try
            {
                List<ClientChecklistResponseBusinessModel> clientChecklistResponseBusinessModels = new List<ClientChecklistResponseBusinessModel>();
                foreach (ClientChecklistResponseViewModel cvm in clientChecklistResponse)
                {
                    ClientChecklistResponseBusinessModel crb = new ClientChecklistResponseBusinessModel();
                    crb.QuestionCode = cvm.QuestionCode;
                    crb.ActualFreeForm = cvm.ActualFreeForm;
                    crb.ActualResponse = cvm.ActualResponse;
                    crb.CheckListAttributeMapID = cvm.CheckListAttributeMapID;
                    crb.ChecklistName = cvm.ChecklistName;
                    crb.ClientCheckListMapID = cvm.ClientCheckListMapID;
                    crb.ExpectedRespone = cvm.ExpectedRespone;
                    crb.IsKPI = cvm.IsKPI;
                    crb.Questionid = cvm.Questionid;
                    crb.QuestionText = cvm.QuestionText;
                    crb.RequireFreeform = cvm.RequireFreeform;
                    clientChecklistResponseBusinessModels.Add(crb);
                }

                return _pendingChecklistRepository.SaveOrSubmitChecklistResponse(clientCode, pendingDate, isSubmit, clientChecklistResponseBusinessModels);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Get Completed Checklists For A Selected Date Range
        /// </summary>
        /// <param name="checklistDataRequest"></param>
        /// <returns></returns>
        public string GetCompletedChecklistsForADateRange(ChecklistDataRequestViewModel checklistDataRequest)
        {
            try
            {
                ChecklistDataRequest request = BusinessMapper.MappingOpenChecklistViewModelToBusinessModel(checklistDataRequest);
                List<ChecklistDataResponse> checklistDataList = _pendingChecklistRepository.GetChecklistsForADateRange(request);
                ChecklistDataResponse[] checklistDataArray = checklistDataList.ToArray();
                List<IDictionary<string, object>> pivotArray = checklistDataArray.ToPivotArray(item => item.EffectiveDate,
                  item => item.RowSelector,
                  items => items.Any() ? items.First().Answer : new AnswerResponse() { SubmittedResponse = "--" });
                foreach (IDictionary<string, object> ele in pivotArray)
                {
                    ChecklistDataResponse result = checklistDataList.First(c => c.RowSelector == (ele.ContainsKey("RowSelector") ? (string)ele["RowSelector"] : string.Empty));
                    if (result != null)
                    {
                        ele.Add("ChecklistName", result.ChecklistName);
                        ele.Add("QuestionText", result.QuestionText);
                    }
                }
                String json = JsonConvert.SerializeObject(pivotArray, new KeyValuePairConverter());
                return json;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Open the selected checklist i.e., Make checklist status as pending 
        /// </summary>
        /// <param name="checklistDataRequest"></param>
        /// <returns></returns>
        public ValidationViewModel OpenChecklist(ChecklistDataRequestViewModel checklistDataRequest)
        {
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                ChecklistDataRequest request = BusinessMapper.MappingOpenChecklistViewModelToBusinessModel(checklistDataRequest);
                businessResponse.IsSuccess = _pendingChecklistRepository.OpenChecklist(request);
                if (businessResponse.IsSuccess)
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CHECKLIST_OPEN_SUCCESS, MessageType = Infrastructure.Enums.MessageType.Info });
                }
                else
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CHECKLIST_OPEN_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                }
                return businessResponse.ToValidationViewModel();
            }
            catch (Exception ex)
            {
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CHECKLIST_OPEN_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return businessResponse.ToValidationViewModel();
            }
        }

        /// <summary>
        /// Get Client ChecklistType Data i.e. week & month effective dates
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<ChecklistDataRequestViewModel> GetClientChecklistTypeData(string clientCode)
        {
            try
            {
                List<ChecklistDataRequest> checklistTypeData = _pendingChecklistRepository.GetClientChecklistTypeData(clientCode);
                List<ChecklistDataRequestViewModel> checklistTypeViewModelData = new List<ChecklistDataRequestViewModel>();
                if (checklistTypeData != null && checklistTypeData.Count > 0)
                {
                    foreach (ChecklistDataRequest data in checklistTypeData)
                    {
                        ChecklistDataRequestViewModel checklistDataRequestViewModel = new ChecklistDataRequestViewModel();
                        checklistDataRequestViewModel = BusinessMapper.MappingOpenChecklistBusinessToViewModel(data);
                        checklistTypeViewModelData.Add(checklistDataRequestViewModel);
                    }
                }
                return checklistTypeViewModelData;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        #endregion public methods      
    }
}
