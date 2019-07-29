using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();

        public ActionResult Index()
        {
            var cinemas = db.sp_GetAvailableMovies().
                Where(item => item.BeginDatetime.IsInSameDay(DateTime.Now)).ToList();
            var cinemasAvailable = ConvertMoviesToViewModels(cinemas);
            return View(cinemasAvailable);
        }

        private List<AvailableMovieViewModel> ConvertMoviesToViewModels(List<sp_GetAvailableMovies_Result> cinemas)
        {
            List<MovieFormat> movieFormats = db.MovieFormat.ToList();
            List<AvailableMovieViewModel> moviesViewModels = new List<AvailableMovieViewModel>();
            var cinemasGroupByMovie = cinemas.GroupBy(item => item.MovieId).
                ToDictionary(item => item.Key, item => item.ToList());

            foreach (var cinemaGroupByMovie in cinemasGroupByMovie)
            {
                foreach (var cinema in cinemasGroupByMovie)
                {
                    var movie = db.Movie.Find(cinema.Key);
                    var cinemaAvailable = new AvailableMovieViewModel
                    {
                        MovieId = movie.MovieId,
                        AudienceClassificationName = movie.AudienceClassification.Acronym,
                        AudienceClassificationId = movie.AudienceClassificationId,
                        DurationTime = movie.DurationTime.TotalMinutes.ToString(),
                        MovieImagePath = FileManager.GetMovieImagePath(movie.ImagePath),
                        MovieName = movie.Name
                    };
                    cinemaAvailable.Schedules = GetSchedules(cinema.Value, movieFormats);
                    moviesViewModels.Add(cinemaAvailable);
                }
            }

            return moviesViewModels;
        }

        private List<AvailableSchedule> GetSchedules(List<sp_GetAvailableMovies_Result> moviesCinema, List<MovieFormat> movieFormats)
        {
            List<AvailableSchedule> schedules = new List<AvailableSchedule>();
            foreach (var item in moviesCinema)
            {
                schedules.Add(new AvailableSchedule()
                {
                    BeginTime = item.BeginDatetime.ToShortTimeString(),
                    CinemaScheduleId = item.CinemaScheduleId,
                    MovieFormat = movieFormats.FirstOrDefault(i => i.MovieFormatId == item.MovieFormatId)
                });
            }
            return schedules;
        }

        public ActionResult About()
        {
            ViewBag.Message = "La página de descripción del sitio.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "La página de contacto.";

            return View();
        }
    }
}