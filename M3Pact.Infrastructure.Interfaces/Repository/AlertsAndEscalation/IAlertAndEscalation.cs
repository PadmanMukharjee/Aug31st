using M3Pact.BusinessModel.AlertAndEscalation;
using M3Pact.DomainModel.DomainModels;
using System.Collections.Generic;

namespace M3Pact.Infrastructure.Interfaces.Repository.AlertsAndEscalation
{
    public interface IAlertAndEscalationRepository
    {       
        void GetBillingAndRelationshipManagers(out List<int?> BillingManagers, out List<int?> RelationshipManagers);              
        List<int> GetAlertRecepient();
        List<DeviatedClientKpi> ManagersDeviatedKPIs(int userId, string managerType);
        List<MailEntity> FormMailEntities(List<DeviatedClientKpi> deviatedClientKpis, string managerType, int userId);
        int SaveJobRun();
        void UpdateJobRun(int jobRunId, string status);
        List<DeviatedClientKpi> AlertRecepientDeviatedKPIs(int userId);        
        void SaveMailRecepientDetails(List<DeviatedClientKpi> deviatedClientKpis, int userId, string alertType);
        List<MailEntity> GetEscalatedMailDetails();
        void SaveMailRecepientDetails(List<int> deviatedClientKpis, string userId, string alertType);
        bool IsMailAlreadySentForKPI(DeviatedClientKpi deviatedClientKpi, int userId);
        void InsertDeviatedMetricKPiAndHeatMapItemScore();
        void GetUserNameAndEmail(int userId, out string email, out string userName);
    }
}
