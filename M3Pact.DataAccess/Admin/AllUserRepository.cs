using M3Pact.BusinessModel.Admin;
using M3Pact.BusinessModel.Common;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Enums;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Business = M3Pact.BusinessModel.BusinessModels;
using Domain = M3Pact.DomainModel.DomainModels;

namespace M3Pact.Repository.Admin
{
    public class AllUserRepository : IAllUserRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private IConfiguration _Configuration { get; }
        private UserContext userContext;

        #endregion private Properties

        #region Constructor
        public AllUserRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
            userContext = UserHelper.getUserContext();
        }

        #endregion Constructor

        #region public Methods

        /// <summary>
        /// Returns the Users
        /// </summary>
        /// <returns></returns>
        public List<AllUsers> GetAllUsersDataAndClientsAssigned(List<int> clientsIDsOfLoggedInUser, bool isActiveUser)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                string recordStatus = isActiveUser ? DomainConstants.RecordStatusActive : DomainConstants.RecordStatusInactive;
                int loggedInUserLevel = (from u in _m3pactContext.UserLogin
                                         join r in _m3pactContext.Roles on u.RoleId equals r.RoleId
                                         where u.UserId == userContext.UserId
                                         select r.Level).FirstOrDefault().Value;              
                if (userContext.Role == RoleType.Admin.ToString())
                {
                    return (from u in _m3pactContext.UserLogin
                            join r in _m3pactContext.Roles on u.RoleId equals r.RoleId
                            join e in _m3pactContext.Employee on u.UserId equals e.EmployeeId.ToString() into ue
                            from subEmployee in ue.DefaultIfEmpty()
                            where r.Level >= loggedInUserLevel
                            select new AllUsers()
                            {
                                UserId = u.UserId,
                                FirstName = u.FirstName,
                                LastName = u.LastName,
                                Email = u.Email,
                                RoleName = u.Role.RoleDesc,
                                RecordStatus = u.RecordStatus,
                                IsMeridianUser = u.IsMeridianUser,
                                ReportsTo = subEmployee.ReportsTo,
                                BusinessUnit = subEmployee.BusinessUnit,
                                Site = subEmployee.Site,
                                Title = subEmployee.Title
                            }).Distinct().ToList();
                }

                return (from u in _m3pactContext.UserLogin
                        join r in _m3pactContext.Roles on u.RoleId equals r.RoleId
                        join ucm in _m3pactContext.UserClientMap on u.Id equals ucm.UserId
                        join e in _m3pactContext.Employee on u.UserId equals e.EmployeeId.ToString() into ue
                        from subEmployee in ue.DefaultIfEmpty()
                        where r.Level >= loggedInUserLevel
                            && clientsIDsOfLoggedInUser.Contains(ucm.ClientId)
                            && ucm.RecordStatus == DomainConstants.RecordStatusActive
                        select new AllUsers()
                        {
                            UserId = u.UserId,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            Email = u.Email,
                            RoleName = u.Role.RoleDesc,
                            RecordStatus = u.RecordStatus,
                            IsMeridianUser = u.IsMeridianUser,
                            ReportsTo = subEmployee.ReportsTo,
                            BusinessUnit = subEmployee.BusinessUnit,
                            Site = subEmployee.Site,
                            Title = subEmployee.Title
                        }).Distinct().ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To get the reports to of the employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public string GetReportsToOfEmployee(string employeeId)
        {
            try
            {
                string reportsTo = null;
                if (employeeId != null)
                {
                    int userId = Convert.ToInt32(employeeId);
                    reportsTo = (from e in _m3pactContext.Employee
                                 where e.EmployeeId == userId
                                 select e.LastName + ' ' + e.FirstName
                                 ).FirstOrDefault();
                }
                return reportsTo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get clients for th loggedin user
        /// </summary>
        /// <returns></returns>
        public List<int> GetClientIDsOfLoggedInUser()
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                List<int> clientIDs = new List<int>();
                if (userContext.Role == RoleType.Admin.ToString())
                {
                    clientIDs = _m3pactContext.Client.Where(c => c.IsActive == DomainConstants.RecordStatusActive).Select(x => x.ClientId).ToList();
                }
                else
                {
                    clientIDs = (from ucm in _m3pactContext.UserClientMap
                                 join ul in _m3pactContext.UserLogin on ucm.UserId equals ul.Id
                                 join c in _m3pactContext.Client on ucm.ClientId equals c.ClientId
                                 where ul.UserId == userContext.UserId
                                 && c.IsActive == DomainConstants.RecordStatusActive
                                 && ucm.RecordStatus == DomainConstants.RecordStatusActive
                                 select c.ClientId).ToList();
                }
                return clientIDs;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get User Details of Given Id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public M3UserModel GetM3UserDetails(string userID)
        {
            try
            {
                if (!string.IsNullOrEmpty(userID))
                {
                    M3UserModel m3model = (from ul in _m3pactContext.UserLogin
                                           join emp in _m3pactContext.Employee
                                           on ul.UserId equals emp.EmployeeId.ToString()
                                           join r in _m3pactContext.Roles
                                           on ul.RoleId equals r.RoleId
                                           where ul.UserId == userID
                                           select new M3UserModel()
                                           {
                                               User = new Business.UserLoginDTO()
                                               {
                                                   UserId = ul.UserId,
                                                   FirstName = ul.FirstName,
                                                   LastName = ul.LastName,
                                                   MobileNumber = ul.MobileNumber,
                                                   Email = ul.Email,
                                                   RoleCode = r.RoleCode,
                                                   RecordStatus = ul.RecordStatus,
                                                   IsMeridianUser = ul.IsMeridianUser
                                               },
                                               BusinessUnit = emp.BusinessUnit,
                                               Title = emp.Title,
                                               ReportsTo = emp.ReportsTo,
                                               Site = emp.Site,
                                               IsUserExist = true,
                                               IsActiveEmployee = emp.RecordStatus == DomainConstants.RecordStatusActive
                                           }
                      ).FirstOrDefault();
                    if (m3model != null)
                    {
                        List<string> clients = (from c in _m3pactContext.Client
                                                join ucm in _m3pactContext.UserClientMap
                                                on c.ClientId equals ucm.ClientId
                                                join ul in _m3pactContext.UserLogin
                                                on ucm.UserId equals ul.Id
                                                where ul.UserId == userID
                                                select
                                                     c.ClientCode
                                                     ).ToList();

                        m3model.Clients = clients;
                    }
                    else
                    {
                        m3model = (from emp in _m3pactContext.Employee
                                   where emp.EmployeeId.ToString() == userID
                                   && emp.RecordStatus == DomainConstants.RecordStatusActive
                                   select new M3UserModel()
                                   {
                                       User = new Business.UserLoginDTO()
                                       {
                                           UserId = emp.EmployeeId.ToString(),
                                           FirstName = emp.FirstName,
                                           LastName = emp.LastName,
                                           MobileNumber = emp.MobileNumber,
                                           Email = emp.Email,
                                           RecordStatus = emp.RecordStatus,
                                           IsMeridianUser = true
                                       },
                                       BusinessUnit = emp.BusinessUnit,
                                       Title = emp.Title,
                                       ReportsTo = emp.ReportsTo,
                                       Site = emp.Site,
                                       IsUserExist = false,
                                       IsActiveEmployee = emp.RecordStatus == DomainConstants.RecordStatusActive
                                   }
                     ).FirstOrDefault();
                    }
                    return m3model;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get employee name suggestions for autofill
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<AutoFillDTO> GetEmpNamesForAutoFill(string key)
        {
            try
            {
                return (from emp in _m3pactContext.Employee
                        where (emp.LastName + " " + emp.FirstName + " - " + emp.EmployeeId).ToLowerInvariant().StartsWith(key.ToLowerInvariant())
                        && emp.RecordStatus == DomainConstants.RecordStatusActive
                        select new AutoFillDTO()
                        {
                            Value = emp.LastName + " " + emp.FirstName + " - " + emp.EmployeeId,
                            Key = emp.EmployeeId.ToString()
                        }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business.UserLoginDTO GetUserDetails(string userID)
        {
            try
            {
                Business.UserLoginDTO userDto = null;
                if (!string.IsNullOrEmpty(userID))
                {

                    UserLogin userlogin = (from u in _m3pactContext.UserLogin
                                           where u.UserId == userID
                                           select new UserLogin
                                           {
                                               UserId = u.UserId,
                                               UserName = u.UserName,
                                               FirstName = u.FirstName,
                                               MiddleName = u.MiddleName,
                                               LastName = u.LastName,
                                               Email = u.Email,
                                               UserClientMap = u.UserClientMap,
                                               MobileNumber = u.MobileNumber,
                                               RecordStatus = u.RecordStatus,
                                               Password = u.Password,
                                               ForgotPasswordToken = u.ForgotPasswordToken
                                           }
                                           ).FirstOrDefault();

                    if (userlogin != null)
                    {
                        userDto = new Business.UserLoginDTO();
                        userDto.UserId = userlogin.UserId.ToString();
                        userDto.FirstName = userlogin.FirstName;
                        userDto.LastName = userlogin.LastName;
                        userDto.Email = userlogin.Email;
                        userDto.MobileNumber = userlogin.MobileNumber;
                        userDto.RoleName = userlogin.Role?.RoleCode;
                        userDto.RecordStatus = userlogin.RecordStatus;
                        userDto.Password = userlogin.Password;
                        userDto.ForgotPasswordToken = userlogin.ForgotPasswordToken;
                        if (userlogin.UserClientMap != null)
                        {
                            foreach (UserClientMap userClient in userlogin.UserClientMap)
                            {
                                Client client = _m3pactContext.Client.FirstOrDefault(x => x.ClientId == userClient.ClientId);
                                Business.ClientDetails clientDetails = new Business.ClientDetails();
                                clientDetails.ClientCode = client.ClientCode;
                                clientDetails.Name = client.Name;
                                userDto.Client.Add(clientDetails);
                            }
                        }
                    }
                }
                return userDto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Saves client User
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        public bool SaveUser(Business.UserLoginDTO userLogin)
        {
            try
            {
                List<Domain.UserClientMap> userClientMapList = new List<DomainModel.DomainModels.UserClientMap>();
                Domain.UserLogin existingClientUser = _m3pactContext.UserLogin.FirstOrDefault(x => x.UserId == userLogin.UserId);

                List<int> clientIdList = new List<int>();
                if (userLogin.Client.Count > 0)
                {
                    foreach (Business.ClientDetails client in userLogin.Client)
                    {
                        clientIdList.Add(_m3pactContext.Client.FirstOrDefault(x => x.ClientCode == client.ClientCode).ClientId);
                    }
                }
                if (existingClientUser != null)
                {
                    List<int> insertableClients = new List<int>();
                    MapClientUser(userLogin, existingClientUser, true);
                    PrepareChangedClientMaps(userLogin, existingClientUser, clientIdList, insertableClients);
                    userClientMapList = MapUserClientMap(existingClientUser, insertableClients);
                    _m3pactContext.UserClientMap.AddRange(userClientMapList);
                }
                else
                {
                    Domain.UserLogin insertableClientUser = new Domain.UserLogin();
                    MapClientUser(userLogin, insertableClientUser, false);
                    _m3pactContext.UserLogin.Add(insertableClientUser);

                    userClientMapList = MapUserClientMap(insertableClientUser, clientIdList);
                    _m3pactContext.UserClientMap.AddRange(userClientMapList);
                }

                int i = _m3pactContext.SaveChanges();
                return i > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get roles list accessible for logged in user
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<BusinessModel.Admin.Role> GetRolesForLoggedInUser()
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();
                int loggedinUserLevel = _m3pactContext.Roles.FirstOrDefault(x => x.RoleCode == userContext.Role).Level.Value;
                return (from r in _m3pactContext.Roles
                        where r.RecordStatus == DomainConstants.RecordStatusActive && r.RoleCode != DomainConstants.Client && r.Level >= loggedinUserLevel
                        orderby r.Level
                        select new BusinessModel.Admin.Role()
                        {
                            RoleCode = r.RoleCode,
                            RoleDesc = r.RoleDesc,
                            RecordStatus = r.RecordStatus,
                            CreatedBy = r.CreatedBy,
                            CreatedDate = r.CreatedDate,
                            ModifiedBy = r.ModifiedBy,
                            ModifiedDate = r.ModifiedDate
                        }).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets ClientUser sequence 
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public string GetClientUserSequenceValue()
        {
            string sequenceNumber = string.Empty;
            try
            {
                string ConnectionString = ConfigurationExtensions.GetConnectionString(_Configuration, DomainConstants.M3PactConnection);
                using (SqlConnection sqlConn = new SqlConnection(ConnectionString))
                {
                    using (SqlCommand sqlCmd = new SqlCommand(DomainConstants.ClientUserGetNextValueSequence, sqlConn))
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
        /// Checkes weather the email address already exists in db or not and returns userid
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetUserIDIfEmailAlreadyExists(string email)
        {
            IQueryable<UserLogin> userLogin = (from ul in _m3pactContext.UserLogin
                                               where ul.Email == email
                                               select ul);
            if (userLogin != null && userLogin.Count() > 0)
            {
                return userLogin.FirstOrDefault().UserId;
            }
            return null;
        }

        /// <summary>
        /// updates password
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public bool UpdatePassword(Business.UserLoginDTO userDto)
        {
            Domain.UserLogin user = _m3pactContext.UserLogin.FirstOrDefault(x => x.UserId == userDto.UserId);
            if (user != null)
            {
                user.Password = userDto.Password;
                user.ForgotPasswordToken = userDto.ForgotPasswordToken;
                user.LastPasswordChanged = userDto.LastPasswordChanged;
                user.ModifiedBy = userContext.UserId;
                user.ModifiedDate = DateTime.UtcNow;
                _m3pactContext.Update(user);
                return _m3pactContext.SaveChanges() > 0;
            }
            return false;
        }

        /// <summary>
        /// Validate username 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string ValidateUsername(string email, bool? isM3User = false)
        {
            try
            {
                if (isM3User == false)
                {
                    IQueryable<UserLogin> userLogin = (from ul in _m3pactContext.UserLogin
                                                       where ul.Email == email
                                                       && ul.RecordStatus == DomainConstants.RecordStatusActive
                                                       && ul.IsMeridianUser == isM3User
                                                       select ul);
                    if (userLogin != null && userLogin.Count() > 0)
                    {
                        return userLogin.FirstOrDefault().UserId;
                    }
                    return null;
                }
                else
                {
                    IQueryable<UserLogin> userLogin = (from ul in _m3pactContext.UserLogin
                                                       where ul.UserName == email
                                                       && ul.RecordStatus == DomainConstants.RecordStatusActive
                                                       && ul.IsMeridianUser == isM3User
                                                       select ul);
                    if (userLogin != null && userLogin.Count() > 0)
                    {
                        return userLogin.FirstOrDefault().UserId;
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get User by userid or username
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public Business.UserLoginDTO GetUser(string userID, string email = null)
        {
            try
            {
                Business.UserLoginDTO userDto = new Business.UserLoginDTO();
                UserLogin userLogin = _m3pactContext.UserLogin.FirstOrDefault(x => string.IsNullOrEmpty(userID) ? x.Email == email : x.UserId == userID);
                if (userLogin != null)
                {
                    userDto.UserId = userLogin.UserId.ToString();
                    userDto.UserName = userLogin.UserName;
                    userDto.FirstName = userLogin.FirstName;
                    userDto.LastName = userLogin.LastName;
                    userDto.Email = userLogin.Email;
                    userDto.MobileNumber = userLogin.MobileNumber;
                    userDto.RoleName = userLogin.Role?.RoleCode;
                    userDto.RecordStatus = userLogin.RecordStatus;
                    userDto.Password = userLogin.Password;
                    userDto.ForgotPasswordToken = userLogin.ForgotPasswordToken;
                    userDto.IsMeridianUser = userLogin.IsMeridianUser;
                    return userDto;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion public Methods


        #region private Methods

        /// <summary>
        /// To prepare removable and insertable UserClientMaps
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="clientUsers"></param>
        /// <param name="clientIdList"></param>
        /// <param name="removableClientMaps"></param>
        /// <param name="insertableClients"></param>
        /// <returns></returns>
        private void PrepareChangedClientMaps(Business.UserLoginDTO userLogin, UserLogin clientUsers, List<int> clientIdList, List<int> insertableClients)
        {
            List<int> loggedInUserClientMapping = new List<int>();
            List<Domain.UserClientMap> userclientMaping = new List<UserClientMap>();
            List<Domain.UserClientMap> removableClientMaps = new List<UserClientMap>();
            UserContext userContext = UserHelper.getUserContext();

            if (!string.IsNullOrEmpty(userContext.UserId))
            {
                if (userLogin.RoleCode != RoleType.Admin.ToString())
                {
                    string loggedinUserRoleCode = userContext.Role;
                    if (loggedinUserRoleCode == RoleType.Admin.ToString())
                    {
                        loggedInUserClientMapping = _m3pactContext.Client.Where(c => c.IsActive == DomainConstants.RecordStatusActive).Select(x => x.ClientId).ToList();
                    }
                    else
                    {
                        loggedInUserClientMapping = (from uc in _m3pactContext.UserClientMap
                                                     join u in _m3pactContext.UserLogin
                                                     on uc.UserId equals u.Id
                                                     where u.UserId == userContext.UserId
                                                     && uc.RecordStatus == DomainConstants.RecordStatusActive
                                                     select uc.ClientId).ToList();
                    }
                    userclientMaping = _m3pactContext.UserClientMap.Where(x => x.UserId == clientUsers.Id
                                       && loggedInUserClientMapping.Contains(x.ClientId)).ToList();
                }
                else
                {
                    userclientMaping = _m3pactContext.UserClientMap.Where(x => x.UserId == clientUsers.Id).ToList();
                }
                //Getting removable cleints which were existing in db and not present in the list which passed from UI
                removableClientMaps = userclientMaping.Where(x => !(clientIdList.Contains(x.ClientId))).ToList();
                if (removableClientMaps != null && removableClientMaps.Count > 0)
                {
                    _m3pactContext.RemoveRange(removableClientMaps);
                }
                //Getting adding cleints which were not existing in db and present in the list which passed from UI
                foreach (int id in clientIdList)
                {
                    if (!(userclientMaping.Exists(x => x.ClientId == id)))
                    {
                        insertableClients.Add(id);
                    }
                }
            }
        }


        /// <summary>
        /// To convert usercleintMap dto to domain model
        /// </summary>
        /// <param name="clientUser"></param>
        /// <param name="insertableClients"></param>
        /// <returns></returns>
        private List<UserClientMap> MapUserClientMap(UserLogin clientUser, List<int> insertableClients)
        {
            List<UserClientMap> userClientMapList = new List<UserClientMap>();
            foreach (int clientId in insertableClients)
            {
                UserClientMap userClientMap = new UserClientMap();
                userClientMap.User = clientUser;
                userClientMap.Client = _m3pactContext.Client.FirstOrDefault(x => x.ClientId == clientId);
                userClientMap.RecordStatus = DomainConstants.RecordStatusActive;
                userClientMap.CreatedBy = userContext.UserId;
                userClientMap.CreatedDate = DateTime.Now;
                userClientMap.ModifiedBy = userContext.UserId;
                userClientMap.ModifiedDate = DateTime.Now;
                userClientMapList.Add(userClientMap);
            }
            return userClientMapList;
        }

        /// <summary>
        /// To convert userloging dto to domain model
        /// </summary>
        /// <param name="userLogin"></param>
        /// <param name="clientUsers"></param>
        /// <param name="isEdit"></param>
        private void MapClientUser(Business.UserLoginDTO userLogin, UserLogin clientUsers, bool isEdit)
        {
            clientUsers.UserId = userLogin.UserId;
            clientUsers.FirstName = userLogin.FirstName;
            clientUsers.LastName = userLogin.LastName;
            clientUsers.Email = userLogin.Email;
            clientUsers.UserName = userLogin.Email;
            clientUsers.MobileNumber = userLogin.MobileNumber;
            clientUsers.ModifiedDate = DateTime.UtcNow;
            clientUsers.ModifiedBy = userContext.UserId;
            clientUsers.RecordStatus = userLogin.RecordStatus;
            clientUsers.RoleId = _m3pactContext.Roles.FirstOrDefault(x => x.RoleCode == userLogin.RoleCode).RoleId;
            clientUsers.ForgotPasswordToken = userLogin.ForgotPasswordToken;
            clientUsers.Password = userLogin.Password;
            if (!isEdit)
            {
                clientUsers.IsMeridianUser = userLogin.IsMeridianUser;
                clientUsers.CreatedDate = DateTime.UtcNow;
                clientUsers.CreatedBy = userContext.UserId;
            }
            //For M3 user we are setting SSO number as username.
            if (userLogin.IsMeridianUser == true)
            {
                clientUsers.UserName = _m3pactContext.Employee.FirstOrDefault(x => x.EmployeeId.ToString() == userLogin.UserId).Sso.ToString();
            }
        }

        #endregion Private Methods
    }
}

