using System;

namespace M3Pact.BusinessModel.Admin
{
    public class UserClientMapDTO
    {        
        public int UserID { get; set; }
        public int ClientID { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
