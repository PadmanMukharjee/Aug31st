using M3Pact.BusinessModel.Admin;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Common;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.Admin
{
    public class ClientUserRepository : IClientUserRepository
    {
        #region private Properties

        private M3PactContext _m3pactContext;
        private IConfiguration _Configuration { get; }

        #endregion private Properties

        #region Constructor
        public ClientUserRepository(M3PactContext m3PactContext, IConfiguration configuration)
        {
            _m3pactContext = m3PactContext;
            _Configuration = configuration;
        }
        #endregion Constructor


        #region public Methods

        /// <summary>
        /// To get all the Users data
        /// </summary>
        /// <returns></returns>
        public List<AllUsers> GetAllUsersData()
        {
            try
            {
                string roleCode = UserHelper.getUserContext().Role;
                int roleLevel;
                if (roleCode != DomainConstants.Admin)
                {
                    roleLevel = (int)_m3pactContext.Roles.Where(r => r.RoleCode == UserHelper.getUserContext().Role && r.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault().Level;
                }
                else
                {
                    roleLevel = (int)_m3pactContext.Roles.Where(r => r.RoleCode == DomainConstants.Admin && r.RecordStatus == DomainConstants.RecordStatusActive)?.FirstOrDefault().Level + 1;
                }

                return _m3pactContext.UserLogin.Include(DomainConstants.Role)
                    .Where(u => u.RecordStatus == DomainConstants.RecordStatusActive
                    && u.Role.Level >= roleLevel)
                    .Select(x => new AllUsers()
                    {
                        Email = x.Email,
                        FirstName = x.FirstName,
                        IsMeridianUser = x.IsMeridianUser,
                        LastName = x.LastName,
                        MobileNumber = x.MobileNumber,
                        RoleName = x.Role.RoleCode,
                        UserName = x.UserName,
                        UserId = x.UserId
                    })?.OrderBy(u => u.FirstName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get All M3 users
        /// </summary>
        /// <returns></returns>
        public List<AllUsers> GetAllM3Users()
        {
            try
            {
                return _m3pactContext.UserLogin.Include(DomainConstants.Role)
                    .Where(u => u.RecordStatus == DomainConstants.RecordStatusActive
                    && u.Role.RoleCode != DomainConstants.Client)
                    .Select(x => new AllUsers()
                    {
                        ID = x.Id,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        IsMeridianUser = x.IsMeridianUser,
                        LastName = x.LastName,
                        MobileNumber = x.MobileNumber,
                        RoleName = x.Role.RoleCode,
                        UserName = x.UserName,
                        UserId = x.UserId
                    })?.OrderBy(u => u.FirstName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// User to save ClientUsers association
        /// </summary>
        /// <param name="clientUserMap"></param>
        /// <returns></returns>
        public bool SaveClientUsers(ClientUserMap clientUserMap)
        {
            try
            {
                UserContext userContext = UserHelper.getUserContext();

                List<string> selectedUsers = clientUserMap.ClientUsers.Select(c => c.Email).ToList();

                //All the Existing UserIds of a client
                List<UserClientMap> existingClientUserMap = _m3pactContext.UserClientMap.Include(DomainConstants.Client)
                                            .Where(c => c.Client.ClientCode == clientUserMap.ClientCode
                                                   && c.RecordStatus == DomainConstants.RecordStatusActive)?.ToList();

                List<int> existingClientUserIds = existingClientUserMap.Select(c => c.UserId).ToList();

                //All the UserIds assigned
                List<int> userIds = _m3pactContext.UserLogin.Where(c => c.Email.Any(cu => selectedUsers.Contains(c.Email))).Select(d => d.Id).ToList();

                //Newly Added UserIds
                IEnumerable<int> newlyAddedUserIds = userIds.Except(existingClientUserIds);
                IEnumerable<int> notChangedUserIds = userIds.Intersect(existingClientUserIds);

                if (notChangedUserIds != null)
                {
                    existingClientUserMap.Where(ec => !notChangedUserIds.Contains(ec.UserId)).ToList()?
                                         .ForEach(c =>
                                         {
                                             c.RecordStatus = DomainConstants.RecordStatusInactive;
                                             c.ModifiedDate = DateTime.Now;
                                             c.ModifiedBy = userContext.UserId;
                                         });
                    _m3pactContext.UserClientMap.UpdateRange(existingClientUserMap);
                }
                int clientId = (int)_m3pactContext.Client.Where(c => c.ClientCode == clientUserMap.ClientCode).First()?.ClientId;
                foreach (int userId in newlyAddedUserIds)
                {
                    _m3pactContext.UserClientMap.Add(new UserClientMap()
                    {
                        ClientId = clientId,
                        UserId = userId,
                        RecordStatus = DomainConstants.RecordStatusActive,
                        CreatedBy = userContext.UserId,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = userContext.UserId,
                        ModifiedDate = DateTime.Now,
                    });

                }
                return _m3pactContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// To get the users associated with the client
        /// </summary>
        /// <param name="clientCode"></param>
        /// <returns></returns>
        public ClientUserMap GetUsersForClient(string clientCode)
        {
            try
            {
                List<AllUsers> clientUsers = (from c in _m3pactContext.Client
                                              join uc in _m3pactContext.UserClientMap
                                              on c.ClientId equals uc.ClientId
                                              join ul in _m3pactContext.UserLogin
                                              on uc.UserId equals ul.Id
                                              join r in _m3pactContext.Roles
                                              on ul.RoleId equals r.RoleId
                                              where c.ClientCode == clientCode
                                              && ul.RecordStatus == DomainConstants.RecordStatusActive
                                              && r.RecordStatus == DomainConstants.RecordStatusActive
                                              && uc.RecordStatus == DomainConstants.RecordStatusActive

                                              select new AllUsers()
                                              {
                                                  Email = ul.Email,
                                                  FirstName = ul.FirstName,
                                                  IsMeridianUser = ul.IsMeridianUser,
                                                  LastName = ul.LastName,
                                                  MobileNumber = ul.MobileNumber,
                                                  RoleName = ul.Role.RoleCode,
                                                  UserName = ul.UserName,
                                                  UserId = ul.UserId
                                              }
                          )?.ToList();
                return new ClientUserMap()
                {
                    ClientCode = clientCode,
                    ClientUsers = clientUsers
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion public Methods
    }
}
