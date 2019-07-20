using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;

namespace TicoCinema.WebApplication.Controllers
{
    public class AudienceClassificationsController : Controller
    {
        private Entities db = new Entities();

        // GET: AudienceClassifications
        public ActionResult Index()
        {
            return View(db.AudienceClassification.ToList());
        }

        // GET: AudienceClassifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceClassification audienceClassification = db.AudienceClassification.Find(id);
            if (audienceClassification == null)
            {
                return HttpNotFound();
            }
            return View(audienceClassification);
        }

        // GET: AudienceClassifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AudienceClassifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AudienceClassificationId,Name,Acronym,Descripcion,Restriction")] AudienceClassification audienceClassification)
        {
            if (ModelState.IsValid)
            {
                db.AudienceClassification.Add(audienceClassification);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(audienceClassification);
        }

        // GET: AudienceClassifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceClassification audienceClassification = db.AudienceClassification.Find(id);
            if (audienceClassification == null)
            {
                return HttpNotFound();
            }
            return View(audienceClassification);
        }

        // POST: AudienceClassifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AudienceClassificationId,Name,Acronym,Descripcion,Restriction")] AudienceClassification audienceClassification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audienceClassification).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(audienceClassification);
        }

        // GET: AudienceClassifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AudienceClassification audienceClassification = db.AudienceClassification.Find(id);
            if (audienceClassification == null)
            {
                return HttpNotFound();
            }
            return View(audienceClassification);
        }

        // POST: AudienceClassifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AudienceClassification audienceClassification = db.AudienceClassification.Find(id);
            db.AudienceClassification.Remove(audienceClassification);
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
