using M3Pact.Infrastructure.Enums;
using System.Collections.Generic;

namespace M3Pact.BusinessModel
{
    public class BusinessRuleDTO
    {
        public BusinessRuleDTO()
        {
            Messages = new List<MessageDTO>();
        }
        public BusinessRule BusinessRuleId { get; set; }
        public List<MessageDTO> Messages { get; set; }
        public string Category { get; set; }
        public bool SkipTransaction { get; set; }
        public string ConfigValue { get; set; }
        public string AdditionalInfo { get; set; }
    }
}
