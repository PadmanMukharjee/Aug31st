using System.Collections.Generic;

namespace M3Pact.BusinessModel.Admin
{
    public class ClientUserMap
    {
        public ClientUserMap()
        {
            ClientUsers = new List<AllUsers>();
        }
        public string ClientCode { get; set; }
        public List<AllUsers> ClientUsers { get; set; }
    }
}
