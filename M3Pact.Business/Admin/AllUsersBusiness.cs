using M3Pact.BusinessModel;
using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.BusinessModels;
using M3Pact.BusinessModel.Common;
using M3Pact.BusinessModel.Mapper;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Enums;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.ClientData;
using M3Pact.LoggerUtility;
using M3Pact.ViewModel;
using M3Pact.ViewModel.Admin;
using M3Pact.ViewModel.Common;
using System;
using System.Collections.Generic;
using Infra = M3Pact.Infrastructure;

namespace M3Pact.Business.Admin
{
    public class AllUsersBusiness : IAllUsersBusiness
    {
        private IAllUserRepository _allUserRepository;
        private IClientDataRepository _clientDataRepository;
        private Common.Helper _helper;
        private ILogger _logger;

        public AllUsersBusiness(IAllUserRepository allUserRepository, IClientDataRepository clientDataRepository,
            Common.Helper helper, ILogger logger)
        {
            _allUserRepository = allUserRepository;
            _clientDataRepository = clientDataRepository;
            _helper = helper;
            _logger = logger;

        }

        #region Public Methods
        /// <summary>
        /// Get all users 
        /// </summary>
        /// <param name="isActiveUser"></param>
        /// <returns></returns>
        public List<AllUsersViewModel> GetAllUsersDataAndClientsAssigned(bool isActiveUser)
        {
            try
            {
                List<AllUsersViewModel> users = new List<AllUsersViewModel>();
                List<int> clientsIDsOfLoggedInUser = _allUserRepository.GetClientIDsOfLoggedInUser();
                List<AllUsers> usersDTO = _allUserRepository.GetAllUsersDataAndClientsAssigned(clientsIDsOfLoggedInUser, isActiveUser);
                foreach (AllUsers userDTO in usersDTO)
                {
                    AllUsersViewModel user = new AllUsersViewModel();
                    user.UserId = userDTO.UserId;
                    user.FirstName = userDTO.FirstName;
                    user.LastName = userDTO.LastName;
                    user.MobileNumber = userDTO.MobileNumber;
                    user.Email = userDTO.Email;
                    user.Title = userDTO.Title;
                    user.BusinessUnit = userDTO.BusinessUnit;
                    if (String.IsNullOrEmpty(userDTO.ReportsTo) || userDTO.ReportsTo == " ")
                    {
                        user.ReportsTo = userDTO.ReportsTo;
                    }
                    else
                    {
                        user.ReportsTo = _allUserRepository.GetReportsToOfEmployee(userDTO.ReportsTo);
                    }
                    user.Site = userDTO.Site;
                    user.IsMeridianUser = userDTO.IsMeridianUser;
                    user.Role = userDTO.RoleName;
                    user.RecordStatus = userDTO.RecordStatus;
                    List<ClientDetails> clientDetailsList = _clientDataRepository.GetUserClientsToShowForLoggedinMeridianUser(clientsIDsOfLoggedInUser, userDTO.UserId);
                    GetClientsAssignedToUser(clientDetailsList , user);
                    users.Add(user);
                }
                return users;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        

        /// <summary>
        /// Get Employee data with given employee id
        /// </summary>
        /// <param name="employeeID"></param>
        /// <returns></returns>
        public M3UserViewModel GetEmpDetails(string employeeID)
        {
            M3UserViewModel m3UserViewModel = new M3UserViewModel();
            try
            {
                if (!string.IsNullOrEmpty(employeeID))
                {
                    M3UserModel m3Userdto = _allUserRepository.GetM3UserDetails(employeeID);
                    if (m3Userdto != null)
                    {
                        m3UserViewModel.User.UserId = m3Userdto.User.UserId;
                        m3UserViewModel.User.FirstName = m3Userdto.User.FirstName;
                        m3UserViewModel.User.LastName = m3Userdto.User.LastName;
                        m3UserViewModel.User.Email = m3Userdto.User.Email;
                        m3UserViewModel.User.MobileNumber = m3Userdto.User.MobileNumber;
                        m3UserViewModel.User.RecordStatus = m3Userdto.User.RecordStatus;
                        m3UserViewModel.User.RoleCode = m3Userdto.User.RoleCode;
                        m3UserViewModel.User.IsMeridianUser = m3Userdto.User.IsMeridianUser;
                        m3UserViewModel.BusinessUnit = m3Userdto.BusinessUnit;
                        m3UserViewModel.Title = m3Userdto.Title;
                        if (String.IsNullOrEmpty(m3Userdto.ReportsTo) || m3Userdto.ReportsTo == " ")
                        {
                            m3Userdto.ReportsTo = m3Userdto.ReportsTo;
                        }
                        else
                        {
                            m3Userdto.ReportsTo = _allUserRepository.GetReportsToOfEmployee(m3Userdto.ReportsTo);
                        }
                        m3UserViewModel.ReportsTo = m3Userdto.ReportsTo;
                        m3UserViewModel.Site = m3Userdto.Site;
                        m3UserViewModel.IsActiveEmployee = m3Userdto.IsActiveEmployee;
                        m3UserViewModel.IsUserExist = m3Userdto.IsUserExist;
                        if (m3Userdto.Clients != null)
                        {
                            m3UserViewModel.Clients = m3Userdto.Clients;
                        }
                        m3UserViewModel.User.IsActive = (m3Userdto.User.RecordStatus == BusinessConstants.RecordStatusActive);                       
                    }
                }
                return m3UserViewModel;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                m3UserViewModel.Success = false;
                m3UserViewModel.IsExceptionOccured = true;
                m3UserViewModel.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
                return m3UserViewModel;
            }
        }

        /// <summary>
        /// For filling the suggestions in auto fill for username
        /// </summary>
        /// <param name="key"></param>
        public List<AutoFillViewModel> GetEmpNamesForAutoFill(string key)
        {
            try
            {
                List<AutoFillViewModel> suggetionsList = new List<AutoFillViewModel>();
                if (!string.IsNullOrEmpty(key))
                {
                    List<AutoFillDTO> suggestionsDTO = _allUserRepository.GetEmpNamesForAutoFill(key);
                    foreach (AutoFillDTO dto in suggestionsDTO)
                    {
                        AutoFillViewModel suggestion = new AutoFillViewModel();
                        suggestion.Key = dto.Key;
                        suggestion.Value = dto.Value;
                        suggetionsList.Add(suggestion);
                    }
                }
                return suggetionsList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get Employee data with given employee id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserLoginViewModel GetUserDetails(string userID)
        {
            UserLoginViewModel user = new UserLoginViewModel();
            try
            {

                UserLoginDTO userDto = _allUserRepository.GetUserDetails(userID);
                if (userDto != null)
                {
                    user.UserId = userDto.UserId.ToString();
                    user.FirstName = userDto.FirstName;
                    user.LastName = userDto.LastName;
                    user.Email = userDto.Email;
                    user.MobileNumber = userDto.MobileNumber;
                    user.RecordStatus = userDto.RecordStatus;
                    user.ForgotPasswordToken = userDto.ForgotPasswordToken;
                    user.Password = userDto.Password;
                    if (userDto.RecordStatus == DomainConstants.RecordStatusActive)
                    {
                        user.IsActive = true;
                    }
                    else
                    {
                        user.IsActive = false;
                    }
                    foreach (ClientDetails client in userDto.Client)
                    {
                        ClientViewModel clientDetails = new ClientViewModel();
                        clientDetails.ClientCode = client.ClientCode;
                        clientDetails.Name = client.Name;
                        clientDetails.ModifiedBy = client.ModifiedBy;
                        clientDetails.ModifiedDate = client.ModifiedDate;
                        user.Clients.Add(clientDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                user.Success = false;
                user.IsExceptionOccured = true;
                user.ErrorMessages.Add(BusinessConstants.ERROR_GET_DETAILS);
            }
            return user;
        }

        /// <summary>
        /// Saves client User
        /// </summary>
        /// <param name="clientUser"></param>
        /// <returns></returns>
        public ValidationViewModel SaveClientUser(UserLoginViewModel clientUser)
        {
            BusinessResponse businessResponse = new BusinessResponse();
            try
            {
                string userID = _allUserRepository.GetUserIDIfEmailAlreadyExists(clientUser.Email);
                if (userID == clientUser.UserId || userID == null)
                {
                    UserLoginDTO userLogin = ToUserLogin(clientUser);

                    userLogin.ForgotPasswordToken = userID == null ? Helper.GenerateRandomToken() : null;
                    userLogin.Password = userID == null ? null : clientUser.Password;

                    businessResponse.IsSuccess = _allUserRepository.SaveUser(userLogin);

                    if (businessResponse.IsSuccess && userID == null)
                    {
                        businessResponse.IsSuccess = SendUserCredentialsMail(userLogin);
                    }
                }
                else
                {
                    businessResponse.IsSuccess = false;
                    businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.EMAIL_ALREADY_EXIST, MessageType = MessageType.Error });
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                businessResponse.IsSuccess = false;
                businessResponse.Messages.Add(new MessageDTO() { Message = BusinessConstants.ERROR_SAVE_DETAILS, MessageType = MessageType.Error });
            }
            return businessResponse.ToValidationViewModel();

        }

        /// <summary>
        /// To save the M3 user data
        /// </summary>
        /// <param name="m3User"></param>
        /// <returns></returns>
        public ValidationViewModel SaveM3User(M3UserViewModel m3User)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {                
                UserLoginDTO userLogin = BusinessMapper.MappingM3UserViewModelToBusinessModel(m3User);
                validationViewModel.Success = _allUserRepository.SaveUser(userLogin);
                if (validationViewModel.Success && !m3User.IsUserExist)
                {
                    string isDefaultEmailEnabled = Helper.GetConfigurationKey(BusinessConstants.USE_DEFAULT_EMAIL_FOR_M3PACT_USER);
                    string email = userLogin.Email;
                    string userFullName = userLogin.FirstName + " " + userLogin.LastName;
                    if (isDefaultEmailEnabled != null)
                    {
                        email = bool.Parse(isDefaultEmailEnabled) == true ? Helper.GetConfigurationKey(BusinessConstants.MAIL_FROM) : userLogin.Email;
                    }
                    validationViewModel.Success = SendLoginSuccessMail(email, userFullName);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.ErrorMessages.Add(BusinessConstants.ERROR_SAVE_DETAILS);
            }
            return validationViewModel;
        }

        /// <summary>
        /// For filling the suggestions in auto fill for username
        /// </summary>
        /// <param name="key"></param>
        public List<RoleViewModel> GetRolesForLoggedInUser()
        {
            List<RoleViewModel> rolesList = new List<RoleViewModel>();
            try
            {
                List<Role> RolesDTO = _allUserRepository.GetRolesForLoggedInUser();
                foreach (Role dto in RolesDTO)
                {
                    RoleViewModel role = new RoleViewModel();
                    role.RoleCode = dto.RoleCode;
                    role.RoleDesc = dto.RoleDesc;
                    role.RecordStatus = dto.RecordStatus;
                    rolesList.Add(role);
                }
                return rolesList;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// updates password for the user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public ValidationViewModel UpdatePassword(UserLoginViewModel user)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                validationViewModel.Success = _allUserRepository.UpdatePassword(ToUserLogin(user));
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
            }
            return validationViewModel;
        }

        /// <summary>
        /// validates password and saves it in DB
        /// </summary>
        /// <param name="resetPasswordInfo"></param>
        /// <returns></returns>
        public ValidationViewModel ValidateForgotPassword(UserLoginViewModel resetPasswordInfo)
        {
            ValidationViewModel response = new ValidationViewModel();
            try
            {
                if (!(string.IsNullOrEmpty(resetPasswordInfo.UserId) && string.IsNullOrEmpty(resetPasswordInfo.ForgotPasswordToken)))
                {
                    string userID = resetPasswordInfo.UserId;

                    UserLoginViewModel user = GetUserDetails(userID);

                    if (user != null && user.ForgotPasswordToken != null && user.ForgotPasswordToken.Equals(resetPasswordInfo.ForgotPasswordToken))
                    {
                        if (user.Password == null || !(user.Password.Equals(resetPasswordInfo.Password)))
                        {
                            CustomPasswordHasher customPasswordHasher = new CustomPasswordHasher();
                            user.Password = customPasswordHasher.HashPassword(resetPasswordInfo.Password);
                            user.ForgotPasswordToken = null;
                            user.LastPasswordChanged = DateTime.Now;
                            if (UpdatePassword(user).Success)
                            {
                                response.Success = true;
                                response.SuccessMessage = BusinessConstants.PASSWORD_SAVED;
                                return response;
                            }

                        }
                        response.Success = false;
                        response.ErrorMessages.Add(BusinessConstants.CANNOT_REUSE_PASSWORD);
                        return response;
                    }
                    else if (user != null && user.ForgotPasswordToken == null)
                    {
                        response.Success = false;
                        response.ErrorMessages.Add(BusinessConstants.LINK_EXPIRED);
                        return response;
                    }
                    response.Success = false;
                    response.ErrorMessages.Add(BusinessConstants.CHANGE_IN_URL);
                    return response;

                }
                response.ErrorMessages.Add(BusinessConstants.EMAIL_IS_EMPTY);
                response.Success = false;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                response.ErrorMessages.Add(BusinessConstants.ERROR_OCCURED);
                response.Success = false;
            }
            return response;
        }

        /// <summary>
        /// validates username
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public ValidationViewModel ValidateUsername(string email)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                string userId = _allUserRepository.ValidateUsername(email);
                if (userId == null)
                {
                    validationViewModel.ErrorMessages.Add(BusinessConstants.INVALID_USERNAME);
                    validationViewModel.Success = false;
                }
                else
                {
                    UserLoginDTO userLoginDTO = _allUserRepository.GetUser(userId, email);
                    userLoginDTO.ForgotPasswordToken = Helper.GenerateRandomToken();

                    validationViewModel.Success = _allUserRepository.UpdatePassword(userLoginDTO);

                    if (!validationViewModel.Success)
                    {
                        validationViewModel.ErrorMessages.Add(BusinessConstants.APPLICATION_ERROR);
                    }
                    else
                    {
                        validationViewModel.Success = SendUserCredentialsMail(userLoginDTO, true);
                        if (!validationViewModel.Success)
                        {
                            validationViewModel.ErrorMessages.Add(BusinessConstants.APPLICATION_ERROR);
                        }
                        else
                        {
                            validationViewModel.SuccessMessage = BusinessConstants.LINK_EMAILED;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.ErrorMessages.Add(BusinessConstants.APPLICATION_ERROR);
            }
            return validationViewModel;
        }

        /// <summary>
        /// validates username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public ValidationViewModel CheckUser(UserLoginViewModel user)
        {
            ValidationViewModel validationViewModel = new ValidationViewModel();
            try
            {
                string userId = _allUserRepository.ValidateUsername(user.Email, user.IsMeridianUser);
                if (userId == null)
                {
                    validationViewModel.ErrorMessages.Add(BusinessConstants.INVALID_USERNAME);
                }
                else
                {
                    UserLoginDTO userLoginDTO = _allUserRepository.GetUser(userId, user.Email);

                    if (user.IsMeridianUser != userLoginDTO.IsMeridianUser)
                    {
                        validationViewModel.Success = false;
                        validationViewModel.ErrorMessages.Add(BusinessConstants.INVALID_USERNAME);
                    }
                    else
                    {
                        validationViewModel.Success = true;
                        validationViewModel.AdditionalInfo = userLoginDTO.Email;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                validationViewModel.Success = false;
                validationViewModel.ErrorMessages.Add(BusinessConstants.APPLICATION_ERROR);
            }
            return validationViewModel;
        }


        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// converts to Business UserLogin model from UserLoginViewModel
        /// </summary>
        /// <param name="clientUser"></param>
        /// <returns></returns>
        private UserLoginDTO ToUserLogin(UserLoginViewModel clientUser)
        {
            UserLoginDTO clientUserlogin = new UserLoginDTO();
            clientUserlogin.UserId = string.IsNullOrEmpty(clientUser.UserId) ? _allUserRepository.GetClientUserSequenceValue() : clientUser.UserId;
            clientUserlogin.UserName = clientUser.UserName;
            clientUserlogin.FirstName = clientUser.FirstName;
            clientUserlogin.LastName = clientUser.LastName;
            clientUserlogin.MobileNumber = clientUser.MobileNumber;
            clientUserlogin.Email = clientUser.Email;
            clientUserlogin.IsMeridianUser = clientUser.IsMeridianUser;
            clientUserlogin.Password = clientUser.Password;
            clientUserlogin.RoleCode = clientUser.RoleCode;
            clientUserlogin.RoleName = clientUser.RoleName;
            clientUserlogin.RecordStatus = clientUser.IsActive ? Infra.DomainConstants.RecordStatusActive : Infra.DomainConstants.RecordStatusInactive;
            clientUserlogin.LoggedInUserID = clientUser.LoggedInUserID;
            clientUserlogin.Client = new List<ClientDetails>();
            foreach (ClientViewModel client in clientUser.Clients)
            {
                ClientDetails clientDetails = new ClientDetails();
                clientDetails.ClientCode = client.ClientCode;
                clientDetails.Name = client.Name;
                clientUserlogin.Client.Add(clientDetails);
            }

            return clientUserlogin;
        }   

        /// <summary>
        /// Sends email to newly created user for registration completion
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private bool SendUserCredentialsMail(UserLoginDTO userLogin, bool isForgotPassword = false)
        {
            Infra.EmailDTO emailDTO = new Infra.EmailDTO();
            emailDTO.ToMail = userLogin.Email;
            string fullName = userLogin.FirstName + " " + userLogin.LastName;
            if (!isForgotPassword)
            {
                emailDTO.MailSubject = Infra.BusinessConstants.MAIL_CLIENTUSER_SUBJECT;
                emailDTO.Body = string.Format(Infra.BusinessConstants.MAIL_BODY, GetForgetPasswordLink(userLogin), fullName);
            }
            else
            {
                emailDTO.MailSubject = Infra.BusinessConstants.MAIL_FORGOTPWD_SUBJECT;
                emailDTO.Body = string.Format(Infra.BusinessConstants.MAIL_FORGOTPWD_BODY, GetForgetPasswordLink(userLogin), fullName);
            }
            emailDTO.IsBodyHtml = true;
            return EmailUtility.SendEmail(emailDTO);
        }

        /// <summary>
        /// M3 User mail sends
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private bool SendLoginSuccessMail(string emailAddress, string userFullName)
        {
            Infra.EmailDTO emailDTO = new Infra.EmailDTO();
            emailDTO.ToMail = emailAddress;
            emailDTO.MailSubject = Infra.BusinessConstants.MAIL_M3USER_SUBJECT;
            string navigationLink = string.Format(Infra.BusinessConstants.M3USER_REDIRECTION, Helper.GetConfigurationKey(BusinessConstants.SERVER_BASE_ADDRESS));
            emailDTO.Body = string.Format(Infra.BusinessConstants.MAIL_M3USER_BODY, userFullName, navigationLink);
            emailDTO.IsBodyHtml = true;
            return EmailUtility.SendEmail(emailDTO);
        }

        /// <summary>
        /// Get Forget Password Link
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        private string GetForgetPasswordLink(UserLoginDTO userLogin)
        {
            return string.Format(Infra.BusinessConstants.MAIL_FORMAT, Helper.GetConfigurationKey(BusinessConstants.SERVER_BASE_ADDRESS), Helper.GetHashedValue(userLogin.UserId), userLogin.ForgotPasswordToken);
        }

        /// <summary>
        /// To get the clients assigned to user.
        /// </summary>
        /// <param name="clientDetailsList"></param>
        /// <param name="user"></param>
        private void GetClientsAssignedToUser(List<ClientDetails> clientDetailsList, AllUsersViewModel user)
        {
            foreach (ClientDetails client in clientDetailsList)
            {
                ClientViewModel clientDetails = new ClientViewModel();
                clientDetails.ClientCode = client.ClientCode;
                clientDetails.Name = client.Name;
                clientDetails.ModifiedBy = client.ModifiedBy;
                clientDetails.ModifiedDate = client.ModifiedDate;
                if (client.System != null)
                {
                    clientDetails.System = new SystemViewModel()
                    {
                        ID = client.System.ID,
                        SystemCode = client.System.SystemCode,
                        SystemName = client.System.SystemName,
                        SystemDescription = client.System.SystemDescription
                    };
                }
                if (client.BusinessUnitDetails != null)
                {
                    clientDetails.BusinessUnit = new BusinessUnitViewModel()
                    {
                        ID = client.BusinessUnitDetails.ID,
                        BusinessUnitCode = client.BusinessUnitDetails.BusinessUnitCode,
                        BusinessUnitName = client.BusinessUnitDetails.BusinessUnitName,
                        BusinessUnitDescription = client.BusinessUnitDetails.BusinessUnitDescription
                    };
                }
                if (client.Speciality != null)
                {
                    clientDetails.Speciality = new SpecialityViewModel()
                    {
                        ID = client.Speciality.ID,
                        SpecialityCode = client.Speciality.SpecialityCode,
                        SpecialityName = client.Speciality.SpecialityName,
                        SpecialityDescription = client.Speciality.SpecialityDescription
                    };
                }
                user.SelectedClients.Add(clientDetails);
            }
        }
        #endregion Private Methods
    }
}
