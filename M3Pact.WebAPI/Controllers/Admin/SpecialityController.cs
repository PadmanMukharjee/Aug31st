using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.ViewModel;
using M3Pact.WebAPI.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace M3Pact.WebAPI.Controllers.Admin
{
    [AuthorizationFilter]
    [Route("api/[controller]/[Action]")]
    public class SpecialityController : Controller
    {
        private ISpecialityBusiness _specialityBusiness;
        public SpecialityController(ISpecialityBusiness specialityBusiness)
        {
            _specialityBusiness = specialityBusiness;
        }

        /// <summary>
        /// API call to return the Specialities
        /// </summary>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Admin, Roles.Executive, Roles.Manager, Roles.User)]
        [HttpGet]
        public List<SpecialityViewModel> GetSpecialities(bool fromClient = false)
        {
            return _specialityBusiness.GetSpecialities(fromClient);
        }

        /// <summary>
        ///API call to save the Specilaities
        /// </summary>
        /// <param name="specialities"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Executive, Roles.Admin)]
        [HttpPost]
        public bool SaveSpecialities([FromBody]List<SpecialityViewModel> specialities)
        {
            if (specialities != null && specialities.Count > 0)
            {
                return _specialityBusiness.SaveSpecialities(specialities);
            }
            else
            {
                return false;
            }
        }

        [AuthorizationFilter(Roles.Executive, Roles.Admin)]
        [HttpPost]
        public bool ActiveOrInactiveSpecialities([FromBody] SpecialityViewModel speciality)
        {
            return _specialityBusiness.ActiveOrInactiveSpecialities(speciality);
        }

        /// <summary>
        /// To Get all the clients associated to the Specialty
        /// </summary>
        /// <param name="businessUnitId"></param>
        /// <returns></returns>
        [AuthorizationFilter(Roles.Executive, Roles.Admin)]
        [HttpGet]
        public List<string> GetClientsAssociatedWithSpecialty(int specialtyId)
        {
            return _specialityBusiness.GetClientsAssociatedWithSpecialty(specialtyId);
        }
    }
}
