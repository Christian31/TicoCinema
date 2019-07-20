using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;

namespace TicoCinema.WebApplication.Controllers
{
    public class MovieCategoriesController : Controller
    {
        private Entities db = new Entities();

        // GET: MovieCategories
        public ActionResult Index()
        {
            List<MovieCategory> categoriesActive = db.MovieCategory.Where(item => item.Status == 1 ).ToList();
            return View(categoriesActive);
        }

        // GET: MovieCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MovieCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryId,CategoryName")] MovieCategory movieCategory)
        {
            if (ModelState.IsValid)
            {
                #region Default Values

                long lastBitAssigned = (from item in db.MovieCategory select item.BitAssigned).Max();
                long bitToAssign = BitManager.GetNextBitToAssign(lastBitAssigned);
                movieCategory.BitAssigned = bitToAssign;
                movieCategory.Status = 1;

                #endregion

                db.MovieCategory.Add(movieCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movieCategory);
        }

        // GET: MovieCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieCategory movieCategory = db.MovieCategory.Find(id);
            if (movieCategory == null)
            {
                return HttpNotFound();
            }
            return View(movieCategory);
        }

        // POST: MovieCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryId,CategoryName,BitAssigned,Status")] MovieCategory movieCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movieCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movieCategory);
        }

        // GET: MovieCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MovieCategory movieCategory = db.MovieCategory.Find(id);
            if (movieCategory == null)
            {
                return HttpNotFound();
            }
            return View(movieCategory);
        }

        // POST: MovieCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MovieCategory movieCategory = db.MovieCategory.Find(id);
            movieCategory.Status = 2;
            db.Entry(movieCategory).State = EntityState.Modified;
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
