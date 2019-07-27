using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TicoCinema.WebApplication.ViewModels
{
    public class RegisterFoodViewModel
    {
        public long FoodId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre de comida")]
        public string FoodName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public string ImagePath { get; set; }
        
        [Display(Name = "Imagen")]
        public HttpPostedFileBase UploadedFile { get; set; }
    }
}