using Meridian.AuthServer.DataModel;
using System.Collections.Generic;

namespace Meridian.AuthServer.Repository
{
    public class ApplicationRepository
    {
        //public ApplicationRepository(IM3PactDbContext dbContext) : base(dbContext)
        //{
        //}

        //#region Mappings

        //public override IEnumerable<BusinessModel.Application> Map(IEnumerable<Application> obj)
        //{
        //    List<BusinessModel.Application> domainObjects = new List<BusinessModel.Application>();
        //    foreach (Application dataObject in obj)
        //    {
        //        domainObjects.Add(Map(dataObject));
        //    }
        //    return domainObjects;
        //}

        //public override IEnumerable<Application> Map(IEnumerable<BusinessModel.Application> obj)
        //{
        //    List<Application> dataObjects = new List<Application>();
        //    foreach (BusinessModel.Application domainObject in obj)
        //    {
        //        dataObjects.Add(Map(domainObject));
        //    }
        //    return dataObjects;
        //}

        //public override BusinessModel.Application Map(Application obj)
        //{
        //    if (obj != null)
        //    {
        //        BusinessModel.Application domainClientApp = new BusinessModel.Application()
        //        {
        //            Id = obj.Id,
        //            Name = obj.Name,
        //            Key = obj.Key,
        //            Secret = obj.Secret,
        //            TypeId = obj.TypeId,
        //            RedirectUrl = obj.RedirectUrl,
        //            ApplicationAPIResources = new List<BusinessModel.ApplicationAPIResource>()
        //        };

        //        foreach (var item in obj.ApplicationApiResource)
        //        {
        //            domainClientApp.ApplicationAPIResources.Add(new BusinessModel.ApplicationAPIResource()
        //            {
        //                ApplicationId = item.ApplicationId,
        //                ApiResourceId = item.ApiResourceId,
        //                APIResourceName = item.ApiResource.Name
        //            });
        //        }

        //        return domainClientApp;
        //    }
        //    return null;
        //}

        //public override Application Map(BusinessModel.Application obj)
        //{
        //    if (obj != null)
        //    {
        //        Application dataClientApp = new Application()
        //        {
        //            Key = obj.Key,
        //            Secret = obj.Secret,
        //            TypeId = obj.TypeId,
        //            RedirectUrl = obj.RedirectUrl
        //        };
        //        return dataClientApp;
        //    }
        //    return null;
        //}

        //public override Application Map(Application _objectData, BusinessModel.Application _objectDomain)
        //{
        //    if (_objectData != null && _objectDomain != null)
        //    {
        //        _objectData.Key = _objectDomain.Key;
        //        _objectData.Secret = _objectDomain.Secret;
        //        _objectData.TypeId = _objectDomain.TypeId;
        //        _objectData.RedirectUrl = _objectData.RedirectUrl;

        //        return _objectData;
        //    }
        //    return null;
        //}

        //#endregion
    }
}
