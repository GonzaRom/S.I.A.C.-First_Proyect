using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class module
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public module()
        {
            operations = new HashSet<operations>();
        }

        public int id { get; set; }
        public string name { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<operations> operations { get; set; }
    }
}