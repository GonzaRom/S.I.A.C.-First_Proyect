using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace S.I.A.C.Models
{
    public class TicketBindingModel 
    {
        public TicketBindingModel()
        {
            [Key]
            [Required]
             int id = Guid.NewGuid().Convert.ToInt32();
        }
    }
}