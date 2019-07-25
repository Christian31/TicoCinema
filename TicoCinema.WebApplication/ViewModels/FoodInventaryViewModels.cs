using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class FoodInventaryViewModel
    {
        public long FoodHistoryId { get; set; }

        [Required]
        [Display(Name = "Comida")]
        public long FoodId { get; set; }

        [Required]
        [Display(Name = "Precio")]
        public string Price { get; set; }

        [Required]
        [Display(Name = "Cantidad de ingreso")]
        public string QuantityChanged { get; set; }

        [Display(Name = "Comida")]
        public string FoodName { get; set; }
    }
}