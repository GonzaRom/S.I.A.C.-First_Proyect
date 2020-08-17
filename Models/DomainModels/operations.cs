using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace S.I.A.C.Models.DomainModels
{
    public class operations
    {
        [SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public operations()
        {
            rolOperations = new HashSet<rolOperations>();
        }

        public int id { get; set; }
        public string name { get; set; }
        public int idModule { get; set; }

        public virtual module module { get; set; }

        [SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rolOperations> rolOperations { get; set; }
    }
}