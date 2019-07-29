using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

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
        public ActionResult Details(int id)
        {
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
            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name");
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name");
            return View();
        }

        // POST: CinemaSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CinemaScheduleViewModel cinemaSchedule)
        {
            if (ModelState.IsValid)
            {
                CinemaSchedulerManager.SaveCinemaSchedules(cinemaSchedule, User.Identity.GetUserName());
                return RedirectToAction("Index");
            }

            ViewBag.MovieId = new SelectList(db.Movie, "MovieId", "Name", cinemaSchedule.MovieId);
            ViewBag.MovieFormatId = new SelectList(db.MovieFormat, "MovieFormatId", "Name", cinemaSchedule.MovieFormatId);
            return View(cinemaSchedule);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetCinemasAvailable(int movieId, int movieFormatId, string beginDate, string finishDate, string beginHour, string hoursRange)
        {
            var cinemaSchedulesToValidate = CinemaSchedulerManager.GenerateCinemaSchedules(movieId, beginDate, finishDate, beginHour, hoursRange);
            var cinemasByFormat = db.Cinema.Where(item => item.MovieFormatId == movieFormatId).Select(item => item.CinemaId).ToList();
            var cinemaSchedulesByCinema = db.CinemaSchedule.Where(item => cinemasByFormat.Contains(item.CinemaId));
            Dictionary<int, bool> cinemasUsed = new Dictionary<int, bool>();
            foreach (var cinemaScheduleToValidate in cinemaSchedulesToValidate)
            {
                foreach (var cinemaRegistered in cinemaSchedulesByCinema)
                {
                    if (!cinemasUsed.ContainsKey(cinemaRegistered.CinemaId))
                    {
                        if (cinemaRegistered.BeginDatetime.IsInRange(cinemaRegistered.BeginDatetime, cinemaRegistered.FinishDatetime) ||
                            cinemaRegistered.FinishDatetime.IsInRange(cinemaRegistered.BeginDatetime, cinemaRegistered.FinishDatetime))
                        {
                            cinemasUsed.Add(cinemaRegistered.CinemaId, true);
                        }
                    }                    
                }
            }
            var cinemasAvailable = db.Cinema.Where(item => item.MovieFormatId == movieFormatId && !cinemasUsed.Keys.Contains(item.CinemaId)).ToList();
            var cinemas = new SelectList(cinemasAvailable, "CinemaId", "Name");
            return Json(cinemas, JsonRequestBehavior.AllowGet);
        }

        // GET: CinemaSchedules/Delete/5
        public ActionResult Delete(int id)
        {
            CinemaSchedule cinemaSchedule = db.CinemaSchedule.Find(id);
            if (cinemaSchedule == null)
            {
                return HttpNotFound();
            }
            if(cinemaSchedule.CinemaScheduleHistory.Where(t => t.TicketId !=0).Count() > 0)
            {
                return RedirectToAction("Index");
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
