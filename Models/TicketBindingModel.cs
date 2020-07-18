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
        private const int ESTIMATEDFINISHDAYS = 7; //Por defecto se asigna solo 7 dias como fecha estimativa
        private const int PENDIENTE = 1; //Se construye como pendiente

        public TicketBindingModel()
        {
            this.creationDate = DateTime.Now;
            this.estimatedFinishDate = DateTime.Now.AddDays(ESTIMATEDFINISHDAYS);
            this.idStatus = PENDIENTE;
        }

        [Display(Name = "Descripción:")]
        [Required]
        [MaxLength(900, ErrorMessage = "Maximo 900 caracteres")]
        public string description { get; set; }

        [DataType(DataType.DateTime)] public DateTime creationDate { get; set; }

        [DataType(DataType.DateTime)] public DateTime estimatedFinishDate { get; set; }

        [Range(1, 4)] [Display(Name = "Prioridad:")] public int idPriority { get; set; }

        [Display(Name = "Categoria:")] public int idCategory { get; set; }

        [Display(Name = "Tecnico Asignado:")] public Nullable<int> idAssignedTechnician { get; set; }
        public int idStatus { get; set; }
    }
}