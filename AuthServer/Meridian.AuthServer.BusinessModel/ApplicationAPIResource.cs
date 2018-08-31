namespace Meridian.AuthServer.BusinessModel
{
    public class ApplicationAPIResource
    {        
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ApiResourceId { get; set; }
        public string APIResourceName { get; set; }
    }
}
