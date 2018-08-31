using M3Pact.BusinessModel.AlertAndEscalation;
using M3Pact.DomainModel.DomainModels;
using M3Pact.Infrastructure;
using M3Pact.Infrastructure.Interfaces.Business.AlertsAndEscalation;
using M3Pact.Infrastructure.Interfaces.Repository.AlertsAndEscalation;
using M3Pact.LoggerUtility;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;

namespace M3Pact.Business.AlertAndEscalation
{
    public class AlertAndEscalationBusiness : IAlertAndEscalation
    {

        #region private variables
        private IAlertAndEscalationRepository _alertAndEscalationRepository;
        private IConfiguration _configuration;
        private ILogger _logger;
        #endregion private variables

        public AlertAndEscalationBusiness(IAlertAndEscalationRepository alertAndEscalationRepository, IConfiguration configuration, ILogger logger)
        {
            _alertAndEscalationRepository = alertAndEscalationRepository;
            _configuration = configuration;
            _logger = logger;
        }
        /// <summary>
        /// this method will be called from Console app to kick start the job
        /// </summary>
        public void SendAlertDaily()
        {
            int jobRunId = _alertAndEscalationRepository.SaveJobRun();
            try
            {
                List<int?> BillingManagers = new List<int?>();
                List<int?> RelationshipManagers = new List<int?>();
                _alertAndEscalationRepository.GetBillingAndRelationshipManagers(out BillingManagers, out RelationshipManagers);
                BillingManagers = BillingManagers.Union(RelationshipManagers).ToList();
                RelationshipManagers = RelationshipManagers.Except(BillingManagers).ToList();
                List<int> AlertRecepients = _alertAndEscalationRepository.GetAlertRecepient();
                List<int?> alertRecepientExcludingRMBM = AlertRecepients?.Select(i => (int?)i).ToList();
                alertRecepientExcludingRMBM = alertRecepientExcludingRMBM.Except(BillingManagers).Except(RelationshipManagers).ToList();


                if (BillingManagers.Count > 0)
                {
                    foreach (int billingManager in BillingManagers)
                    {
                        _alertAndEscalationRepository.GetUserNameAndEmail(billingManager, out string userEmail, out string userName);

                        List<DeviatedClientKpi> deviatedClientKpis = _alertAndEscalationRepository.ManagersDeviatedKPIs(billingManager, DomainConstants.BillingManager);
                        if (deviatedClientKpis != null && deviatedClientKpis.Count > 0)
                        {
                            List<MailEntity> billingManagerMailEntities = _alertAndEscalationRepository.FormMailEntities(deviatedClientKpis, DomainConstants.BillingManager, billingManager);
                            if (billingManagerMailEntities.Count > 0)
                            {
                                EmailDTO emailDTO = GetEmailDTO(billingManagerMailEntities, userName, userEmail, deviatedClientKpis, billingManager);
                                SendEmail(emailDTO);
                                _alertAndEscalationRepository.SaveMailRecepientDetails(deviatedClientKpis, billingManager, DomainConstants.Alert);
                            }
                        }
                    }
                }
                if (RelationshipManagers.Count > 0)
                {
                    foreach (int relationshipManager in RelationshipManagers)
                    {
                        _alertAndEscalationRepository.GetUserNameAndEmail(relationshipManager, out string userEmail, out string userName);

                        List<DeviatedClientKpi> deviatedClientKpis = _alertAndEscalationRepository.ManagersDeviatedKPIs(relationshipManager, DomainConstants.RelationshipManager);
                        if (deviatedClientKpis != null && deviatedClientKpis.Count > 0)
                        {
                            List<MailEntity> relationshipManagerMailEntities = _alertAndEscalationRepository.FormMailEntities(deviatedClientKpis, DomainConstants.RelationshipManager, relationshipManager);
                            if (relationshipManagerMailEntities.Count > 0)
                            {
                                EmailDTO emailDTO = GetEmailDTO(relationshipManagerMailEntities, userName, userEmail, deviatedClientKpis, relationshipManager);
                                SendEmail(emailDTO);
                                _alertAndEscalationRepository.SaveMailRecepientDetails(deviatedClientKpis, relationshipManager, DomainConstants.Alert);
                            }
                        }
                    }
                }
                if (alertRecepientExcludingRMBM.Count > 0)
                {
                    foreach (int alertRecepient in alertRecepientExcludingRMBM)
                    {
                        _alertAndEscalationRepository.GetUserNameAndEmail(alertRecepient, out string userEmail, out string userName);

                        List<DeviatedClientKpi> deviatedClientKpis = _alertAndEscalationRepository.AlertRecepientDeviatedKPIs(alertRecepient);
                        if (deviatedClientKpis != null && deviatedClientKpis.Count > 0)
                        {
                            List<MailEntity> mailEntities = _alertAndEscalationRepository.FormMailEntities(deviatedClientKpis, DomainConstants.AlertRecepient, alertRecepient);
                            if (mailEntities.Count > 0)
                            {
                                EmailDTO emailDTO = GetEmailDTO(mailEntities, userName, userEmail, deviatedClientKpis, alertRecepient);
                                SendEmail(emailDTO);
                                _alertAndEscalationRepository.SaveMailRecepientDetails(deviatedClientKpis, alertRecepient, DomainConstants.Alert);
                            }
                        }
                    }
                }
                _alertAndEscalationRepository.UpdateJobRun(jobRunId, BusinessConstants.Completed);

                // Escalations
                _alertAndEscalationRepository.UpdateJobRun(jobRunId, BusinessConstants.EscalationStart);
                SendEscalatedAlertDaily();
                _alertAndEscalationRepository.UpdateJobRun(jobRunId, BusinessConstants.EscalationEnd);
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                _alertAndEscalationRepository.UpdateJobRun(jobRunId, BusinessConstants.Failed);
            }
        }
        /// <summary>
        /// Form mail content and send email
        /// </summary>
        /// <param name="mailEntities"></param>
        /// <param name="userName"></param>
        /// <param name="userEmail"></param>
        /// <param name="deviatedClientKpis"></param>
        /// <param name="alertRecepient"></param>
        private EmailDTO GetEmailDTO(List<MailEntity> mailEntities, string userName, string userEmail, List<DeviatedClientKpi> deviatedClientKpis, int alertRecepient)
        {
            EmailDTO emailDTO = new EmailDTO();
            string body = GetMailBody(mailEntities);
            body = body.Replace("##RECIPIENTNAME", userName);
            emailDTO.MailSubject = DomainConstants.AlertMailSubject + " " + DateTime.Now.Date.ToString("MM/dd/yyyy");
            emailDTO.Body = body;
            emailDTO.IsBodyHtml = true;
            // emailDTO.ToMail = int.Parse(Helper.GetConfigurationKey(BusinessConstants.USE_DEFAULT_EMAIL_FOR_M3PACT_USER)) == 1 ? Helper.GetConfigurationKey(BusinessConstants.MAIL_FROM) : userEmail;
            emailDTO.ToMail = int.Parse(_configuration.GetSection(BusinessConstants.USE_DEFAULT_EMAIL_FOR_M3PACT_USER).Value) == 1 ? _configuration.GetSection(BusinessConstants.MAIL_FROM).Value.ToString() : userEmail;
            return emailDTO;
        }

        /// <summary>
        /// Send Escalated Mails
        /// </summary>
        public void SendEscalatedAlertDaily()
        {
            try
            {
                List<MailEntity> mailEntities = _alertAndEscalationRepository.GetEscalatedMailDetails();
                List<string> toMailRecipients = mailEntities.SelectMany(x => x.EscalatedRecipients.Split(',')).Distinct().ToList();
                foreach (string recipient in toMailRecipients)
                {
                    string[] userMailAndID = recipient.Split('-');
                    string userName = userMailAndID[0].Split('.')[0];
                    List<MailEntity> toRecipientData = mailEntities.Where(x => x.EscalatedRecipients.Contains(recipient)).ToList();
                    EmailDTO emailDTO = new EmailDTO();
                    string body = GetMailBody(toRecipientData, true);
                    body = body.Replace("##RECIPIENTNAME", userName);
                    emailDTO.MailSubject = DomainConstants.EscalatedAlertMailSubject;
                    emailDTO.Body = body;
                    emailDTO.IsBodyHtml = true;
                    //emailDTO.ToMail = int.Parse(Helper.GetConfigurationKey(BusinessConstants.USE_DEFAULT_EMAIL_FOR_M3PACT_USER)) == 1 ? Helper.GetConfigurationKey(BusinessConstants.MAIL_FROM) : userMailAndID[0];
                    emailDTO.ToMail = int.Parse(_configuration.GetSection(BusinessConstants.USE_DEFAULT_EMAIL_FOR_M3PACT_USER).Value) == 1 ? _configuration.GetSection(BusinessConstants.MAIL_FROM).Value.ToString() : userMailAndID[0];
                    SendEmail(emailDTO);
                    List<int> deviatedClientKPIs = toRecipientData.Select(x => x.DeviatedClientKPIId).ToList();
                    _alertAndEscalationRepository.SaveMailRecepientDetails(deviatedClientKPIs, userMailAndID[1], DomainConstants.Escalation);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
            }
        }

        /// <summary>
        /// to send the Email to each user
        /// </summary>
        /// <param name="emailDTO"></param>
        //public void SendEmail(EmailDTO emailDTO)
        //{
        //    EmailUtility emailUtility = new EmailUtility();
        //    emailUtility.SendEmail(emailDTO);
        //}

        /// <summary>
        /// To get the mail body 
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public string GetMailBody(List<MailEntity> entities, bool isEscalation = false)
        {
            try
            {
                string messageBody = BusinessConstants.MAIL_DEVIATED_KPI_BODY_START;
                int width = isEscalation ? 1280 : 725;
                string htmlTableStart = "<table style='border-collapse:collapse;' width='" + width + "'>";
                string htmlHeaderRowStart = "<tr style='background-color:#BC3036; color:white; text-align:left;'>";
                string htmlTrStart = "<tr>";
                string htmlTrStartEvenRow = "<tr style='background-color: #f2f2f2;'>";
                string htmlTdStart = "<td style='border:1px solid #ddd; padding:8px;'>";
                string htmlTdEnd = "</td>";
                string htmlTrEnd = "</tr>";
                string htmlTableEnd = "</table><br>";

                List<string> distinctClients = entities.Select(x => x.Client).Distinct().ToList();
                foreach (string client in distinctClients)
                {
                    messageBody += client;
                    List<MailEntity> clientKPIs = new List<MailEntity>();
                    clientKPIs = entities.Where(x => x.Client == client).ToList();

                    messageBody += htmlTableStart;
                    messageBody += htmlHeaderRowStart;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_KPI + htmlTdEnd;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_DATE + htmlTdEnd;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_TYPE + htmlTdEnd;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_SLA + htmlTdEnd;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_STANDARD + htmlTdEnd;
                    messageBody += htmlTdStart + BusinessConstants.COLUMN_ACTUAL + htmlTdEnd;
                    if (isEscalation)
                    {
                        messageBody += htmlTdStart + BusinessConstants.COLUMN_DEVIATION + htmlTdEnd;
                        messageBody += htmlTdStart + BusinessConstants.COLUMN_BILLING_MGR + htmlTdEnd;
                        messageBody += htmlTdStart + BusinessConstants.COLUMN_RELATIONSHIP_MGR + htmlTdEnd;
                    }
                    messageBody += htmlTrEnd;

                    int i = 0;
                    foreach (MailEntity clientKPI in clientKPIs)
                    {
                        i += 1;
                        string trStart = i % 2 == 0 ? htmlTrStartEvenRow : htmlTrStart;
                        string isSLA = clientKPI.IsSLA == null ? BusinessConstants.NOT_APPLICABLE : (clientKPI.IsSLA == true ? BusinessConstants.YES : BusinessConstants.NO);
                        messageBody += trStart;
                        messageBody += htmlTdStart + clientKPI.KPIDescription + htmlTdEnd;
                        messageBody += htmlTdStart + clientKPI.ChecklistDate.ToString("MM/dd/yyyy") + htmlTdEnd;
                        messageBody += htmlTdStart + clientKPI.KPIType + htmlTdEnd;
                        messageBody += htmlTdStart + isSLA + htmlTdEnd;
                        messageBody += htmlTdStart + clientKPI.Standard + htmlTdEnd;
                        messageBody += htmlTdStart + clientKPI.Response + htmlTdEnd;
                        if (isEscalation)
                        {
                            messageBody += htmlTdStart + clientKPI.DeviatedSince + htmlTdEnd;
                            messageBody += htmlTdStart + clientKPI.BillingManager ?? string.Empty + htmlTdEnd;
                            messageBody += htmlTdStart + clientKPI.RelationshipManager ?? string.Empty + htmlTdEnd;
                        }
                        messageBody += htmlTrEnd;
                    }
                    messageBody += htmlTableEnd;
                }

                messageBody += BusinessConstants.MAIL_DEVIATED_KPI_BODY_END;

                return messageBody;
            }
            catch (Exception ex)
            {
                _logger.Log(ex, LogLevel.Error, ex.Message);
                return null;
            }
        }

        public bool SendEmail(EmailDTO emailDTO)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                string _fromMail = _configuration.GetSection(BusinessConstants.MAIL_FROM).Value.ToString();
                string _fromMailDisplayName = _configuration.GetSection(BusinessConstants.MAIL_FROM_DISPLAYNAME).Value.ToString();
                string _fromMailPassword = _configuration.GetSection(BusinessConstants.FROM_PASSWORD).Value.ToString();
                string _smtpHost = _configuration.GetSection(BusinessConstants.SMTP_HOST).Value.ToString();
                int _smtpPort = int.Parse(_configuration.GetSection(BusinessConstants.SMTP_PORT).Value);
                mailMessage.From = new MailAddress(_fromMail, _fromMailDisplayName);
                mailMessage.To.Add(new MailAddress(emailDTO.ToMail));
                mailMessage.Subject = emailDTO.MailSubject;
                mailMessage.Body = emailDTO.Body;
                mailMessage.IsBodyHtml = emailDTO.IsBodyHtml;
                smtpClient.Port = _smtpPort;
                smtpClient.Host = _smtpHost;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(_fromMail, _fromMailPassword);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        public void InsertDeviatedMetricKPi()
        {
            _alertAndEscalationRepository.InsertDeviatedMetricKPiAndHeatMapItemScore();
        }
    }
}
