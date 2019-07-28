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

        // GET: CinemaService
        public ActionResult Index()
        {
            var movie = db.Movie.Include(m => m.AudienceClassification);
            return View(movie.ToList());
        }

        // GET: CinemaService/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: CinemaService/Create
        public ActionResult Create()
        {
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name");
            return View();
        }

        // POST: CinemaService/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieId,AudienceClassificationId,Name,ReleaseDate,DurationTime,CategoriesAssigned,ImagePath")] Movie movie)
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

        // GET: CinemaService/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movie);
        }

        // POST: CinemaService/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,AudienceClassificationId,Name,ReleaseDate,DurationTime,CategoriesAssigned,ImagePath")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AudienceClassificationId = new SelectList(db.AudienceClassification, "AudienceClassificationId", "Name", movie.AudienceClassificationId);
            return View(movie);
        }

        // GET: CinemaService/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movie.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: CinemaService/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movie.Find(id);
            db.Movie.Remove(movie);
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
