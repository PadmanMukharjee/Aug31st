using System;
using System.Collections.Generic;
using System.Text;

namespace Meridian.AuthServer.BusinessModel
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleDesc { get; set; }
        public string RecordStatus { get; set; }
    }
}
