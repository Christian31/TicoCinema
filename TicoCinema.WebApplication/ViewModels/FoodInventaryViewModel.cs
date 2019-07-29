using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class FoodInventaryViewModel
    {
        public long FoodHistoryId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Comida")]
        public long FoodId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Precio")]
        public string Price { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Cantidad disponible")]
        public int QuantityChanged { get; set; }

        [Display(Name = "Comida")]
        public string FoodName { get; set; }
    }
}