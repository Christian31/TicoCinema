using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    public class FoodsController : Controller
    {
        private Entities db = new Entities();

        // GET: Foods
        public ActionResult Index()
        {
            return View(db.Food.ToList());
        }

        // GET: Foods/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }

            food.ImagePath = FileManager.GetFoodImagePath(food.ImagePath);
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterFoodViewModel food)
        {
            if (ModelState.IsValid)
            {
                food.ImagePath = FileManager.SaveFoodImage(food.FoodName, food.UploadedFile);
                Food dbFood = ConvertViewModelToFood(food);
                db.Food.Add(dbFood);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Foods/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RegisterFoodViewModel food)
        {
            if (ModelState.IsValid)
            {
                food.ImagePath = FileManager.ReplaceFoodImage(food.FoodName, food.UploadedFile, food.ImagePath);
                Food dbFood = ConvertViewModelToFood(food);

                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Food.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }

            food.ImagePath = FileManager.GetFoodImagePath(food.ImagePath);
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Food food = db.Food.Find(id);
            db.Food.Remove(food);
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

        private Food ConvertViewModelToFood(RegisterFoodViewModel foodViewModel)
        {
            Food food = db.Food.Find(foodViewModel.FoodId);
            if(food == null)
            {
                food = new Food
                {
                    Description = foodViewModel.Description,
                    FoodId = foodViewModel.FoodId,
                    FoodName = foodViewModel.FoodName,
                    ImagePath = foodViewModel.ImagePath
                };
            }
            else
            {
                food.Description = foodViewModel.Description;
                food.FoodId = foodViewModel.FoodId;
                food.FoodName = foodViewModel.FoodName;
                food.ImagePath = foodViewModel.ImagePath;
            }

            return food;
        }
    }
}
