using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TicoCinema.WebApplication.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }

        [Required]
        [Display(Name = "Audiencia permitida")]
        public int AudienceClassificationId { get; set; }

        [Display(Name = "Audiencia permitida")]
        public string AudienceClassificationName { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Fecha de estreno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime ReleaseDate { get; set; }

        [Required]
        [Display(Name = "Duración en minutos")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo")]
        public string DurationTime { get; set; }

        public int CategoriesAssigned { get; set; }

        [Display(Name = "Imagen")]
        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "Imagen")]
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}