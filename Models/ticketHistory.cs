//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace S.I.A.C.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ticketHistory
    {
        public int id { get; set; }
        public int idTicket { get; set; }
        public System.DateTime date { get; set; }
        public string note { get; set; }
        public Nullable<int> idStatus { get; set; }
        public int idPeople { get; set; }
    
        public virtual people people { get; set; }
        public virtual status status { get; set; }
        public virtual ticket ticket { get; set; }
    }
}
