using System.ComponentModel.DataAnnotations;

namespace S.I.A.C.Models
{
    /// <summary>
    ///     Valida datos antes de hacer login
    /// </summary>
    public class LoginDataModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [RegularExpression(
            @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4})(\]?)$",
            ErrorMessage = "Email no valido")]
        [StringLength(125, ErrorMessage = "Longitud maxima 100")]
        [DataType(DataType.EmailAddress)]
        public string email { get; set; }


        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [StringLength(15, ErrorMessage = "Longitud entre 6 y 15 caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}