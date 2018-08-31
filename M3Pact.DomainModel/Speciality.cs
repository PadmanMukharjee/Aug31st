using System;
using System.Collections.Generic;

namespace M3Pact.DomainModel.DomainModels
{
    public partial class Speciality
    {
        public Speciality()
        {
            Client = new HashSet<Client>();
        }

        public int SpecialityId { get; set; }
        public string SpecialityCode { get; set; }
        public string SpecialityName { get; set; }
        public string SpecialityDescription { get; set; }
        public string RecordStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }

        public ICollection<Client> Client { get; set; }
    }
}
