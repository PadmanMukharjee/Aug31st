using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
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
