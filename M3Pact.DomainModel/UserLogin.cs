using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class UserLogin
    {
        public UserLogin()
        {
            ClientBillingManager = new HashSet<Client>();
            ClientKpiuserMap = new HashSet<ClientKpiuserMap>();
            ClientRelationShipManager = new HashSet<Client>();
            ClientUserNoticeAlerts = new HashSet<ClientUserNoticeAlerts>();
            UserClientMap = new HashSet<UserClientMap>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public int? RoleId { get; set; }
        public bool? IsMeridianUser { get; set; }
        public string Password { get; set; }
        public short? IncorrectLoginAttemptsCount { get; set; }
        public DateTime? LastSuccessfulLogin { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public bool? LockoutEnabled { get; set; }
        public string LastName { get; set; }
        public string ForgotPasswordToken { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public Roles Role { get; set; }
        public ICollection<Client> ClientBillingManager { get; set; }
        public ICollection<ClientKpiuserMap> ClientKpiuserMap { get; set; }
        public ICollection<Client> ClientRelationShipManager { get; set; }
        public ICollection<ClientUserNoticeAlerts> ClientUserNoticeAlerts { get; set; }
        public ICollection<UserClientMap> UserClientMap { get; set; }
    }
}
