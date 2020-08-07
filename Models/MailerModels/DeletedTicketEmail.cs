using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Postal;

namespace S.I.A.C.Models.MailerModels
{
    public class DeletedTicketEmail : Email
    {
        public string to { get; set; }
        public string cc { get; set; }
        public string clientName { get; set; }
        public string currentUser { get; set; }
        public string baseComment { get; set; }
        public int idTicket { get; set; }
    }
}