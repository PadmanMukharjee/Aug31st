using System;

namespace M3Pact.BusinessModel
{
    public class BusinessDTO
    {
        public BusinessDTO()
        {
            BusinessResponse = new BusinessResponse();
        }

        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }


        public string User { get; set; }

        public string RecordStatus { get; set; }

        public DateTime CurrentDate
        {
            get
            {
                return DateTime.Now;
            }
        }

        public int UserID { get; set; }

        public BusinessResponse BusinessResponse { get; set; }
    }
}
