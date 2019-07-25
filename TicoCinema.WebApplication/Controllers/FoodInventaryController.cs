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
    public class FoodInventaryController : Controller
    {
        private Entities db = new Entities();

        // GET: FoodInventary
        public ActionResult Index()
        {


            var foodViewModel = from food in db.Food
                                join foodHist in 
                                (from item in db.FoodHistory 
                                 join itemHist in db.FoodHistory
                                 on item.FoodId equals itemHist.FoodId and item.FoodHistoryId equals itemHist.FoodHistoryId)



                                on food.FoodId equals foodHist.FoodId
                                select new FoodInventaryViewModel
                                {
                                    FoodHistoryId = foodHist.FoodHistoryId,
                                    FoodId = food.FoodId,
                                    FoodName = food.FoodName,
                                    Price = foodHist.Price.ToString(),
                                    QuantityChanged = foodHist.QuantityAvailable.ToString()
                                };

            var foodHistory = db.FoodHistory.Include(f => f.Food);
            return View(foodHistory.ToList());
        }

        // GET: FoodInventary/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodHistory foodHistory = db.FoodHistory.Find(id);
            if (foodHistory == null)
            {
                return HttpNotFound();
            }
            return View(foodHistory);
        }

        // GET: FoodInventary/Create
        public ActionResult Create()
        {
            ViewBag.FoodId = new SelectList(db.Food, "FoodId", "FoodName");
            return View();
        }

        // POST: FoodInventary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodHistoryId,FoodId,Price,QuantityAvailable,QuantityChanged,Status,UserName,Description,ModificationDate")] FoodHistory foodHistory)
        {
            if (ModelState.IsValid)
            {
                db.FoodHistory.Add(foodHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.FoodId = new SelectList(db.Food, "FoodId", "FoodName", foodHistory.FoodId);
            return View(foodHistory);
        }

        // GET: FoodInventary/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodHistory foodHistory = db.FoodHistory.Find(id);
            if (foodHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodId = new SelectList(db.Food, "FoodId", "FoodName", foodHistory.FoodId);
            return View(foodHistory);
        }

        // POST: FoodInventary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodHistoryId,FoodId,Price,QuantityAvailable,QuantityChanged,Status,UserName,Description,ModificationDate")] FoodHistory foodHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(foodHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.FoodId = new SelectList(db.Food, "FoodId", "FoodName", foodHistory.FoodId);
            return View(foodHistory);
        }

        // GET: FoodInventary/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodHistory foodHistory = db.FoodHistory.Find(id);
            if (foodHistory == null)
            {
                return HttpNotFound();
            }
            return View(foodHistory);
        }

        // POST: FoodInventary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            FoodHistory foodHistory = db.FoodHistory.Find(id);
            db.FoodHistory.Remove(foodHistory);
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

        private List<FoodInventaryViewModel> ConvertViewModelsToMovies(List<FoodHistory> foodHistories)
        {
            List<FoodInventaryViewModel> foodInventaryViewModels = new List<FoodInventaryViewModel>();
            foreach (var foodHistory in foodHistories)
            {
                foodInventaryViewModels.Add(ConvertFoodHistoryToViewModel(foodHistory));
            }

            return foodInventaryViewModels;
        }

        private FoodHistory ConvertViewModelToFoodHistory(FoodInventaryViewModel foodInventaryViewModel)
        {
            FoodHistory foodHistory = db.FoodHistory.Find(foodInventaryViewModel.FoodHistoryId);
            if (foodHistory == null)
            {
                foodHistory = new FoodHistory
                {
                    //AudienceClassificationId = foodInventaryViewModel.AudienceClassificationId,
                    //DurationTime = new TimeSpan(0, int.Parse(foodInventaryViewModel.DurationTime), 0),
                    //MovieId = foodInventaryViewModel.MovieId,
                    //Name = foodInventaryViewModel.Name,
                    //ImagePath = foodInventaryViewModel.ImagePath,
                    //ReleaseDate = foodInventaryViewModel.ReleaseDate,
                    //CategoriesAssigned = foodInventaryViewModel.CategoriesAssigned
                };
            }
            else
            {
                //foodHistory.AudienceClassificationId = foodInventaryViewModel.AudienceClassificationId;
                //foodHistory.DurationTime = new TimeSpan(0, int.Parse(foodInventaryViewModel.DurationTime), 0);
                //foodHistory.MovieId = foodInventaryViewModel.MovieId;
                //foodHistory.Name = foodInventaryViewModel.Name;
                //foodHistory.ImagePath = foodInventaryViewModel.ImagePath;
                //foodHistory.ReleaseDate = foodInventaryViewModel.ReleaseDate;
                //foodHistory.CategoriesAssigned = foodInventaryViewModel.CategoriesAssigned;
            }

            return foodHistory;
        }

        private FoodInventaryViewModel ConvertFoodHistoryToViewModel(FoodHistory movie)
        {
            return new FoodInventaryViewModel
            {
                //AudienceClassificationId = movie.AudienceClassificationId,
                //AudienceClassificationName = movie.AudienceClassification.Name,
                //DurationTime = movie.DurationTime.TotalMinutes.ToString(),
                //MovieId = movie.MovieId,
                //Name = movie.Name,
                //ImagePath = movie.ImagePath,
                //ReleaseDate = movie.ReleaseDate,
                //CategoriesAssigned = 0
            };
        }
    }
}
