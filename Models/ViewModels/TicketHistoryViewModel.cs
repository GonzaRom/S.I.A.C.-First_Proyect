﻿using S.I.A.C.Models.DomainModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace S.I.A.C.Models.ViewModels
{
    public class TicketHistoryViewModel
    {
        [ScaffoldColumn(false)] public int idTicket { get; set; }

        [Display(Name = "Fecha Modificacion:")]
        public System.DateTime date { get; set; }

        [Display(Name = "Descripción:")]
        [Required]
        [MaxLength(900, ErrorMessage = "Maximo 900 caracteres")]
        public string note { get; set; }

        [Display(Name = "Estado:")] public Nullable<int> idStatus { get; set; }
        [ScaffoldColumn(false)] public int idPeople { get; set; }
        [Display(Name = "Modificado por:")] public string peopleFullName
        {
            get
            {
                return peopleName + " " + peopleLastName;
            }
        }
        [ScaffoldColumn(false)] public string peopleName { get; set; }
        [ScaffoldColumn(false)] public string peopleLastName { get; set; }

        /*public void SetFullName()
        {
            var fullName = new StringBuilder();
            fullName.Append(peopleName);
            fullName.Append(" ");
            fullName.Append(peopleLastName);

            this.peopleFullName = fullName.ToString();
        }*/
    }
}