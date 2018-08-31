using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.DataModel
{
    public partial class ApplicationApiResource
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public int ApiResourceId { get; set; }

        public ApiResource ApiResource { get; set; }
        public Application Application { get; set; }
    }
}
