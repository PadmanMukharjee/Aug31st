namespace M3Pact.BusinessModel.Admin
{
    public class AttributesBusinessModel
    {
        public int AttributeId { get; set; }
        public string AttributeCode { get; set; }
        public string AttributeName { get; set; }
        public string AttributeDescription { get; set; }
        public string RecordStatus { get; set; }      
        public string AttributeType { get; set; }
        public int? ControlTypeId { get; set; }
        public string Control { get; set; }
        public string AttributeValue { get; set; }
    }
}
