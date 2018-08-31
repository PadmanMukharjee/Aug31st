using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Attribute
    {
        public Attribute()
        {
            AdminConfigValues = new HashSet<AdminConfigValues>();
            ClientConfig = new HashSet<ClientConfig>();
        }

        public int AttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string AttributeType { get; set; }
        public int? ControlTypeId { get; set; }

        public ControlType ControlType { get; set; }
        public ICollection<AdminConfigValues> AdminConfigValues { get; set; }
        public ICollection<ClientConfig> ClientConfig { get; set; }
    }
}
