using M3Pact.BusinessModel.BusinessModels;
using System.Collections.Generic;

namespace M3Pact.ViewModel.HeatMap
{
    public class HeatMapFiltersData : ValidationViewModel
    {
        public List<AllUsersViewModel> allUsersViewModel { get; set; }
        public List<BusinessModel.Admin.System> systemList { get; set; }
        public List<BusinessUnit> businessUnitList { get; set; }
        public List<Speciality> specialityList { get; set; }

    }
}
