using Postal;

namespace S.I.A.C.Models.MailerModels
{
    public class NewTicketEmail : Email
    {
        public string to { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
    }
}