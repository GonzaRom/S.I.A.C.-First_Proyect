using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class rol
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public rol()
        {
            people = new HashSet<people>();
            rolOperations = new HashSet<rolOperations>();
        }

        public int id { get; set; }
        public string name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<people> people { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rolOperations> rolOperations { get; set; }
    }
}