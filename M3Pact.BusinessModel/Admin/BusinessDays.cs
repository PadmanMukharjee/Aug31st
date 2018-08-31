namespace M3Pact.BusinessModel.BusinessModels
{
    public class BusinessDays : BaseBusinessModel
    {
        public int Year { get; set; }
        public int MonthID { get; set; }
        public string Month { get; set; }
        public int NumberOfBusinessDays { get; set; }       
    }
}
