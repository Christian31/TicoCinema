using System.ComponentModel.DataAnnotations;

namespace TicoCinema.WebApplication.ViewModels
{
    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El campo {0} debe de tener almenos {2} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "La contraseña y la confirmación de la contraseña no coinciden.")]
        public string ConfirmPassword { get; set; }

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
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }
    }
}
