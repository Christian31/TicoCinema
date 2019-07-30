using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    [Authorize]
    public class CinemasController : Controller
    {
        private Entities db = new Entities();

        // GET: Cinemas
        public ActionResult Index()
        {
            var cinemas = db.Cinema.Include(c => c.MovieFormat);

            List<CinemaViewModel> cinemasViewModel = new List<CinemaViewModel>();
            foreach (var cinema in cinemas)
            {
                cinemasViewModel.Add(ConvertCinemaToViewModel(cinema));
            }

            return View(cinemasViewModel);
        }

        private CinemaViewModel ConvertCinemaToViewModel(Cinema cinema)
        {
            return new CinemaViewModel()
            {
                Capacity = cinema.Capacity.ToString(),
                CinemaDesignId = (Utils.Enums.CinemaDesign)cinema.CinemaDesignId,
                CinemaId = cinema.CinemaId,
                MovieFormatId = cinema.MovieFormatId,
                Name = cinema.Name,
                MovieFormatName = cinema.MovieFormat.Name,
                CinemaDesignName = RazorHelper.GetEnumDescription((Utils.Enums.CinemaDesign)cinema.CinemaDesignId)
            };
        }

        private Cinema ConvertViewModelToCinema(CinemaViewModel cinemaViewModel)
        {
            Cinema cinema = db.Cinema.Find(cinemaViewModel.CinemaId);
            int.TryParse(cinemaViewModel.Capacity, out int cinemaCapacity);
            if (cinema == null)
            {
                cinema = new Cinema
                {
                    Capacity = cinemaCapacity,
                    CinemaDesignId = (int)cinemaViewModel.CinemaDesignId,
                    MovieFormatId = cinemaViewModel.MovieFormatId,
                    Name = cinemaViewModel.Name
                };
            }
            else
            {
                cinema.Capacity = cinemaCapacity;
                cinema.CinemaDesignId = (int)cinemaViewModel.CinemaDesignId;
                cinema.MovieFormatId = cinemaViewModel.MovieFormatId;
                cinema.Name = cinemaViewModel.Name;
            }

            return cinema;
        }

        // GET: Cinemas/Create
        public ActionResult Create()
        {
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name");
            return View();
        }

        // POST: Cinemas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CinemaViewModel cinema)
        {
            if (ModelState.IsValid)
            {
                Cinema dbCinema = ConvertViewModelToCinema(cinema);
                db.Cinema.Add(dbCinema);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieFormats = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinema.MovieFormatId);
            return View(cinema);
        }

        // GET: Cinemas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinema.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            CinemaViewModel cinemaViewModel = ConvertCinemaToViewModel(cinema);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", 
                cinemaViewModel.MovieFormatId);
            return View(cinemaViewModel);
        }

        // POST: Cinemas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CinemaViewModel cinema)
        {
            if (ModelState.IsValid)
            {
                Cinema dbCinema = ConvertViewModelToCinema(cinema);
                db.Entry(dbCinema).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinema.MovieFormatId);
            return View(cinema);
        }

        // GET: Cinemas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cinema cinema = db.Cinema.Find(id);
            if (cinema == null)
            {
                return HttpNotFound();
            }

            CinemaViewModel cinemaViewModel = ConvertCinemaToViewModel(cinema);
            return View(cinemaViewModel);
        }

        // POST: Cinemas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cinema cinema = db.Cinema.Find(id);
            db.Cinema.Remove(cinema);
            db.SaveChanges();
            return RedirectToAction("Index");
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
