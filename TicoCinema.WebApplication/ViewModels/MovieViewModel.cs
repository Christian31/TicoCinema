﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace TicoCinema.WebApplication.ViewModels
{
    public class MovieViewModel
    {
        public int MovieId { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Audiencia permitida")]
        public int AudienceClassificationId { get; set; }

        [Display(Name = "Audiencia permitida")]
        public string AudienceClassificationName { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Fecha de estreno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime ReleaseDate { get; set; }

        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [Display(Name = "Duración en minutos")]
        [Range(1, int.MaxValue, ErrorMessage = "El valor debe ser un número positivo")]
        public string DurationTime { get; set; }

        public int CategoriesAssigned { get; set; }

        [Display(Name = "Imagen")]
        public string ImagePath { get; set; }
        
        [Display(Name = "Imagen")]
        public HttpPostedFileBase UploadedFile { get; set; }

        [Display(Name = "Categorias")]
        public IList<SelectListItem> Categories { get; set; }

        public IList<string> SelectedCategories { get; set; }
    }
}