using System.ComponentModel.DataAnnotations;

namespace S.I.A.C.Models
{
    public class RegistrationViewModel
    {
        [Required(ErrorMessage = "El DNI es obligatoria")]
        [DataType(DataType.Text)]
        [Display(Name = "Dni")]
        public string dni { get; set; }

        [Required(ErrorMessage = "El nombre es obligatoria")]
        [DataType(DataType.Text)]
        [Display(Name = "Nombre")]
        public string name { get; set; }

        [Required(ErrorMessage = "El apellido es obligatoria")]
        [DataType(DataType.Text)]
        [Display(Name = "Apellido")]
        public string lastname { get; set; }

        [Required(ErrorMessage = "El email es obligatoria")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string pass { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme Contraseña")]
        [Compare("pass", ErrorMessage = "Contraseña no coincide.")]
        public string confirmPass { get; set; }

        [Required(ErrorMessage = "La direccion es obligatoria")]
        [DataType(DataType.Text)]
        [Display(Name = "Direccion")]
        public string address { get; set; }

        [Required(ErrorMessage = "Id Incorrecto")]
        [Display(Name = "Rol")]
        public int idRol { get; set; }
    }
}