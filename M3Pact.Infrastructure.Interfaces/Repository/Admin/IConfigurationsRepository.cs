using M3Pact.BusinessModel.Admin;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.Admin
{
    public interface IConfigurationsRepository
    {
        List<AttributesBusinessModel> GetAttributesForConfig();
        bool SaveAttributeValue(AttributesBusinessModel attribute);
    }
}
