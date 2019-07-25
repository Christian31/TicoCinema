using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class RegisterMoviePriceViewModel
    {
        [Required]
        [Display(Name = "Formato")]
        public int MovieFormatId { get; set; }

        [Required]
        [Display(Name = "Precio")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Tipo de entrada")]
        public string Name { get; set; }
    }
}