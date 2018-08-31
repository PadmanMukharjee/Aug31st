using M3Pact.BusinessModel.BusinessModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Payer;
using M3Pact.Infrastructure.Interfaces.Repository;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Client;
using M3Pact.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Business.Payer
{
    public class PayerBusiness : IPayerBusiness
    {
        #region Internal Properties
        private IPayerRepository _payerRepository;
        private ILogger _logger;
        #endregion Internal Properties

        #region Constructor
        public PayerBusiness(IPayerRepository payerResposiotory, ILogger logger)
        {
            _payerRepository = payerResposiotory;
            _logger = logger;
        }
        #endregion Constructor

        #region Public Methods
        /// <summary>
        /// Returns the Payers
        /// </summary>
        /// <returns></returns>
        public PayerViewModelList GetPayers(bool fromClient)
        {
            PayerViewModelList payerViewModelList = new PayerViewModelList();
            try
            {
                List<PayerViewModel> payers = new List<PayerViewModel>();
                List<BusinessModel.BusinessModels.Payer> payersDTO = _payerRepository.GetPayers();
                if (payersDTO != null && payersDTO.Count > 0)
                {
                    payers = ConstructViewModelFromPayerDTO(payersDTO);
                    payerViewModelList.ListOfPayerViewModel = fromClient ? payers.OrderBy(p => p.PayerName).ToList() : payers;
                    payerViewModelList.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                payerViewModelList.Success = false;
                payerViewModelList.IsExceptionOccured = true;
                payerViewModelList.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return payerViewModelList;
        }

        /// <summary>
        /// Get Active & UnAssigned Payers to a client
        /// </summary>
        /// <returns></returns>
        public PayerViewModelList GetActivePayersToAssignForClient(string clientCode)
        {
            PayerViewModelList payerViewModelList = new PayerViewModelList();
            try
            {
                List<PayerViewModel> payers = new List<PayerViewModel>();
                List<BusinessModel.BusinessModels.Payer> payersDTO = _payerRepository.GetActivePayersToAssignForClient(clientCode);
                if (payersDTO != null && payersDTO.Count > 0)
                {
                    payers = ConstructViewModelFromPayerDTO(payersDTO);
                    payerViewModelList.ListOfPayerViewModel = payers;
                    payerViewModelList.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                payerViewModelList.Success = false;
                payerViewModelList.IsExceptionOccured = true;
                payerViewModelList.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return payerViewModelList;
        }

        /// <summary>
        /// To get the top payers data.
        /// </summary>
        /// <param name="clientCode"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public PayerDataViewModelList GetTopPayersData(string clientCode, int month, int year)
        {
            PayerDataViewModelList payerDataViewModelList = new PayerDataViewModelList();
            try
            {                
                List<ClientPayer> payerData = _payerRepository.GetTopPayersData(clientCode, month, year);
                if (payerData != null)
                {
                    List<PayerDataViewModel>  payers = new List<PayerDataViewModel>();
                    foreach (BusinessModel.BusinessModels.ClientPayer payer in payerData)
                    {
                        payers.Add(new PayerDataViewModel()
                        {
                            PayerCode = payer.Payer.PayerCode,
                            PayerName = payer.Payer.PayerName,
                            Amount = payer.DepositLog.First().Amount
                        });
                    }
                    payerDataViewModelList.ListOfPayerDataViewModel = payers;
                    payerDataViewModelList.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                payerDataViewModelList.Success = false;
                payerDataViewModelList.IsExceptionOccured = true;
                payerDataViewModelList.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return payerDataViewModelList;
        }

        /// <summary>
        /// Saves the Payers
        /// </summary>
        /// <param name="payers"></param>
        /// <returns></returns>
        public ValidationViewModel SavePayers(List<PayerViewModel> payers)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                if (payers != null && payers.Count > 0)
                {
                    List<BusinessModel.BusinessModels.Payer> payersDTO = new List<BusinessModel.BusinessModels.Payer>();
                    BusinessModel.BusinessModels.Payer payerDTO;
                    foreach (PayerViewModel payer in payers)
                    {
                        payerDTO = new BusinessModel.BusinessModels.Payer();
                        payerDTO.PayerCode = payer.PayerCode.Trim();
                        payerDTO.PayerName = payer.PayerName.Trim();
                        payerDTO.PayerDescription = payer.PayerDescription.Trim();
                        payerDTO.RecordStatus = payer.RecordStatus;
                        payerDTO.ID = payer.ID;
                        payersDTO.Add(payerDTO);
                    }
                    _payerRepository.SavePayers(payersDTO);
                    validationViewModel.Success = true;
                    validationViewModel.SuccessMessage = BusinessConstants.SaveSuccess;
                }
                else
                {
                    validationViewModel.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.IsExceptionOccured = true;
                validationViewModel.ErrorMessages.Add(BusinessConstants.ERROR_SAVE_DETAILS);
            }
            return validationViewModel;
        }

        /// <summary>
        /// Get Assigned Payers of a client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientPayerViewModelList GetClientPayers(string clientCode)
        {
            ClientPayerViewModelList clientPayerViewModelList = new ClientPayerViewModelList();
            try
            {
                List<ClientPayerViewModel> clientPayers = new List<ClientPayerViewModel>();
                List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO = _payerRepository.GetClientPayers(clientCode);
                if (clientPayersDTO != null && clientPayersDTO.Count > 0)
                {
                    clientPayers =BusinessMapper.ConstructViewModelFromClientPayerDTO(clientPayersDTO);
                    clientPayerViewModelList.ListOfClientPayerViewModel = clientPayers;
                    clientPayerViewModelList.Success = true;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                clientPayerViewModelList.Success = false;
                clientPayerViewModelList.IsExceptionOccured = true;
                clientPayerViewModelList.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return clientPayerViewModelList;
        }

        /// <summary>
        /// Save the Client Payers
        /// </summary>
        /// <param name="clientPayers"></param>
        /// <returns></returns>
        public ValidationViewModel SaveClientPayers(List<ClientPayerViewModel> clientPayers)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                if (clientPayers != null && clientPayers.Count > 0)
                {
                    List<BusinessModel.BusinessModels.ClientPayer> clientPayersDTO = new List<BusinessModel.BusinessModels.ClientPayer>();
                    BusinessModel.BusinessModels.ClientPayer clientPayerDTO;
                    foreach (ClientPayerViewModel clientPayer in clientPayers)
                    {
                        clientPayerDTO = new BusinessModel.BusinessModels.ClientPayer();
                        clientPayerDTO.ID = clientPayer.ID;
                        clientPayerDTO.IsM3feeExempt = clientPayer.IsM3FeeExempt;
                        clientPayerDTO.Payer.PayerCode = clientPayer.PayerCode;
                        clientPayerDTO.RecordStatus = clientPayer.RecordStatus;
                        clientPayerDTO.ClientCode = clientPayer.ClientCode;
                        clientPayersDTO.Add(clientPayerDTO);
                    }
                    validationViewModel.Success = _payerRepository.SaveClientPayers(clientPayersDTO);
                }
                else
                {
                    validationViewModel.Success = false;
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.IsExceptionOccured = true;
                validationViewModel.ErrorMessages.Add(BusinessConstants.ERROR_SAVE_DETAILS);
            }
            return validationViewModel;
        }

        /// <summary>
        /// To get the clients assigned to payer.
        /// </summary>
        /// <param name="payerCode"></param>
        /// <param name="isRecordStatus"></param>
        /// <returns></returns>
        public StringList GetClientsAssignedtoPayer(string payerCode, bool isRecordStatus = true)
        {
            StringList listOfString = new StringList();
            try
            {
                listOfString.ListOfStrings = _payerRepository.GetClientsAssignedtoPayer(payerCode, isRecordStatus);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                listOfString.Success = false;
                listOfString.IsExceptionOccured = true;
                listOfString.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return listOfString;
        }

        /// <summary>
        /// To active or Inactive payers.
        /// </summary>
        /// <param name="payerViewModel"></param>
        public ValidationViewModel ActivateOrDeactivatePayer(PayerViewModel payerViewModel)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                BusinessModel.BusinessModels.Payer payer = new BusinessModel.BusinessModels.Payer();
                payer.PayerCode = payerViewModel.PayerCode;
                _payerRepository.ActivateOrDeactivatePayer(payer);
                validationViewModel.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.IsExceptionOccured = true;
                validationViewModel.ErrorMessages.Add(BusinessConstants.ERROR_SAVE_DETAILS);
            }
            return validationViewModel;
        }

        /// <summary>
        /// To activate or inactivate Client payer
        /// </summary>
        /// <param name="clientPayer"></param>
        public ValidationViewModel ActivateOrDeactivateClientPayer(ClientPayerViewModel clientPayer)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                ClientPayer clientPayerDTO = new ClientPayer()
                {
                    ID = clientPayer.ID,
                    RecordStatus = clientPayer.RecordStatus
                };
                _payerRepository.ActivateOrDeactivateClientPayer(clientPayerDTO);
                validationViewModel.Success = true;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.IsExceptionOccured = true;
                validationViewModel.ErrorMessages.Add(BusinessConstants.ERROR_SAVE_DETAILS);
            }
            return validationViewModel;
        }

        #endregion Public Methods

        #region Mapper Methods

        /// <summary>
        /// To get PayerViewModel from Payer DTO
        /// </summary>
        /// <param name="payersDTO"></param>
        /// <returns></returns>
        public List<PayerViewModel> ConstructViewModelFromPayerDTO(List<BusinessModel.BusinessModels.Payer> payersDTO)
        {
            List<PayerViewModel> payers = new List<PayerViewModel>();
            foreach (BusinessModel.BusinessModels.Payer payerDTO in payersDTO)
            {
                PayerViewModel payer = new PayerViewModel();
                payer.PayerCode = payerDTO.PayerCode;
                payer.PayerName = payerDTO.PayerName;
                payer.PayerDescription = payerDTO.PayerDescription;
                payer.RecordStatus = payerDTO.RecordStatus;
                payer.ID = payerDTO.ID;
                StringList listOfString = new StringList();
                listOfString = GetClientsAssignedtoPayer(payer.PayerCode, false);
                payer.Clients = listOfString.ListOfStrings;
                payers.Add(payer);
            }
            return payers;
        }       

        #endregion Mapper Methods
    }
}
