using System;
using System.Collections.Generic;

namespace Meridian.AuthServer.BusinessModel
{
    public class Application
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public int? TypeId { get; set; }
        public string RedirectUrl { get; set; }
        public virtual ICollection<ApplicationAPIResource> ApplicationAPIResources { get; set; }
    }
}