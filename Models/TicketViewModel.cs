using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace S.I.A.C.Models
{
    public class TicketViewModel
    {
        private const int ESTIMATEDFINISHDAYS = 7; //Por defecto se asigna solo 7 dias como fecha estimativa
        private const int PENDIENTE = 1; //Se construye como pendiente

        public TicketViewModel()
        {
            creationDate = DateTime.Now;
            estimatedFinishDate = DateTime.Now.AddDays(ESTIMATEDFINISHDAYS);
            idStatus = PENDIENTE;
        }

        [Display(Name = "Descripción:")]
        [Required]
        [MaxLength(900, ErrorMessage = "Maximo 900 caracteres")]
        public string description { get; set; }

        [DataType(DataType.DateTime)] public DateTime creationDate { get; set; }

        [DataType(DataType.DateTime)] public DateTime estimatedFinishDate { get; set; }

        [Display(Name = "Prioridad:")] public int idPriority { get; set; }

        [Display(Name = "Categoria:")] public int idCategory { get; set; }
        [Display(Name = "Cliente:")] public int idClient { get; set; }
        [Display(Name = "Tecnico Asignado:")] public int? idAssignedTechnician { get; set; }
        public int idStatus { get; set; }

    }

    public class TicketPrintableModel
    {
        [Display(Name = "Fecha de carga:")] public DateTime CreationDateTime { get; set; }

        [Display(Name = "Cliente:")] public string client { get; set; }

        [Display(Name = "Cliente apellido:")] public string clientLastname { get; set; }

        [Display(Name = "Direccion:")] public string address { get; set; }

        [Display(Name = "Email:")] public string email { get; set; }

        [Display(Name = "Tarea a realizar")] public string description { get; set; }

        [Display(Name = "Estado")] public string status { get; set; }

        [Display(Name = "Asignado a:")] public string assignedTechnician { get; set; }

        [Display(Name = "Asignado apellido:")] public string assignedTechnicianLastname { get; set; }

        [Display(Name = "Creador:")] public string creatorPeople { get; set; }

        [Display(Name = "Creador apellido:")] public string creatorPeopleLastname { get; set; }

        public string getClientFullName()
        {
            var clientFullName = new StringBuilder();
            clientFullName.Append(client);
            clientFullName.Append(" ");
            clientFullName.Append(clientFullName);
            return clientFullName.ToString();
        }

        public string getCreatorFullName()
        {
            var creatorFullName = new StringBuilder();
            creatorFullName.Append(creatorPeople);
            creatorFullName.Append(" ");
            creatorFullName.Append(creatorPeopleLastname);
            return creatorFullName.ToString();
        }

        public string getTecniFullName()
        {
            var tecniFullName = new StringBuilder();
            tecniFullName.Append(assignedTechnician);
            tecniFullName.Append(" ");
            tecniFullName.Append(assignedTechnicianLastname);
            return tecniFullName.ToString();
        }
    }
}