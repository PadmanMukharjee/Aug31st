using Meridian.AuthServer.DataModel;
using Meridian.AuthServer.RepositoryInterfaces;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Business = Meridian.AuthServer.BusinessModel;

namespace Meridian.AuthServer.Repository
{
    public class UserLoginRepository : IUserLoginRepository
    {
        M3PactContext _m3pactContext;
        public IConfiguration _configuration { get; }

        public UserLoginRepository(M3PactContext dbContext, IConfiguration configuration)
        {
            _m3pactContext = dbContext;
            _configuration = configuration;
        }

        /// <summary>
        /// Get user details by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Business.UserLogin GetUserByEmail(string email)
        {
            Business.UserLogin user = null;
            if (!string.IsNullOrEmpty(email))
            {
                user = (from U in _m3pactContext.UserLogin
                        join R in _m3pactContext.Roles on U.RoleId equals R.RoleId
                        where U.Email == email && U.RecordStatus == "A"
                        select new Business.UserLogin()
                        {
                            UserId = U.UserId,
                            UserName = U.UserName,
                            FirstName = U.FirstName,
                            MiddleName = U.MiddleName,
                            LastName = U.LastName,
                            MobileNumber = U.MobileNumber,
                            Email = U.Email,
                            RoleId = U.RoleId,
                            RoleName = R.RoleCode,
                            PasswordHash = U.Password,
                            RecordStatus = U.RecordStatus,
                            IsMeridianUser = U.IsMeridianUser
                        }).FirstOrDefault();
            }
            return user;
        }

        /// <summary>
        /// Get user details by user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public Business.UserLogin GetUserById(string userID)
        {
            Business.UserLogin user = null;
            if (!string.IsNullOrEmpty(userID))
            {
                user = (from U in _m3pactContext.UserLogin
                        join R in _m3pactContext.Roles on U.RoleId equals R.RoleId
                        where U.UserId == userID
                        select new Business.UserLogin()
                        {
                            UserId = U.UserId,
                            UserName = U.UserName,
                            FirstName = U.FirstName,
                            MiddleName = U.MiddleName,
                            LastName = U.LastName,
                            MobileNumber = U.MobileNumber,
                            Email = U.Email,
                            RoleId = U.RoleId,
                            RoleName = R.RoleCode,
                            PasswordHash = U.Password,
                            RecordStatus = U.RecordStatus,
                            IsMeridianUser = U.IsMeridianUser
                        }).FirstOrDefault();
            }
            return user;
        }
    }
}
