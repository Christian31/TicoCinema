using System;
using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class RegisterUserViewModel
    {
        public Guid UserId { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Fecha de nacimiento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime Birthdate { get; set; }

        [Display(Name = "Género")]
        public Utils.Enums.Gender Gender { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        public int Province { get; set; }

        [Required]
        [Display(Name = "Cantón")]
        public int Canton { get; set; }

        [Required]
        [Display(Name = "Distrito")]
        public int District { get; set; }

        [Required]
        [Display(Name = "Detalle de dirección")]
        public string Details { get; set; }

        [Display(Name = "Provincia")]
        public string ProvinceName { get; set; }

        [Display(Name = "Cantón")]
        public string CantonName { get; set; }

        [Display(Name = "Distrito")]
        public string DistrictName { get; set; }
    }
}