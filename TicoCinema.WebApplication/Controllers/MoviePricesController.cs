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
    public class MoviePricesController : Controller
    {
        private Entities db = new Entities();

        // GET: MoviePrices
        public ActionResult Index()
        {
            var moviePrice = db.MoviePrice.Include(m => m.MovieFormat);
            return View(moviePrice.ToList());
        }

        // GET: MoviePrices/Details/5
        public ActionResult Details(int id, string name)
        {
            MoviePrice moviePrice = db.MoviePrice.
                FirstOrDefault(item => item.MovieFormatId == id && item.Name == name);

            if (moviePrice == null)
            {
                return HttpNotFound();
            }
            return View(moviePrice);
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
        public ActionResult Create([Bind(Include = "MovieFormatId,Price,Name")] MoviePrice moviePrice)
        {
            if (ModelState.IsValid)
            {
                db.MoviePrice.Add(moviePrice);
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
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", moviePrice.MovieFormatId);
            return View(moviePrice);
        }

        // POST: MoviePrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieFormatId,Price,Name")] MoviePrice moviePrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(moviePrice).State = EntityState.Modified;
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
    }
}
