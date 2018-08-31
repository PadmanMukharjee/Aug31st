namespace Meridian.AuthServer.Repository
{
    public class APIResourceRepository
    {
        //private readonly IM3PactDbContext _dbContext;
        //public APIResourceRepository(IM3PactDbContext dbContext) : base(dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        //#region Mappings

        //public override IEnumerable<BusinessModel.APIResource> Map(IEnumerable<ApiResource> obj)
        //{
        //    List<BusinessModel.APIResource> domainObjects = new List<BusinessModel.APIResource>();
        //    foreach (ApiResource dataObject in obj)
        //    {
        //        domainObjects.Add(Map(dataObject));
        //    }
        //    return domainObjects;
        //}

        //public override IEnumerable<ApiResource> Map(IEnumerable<BusinessModel.APIResource> obj)
        //{
        //    List<ApiResource> dataObjects = new List<ApiResource>();
        //    foreach (BusinessModel.APIResource domainObject in obj)
        //    {
        //        dataObjects.Add(Map(domainObject));
        //    }
        //    return dataObjects;
        //}

        //public override BusinessModel.APIResource Map(ApiResource obj)
        //{
        //    if (obj != null)
        //    {
        //        BusinessModel.APIResource domainApiResource = new BusinessModel.APIResource()
        //        {
        //            Id = obj.Id,
        //            Enabled = obj.Enabled,
        //            Name = obj.Name,
        //            Description = obj.Description
        //        };
        //        return domainApiResource;
        //    }
        //    return null;
        //}

        //public override ApiResource Map(BusinessModel.APIResource obj)
        //{
        //    if (obj != null)
        //    {
        //        ApiResource dataApiResource = new ApiResource()
        //        {
        //            Name = obj.Name,
        //            Id = obj.Id,
        //            Enabled = obj.Enabled,
        //            Description = obj.Description
        //        };
        //        return dataApiResource;
        //    }
        //    return null;
        //}

        //public override ApiResource Map(ApiResource _objectData, BusinessModel.APIResource _objectDomain)
        //{
        //    if (_objectData != null && _objectDomain != null)
        //    {
        //        _objectData.Id = _objectDomain.Id;
        //        _objectData.Name = _objectDomain.Name;
        //        _objectData.Description = _objectDomain.Description;
        //        _objectData.Enabled = _objectDomain.Enabled;

        //        return _objectData;
        //    }
        //    return null;
        //}

        //#endregion
    }
}
