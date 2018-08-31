using Notification.Helpers.Interface;
using ConfigManager;
using MailKit.Net.Smtp;
using MimeKit;
namespace Notification.Helpers
{
    /// <summary>
    /// Email Helper to send Emails
    /// </summary>
    public class EmailHelper : IEmailHelper
    {
        IConfigurationProvider config;

        private readonly string _fromMail;
        private readonly string _toMail;
        private readonly string _fromMailPassword;
        private readonly string _mailSubject;
        private readonly string _smtpHost;
        private readonly int _smtpPort;

        /// <summary>
        /// Constructor creates an object of IEmailHelper
        /// </summary>
        public EmailHelper()
        {
            var appsettings = new ConfigurationProvider();
            var configFile = appsettings.GetValue(Constants.ConfigFileName);
            config = new ConfigurationProvider(configFile);

            _fromMail = config.GetValue(Constants.MailFrom);
            _toMail = config.GetValue(Constants.MailTo);
            _fromMailPassword = config.GetValue(Constants.FromPassword);
            _smtpHost = config.GetValue(Constants.SMTPHost);
            _smtpPort = int.Parse(config.GetValue(Constants.SMTPPort));
            _mailSubject = config.GetValue(Constants.MailSubject);

        }

        /// <summary>
        /// Send the Email
        /// </summary>
        public void Send(string messageContent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", _fromMail));
            message.To.Add(new MailboxAddress("", _toMail));
            message.Subject = _mailSubject;

            message.Body = new TextPart("plain")
            {
                Text = messageContent
            };

            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(_smtpHost, _smtpPort , false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(_fromMail , _fromMailPassword);
                client.Send(message);
                client.Disconnect(true);
            }

        }


    }
}
