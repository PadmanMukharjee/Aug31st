namespace M3Pact.Infrastructure.Interfaces.Business.AlertsAndEscalation
{
    public interface IAlertAndEscalation
    {
        void SendAlertDaily();

        void InsertDeviatedMetricKPi();
    }
}
