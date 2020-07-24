using S.I.A.C.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;

namespace S.I.A.C.Models.ViewModels
{
    public class TicketHistoryViewModel
    {
        public int id { get; set; }
        public int idTicket { get; set; }
        public System.DateTime date { get; set; }

        [Display(Name = "Descripción:")]
        [Required]
        [MaxLength(900, ErrorMessage = "Maximo 900 caracteres")]
        public string note { get; set; }

        public Nullable<int> idStatus { get; set; }
        public int idPeople { get; set; }

        public virtual people people { get; set; }
        public virtual status status { get; set; }
        public virtual ticket ticket { get; set; }
    }
}