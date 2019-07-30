using System.Collections.Generic;
using TicoCinema.WebApplication.Models;

namespace TicoCinema.WebApplication.ViewModels
{
    public class CinemaAvailableViewModel
    {
        public List<AvailableMovieViewModel> AvailableMovies { get; set; }
        public List<MovieRecomendationViewModel> Recomendations { get; set; }
    }

    public class AvailableMovieViewModel
    {
        public long MovieId { get; set; }

        public string MovieName { get; set; }
        public string MovieImagePath { get; set; }
        public string DurationTime { get; set; }
        public int AudienceClassificationId { get; set; }
        public string AudienceClassificationName { get; set; }
        public int Category { get; set; }

        public List<AvailableSchedule> Schedules { get; set; }
    }

    public class MovieRecomendationViewModel
    {
        public string MovieName { get; set; }
        public string MovieImagePath { get; set; }
    }

    public class AvailableSchedule
    {
        public long CinemaScheduleId { get; set; }
        public string BeginTime { get; set; }
        public MovieFormat MovieFormat { get; set; }
    }
}