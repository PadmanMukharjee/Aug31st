using System;

namespace M3Pact.ViewModel.Admin
{
    public class EmployeeViewModel
    {

        #region Properties

        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string BusinessUnit { get; set; }
        public string ReportsTo { get; set; }
        public string Site { get; set; }
        public string Role { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        #endregion Properties

    }
}
