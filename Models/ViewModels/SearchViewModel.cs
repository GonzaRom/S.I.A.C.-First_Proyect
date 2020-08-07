using System.ComponentModel.DataAnnotations;

namespace S.I.A.C.Models.ViewModels
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        [Display(Name = "A Buscar:")]
        public string toSearch { get; set; }
    }
}