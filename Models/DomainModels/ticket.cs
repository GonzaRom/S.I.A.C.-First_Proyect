using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class ticket
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ticket()
        {
            ticketHistory = new HashSet<ticketHistory>();
        }

        public int id { get; set; }
        public int idCreatorPeople { get; set; }
        public int? idAssignedTechnician { get; set; }
        public string description { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime estimatedFinishDate { get; set; }
        public DateTime? editionDate { get; set; }
        public int idStatus { get; set; }
        public int idPriority { get; set; }
        public int idCategory { get; set; }
        public int? idClient { get; set; }
        public int idLocal { get; set; }

        public virtual category category { get; set; }
        public virtual people people { get; set; }
        public virtual people people1 { get; set; }
        public virtual priority priority { get; set; }
        public virtual status status { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticketHistory> ticketHistory { get; set; }
    }
}