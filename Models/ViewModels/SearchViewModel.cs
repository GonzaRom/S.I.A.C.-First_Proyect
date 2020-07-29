using System.ComponentModel.DataAnnotations;

namespace S.I.A.C.Models.ViewModels
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Este campo es obligatorio.")]
        
        public string toSearch { get; set; }
    }
}