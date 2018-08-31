using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class UserClientMap
    {
        public int UserClientMapId { get; set; }
        public int UserId { get; set; }
        public int ClientId { get; set; }
        public string RecordStatus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Client Client { get; set; }
        public UserLogin User { get; set; }
    }
}
