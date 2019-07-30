using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    public class CinemaServiceController : Controller
    {
        private const string cinemasAvailableKey = "CINEMASAVAILABLE-01";
        private const int restrictionToCheck = 18;

        private Entities db = new Entities();

        // GET: CinemaService/Create
        public ActionResult Create(long scheduleId)
        {
            if (User.Identity.IsAuthenticated)
            {
                if (HttpContext.KeyExistsOnCache(cinemasAvailableKey))
                {
                    var userId = User.Identity.GetUserId();
                    var user = db.User.Find(new Guid(userId));
                    if (user != null)
                    {
                        var cinemasAvailable = (List<AvailableMovieViewModel>)HttpContext.GetValuesFromCache(cinemasAvailableKey);
                        var movieAvailable = cinemasAvailable.FirstOrDefault(item => item.Schedules.Select(sch => sch.CinemaScheduleId).ToList().Contains(scheduleId));
                        var audienceClassification = db.AudienceClassification.Find(movieAvailable.AudienceClassificationId);
                        if (audienceClassification.Restriction == restrictionToCheck)
                        {
                            if (user.Birthdate.GetYearsBetweenDateAndNow() < restrictionToCheck)
                            {
                                TempData["MessageValidation"] = string.Format("Usted no puede comprar un tiquete para la película seleccionada, {0} es apto para {1}",
                                movieAvailable.MovieName, audienceClassification.Name);
                                return RedirectToAction(actionName: "Index", controllerName: "Home");
                            }
                        }

                        var scheduleSelected = movieAvailable.Schedules.FirstOrDefault(item => item.CinemaScheduleId == scheduleId);
                        
                        CinemaServiceStep1 cinemaServiceStep1 = new CinemaServiceStep1()
                        {
                            CinemaScheduleId = scheduleId,
                            MovieFormatId = scheduleSelected.MovieFormat.MovieFormatId,
                            FormatQuantities = GetQuantityTickets(scheduleSelected.MovieFormat.MovieFormatId)
                        };

                        return View(cinemaServiceStep1);
                    }
                    else
                    {
                        return RedirectToAction(actionName: "Index", controllerName: "Home");
                    }
                }
                else
                {
                    return RedirectToAction(actionName: "Index", controllerName: "Home");
                }                
            }
            else
            {
                return RedirectToAction(actionName: "Login", controllerName: "Account");
            }
        }

        private List<QuantityTicketsViewModel> GetQuantityTickets(int movieFormatId)
        {
            List<QuantityTicketsViewModel> quantityTickets = new List<QuantityTicketsViewModel>();
            var moviePrices = db.MoviePrice.Where(item => item.MovieFormatId == movieFormatId).ToList();
            foreach (var moviePrice in moviePrices)
            {
                quantityTickets.Add(new QuantityTicketsViewModel() {
                    MovieFormatName = moviePrice.Name,
                    Price = moviePrice.Price
                });
            }

            return quantityTickets;
        }

        // POST: CinemaService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CinemaServiceStep1 cinemaServiceStep1)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.KeyExistsOnCache("cinemaTemp"))
                    HttpContext.RemoveValuesFromCache("cinemaTemp");

                HttpContext.AddValuesToCache("cinemaTemp", cinemaServiceStep1);

                var cinemasAvailable = (List<AvailableMovieViewModel>)HttpContext.GetValuesFromCache(cinemasAvailableKey);
                var movieAvailable = cinemasAvailable.
                    FirstOrDefault(item => item.Schedules.Select(sch => sch.CinemaScheduleId).ToList()
                    .Contains(cinemaServiceStep1.CinemaScheduleId));

                var scheduleSelected = movieAvailable.Schedules.
                    FirstOrDefault(item => item.CinemaScheduleId == cinemaServiceStep1.CinemaScheduleId);

                return View("CreateStep2", null);
            }

            return View(cinemaServiceStep1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
