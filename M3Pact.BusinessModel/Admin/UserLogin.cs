using System;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.BusinessModels
{
    public class UserLoginDTO
    {
        public UserLoginDTO()
        {
            Client = new List<ClientDetails>();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool? IsMeridianUser { get; set; }
        public string Password { get; set; }
        public string ForgotPasswordToken { get; set; }
        public short? IncorrectLoginAttemptsCount { get; set; }
        public DateTime? LastSuccessfulLogin { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string RecordStatus { get; set; }
        public List<ClientDetails> Client { get; set; }
        public string LoggedInUserID { get; set; }
    }
}
