using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class RegisterAudienceClassificationViewModel
    {
        public int AudienceClassificationId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Acrónimo")]
        public string Acronym { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Restricción")]
        [Range(0, int.MaxValue, ErrorMessage = "El valor debe ser un número mayor o igual a cero.")]
        public string Restriction { get; set; }
    }
}