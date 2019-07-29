using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class CinemaScheduleViewModel
    {
        public int CinemaScheduleId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Película")]
        public int MovieId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Formato")]
        public int MovieFormatId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de inicio")]
        public System.DateTime BeginDatetime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha final")]
        public System.DateTime FinishDatetime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Hora de inicio")]
        public System.TimeSpan BeginTime { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Rango (en horas) para calendarizar múltiples tandas diarias")]
        public Utils.Enums.HoursRange HoursRange { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Sala")]
        public int CinemaId { get; set; }
    }
}