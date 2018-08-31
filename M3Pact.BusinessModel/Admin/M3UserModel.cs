using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.BusinessModel.Admin
{
    public class M3UserModel
    {
        public UserLoginDTO User { get; set; }
        public string BusinessUnit { get; set; }
        public string Title { get; set; }
        public string ReportsTo { get; set; }
        public string Site { get; set; }
        public List<string> Clients { get; set; }
        public bool IsUserExist { get; set; }
        public bool IsActiveEmployee { get; set; }
    }
}
