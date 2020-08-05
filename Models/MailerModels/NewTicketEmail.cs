using Postal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace S.I.A.C.Models.MailerModels
{
    public class NewTicketEmail : Email
    {
        public string to { get; set; }
        public string userName { get; set; }
        public string comment { get; set; }
    }
}