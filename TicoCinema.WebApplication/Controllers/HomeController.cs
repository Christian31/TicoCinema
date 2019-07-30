using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        private const string cinemasAvailableKey = "CINEMASAVAILABLE-01";
        private Entities db = new Entities();

        public ActionResult Index()
        {
            var cinemas = db.sp_GetAvailableMovies().
                Where(item => item.BeginDatetime.IsInSameWeek(DateTime.Now)).ToList();
            List<AvailableMovieViewModel> cinemasAvailable = ConvertMoviesToViewModels(cinemas);

            if (HttpContext.KeyExistsOnCache(cinemasAvailableKey))
                HttpContext.RemoveValuesFromCache(cinemasAvailableKey);

            HttpContext.AddValuesToCache(cinemasAvailableKey, cinemasAvailable);

            if(TempData["MessageValidation"] != null)
            {
                ViewBag.MessageValidation = (string)TempData["MessageValidation"];
            }

            CinemaAvailableViewModel moviesAvailable = new CinemaAvailableViewModel()
            {
                AvailableMovies = cinemasAvailable,
                Recomendations = GetRecomendations()
            };
            return View(moviesAvailable);
        }

        private List<MovieRecomendationViewModel> GetRecomendations()
        {
            List<MovieRecomendationViewModel> recomendations = new List<MovieRecomendationViewModel>();
            List<AvailableMovieViewModel> cinemasAvailable = (List<AvailableMovieViewModel>)HttpContext.GetValuesFromCache(cinemasAvailableKey);
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.User.Find(new Guid(userId));

                if (user != null && user.CategoryPreferences != 0)
                {
                    foreach (var item in cinemasAvailable)
                    {
                        if (BitManager.ContainsBit(user.CategoryPreferences, item.Category))
                        {
                            recomendations.Add(new MovieRecomendationViewModel() { MovieImagePath = item.MovieImagePath, MovieName = item.MovieName });
                        }
                        if (recomendations.Count == 3)
                            break;
                    }
                }
            }

            if (recomendations.Count == 0)
            {
                foreach (var item in cinemasAvailable)
                {
                    recomendations.Add(new MovieRecomendationViewModel() { MovieImagePath = item.MovieImagePath, MovieName = item.MovieName });
                    if (recomendations.Count == 3)
                        break;
                }
            }

            return recomendations;
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
                        MovieName = movie.Name,
                        Category = movie.CategoriesAssigned
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