//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicoCinema.WebApplication.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Movie
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Movie()
        {
            this.CinemaSchedule = new HashSet<CinemaSchedule>();
        }
    
        public int MovieId { get; set; }

        [Display(Name = "Audiencia permitida")]
        public int AudienceClassificationId { get; set; }

        [Display(Name = "Nombre de pel�cula")]
        public string Name { get; set; }

        [Display(Name = "Fecha de estreno")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.DateTime ReleaseDate { get; set; }

        [Display(Name = "Duraci�n")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MMM/yyyy}")]
        public System.TimeSpan DurationTime { get; set; }

        public int CategoriesAssigned { get; set; }

        public string ImagePath { get; set; }
    
        public virtual AudienceClassification AudienceClassification { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CinemaSchedule> CinemaSchedule { get; set; }
    }
}
