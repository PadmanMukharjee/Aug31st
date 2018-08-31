using M3Pact.BusinessModel;
using M3Pact.BusinessModel.Admin;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using System;
using System.Collections.Generic;

namespace M3Pact.Business.Admin
{
    public class ClientUserBusiness : IClientUserBusiness
    {
        #region Private Properties
        private IClientUserRepository _clientUserRepository;
        private ILogger _logger;
        #endregion Private Properties

        #region Constructor 
        public ClientUserBusiness(IClientUserRepository clientUserRepository, ILogger logger)
        {
            _clientUserRepository = clientUserRepository;
            _logger = logger;

        }

        #endregion Constructor 

        #region Public Methods
        /// <summary>
        /// To get all the Users data
        /// </summary>
        /// <returns></returns>
        public List<AllUsersViewModel> GetAllUsersData()
        {
            try
            {
                List<AllUsers> users = _clientUserRepository.GetAllUsersData();
                List<AllUsersViewModel> allUsers = new List<AllUsersViewModel>();
                if (users != null && users.Count > 0)
                {
                    foreach (AllUsers user in users)
                    {
                        allUsers.Add(BusinessMapper.AllUsersBusinessToAllUsesrViewModel(user));
                    }
                }
                return allUsers;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// To get all M3Users
        /// </summary>
        /// <returns></returns>
        public List<AllUsersViewModel> GetAllM3Users()
        {
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                List<AllUsers> users = _clientUserRepository.GetAllM3Users();
                List<AllUsersViewModel> allUsers = new List<AllUsersViewModel>();
                if (users != null && users.Count > 0)
                {
                    foreach (AllUsers user in users)
                    {
                        allUsers.Add(BusinessMapper.AllUsersBusinessToAllUsesrViewModel(user));
                    }
                }
                return allUsers;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.IsExceptionOccured = true;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.M3USERS_GET_FAIL, MessageType = Infrastructure.Enums.MessageType.Error });
                return null;
            }
        }

        /// <summary>
        /// To save the Client User association
        /// </summary>
        /// <param name="clientUserMapViewModel"></param>
        /// <returns></returns>
        public bool SaveClientUsers(ClientUserMapViewModel clientUserMapViewModel)
        {
            try
            {
                if (clientUserMapViewModel != null)
                {
                    ClientUserMap clientUserMap = new ClientUserMap();
                    clientUserMap.ClientCode = clientUserMapViewModel.ClientCode;
                    foreach (AllUsersViewModel clientUserViewModel in clientUserMapViewModel.ClientUsers)
                    {
                        AllUsers clientUser = new AllUsers();
                        clientUser.Email = clientUserViewModel.Email;
                        clientUser.RoleName = clientUserViewModel.Role;
                        clientUserMap.ClientUsers.Add(clientUser);
                    }
                    return _clientUserRepository.SaveClientUsers(clientUserMap);
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// To Get all the users associated with the client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientUserMapViewModel GetUsersForClient(string clientCode)
        {

            ClientUserMapViewModel clientUserMapViewModel = new ClientUserMapViewModel();
            try
            {
                ClientUserMap clientUserData = _clientUserRepository.GetUsersForClient(clientCode);
                clientUserMapViewModel.ClientCode = clientCode;
                foreach (AllUsers user in clientUserData?.ClientUsers)
                {
                    clientUserMapViewModel.ClientUsers.Add(BusinessMapper.AllUsersBusinessToAllUsesrViewModel(user));
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                clientUserMapViewModel.Success = false;
                clientUserMapViewModel.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return clientUserMapViewModel;
        }

        #endregion Public Methods       

    }
}
