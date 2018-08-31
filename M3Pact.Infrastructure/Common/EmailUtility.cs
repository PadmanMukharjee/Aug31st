using System;
using System.Net.Mail;

namespace M3Pact.Infrastructure.Common
{
    public static class EmailUtility
    {
        private static readonly string _fromMail;
        private static readonly string _fromMailDisplayName;
        private static readonly string _fromMailPassword;
        private static readonly string _smtpHost;
        private static readonly int _smtpPort;

        static EmailUtility()
        {
            _fromMail = Helper.GetConfigurationKey(BusinessConstants.MAIL_FROM);
            _fromMailDisplayName = Helper.GetConfigurationKey(BusinessConstants.MAIL_FROM_DISPLAYNAME);
            _fromMailPassword = Helper.GetConfigurationKey(BusinessConstants.FROM_PASSWORD);
            _smtpHost = Helper.GetConfigurationKey(BusinessConstants.SMTP_HOST);
            _smtpPort = int.Parse(Helper.GetConfigurationKey(BusinessConstants.SMTP_PORT));
        }

        /// <summary>
        /// Sends mail to give address
        /// </summary>
        /// <param name="emailDTO"></param>
        /// <returns></returns>
        public static bool SendEmail(EmailDTO emailDTO)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
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
    }
}
