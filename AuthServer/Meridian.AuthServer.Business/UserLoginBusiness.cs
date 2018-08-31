using Meridian.AuthServer.BusinessInterfaces;
using Meridian.AuthServer.BusinessModel;
using Meridian.AuthServer.RepositoryInterfaces;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Net;

namespace Meridian.AuthServer.Business
{
    public class UserLoginBusiness : IUserLoginBusiness
    {
        private readonly IUserLoginRepository _userRepository;
        IPasswordHasher _passwordHasher;
        string _hostAddress;

        public UserLoginBusiness(IUserLoginRepository userRepository, IPasswordHasher passwordHasher, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _hostAddress = configuration["Ldap:host"];
        }

        /// <summary>
        /// Get user details by user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public UserLogin FindUserById(string userID)
        {
            return _userRepository.GetUserById(userID);
        }

        /// <summary>
        /// Get user details by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserLogin FindUserByEmail(string email)
        {
            return _userRepository.GetUserByEmail(email);
        }

        /// <summary>
        /// Validate provided credentials with database and generate token if user is valid
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public UserLogin ValidateUserCredentials(string userName, string password, string applicationName)
        {
            UserLogin user = null;
            switch (applicationName)
            {
                case "M3PACT":
                    user = _userRepository.GetUserByEmail(userName);
                    break;
                default: break;
            }
            if(user != null)
            {
                if (user.IsMeridianUser == true && CheckUser(user.UserName, password))
                {
                    return user;
                }
                else
                {
                    if (_passwordHasher.VerifyHashedPassword(user.PasswordHash, password) == PasswordVerificationResult.Success)
                    {
                        return user;
                    }
                }
            }
            return null;
        }


        /// <summary>
        /// checks user is present in active directory or not
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool CheckUser(string userName, string password)
        {
            bool result = true;
            NetworkCredential credentials = new NetworkCredential(userName, password);
            LdapDirectoryIdentifier serverId = new LdapDirectoryIdentifier(_hostAddress);

            LdapConnection conn = new LdapConnection(serverId, credentials);

            try
            {
                conn.Bind();
            }
            catch(Exception ex)
            {
                result = false;
            }
            return result;
        }
    }
}
