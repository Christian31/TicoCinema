using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
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
            var foodHistory = db.sp_GetFoodInventary();
            var foodViewModels = ConvertResultsToViewModels(foodHistory);

            return View(foodViewModels);
        }

        // GET: FoodInventary/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var foodHistorial = db.sp_GetFoodInventary();
            var foodViewModels = ConvertResultsToViewModels(foodHistorial);
            var foodHistory = foodViewModels.FirstOrDefault(item => item.FoodId == id);
            if (foodHistory == null)
            {
                return HttpNotFound();
            }
            return View(foodHistory);
        }

        // GET: FoodInventary/Create
        public ActionResult Create()
        {
            IEnumerable<SelectListItem> foodListItems = (from item in db.Food
                                                         select new SelectListItem
                                                         {
                                                             Value = item.FoodId.ToString(),
                                                             Text = item.FoodName.ToString()
                                                         }).ToList();
            ViewBag.FoodId = foodListItems;
            return View();
        }

        // POST: FoodInventary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodInventaryViewModel food)
        {
            if (ModelState.IsValid)
            {
                var foodHistorial = db.sp_GetFoodInventary();
                var foodViewModels = ConvertResultsToViewModels(foodHistorial);
                var foodHistory = foodViewModels.FirstOrDefault(item => item.FoodId == food.FoodId);

                var fooddb = ConvertViewModelToFoodHistory(food, foodHistory.QuantityChanged);
                db.FoodHistory.Add(fooddb);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            IEnumerable<SelectListItem> foodListItems = (from item in db.Food
                                                         select new SelectListItem
                                                         {
                                                             Value = item.FoodId.ToString(),
                                                             Text = item.FoodName.ToString()
                                                         }).ToList();
            ViewBag.FoodId = foodListItems;
            return View(food);
        }

        // GET: FoodInventary/Edit/5
        public ActionResult Edit(long id)
        {
            var foodHistorial = db.sp_GetFoodInventary();
            var foodViewModels = ConvertResultsToViewModels(foodHistorial);
            var foodHistory = foodViewModels.FirstOrDefault(item => item.FoodId == id);
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
        public ActionResult Edit([Bind(Include = "Price, FoodId")]FoodInventaryViewModel food)
        {
            if (ModelState.IsValid)
            {
                var foodHistorial = db.sp_GetFoodInventary();
                var foodViewModels = ConvertResultsToViewModels(foodHistorial);
                var foodHistory = foodViewModels.FirstOrDefault(item => item.FoodId == food.FoodId);

                var fooddb = ConvertViewModelToFoodHistory(food, foodHistory.QuantityChanged);
                db.FoodHistory.Add(fooddb);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.FoodId = new SelectList(db.Food, "FoodId", "FoodName", food.FoodId);
            return View(food);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private List<FoodInventaryViewModel> ConvertResultsToViewModels(ObjectResult<sp_GetFoodInventary_Result> foodHistories)
        {
            List<FoodInventaryViewModel> foodInventaryViewModels = new List<FoodInventaryViewModel>();
            foreach (var foodHistory in foodHistories)
            {
                foodInventaryViewModels.Add(ConvertFoodHistoryToViewModel(foodHistory));
            }

            return foodInventaryViewModels;
        }

        private FoodHistory ConvertViewModelToFoodHistory(FoodInventaryViewModel foodInventaryViewModel, int quantityAvailable)
        {
            decimal.TryParse(foodInventaryViewModel.Price, out decimal price);
            FoodHistory foodHistory = new FoodHistory
            {
                Description = "Se actualiza el inventario",
                FoodId = foodInventaryViewModel.FoodId,
                ModificationDate = DateTime.Now,
                Price = price,
                QuantityChanged = foodInventaryViewModel.QuantityChanged,
                QuantityAvailable = quantityAvailable + foodInventaryViewModel.QuantityChanged,
                Status = 1,
                UserName = User.Identity.GetUserName()
            };

            return foodHistory;
        }

        private FoodInventaryViewModel ConvertFoodHistoryToViewModel(sp_GetFoodInventary_Result food)
        {
            return new FoodInventaryViewModel
            {
                FoodId = food.FoodId,
                FoodName = food.FoodName,
                Price = food.Price.ToString(),
                QuantityChanged = food.QuantityAvailable
            };
        }
    }
}
