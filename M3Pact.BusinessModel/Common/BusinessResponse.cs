using System.Collections.Generic;

namespace M3Pact.BusinessModel
{
    public class BusinessResponse
    {
        public BusinessResponse()
        {
            BusinessRules = new List<BusinessRuleDTO>();
            Messages = new List<MessageDTO>();
            IsSuccess = true;
        }
        public bool IsSuccess { get; set; }
        public bool IsExceptionOccured { get; set; }
        public List<MessageDTO> Messages { get; set; }
        public List<BusinessRuleDTO> BusinessRules { get; set; }
        public string AdditionalInfo { get; set; }

        //Can be used for any data that required to set in Repository
        //For getting Inserted entity primary key
        public object PrimaryKey { get; set; }
    }
}
