using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class priority
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public priority()
        {
            ticket = new HashSet<ticket>();
        }

        public int id { get; set; }
        public string name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ticket> ticket { get; set; }
    }
}