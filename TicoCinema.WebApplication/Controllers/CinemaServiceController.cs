using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;

namespace TicoCinema.WebApplication.Controllers
{
    public class CinemaServiceController : Controller
    {
        private Entities db = new Entities();

        // GET: CinemaService/Create
        public ActionResult Create(int id)
        {
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name");
            return View();
        }

        // POST: CinemaService/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movie.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movie);
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
