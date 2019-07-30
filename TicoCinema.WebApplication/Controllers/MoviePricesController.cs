using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    [Authorize]
    public class MoviePricesController : Controller
    {
        private Entities db = new Entities();

        // GET: MoviePrices
        public ActionResult Index()
        {
            var moviePrice = db.MoviePrice.Include(m => m.MovieFormat);
            return View(moviePrice.ToList());
        }

        // GET: MoviePrices/Create
        public ActionResult Create()
        {
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name");
            return View();
        }

        // POST: MoviePrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieFormatId,Price,Name")] RegisterMoviePriceViewModel moviePrice)
        {
            if (ModelState.IsValid)
            {
                MoviePrice moviePricedb = ConvertViewModelToMoviePrice(moviePrice);
                db.MoviePrice.Add(moviePricedb);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", moviePrice.MovieFormatId);
            return View(moviePrice);
        }

        // GET: MoviePrices/Edit/5
        public ActionResult Edit(int id, string name)
        {
            MoviePrice moviePrice = db.MoviePrice.
                FirstOrDefault(item => item.MovieFormatId == id && item.Name == name);
            if (moviePrice == null)
            {
                return HttpNotFound();
            }

            RegisterMoviePriceViewModel moviePriceViewModel = ConvertMoviePriceToViewModel(moviePrice);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", moviePrice.MovieFormatId);
            return View(moviePriceViewModel);
        }

        // POST: MoviePrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieFormatId,Price,Name")] RegisterMoviePriceViewModel moviePrice)
        {
            if (ModelState.IsValid)
            {
                MoviePrice moviePricedb = ConvertViewModelToMoviePrice(moviePrice);
                db.Entry(moviePricedb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", moviePrice.MovieFormatId);
            return View(moviePrice);
        }

        // GET: MoviePrices/Delete/5
        public ActionResult Delete(int id, string name)
        {
            MoviePrice moviePrice = db.MoviePrice.
                FirstOrDefault(item => item.MovieFormatId == id && item.Name == name);
            if (moviePrice == null)
            {
                return HttpNotFound();
            }
            return View(moviePrice);
        }

        // POST: MoviePrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string name)
        {
            MoviePrice moviePrice = db.MoviePrice.
                FirstOrDefault(item => item.MovieFormatId == id && item.Name == name);
            db.MoviePrice.Remove(moviePrice);
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

        private MoviePrice ConvertViewModelToMoviePrice(RegisterMoviePriceViewModel moviePriceViewModel)
        {
            MoviePrice moviePrice = db.MoviePrice.
                FirstOrDefault(item => item.MovieFormatId == moviePriceViewModel.MovieFormatId && 
                item.Name == moviePriceViewModel.Name);
            decimal.TryParse(moviePriceViewModel.Price, out decimal price);
            if (moviePrice == null)
            {
                moviePrice = new MoviePrice
                {
                    MovieFormatId = moviePriceViewModel.MovieFormatId,
                    Name = moviePriceViewModel.Name,
                    Price = price
                };
            }
            else
            {
                moviePrice.Price = price;
            }

            return moviePrice;
        }

        private RegisterMoviePriceViewModel ConvertMoviePriceToViewModel(MoviePrice moviePrice)
        {
            return new RegisterMoviePriceViewModel
            {
                MovieFormatId = moviePrice.MovieFormatId,
                Price = moviePrice.Price.ToString(),
                Name = moviePrice.Name
            };
        }
    }
}
