using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.CheckList;
using M3Pact.BusinessModel.HeatMap;
using M3Pact.BusinessModel.Todo;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Checklist;
using M3Pact.ViewModel.Client;
using M3Pact.ViewModel.HeatMap;
using M3Pact.ViewModel.Todo;
using System.Collections.Generic;
using Infra = M3Pact.Infrastructure;

namespace M3Pact.Business
{
    public static class BusinessMapper
    {      
        /// <summary>
        /// To map M3User login data
        /// </summary>
        /// <param name="m3User"></param>
        /// <returns></returns>
        public static UserLoginDTO MappingM3UserViewModelToBusinessModel(M3UserViewModel m3User)
        {
            UserLoginDTO clientUserlogin = new UserLoginDTO();
            if (m3User != null)
            {
                clientUserlogin.UserId = m3User?.User.UserId;
                clientUserlogin.UserName = m3User?.User.UserName;
                clientUserlogin.FirstName = m3User?.User.FirstName;
                clientUserlogin.LastName = m3User?.User.LastName;
                clientUserlogin.MobileNumber = m3User?.User.MobileNumber;
                clientUserlogin.Email = m3User?.User.Email;
                clientUserlogin.IsMeridianUser = m3User?.User.IsMeridianUser;
                clientUserlogin.Password = m3User?.User.Password;
                clientUserlogin.RoleCode = m3User?.User.RoleCode;
                clientUserlogin.RoleName = m3User?.User.RoleName;
                clientUserlogin.RecordStatus = m3User.User.IsActive ? Infra.DomainConstants.RecordStatusActive : Infra.DomainConstants.RecordStatusInactive;
                clientUserlogin.LoggedInUserID = m3User.User.LoggedInUserID;
                clientUserlogin.Client = new List<ClientDetails>();
                if (clientUserlogin.RoleCode != Infra.BusinessConstants.Admin)
                {
                    foreach (string client in m3User.Clients)
                    {
                        ClientDetails clientDetails = new ClientDetails();
                        clientDetails.ClientCode = client;
                        clientUserlogin.Client.Add(clientDetails);
                    }
                }
            }
            return clientUserlogin;
        }

        /// <summary>
        /// Maps Holiday View Model to Holiday Business model
        /// </summary>
        /// <param name="holiday"></param>
        /// <returns></returns>
        public static Holiday MapHolidayViewModelToBusinessModel(HolidayViewModel holiday)
        {
            Holiday hld = new Holiday();
            if (holiday != null)
            {
                hld.HolidayDate = holiday.HolidayDate;
                hld.HolidayName = holiday.HolidayName;
                hld.DateKey = holiday.DateKey;
            }
            return hld;
        }

        /// <summary>
        /// Mapping view model to businessModel
        /// </summary>
        /// <param name="checkListItemViewModel"></param>
        /// <returns></returns>
        public static Question MappingChecklistItemViewModelToBusinessModel(CheckListItemViewModel checkListItemViewModel)
        {
            if (checkListItemViewModel != null)
            {
                Question checklistitem = new Question();
                checklistitem.ExpectedResponse = checkListItemViewModel.ExpectedResponse;
                checklistitem.IsFreeform = checkListItemViewModel.Freeform;
                checklistitem.IsKpi = checkListItemViewModel.Kpi;
                checklistitem.QuestionId = checkListItemViewModel.QuestionID;
                checklistitem.QuestionText = checkListItemViewModel.Question;
                checklistitem.IsUniversal = checkListItemViewModel.Universal;
                checklistitem.QuestionCode = checkListItemViewModel.QuestionCode;

                checklistitem.checkListType.CheckListTypeID = checkListItemViewModel.checkListType.CheckListTypeID;
                checklistitem.checkListType.CheckListTypeCode = checkListItemViewModel.checkListType.CheckListTypeCode;
                checklistitem.checkListType.CheckListTypeName = checkListItemViewModel.checkListType.CheckListTypeName;


                checklistitem.KPIDescription.KPIDescription = checkListItemViewModel.KPIDescription.KPIDescription;
                checklistitem.KPIDescription.Source = new CheckListType
                {
                    CheckListTypeID = checkListItemViewModel.checkListType.CheckListTypeID,
                    CheckListTypeCode = checkListItemViewModel.checkListType.CheckListTypeCode,
                    CheckListTypeName = checkListItemViewModel.checkListType.CheckListTypeName
                };
                checklistitem.KPIDescription.IsHeatMapItem = checkListItemViewModel.KPIDescription.IsHeatMapItem;
                checklistitem.KPIDescription.HeatMapScore = checkListItemViewModel.KPIDescription.HeatMapScore;
                checklistitem.KPIDescription.KPIAlert = new KPIAlert
                {
                    KPIAlertId = checkListItemViewModel.KPIDescription.KPIAlertId,
                    SendAlert = checkListItemViewModel.KPIDescription.SendAlert,
                    EscalateAlert = checkListItemViewModel.KPIDescription.EscalateAlert,
                    SendToRelationshipManager = checkListItemViewModel.KPIDescription.SendToRelationshipManager,
                    SendToBillingManager = checkListItemViewModel.KPIDescription.SendToBillingManager,
                    EscalateTriggerTime = checkListItemViewModel.KPIDescription.EscalateTriggerTime,
                };
                return checklistitem;
            }
            else
            {
                return new Question();
            }
        }

        /// <summary>
        /// Converting from All users business model to View model
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public static AllUsersViewModel AllUsersBusinessToAllUsesrViewModel(AllUsers user)
        {
            AllUsersViewModel userViewModel = new AllUsersViewModel();
            userViewModel.ID = user.ID;
            userViewModel.Email = user.Email;
            userViewModel.FirstName = user.FirstName;
            userViewModel.IsMeridianUser = user.IsMeridianUser;
            userViewModel.LastName = user.LastName;
            userViewModel.MobileNumber = user.MobileNumber;
            userViewModel.Role = user.RoleName;
            userViewModel.UserId = user.UserId;
            userViewModel.UserName = user.UserName;
            return userViewModel;
        }

        /// <summary>
        /// Business model to View model mapper
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public static List<AttributesViewModel> AttributesBusinessModelToAttributesViewModel(List<AttributesBusinessModel> attributes)
        {
            List<AttributesViewModel> attributesViewModels = new List<AttributesViewModel>();
            foreach (AttributesBusinessModel attr in attributes)
            {
                AttributesViewModel attribute = new AttributesViewModel();
                attribute.AttributeId = attr.AttributeId;
                attribute.AttributeCode = attr.AttributeCode;
                attribute.AttributeName = attr.AttributeName;
                attribute.AttributeDescription = attr.AttributeDescription;
                attribute.Control = attr.Control;
                attribute.RecordStatus = attr.RecordStatus;
                attribute.AttributeType = attr.AttributeType;
                attribute.AttributeValue = attr.AttributeValue;
                attributesViewModels.Add(attribute);
            }
            return attributesViewModels;
        }

        /// <summary>
        /// mapping domain model to business model.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static KPIViewModel MappingKPIBusinessToKPIViewModel(KPI item)
        {
            KPIViewModel kpiDTO = new KPIViewModel();
            kpiDTO.KPIID = item.KPIID;
            kpiDTO.KPIDescription = item.KPIDescription;
            kpiDTO.Source = new CheckListTypeViewModel();
            kpiDTO.Source.CheckListTypeID = item.Source.CheckListTypeID;
            kpiDTO.Source.CheckListTypeCode = item.Source.CheckListTypeCode;
            kpiDTO.Source.CheckListTypeName = item.Source.CheckListTypeName;
            kpiDTO.Source.RecordStatus = item.Source.RecordStatus;

            kpiDTO.Measure = new MeasureViewModel();
            kpiDTO.Measure.MeasureId = item.Measure.MeasureId;
            kpiDTO.Measure.MeasureText = item.Measure.MeasureText;
            kpiDTO.Measure.KPIID = item.KPIID;
            kpiDTO.Measure.Standard = item.Measure.Standard;
            kpiDTO.Measure.Universal = item.Measure.Universal;
            kpiDTO.Measure.MeasureCode = item.Measure.MeasureCode;
            kpiDTO.Measure.MeasureUnit = item.Measure.MeasureUnit;

            kpiDTO.Standard = item.Standard;
            kpiDTO.AlertLevel = item.AlertLevel;

            kpiDTO.KPIMeasure = new KPIMeasureViewModel();
            kpiDTO.KPIMeasure.KpimeasureId = item.KPIMeasure.KpimeasureId;
            kpiDTO.KPIMeasure.Measure = item.KPIMeasure.Measure;
            kpiDTO.KPIMeasure.CheckListTypeId = item.KPIMeasure.CheckListTypeId;
            kpiDTO.KPIMeasure.RecordStatus = item.KPIMeasure.RecordStatus;

            kpiDTO.IsHeatMapItem = item.IsHeatMapItem;
            kpiDTO.HeatMapScore = item.HeatMapScore;
            kpiDTO.IsUniversal = item.IsUniversal;
            kpiDTO.RecordStatus = item.RecordStatus;

            kpiDTO.KPIAlertId = item.KPIAlert.KPIAlertId;
            kpiDTO.SendAlert = item.KPIAlert.SendAlert;
            kpiDTO.SendAlertTitle = item.KPIAlert.SendAlertTitle;
            kpiDTO.SendToRelationshipManager = item.KPIAlert.SendToRelationshipManager;
            kpiDTO.SendToBillingManager = item.KPIAlert.SendToBillingManager;
            kpiDTO.EscalateAlert = item.KPIAlert.EscalateAlert;
            kpiDTO.EscalateAlertTitle = item.KPIAlert.EscalateAlertTitle;
            kpiDTO.EscalateTriggerTime = item.KPIAlert.EscalateTriggerTime;
            kpiDTO.IncludeKPITarget = item.KPIAlert.IncludeKPITarget;
            kpiDTO.IncludeDeviationTarget = item.KPIAlert.IncludeDeviationTarget;
            kpiDTO.IsSla = item.KPIAlert.IsSla;

            return kpiDTO;
        }

        /// <summary>
        /// Map ChecklistDataRequest View Model to ChecklistDataRequest Business Model 
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public static ChecklistDataRequest MappingOpenChecklistViewModelToBusinessModel(ChecklistDataRequestViewModel requestViewModel)
        {
            ChecklistDataRequest request = new ChecklistDataRequest();
            if (requestViewModel != null)
            {
                request.ChecklistType = requestViewModel.ChecklistType;
                request.FromDate = requestViewModel.FromDate;
                request.ToDate = requestViewModel.ToDate;
                request.ClientCode = requestViewModel.ClientCode;
            }
            return request;
        }

        /// <summary>
        /// Map ChecklistDataRequest Business Model to ChecklistDataRequest View Model
        /// </summary>
        /// <param name="checklistDataRequest"></param>
        /// <returns></returns>
        public static ChecklistDataRequestViewModel MappingOpenChecklistBusinessToViewModel(ChecklistDataRequest checklistDataRequest)
        {
            ChecklistDataRequestViewModel checklistDataRequestViewModel = new ChecklistDataRequestViewModel();
            if (checklistDataRequest != null)
            {
                checklistDataRequestViewModel.ChecklistType = checklistDataRequest.ChecklistType;
                checklistDataRequestViewModel.FromDate = checklistDataRequest.FromDate;
                checklistDataRequestViewModel.ToDate = checklistDataRequest.ToDate;
                checklistDataRequestViewModel.ClientCode = checklistDataRequest.ClientCode;
            }
            return checklistDataRequestViewModel;
        }

        /// <summary>
        /// Converting heat map view model to business model
        /// </summary>
        /// <param name="requestViewModel"></param>
        /// <returns></returns>
        public static ClientsHeatMapRequest MappingHeatMapViewModelToBusinessModel(ClientsHeatMapRequestViewModel requestViewModel)
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

        /// <summary>
        /// To get ClientPayerViewModel from ClientPayer DTO
        /// </summary>
        /// <param name="clientPayersDTO"></param>
        /// <returns></returns>
        public static List<ClientPayerViewModel> ConstructViewModelFromClientPayerDTO(List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO)
        {
            List<ClientPayerViewModel> clientPayers = new List<ClientPayerViewModel>();
            ClientPayerViewModel clientPayer;

            foreach (ClientPayer clientpayerDTO in clientPayersDTO)
            {
                clientPayer = new ClientPayerViewModel();
                clientPayer.PayerCode = clientpayerDTO.Payer.PayerCode;
                clientPayer.PayerName = clientpayerDTO.Payer.PayerName;
                clientPayer.IsM3FeeExempt = clientpayerDTO.IsM3feeExempt;
                clientPayer.RecordStatus = clientpayerDTO.RecordStatus;
                clientPayer.ID = clientpayerDTO.ID;
                clientPayer.IsEditable = clientpayerDTO.IsEditable;
                clientPayer.CanDelete = clientpayerDTO.CanDelete;
                clientPayers.Add(clientPayer);
            }
            return clientPayers;
        }

        /// <summary>
        /// mapping from  todo business model to view model
        /// </summary>
        /// <param name="todoBusinessModels"></param>
        /// <returns></returns>
        public static List<TodoViewModel> MappingTodoBusinessToViewModel(List<TodoBusinessModel> todoBusinessModels)
        {
            List<TodoViewModel> todoViewModels = new List<TodoViewModel>();
            foreach (TodoBusinessModel todoBusiness in todoBusinessModels)
            {
                TodoViewModel todoViewModel = new TodoViewModel();
                todoViewModel.ClientCode = todoBusiness.ClientCode;
                todoViewModel.ClientId = todoBusiness.ClientId;
                todoViewModel.ClientName = todoBusiness.ClientName;
                todoViewModel.TaskName = todoBusiness.TaskName;
                todoViewModels.Add(todoViewModel);
            }
            return todoViewModels;
        }
    }
}
