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
    public class MovieFormatsController : Controller
    {
        private Entities db = new Entities();

        // GET: MovieFormats
        public ActionResult Index()
        {
            return View(db.MovieFormat.ToList());
        }

        // GET: MovieFormats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieFormat movieFormat = db.MovieFormat.Find(id);
            if (movieFormat == null)
            {
                return HttpNotFound();
            }
            return View(movieFormat);
        }

        // GET: MovieFormats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovieFormats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieFormatId,Name")] MovieFormat movieFormat)
        {
            if (ModelState.IsValid)
            {
                db.MovieFormat.Add(movieFormat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movieFormat);
        }

        // GET: MovieFormats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieFormat movieFormat = db.MovieFormat.Find(id);
            if (movieFormat == null)
            {
                return HttpNotFound();
            }
            return View(movieFormat);
        }

        // POST: MovieFormats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieFormatId,Name")] MovieFormat movieFormat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movieFormat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieFormat);
        }

        // GET: MovieFormats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieFormat movieFormat = db.MovieFormat.Find(id);
            if (movieFormat == null)
            {
                return HttpNotFound();
            }
            return View(movieFormat);
        }

        // POST: MovieFormats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieFormat movieFormat = db.MovieFormat.Find(id);
            db.MovieFormat.Remove(movieFormat);
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
