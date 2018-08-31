using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class ApplicationType
    {
        public ApplicationType()
        {
            Application = new HashSet<Application>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Application> Application { get; set; }
    }
}
