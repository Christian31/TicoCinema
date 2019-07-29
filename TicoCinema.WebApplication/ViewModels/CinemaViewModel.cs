using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class CinemaViewModel
    {
        public int CinemaId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Formato de sala")]
        public int MovieFormatId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Capacidad")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo")]
        public string Capacity { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Diseño de sala")]
        public Utils.Enums.CinemaDesign CinemaDesignId { get; set; }
        
        [Display(Name = "Diseño de sala")]
        public string CinemaDesignName { get; set; }
        
        [Display(Name = "Formato de sala")]
        public string MovieFormatName { get; set; }
    }
}