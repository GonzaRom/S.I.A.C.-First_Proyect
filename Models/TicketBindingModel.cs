using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace S.I.A.C.Models
{
    public class TicketBindingModel 
    {
        
            [Display (Name = "Descripción")]
            [Required]
            [RegularExpression(@"^{0,500}$", ErrorMessage = "Excediste el maximo de 500 caracteres.")]
            public string description { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime CreationDateTime { get; set; }

            [DataType(DataType.DateTime)]
            public DateTime EstimatedFinishDateTime { get; set; }

            [Display(Name = "Prioridad")] 
            public int idPriority { get; set; }

            [Display(Name = "Categoria")]
            public int idCategory { get; set; }
    }
}