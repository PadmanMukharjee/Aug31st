using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Repository.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace M3Pact.Repository.User
{
    public class UserRepository : IUserRepository
    {
        #region Internal Properties
        private M3PactContext _m3pactContext;
        #endregion

        #region Constructor
        public UserRepository(M3PactContext m3PactContext)
        {
            _m3pactContext = m3PactContext;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Repository method to get the screen actions based on user role
        /// </summary>
        /// <param name="screenCode"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<string> GetUserScreenActions(string screenCode, string role)
        {
            try
            {
                List<string> userActions = new List<string>();
                bool canEdit = (from SURM in _m3pactContext.ScreenUserRoleMap
                                join R in _m3pactContext.Roles on SURM.RoleId equals R.RoleId
                                join S in _m3pactContext.Screen on SURM.ScreenId equals S.ScreenId
                                where R.RoleCode == role && S.ScreenCode == screenCode && SURM.CanEdit == true
                                    && S.RecordStatus == DomainConstants.RecordStatusActive
                                    && SURM.RecordStatus == DomainConstants.RecordStatusActive
                                    && R.RecordStatus == DomainConstants.RecordStatusActive
                                select SURM).Any();

                if (canEdit)
                {
                    userActions = (from RA in _m3pactContext.RoleAction
                                   join SA in _m3pactContext.ScreenAction on RA.ScreenActionId equals SA.ScreenActionId
                                   join R in _m3pactContext.Roles on RA.RoleId equals R.RoleId
                                   join S in _m3pactContext.Screen on SA.ScreenId equals S.ScreenId
                                   where R.RoleCode == role && S.ScreenCode == screenCode
                                        && S.RecordStatus == DomainConstants.RecordStatusActive
                                        && SA.RecordStatus == DomainConstants.RecordStatusActive
                                        && RA.RecordStatus == DomainConstants.RecordStatusActive
                                        && R.RecordStatus == DomainConstants.RecordStatusActive
                                   select SA.ActionName).ToList();
                }

                return userActions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// To get all the nav menu screens based on role.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<M3Pact.DomainModel.DomainModels.Screen> GetScreensOfNavMenuBasedOnRole(string roleCode)
        {
            try
            {
                List<M3Pact.DomainModel.DomainModels.Screen> screenList = (from s in _m3pactContext.Screen
                                                                           join sr in _m3pactContext.ScreenUserRoleMap
                                                                           on s.ScreenId equals sr.ScreenId
                                                                           join r in _m3pactContext.Roles
                                                                           on sr.RoleId equals r.RoleId
                                                                           where s.IsMenuItem == true
                                                                           && r.RoleCode == roleCode
                                                                           && r.RecordStatus == DomainConstants.RecordStatusActive
                                                                           && s.RecordStatus == DomainConstants.RecordStatusActive
                                                                           && sr.RecordStatus == DomainConstants.RecordStatusActive
                                                                           orderby s.Displayorder ascending
                                                                           select new M3Pact.DomainModel.DomainModels.Screen()
                                                                           {
                                                                               ScreenId = s.ScreenId,
                                                                               ScreenCode = s.ScreenCode,
                                                                               ScreenName = sr.DisplayScreenName,
                                                                               ScreenDescription = s.ScreenDescription,
                                                                               Icon = s.Icon,
                                                                               ScreenPath = s.ScreenPath,
                                                                               IsMenuItem = s.IsMenuItem,
                                                                               ParentScreenId = s.ParentScreenId,
                                                                               Displayorder = s.Displayorder
                                                                           }).ToList();
                return screenList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
