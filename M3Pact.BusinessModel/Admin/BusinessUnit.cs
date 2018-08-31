namespace M3Pact.BusinessModel.BusinessModels
{
    public class BusinessUnit : BaseBusinessModel
    {
        public BusinessUnit()
        {
            Site = new Site();
        }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitDescription { get; set; }
        public Site Site { get; set; }
                
    }
}
