namespace Meridian.AuthServer.Repository
{
    public class UserRepository
    {
        //public UserRepository(IM3PactDbContext dbContext) : base(dbContext)
        //{
        //}

        //#region Mappings

        //public override IEnumerable<BusinessModel.UserLogin> Map(IEnumerable<UserLogin> obj)
        //{
        //    List<BusinessModel.UserLogin> domainObjects = new List<BusinessModel.UserLogin>();
        //    foreach (UserLogin dataObject in obj)
        //    {
        //        domainObjects.Add(Map(dataObject));
        //    }
        //    return domainObjects;
        //}

        //public override IEnumerable<UserLogin> Map(IEnumerable<BusinessModel.UserLogin> obj)
        //{
        //    List<UserLogin> dataObjects = new List<UserLogin>();
        //    foreach (BusinessModel.UserLogin domainObject in obj)
        //    {
        //        dataObjects.Add(Map(domainObject));
        //    }
        //    return dataObjects;
        //}

        //public override BusinessModel.UserLogin Map(UserLogin obj)
        //{
        //    if (obj != null)
        //    {
        //        BusinessModel.UserLogin domainUser = new BusinessModel.UserLogin()
        //        {
        //            UserName = obj.UserName,
        //            Password = obj.Password,
        //            FirstName = obj.FirstName,
        //            LastName = obj.LastName,
        //            Email = obj.Email,
        //            UserId = obj.UserId,
        //            Id = obj.Id,
        //            RoleId = obj.Role.RoleId,
        //            RoleName = obj.Role.RoleCode
        //        };
        //        return domainUser;
        //    }
        //    return null;
        //}

        //public override UserLogin Map(BusinessModel.UserLogin obj)
        //{
        //    if (obj != null)
        //    {
        //        UserLogin dataUser = new UserLogin()
        //        {
        //            UserName = obj.UserName,
        //            Password = obj.Password,
        //            FirstName = obj.FirstName,
        //            LastName = obj.LastName,
        //            Email = obj.Email,
        //            UserId = obj.UserId,
        //            Id = obj.Id,
        //        };

        //        return dataUser;
        //    }
        //    return null;
        //}

        //public override UserLogin Map(UserLogin _objectData, BusinessModel.UserLogin _objectDomain)
        //{
        //    if (_objectData != null && _objectDomain != null)
        //    {
        //        _objectData.UserName = _objectDomain.UserName;
        //        _objectData.FirstName = _objectDomain.FirstName;
        //        _objectData.LastName = _objectDomain.LastName;
        //        _objectData.Email = _objectDomain.Email;
        //        _objectData.Password = _objectDomain.Password;

        //        return _objectData;
        //    }
        //    return null;
        //}

        //#endregion
    }
}
