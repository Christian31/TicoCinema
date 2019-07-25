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
    public class CinemaSchedulesController : Controller
    {
        private Entities db = new Entities();

        // GET: CinemaSchedules
        public ActionResult Index()
        {
            var cinemaSchedule = db.CinemaSchedule.Include(c => c.Cinema).Include(c => c.Movie).Include(c => c.MovieFormat);
            return View(cinemaSchedule.ToList());
        }

        // GET: CinemaSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaSchedule cinemaSchedule = db.CinemaSchedule.Find(id);
            if (cinemaSchedule == null)
            {
                return HttpNotFound();
            }
            return View(cinemaSchedule);
        }

        // GET: CinemaSchedules/Create
        public ActionResult Create()
        {
            ViewBag.CinemaId = new SelectList(db.Cinema, "CinemaId", "Name");
            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name");
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name");
            return View();
        }

        // POST: CinemaSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CinemaScheduleId,CinemaId,MovieFormatId,MovieId,BeginDatetime,FinishDatetime")] CinemaSchedule cinemaSchedule)
        {
            if (ModelState.IsValid)
            {
                db.CinemaSchedule.Add(cinemaSchedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CinemaId = new SelectList(db.Cinema, "CinemaId", "Name", cinemaSchedule.CinemaId);
            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name", cinemaSchedule.MovieId);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinemaSchedule.MovieFormatId);
            return View(cinemaSchedule);
        }

        // GET: CinemaSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaSchedule cinemaSchedule = db.CinemaSchedule.Find(id);
            if (cinemaSchedule == null)
            {
                return HttpNotFound();
            }
            ViewBag.CinemaId = new SelectList(db.Cinema, "CinemaId", "Name", cinemaSchedule.CinemaId);
            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name", cinemaSchedule.MovieId);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinemaSchedule.MovieFormatId);
            return View(cinemaSchedule);
        }

        // POST: CinemaSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CinemaScheduleId,CinemaId,MovieFormatId,MovieId,BeginDatetime,FinishDatetime")] CinemaSchedule cinemaSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cinemaSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CinemaId = new SelectList(db.Cinema, "CinemaId", "Name", cinemaSchedule.CinemaId);
            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name", cinemaSchedule.MovieId);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinemaSchedule.MovieFormatId);
            return View(cinemaSchedule);
        }

        // GET: CinemaSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CinemaSchedule cinemaSchedule = db.CinemaSchedule.Find(id);
            if (cinemaSchedule == null)
            {
                return HttpNotFound();
            }
            return View(cinemaSchedule);
        }

        // POST: CinemaSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CinemaSchedule cinemaSchedule = db.CinemaSchedule.Find(id);
            db.CinemaSchedule.Remove(cinemaSchedule);
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
