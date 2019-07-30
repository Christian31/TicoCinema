using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TicoCinema.WebApplication.ViewModels
{
    public class CinemaServiceStep1
    {
        public long CinemaScheduleId { get; set; }
        public int MovieFormatId { get; set; }
        public List<QuantityTicketsViewModel> FormatQuantities { get; set; }
    }

    public class QuantityTicketsViewModel
    {
        [Display(Name = "Tipo de entrada")]
        public string MovieFormatName { get; set; }

        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }

        [Display(Name = "Precio")]
        public decimal Price { get; set; }

        [Display(Name = "Subtotal")]
        public decimal Subtotal { get; set; }
    }

    public class CinemaServiceStep2
    {
        public int SeatId { get; set; }
        public bool Available { get; set; }
        public bool Selected { get; set; }
    }
}