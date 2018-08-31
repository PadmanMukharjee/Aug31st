using System;
using System.Collections.Generic;

namespace M3Pact.ViewModel.Admin
{
    public class UserLoginViewModel : ValidationViewModel
    {
        public UserLoginViewModel()
        {
            Clients = new List<ClientViewModel>();
        }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public bool? IsMeridianUser { get; set; }
        public string Password { get; set; }
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
        public string ForgotPasswordToken { get; set; }
        public string RedirectUrl { get; set; }
        public List<ClientViewModel> Clients { get; set; }
        public bool IsActive { get; set; }
        public string RedirectionUrl { get; set; }
        public string LoggedInUserID { get; set; }
    }
}
