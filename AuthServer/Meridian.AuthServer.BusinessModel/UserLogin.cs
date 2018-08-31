using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.BusinessModel
{
    public class UserLogin
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public bool? IsMeridianUser { get; set; }
        public string PasswordHash { get; set; }
        public short? IncorrectLoginAttemptsCount { get; set; }
        public DateTime? LastSuccessfulLogin { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }

    }
}
