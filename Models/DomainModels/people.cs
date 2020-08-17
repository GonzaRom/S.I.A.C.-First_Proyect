using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class people
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public people()
        {
            ticket = new HashSet<ticket>();
            ticket1 = new HashSet<ticket>();
            ticketHistory = new HashSet<ticketHistory>();
        }

        public int id { get; set; }
        public string dni { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        public string email { get; set; }
        public string pass { get; set; }
        public string address { get; set; }
        public DateTime? creationDate { get; set; }
        public int idRol { get; set; }
        public int isActive { get; set; }

        public virtual rol rol { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket> ticket { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket> ticket1 { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticketHistory> ticketHistory { get; set; }
    }
}