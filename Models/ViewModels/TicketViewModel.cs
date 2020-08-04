using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using S.I.A.C.Service;

namespace S.I.A.C.Models
{
    public class TicketViewModel
    {
        private const int EstimatedFinishDays = 7; //Por defecto se asigna solo 7 dias como fecha estimativa
        private const int Pendiente = 1; //Se construye como pendiente
        private readonly ViewUtilityServices _viewUtilityServices;


        public TicketViewModel()
        {
            _viewUtilityServices = new ViewUtilityServices();
            creationDate = DateTime.Now;
            estimatedFinishDate = DateTime.Now.AddDays(EstimatedFinishDays);
            idStatus = Pendiente;
            ticketIdLocal = new Guid();
        }

        public Guid ticketIdLocal { get; }
        [Display(Name = "Ticket N°:")] public int idLocal { get; set; } //<----Buscar alernativa

        [Display(Name = "Descripción:")]
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(900, ErrorMessage = "Maximo 900 caracteres")]
        public string description { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha creacion:")]
        public DateTime creationDate { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha estimativa:")]
        public DateTime estimatedFinishDate { get; set; }

        [Display(Name = "Prioridad:")] public int idPriority { get; set; }
        [Display(Name = "Categoria:")] public int idCategory { get; set; }
        [Display(Name = "Cliente:")] public int idClient { get; set; }
        [Display(Name = "Tecnico Asignado:")] public int? idAssignedTechnician { get; set; }
        [Display(Name = "Estado:")] public int idStatus { get; set; }

        public List<SelectListItem> prioritiesList => _viewUtilityServices.GetListOfPriorities();

        public List<SelectListItem> categoriesList => _viewUtilityServices.GetListOfCategories();

        public List<SelectListItem> technicianList => _viewUtilityServices.GetListOfTechnicians();

        public List<SelectListItem> clientsList => _viewUtilityServices.GetListOfClients();

        public List<SelectListItem> rolsList => _viewUtilityServices.GetListOfRols();
    }

    public class TicketPrintableModel
    {
        [Display(Name = "Ticket N°:")] public int idLocal { get; set; } //<----Buscar alernativa

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

        public string getFullName(string name, string lastName)
        {
            var fullName = new StringBuilder();
            fullName.Append(name);
            fullName.Append(" ");
            fullName.Append(lastName);
            return fullName.ToString();
        }
    }
}