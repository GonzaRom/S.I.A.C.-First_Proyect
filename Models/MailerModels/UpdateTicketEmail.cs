using Postal;

namespace S.I.A.C.Models.MailerModels
{
    public class UpdateTicketEmail : Email
    {
        public string to { get; set; }
        public string cc { get; set; }
        public string userName { get; set; }
        public string technicianName { get; set; }
        public string baseComment { get; set; }
        public string updateComment { get; set; }
        public int idTicket { get; set; }
    }
}