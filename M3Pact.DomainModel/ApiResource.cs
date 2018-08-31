using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ApiResource
    {
        public ApiResource()
        {
            ApplicationApiResource = new HashSet<ApplicationApiResource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Enabled { get; set; }

        public ICollection<ApplicationApiResource> ApplicationApiResource { get; set; }
    }
}
