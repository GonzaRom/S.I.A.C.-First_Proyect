using System;

namespace S.I.A.C.Models.DomainModels
{
    public class ticketHistory
    {
        public int id { get; set; }
        public int idTicket { get; set; }
        public DateTime date { get; set; }
        public string note { get; set; }
        public int idStatus { get; set; }
        public int idPeople { get; set; }

        public virtual people people { get; set; }
        public virtual status status { get; set; }
        public virtual ticket ticket { get; set; }
    }
}