
namespace M3Pact.Infrastructure
{
    public class EmailDTO
    {
        public string FromMail { get; set; }
        public string ToMail { get; set; }
        public string FromMailPassword { get; set; }
        public string MailSubject { get; set; }
        public string Body { get; set; }
        public bool IsBodyHtml { get; set; }

    }
}
