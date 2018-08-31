using M3Pact.Business.Admin;
using M3Pact.Business.ClientData;
using M3Pact.Business.Common;
using M3Pact.Business.DepositLog;
using M3Pact.Business.Payer;
using M3Pact.Business.User;
using M3Pact.Infrastructure.Interfaces.Business.Admin;
using M3Pact.Infrastructure.Interfaces.Business.ClientData;
using M3Pact.Infrastructure.Interfaces.Business.DepositLog;
using M3Pact.Infrastructure.Interfaces.Business.Payer;
using M3Pact.Infrastructure.Interfaces.Business.User;
using M3Pact.Infrastructure.Interfaces.Repository;
using M3Pact.Infrastructure.Interfaces.Repository.Admin;
using M3Pact.Infrastructure.Interfaces.Repository.ClientData;
using M3Pact.Infrastructure.Interfaces.Repository.User;
using M3Pact.Repository;
using M3Pact.Repository.Admin;
using M3Pact.Repository.ClientData;
using M3Pact.Repository.Payer;
using M3Pact.Repository.User;
using Microsoft.Extensions.DependencyInjection;
using M3Pact.Infrastructure.Interfaces.Business.Checklist;
using M3Pact.Business.Checklist;
using M3Pact.Infrastructure.Interfaces.Repository.Checklist;
using M3Pact.Repository.Checklist;
using M3Pact.Infrastructure.Interfaces.Business.AlertsAndEscalation;
using M3Pact.Business.AlertAndEscalation;
using M3Pact.Infrastructure.Interfaces.Repository.AlertsAndEscalation;
using M3Pact.Repository.AlertAndEscalation;
using M3Pact.Infrastructure.Interfaces.Repository.HeatMap;
using M3Pact.Repository.HeatMap;
using M3Pact.Infrastructure.Interfaces.Business.HeatMap;
using M3Pact.Business.HeatMap;
using M3Pact.Infrastructure.Interfaces.Business.ToDo;
using M3Pact.Business.ToDo;
using M3Pact.Infrastructure.Interfaces.Repository.ToDo;
using M3Pact.Repository.ToDo;

namespace M3Pact.Business
{
    public static class IServiceCollectionExtension
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IPayerBusiness, PayerBusiness>();
            services.AddTransient<IDepositLogBusiness, DepositLogBusiness>();
            services.AddTransient<ISiteBusiness, SiteBusiness>();
            services.AddTransient<IBusinessUnitBusiness, BusinessUnitBusines>();
            services.AddTransient<IBusinessDaysBusiness, BusinessDaysBusiness>();
            services.AddTransient<ISpecialityBusiness, SpecialityBusiness>();
            services.AddTransient<IClientDataBusiness, ClientDataBusiness>();
            services.AddTransient<ISystemBusiness, SystemBusiness>();
            services.AddTransient<IAllUsersBusiness, AllUsersBusiness>();
            services.AddTransient<IClientUserBusiness, ClientUserBusiness>();
            services.AddTransient<IUserBusiness, UserBusiness>();
            services.AddTransient<Helper>();
            services.AddScoped<ICheckListBusiness, CheckListBusiness>();
            services.AddTransient<IKPIBusiness, KPIBusiness>();
            services.AddScoped<IPendingChecklistBusiness, PendingChecklistBusiness>();
            services.AddTransient<IAlertAndEscalation, AlertAndEscalationBusiness>();
            services.AddTransient<Infrastructure.Interfaces.Business.Admin.IHeatMapBusiness, Admin.HeatMapBusiness>();
            services.AddTransient<IClientsHeatMapBusiness, ClientsHeatMapBusiness>();
            services.AddTransient<IConfigurationsBusiness, ConfigurationsBusiness>();
            services.AddTransient<IToDoBusiness, ToDoBusiness>();

            services.AddTransient<IPayerRepository, PayerRepository>();
            services.AddTransient<ISiteRepository, SiteRepository>();
            services.AddTransient<IBusinesssUnitRepository, BusinessUnitRepository>();
            services.AddTransient<IBusinessDaysRepository, BusinessDaysRepository>();
            services.AddTransient<ISpecialityRespository, SpecialityRepository>();
            services.AddTransient<IDepositLogRepository, Repository.DepositLog.DepositLogRepository>();
            services.AddTransient<IClientDataRepository, ClientDataRepository>();
            services.AddTransient<ISystemRepository, SystemRepository>();
            services.AddTransient<IAllUserRepository, AllUserRepository>();
            services.AddTransient<IClientUserRepository, ClientUserRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddScoped<ICheckListRepository, CheckListRepository>();
            services.AddTransient<IKPIRepository, KPIRepository>();
            services.AddScoped<IPendingChecklistRepository, PendingChecklistRepository>();
            services.AddTransient<IAlertAndEscalationRepository, AlertAndEscalationRepository>();
            services.AddTransient<Infrastructure.Interfaces.Repository.Admin.IHeatMapRepository, Repository.Admin.HeatMapRepository>();
            services.AddTransient<IClientsHeatMapRepository, ClientsHeatMapRepository>();
            services.AddTransient<IConfigurationsRepository, ConfigurationRepository>();
            services.AddTransient<IToDoRepository, ToDoRepository>();

            return services;
        }
    }
}
