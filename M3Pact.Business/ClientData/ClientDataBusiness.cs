using M3Pact.BusinessModel;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Client;
using M3Pact.BusinessModel.Mapper;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Business.ClientData;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.ClientData;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace M3Pact.Business.ClientData
{
    public class ClientDataBusiness : IClientDataBusiness
    {
        #region private properties
        private IClientDataRepository _clientDataRepository;
        private IHostingEnvironment _hostingEnvironment;
        private ILogger _logger;
        private ISiteRepository _siteRepository;
        #endregion properties

        #region constructor
        public ClientDataBusiness(IClientDataRepository clientDataRepository,
            IHostingEnvironment hostingEnvironment,
            ILogger logger,
            ISiteRepository siteRepository)
        {
            _clientDataRepository = clientDataRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            _siteRepository = siteRepository;
        }
        #endregion constructor

        #region public methods
        /// <summary> 
        /// To get the client Data depending upon ClienCode
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>      
        public ClientViewModel GetClientData(string clientCode)
        {
            try
            {
                ClientDetails clientDetails = _clientDataRepository.GetClientData(clientCode);
                if (clientDetails != null)
                {
                    return new ClientViewModel()
                    {
                        Acronym = clientDetails.Acronym,
                        BusinessUnit = new BusinessUnitViewModel()
                        {
                            BusinessUnitCode = clientDetails.BusinessUnitDetails?.BusinessUnitCode,
                            BusinessUnitName = clientDetails.BusinessUnitDetails?.BusinessUnitName,
                            ID = (int)clientDetails.BusinessUnitDetails?.ID
                        },
                        ClientCode = clientDetails.ClientCode,
                        ContactEmail = clientDetails.ContactEmail,
                        ContactName = clientDetails.ContactName,
                        ContactPhone = clientDetails.ContactPhone,
                        ContractEndDate = clientDetails.ContractEndDate,
                        ContractStartDate = clientDetails.ContractStartDate,
                        IsActive = clientDetails.IsActive,
                        FlatFee = clientDetails.FlatFee,
                        Name = clientDetails.Name,
                        NoticePeriod = clientDetails.NoticePeriod,
                        NumberOfProviders = clientDetails.NumberOfProviders,
                        ContractFilePath = clientDetails.ContractFilePath,
                        ContractFileName = GetFileNameFromFilePath(clientDetails.ContractFilePath),
                        PercentageOfCash = clientDetails.PercentageOfCash,
                        Speciality = new SpecialityViewModel()
                        {
                            SpecialityCode = clientDetails.Speciality?.SpecialityCode,
                            SpecialityName = clientDetails.Speciality?.SpecialityName,
                            ID = (int)clientDetails.Speciality?.ID,
                            SpecialityDescription = clientDetails.Speciality?.SpecialityDescription
                        },
                        System = new ViewModel.Admin.SystemViewModel()
                        {
                            SystemCode = clientDetails.System.SystemCode,
                            SystemName = clientDetails.System.SystemName,
                            ID = (int)clientDetails.System?.ID
                        },
                        Site = clientDetails.Site,
                        MonthlyChecklist = clientDetails.MonthlyCheckList,
                        WeeklyChecklist = clientDetails.WeeklyCheckList,
                        RelationShipManager = new AllUsersViewModel() { Email = clientDetails.RelationShipManager.Email },
                        BillingManager = new AllUsersViewModel() { Email = clientDetails.BillingManager.Email },
                        WeeklyChecklistEffectiveDate = clientDetails.WeeklyChecklistEffectiveDate,
                        MonthlyChecklistEffectiveDate = clientDetails.MonthlyChecklistEffectiveDate,
                        SendAlertsUsers = clientDetails.SendAlertsUsers
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To upate and save client data into database
        /// </summary>
        /// <param name="clientData"></param>
        public ValidationViewModel SaveClientData(ClientViewModel clientData)
        {
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                if (clientData != null && clientData.IsNewClient)
                {
                    clientData.IsActive = BusinessConstants.PartialCompleted;
                    bool isClientExists = CheckForExistingClientCode(clientData.ClientCode);
                    if (isClientExists)
                    {
                        businessResponse.IsExceptionOccured = false;
                        businessResponse.IsSuccess = false;
                        businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CLIENT_CODE_EXISTS, MessageType = Infrastructure.Enums.MessageType.Error });
                        return businessResponse.ToValidationViewModel();
                    }
                }

                ClientDetails clientDetails = new ClientDetails();
                clientDetails.Acronym = clientData.Acronym;
                clientDetails.BusinessUnitDetails = new BusinessUnit() { BusinessUnitCode = clientData.BusinessUnit?.BusinessUnitCode };
                clientDetails.ClientCode = clientData.ClientCode;
                clientDetails.ContactEmail = clientData.ContactEmail;
                clientDetails.ContactName = clientData.ContactName;
                clientDetails.ContactPhone = clientData.ContactPhone;
                clientDetails.ContractEndDate = clientData.ContractEndDate;
                clientDetails.ContractStartDate = clientData.ContractStartDate;
                clientDetails.FlatFee = clientData.FlatFee;
                clientDetails.Name = clientData.Name;
                clientDetails.ContractFilePath = clientData.Contract != null ? SaveContractFile(clientData.Contract) : clientData.ContractFilePath;
                //clientDetails.//MonthlyCheckList need to map
                //clientDetails.//MonthlyCheckList need to map
                clientDetails.MonthlyCheckList = clientData.MonthlyChecklist;
                clientDetails.WeeklyCheckList = clientData.WeeklyChecklist;
                clientDetails.NoticePeriod = clientData.NoticePeriod;
                clientDetails.NumberOfProviders = clientData.NumberOfProviders;
                clientDetails.PercentageOfCash = clientData.PercentageOfCash;
                clientDetails.IsActive = clientData.IsActive;
                clientDetails.RelationShipManager = new BusinessModel.Admin.AllUsers() { Email = clientData.RelationShipManager.Email };
                clientDetails.BillingManager = new BusinessModel.Admin.AllUsers() { Email = clientData.BillingManager.Email };
                clientDetails.Speciality = new Speciality() { SpecialityCode = clientData.Speciality?.SpecialityCode };
                clientDetails.System = new BusinessModel.Admin.System() { SystemCode = clientData.System?.SystemCode };
                clientDetails.Site = clientData.Site;
                clientDetails.SendAlertsUsers = clientData.SendAlertsUsers;

                businessResponse.IsSuccess = _clientDataRepository.SaveClientData(clientDetails);
                if (businessResponse.IsSuccess)
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CLIENT_DATA_SAVE_SUCCESS, MessageType = Infrastructure.Enums.MessageType.Info });
                }
                else
                {
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CLIENT_DATA_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                }
                return businessResponse.ToValidationViewModel();

            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.CLIENT_DATA_SAVE_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                return businessResponse.ToValidationViewModel();
            }
        }

        /// <summary>
        /// To get document depending on filepath
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetClientDocument(string fileName)
        {
            byte[] requiredDocument = new byte[] { };
            try
            {
                string folderName = BusinessConstants.Contract;
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                string fullPath = Path.Combine(newPath, fileName);
                if (!string.IsNullOrEmpty(fullPath))
                {
                    requiredDocument = File.ReadAllBytes(fullPath);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return requiredDocument;
        }

        /// <summary>
        /// To Save the Clients Monthly Target Data through the Quick mode
        /// </summary>
        /// <param name="clientTargetData"></param>
        /// <returns></returns>
        public List<ClientTargetViewModel> SaveClientTargetData(ClientTargetViewModel clientTargetData)
        {
            try
            {
                List<ClientTargetModel> clientTargetModels = _clientDataRepository.SaveClientTargetData(new ClientTargetModel()
                {
                    Client = new ClientDetails() { ClientCode = clientTargetData.ClientCode },
                    ClientId = 0,
                    CalendarYear = clientTargetData.CalendarYear,
                    AnnualCharges = clientTargetData.AnnualCharges,
                    GrossCollectionRate = clientTargetData.GrossCollectionRate,
                });
                List<ClientTargetViewModel> clientTargetViewModels = new List<ClientTargetViewModel>();
                foreach (ClientTargetModel ctm in clientTargetModels)
                {
                    ClientTargetViewModel ctvm = new ClientTargetViewModel();
                    ctvm.AnnualCharges = ctm.AnnualCharges;
                    ctvm.CalendarYear = ctm.CalendarYear;
                    ctvm.GrossCollectionRate = ctm.GrossCollectionRate;
                    ctvm.Month = ctm.Month.MonthName;
                    ctvm.Charges = ctm.Charges;
                    ctvm.Payments = ctm.Payments;
                    ctvm.Revenue = ctm.Revenue;
                    clientTargetViewModels.Add(ctvm);
                }
                return clientTargetViewModels;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get the Client's Monthly Target Data
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public List<ClientTargetViewModel> GetClientTargetData(string clientCode, int year)
        {
            List<ClientTargetViewModel> clientTargetViewModels = new List<ClientTargetViewModel>();
            try
            {
                List<ClientTargetModel> clientTargetModels = _clientDataRepository.GetClientTargetData(clientCode, year);

                foreach (ClientTargetModel ctm in clientTargetModels)
                {
                    ClientTargetViewModel ctvm = new ClientTargetViewModel();
                    ctvm.AnnualCharges = ctm.AnnualCharges;
                    ctvm.CalendarYear = ctm.CalendarYear;
                    ctvm.GrossCollectionRate = ctm.GrossCollectionRate;
                    ctvm.Month = ctm.Month.MonthName;
                    ctvm.Charges = ctm.Charges;
                    ctvm.Payments = ctm.Payments;
                    ctvm.Revenue = ctm.Revenue;
                    ctvm.IsManualEntry = ctm.IsManualEntry;
                    clientTargetViewModels.Add(ctvm);
                }
                return clientTargetViewModels;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Saving the edited targets in Grid view
        /// </summary>
        /// <param name="manuallyEditedTargets"></param>
        public ValidationViewModel SaveManuallyEditedTargetData(ManuallyEditedTargets manuallyEditedTargets)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                List<ChargesBusinessModel> chargesBusinessModels = new List<ChargesBusinessModel>();
                foreach (Charges c in manuallyEditedTargets.charges)
                {
                    ChargesBusinessModel cb = new ChargesBusinessModel();
                    cb.name = c.name;
                    cb.value = c.value;
                    chargesBusinessModels.Add(cb);
                }

                List<PaymentsBusinessModel> paymentsBusinessModels = new List<PaymentsBusinessModel>();

                foreach (Payments p in manuallyEditedTargets.payments)
                {
                    PaymentsBusinessModel pb = new PaymentsBusinessModel();
                    pb.name = p.name;
                    pb.value = p.value;
                    paymentsBusinessModels.Add(pb);
                }

                List<RevenueBusinessModel> revenueBusinessModels = new List<RevenueBusinessModel>();

                foreach (Revenue r in manuallyEditedTargets.revenue)
                {
                    RevenueBusinessModel rb = new RevenueBusinessModel();
                    rb.name = r.name;
                    rb.value = r.value;
                    revenueBusinessModels.Add(rb);
                }


                ManuallyEditedTargetsBusinessModel manuallyEditedTargetsbusiness = new ManuallyEditedTargetsBusinessModel()
                {
                    charges = chargesBusinessModels,
                    payments = paymentsBusinessModels,
                    revenue = revenueBusinessModels,
                    clientCode = manuallyEditedTargets.clientCode,
                    year = manuallyEditedTargets.year
                };
                _clientDataRepository.SaveManuallyEditedTargetData(manuallyEditedTargetsbusiness);
                validationViewModel.Success = true;
                return validationViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                return validationViewModel;
            }
        }

        /// <summary>
        /// to get client step status details
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public List<ClientStepDetailViewModel> GetClientStepStatusDetails(string clientCode)
        {

            List<ClientStepDetailViewModel> stepStatusDetails = new List<ClientStepDetailViewModel>();
            try
            {
                List<ClientStepStatusDetail> stepStatusDetailsDTO = GetStepStausDetailsOfClient(clientCode);
                if (stepStatusDetailsDTO != null && stepStatusDetailsDTO.Count > 0)
                {
                    stepStatusDetails = ConstructViewModelFromStepStatusDetailDTO(stepStatusDetailsDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return stepStatusDetails;
        }

        /// <summary>
        /// to save client step details
        /// </summary>
        /// <param name="stepDetail"></param>
        /// <returns></returns>
        public bool SaveClientStepStatusDetail(ClientStepDetailViewModel stepDetail)
        {
            try
            {
                ClientStepStatusDetail clientStepDetailDTO = new ClientStepStatusDetail();
                if (stepDetail != null)
                {
                    clientStepDetailDTO.ID = stepDetail.StepDetailID;
                    clientStepDetailDTO.ClientCode = stepDetail.ClientCode;
                    clientStepDetailDTO.StepID = stepDetail.StepID;
                    clientStepDetailDTO.StepStatusID = stepDetail.StepStatusID;
                }
                return _clientDataRepository.SaveClientStepStatusDetail(clientStepDetailDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To get the data of all client data.
        /// </summary>
        /// <returns></returns>
        public List<ClientsDataViewModel> GetAllClientsData()
        {
            try
            {
                List<ClientsDataViewModel> clientList = null;
                List<ClientsData> clientsDataList = _clientDataRepository.GetAllClientsData();
                if (clientsDataList != null)
                {
                    clientList = new List<ClientsDataViewModel>();
                    foreach (ClientsData clientData in clientsDataList)
                    {
                        ClientsDataViewModel client = new ClientsDataViewModel();
                        client.ClientId = clientData.ClientId;
                        client.ClientCode = clientData.ClientCode;
                        client.ClientName = clientData.ClientName;
                        client.Site = clientData.Site;
                        client.RelationshipManager = clientData.RelationshipManager;
                        client.BillingManager = clientData.BillingManager;
                        client.MTDDeposit = clientData.MTDDeposit;
                        client.MTDTarget = clientData.MTDTarget;
                        client.ProjectedCash = clientData.ProjectedCash;
                        client.MonthlyTarget = clientData.MonthlyTarget;
                        client.ActualM3Revenue = clientData.ActualM3Revenue;
                        client.ForecastedM3Revenue = clientData.ForecastedM3Revenue;
                        switch (clientData.Status)
                        {
                            case DomainConstants.RecordStatusActive:
                                client.Status = DomainConstants.Active;
                                break;
                            case DomainConstants.RecordStatusInactive:
                                client.Status = DomainConstants.InActive;
                                break;
                            case DomainConstants.RecordStatusPartial:
                                client.Status = DomainConstants.PartiallyCompleted;
                                break;
                        }

                        clientList.Add(client);
                    }
                }
                return clientList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// get completed steps
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public int GetNoOfCompletedStepsOfClientConfiguration(string clientCode)
        {
            try
            {
                List<ClientStepStatusDetail> stepDetails = GetStepStausDetailsOfClient(clientCode);
                int completedSteps = stepDetails.FindAll(x => x.StepStatusID == DomainConstants.ClientStepCompleted).Count;
                return completedSteps;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return 0;
            }
        }

        /// <summary>
        /// Get Active clients of a user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ClientsDataViewModel> GetActiveClientsForAUser()
        {
            UserContext userContext = UserHelper.getUserContext();
            List<ClientsDataViewModel> activeClients = null;
            try
            {
                List<ClientDetails> activeClientsList = _clientDataRepository.GetClientsByUser(userContext.UserId, userContext.Role);
                if (activeClientsList != null)
                {
                    activeClients = new List<ClientsDataViewModel>();
                    foreach (ClientDetails activeClient in activeClientsList)
                    {
                        ClientsDataViewModel client = new ClientsDataViewModel();
                        client.ClientCode = activeClient.ClientCode;
                        client.ClientName = activeClient.Name;
                        activeClients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return activeClients;
        }

        /// <summary>
        /// Get clients of a user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<ClientViewModel> GetClientsByUser()
        {
            List<ClientViewModel> activeClients = new List<ClientViewModel>();
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<ClientDetails> activeClientsList = _clientDataRepository.GetClientsByUser(userContext.UserId, userContext.Role);

                if (activeClientsList != null)
                {
                    foreach (ClientDetails activeClient in activeClientsList)
                    {
                        ClientViewModel client = new ClientViewModel();
                        client.ClientCode = activeClient.ClientCode;
                        client.Name = activeClient.Name;
                        activeClients.Add(client);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return activeClients;
        }

        /// <summary>
        /// Check if given client code exists or not
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public bool CheckForExistingClientCode(string clientCode)
        {
            try
            {
                return _clientDataRepository.CheckForExistingClientCode(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To activate ClientData
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public bool ActivateClient(string clientCode)
        {
            try
            {
                DateTime contractStartDate = _clientDataRepository.GetClientContractStartDate(clientCode);
                DateTime date = DateTime.Today < contractStartDate ? contractStartDate : DateTime.Today;
                DateTime checklistEffectiveWeek = DateHelper.GetMondayOfEffectiveWeek(date);
                DateTime checklistEffectiveMonth = DateHelper.GetFirstDayOfEffectiveMonth(date);
                return _clientDataRepository.ActivateClient(clientCode, checklistEffectiveWeek, checklistEffectiveMonth);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To get all the sites.
        /// </summary>
        /// <returns></returns>
        public List<KeyValueModel> GetAllSites()
        {
            List<KeyValueModel> allSitesKeyValue = null;
            try
            {
                List<Site> listOfSites = _siteRepository.GetSites(false);
                if (listOfSites?.Count > 0)
                {
                    allSitesKeyValue = new List<KeyValueModel>();
                    foreach (Site site in listOfSites)
                    {
                        KeyValueModel keyValue = new KeyValueModel();
                        keyValue.Key = site.SiteName;
                        keyValue.Value = site.SiteName;
                        allSitesKeyValue.Add(keyValue);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return allSitesKeyValue;
        }

        /// <summary>
        /// To get the client history when start date and end date are given.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<ClientHistoryViewModel> GetClientHistory(string clientCode, DateTime startDate, DateTime endDate)
        {
            List<ClientHistoryViewModel> clientHistoryViewModelList = null;
            try
            {
                List<ClientHistory> clientHistoryList = _clientDataRepository.GetClientHistory(clientCode, startDate, endDate);
                if (clientHistoryList != null)
                {
                    clientHistoryViewModelList = new List<ClientHistoryViewModel>();
                    foreach (ClientHistory clientHistory in clientHistoryList)
                    {
                        ClientHistoryViewModel clientHistoryViewModel = new ClientHistoryViewModel();
                        clientHistoryViewModel.ModifiedOrAddedBy = clientHistory.ModifiedOrAddedBy;
                        clientHistoryViewModel.ModifiedOrAddedDate = clientHistory.ModifiedOrAddedDate;
                        clientHistoryViewModel.UpdatedOrAddedProperty = clientHistory.UpdatedOrAddedProperty;
                        clientHistoryViewModel.NewValue = clientHistory.NewValue;
                        clientHistoryViewModel.OldValue = clientHistory.OldValue;
                        clientHistoryViewModel.Action = clientHistory.Action;
                        clientHistoryViewModelList.Add(clientHistoryViewModel);
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return clientHistoryViewModelList;
        }

        /// <summary>
        /// To get the client created date and created user.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public Dictionary<string, object> GetClientCreationDetails(string clientCode)
        {
            Dictionary<string, object> clientDetails = null;
            try
            {
                clientDetails = _clientDataRepository.GetClientCreationDetails(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
            return clientDetails;
        }

        #endregion public methods

        #region Mapper Methods

        /// <summary>
        /// To get ClientStepDetailViewModel from ClientStepStatusDetail DTO
        /// </summary>
        /// <param name="stepStatusDetailsDTO"></param>
        /// <returns></returns>
        private List<ClientStepDetailViewModel> ConstructViewModelFromStepStatusDetailDTO(List<ClientStepStatusDetail> stepStatusDetailsDTO)
        {
            List<ClientStepDetailViewModel> clientStepStatusDetails = new List<ClientStepDetailViewModel>();
            ClientStepDetailViewModel clientStepStatusDetail;

            foreach (ClientStepStatusDetail stepStatusDetailDTO in stepStatusDetailsDTO)
            {
                clientStepStatusDetail = new ClientStepDetailViewModel();
                clientStepStatusDetail.StepID = stepStatusDetailDTO.StepID;
                clientStepStatusDetail.StepName = stepStatusDetailDTO.StepName;
                clientStepStatusDetail.StepStatusID = stepStatusDetailDTO.StepStatusID;
                clientStepStatusDetail.StepDetailID = stepStatusDetailDTO.ID;
                clientStepStatusDetail.CanView = stepStatusDetailDTO.CanView;
                clientStepStatusDetail.CanEdit = stepStatusDetailDTO.CanEdit;
                clientStepStatusDetails.Add(clientStepStatusDetail);
            }

            ClientStepDetailViewModel isInProgressStep = clientStepStatusDetails.Find(x => x.StepStatusID == DomainConstants.ClientStepInProgress);
            if (isInProgressStep == null)
            {
                int progressStepIndex = clientStepStatusDetails.FindIndex(x => x.StepStatusID == BusinessConstants.CLIENTSTEP_STATUS_NEW);
                if (progressStepIndex != -1)
                {
                    clientStepStatusDetails[progressStepIndex].StepStatusID = DomainConstants.ClientStepInProgress;
                }
            }

            return clientStepStatusDetails;
        }
        #endregion Mapper Methods

        #region private methods
        private string SaveContractFile(Microsoft.AspNetCore.Http.IFormFile contractFile)
        {
            string folderName = BusinessConstants.Contract;
            string webRootPath = _hostingEnvironment.WebRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            if (contractFile.Length > 0)
            {
                string fileName = DateTime.Now.Ticks + "_" + ContentDispositionHeaderValue.Parse(contractFile.ContentDisposition).FileName.Trim('"');
                string fullPath = Path.Combine(newPath, fileName);
                using (FileStream stream = new FileStream(fullPath, FileMode.Create))
                {
                    contractFile.CopyTo(stream);
                }
                return fullPath;
            }
            return null;
        }

        /// <summary>
        /// Get FileName From FilePath
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private string GetFileNameFromFilePath(string filePath)
        {
            string fileName = string.Empty;
            filePath = "@" + filePath;
            fileName = Path.GetFileName(filePath);
            return fileName;
        }

        /// <summary>
        /// to get step status details of client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        private List<ClientStepStatusDetail> GetStepStausDetailsOfClient(string clientCode)
        {
            try
            {
                return _clientDataRepository.GetClientStepStausDetails(clientCode);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }
        #endregion private methods
    }
}
