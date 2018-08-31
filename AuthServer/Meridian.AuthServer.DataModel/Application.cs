using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.DataModel
{
    public partial class Application
    {
        public Application()
        {
            ApplicationApiResource = new HashSet<ApplicationApiResource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public int? TypeId { get; set; }
        public string RedirectUrl { get; set; }

        public ApplicationType Type { get; set; }
        public ICollection<ApplicationApiResource> ApplicationApiResource { get; set; }
    }
}
