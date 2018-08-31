using M3Pact.ViewModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Business.Admin
{
    public interface IConfigurationsBusiness
    {
        List<AttributesViewModel> GetAttributesForConfig();
        bool SaveAttributeValue(AttributesViewModel attribute);
    }
}
