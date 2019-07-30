using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    [Authorize]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AudienceClassificationId,Name,Acronym,Descripcion,Restriction")] RegisterAudienceClassificationViewModel audienceClassification)
        {
            if (ModelState.IsValid)
            {
                AudienceClassification audience = ConvertViewModelToAudience(audienceClassification);
                db.AudienceClassification.Add(audience);
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

            RegisterAudienceClassificationViewModel audience = ConvertAudienceToViewModel(audienceClassification);
            return View(audience);
        }

        // POST: AudienceClassifications/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AudienceClassificationId,Name,Acronym,Descripcion,Restriction")] RegisterAudienceClassificationViewModel audienceClassification)
        {
            if (ModelState.IsValid)
            {
                AudienceClassification audience = ConvertViewModelToAudience(audienceClassification);
                db.Entry(audience).State = EntityState.Modified;
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

        private AudienceClassification ConvertViewModelToAudience(RegisterAudienceClassificationViewModel audienceViewModel)
        {
            AudienceClassification audience = db.AudienceClassification.Find(audienceViewModel.AudienceClassificationId);
            int.TryParse(audienceViewModel.Restriction, out int audienceRestriction);
            if (audience == null)
            {
                audience = new AudienceClassification
                {
                    Acronym = audienceViewModel.Acronym,
                    AudienceClassificationId = audienceViewModel.AudienceClassificationId,
                    Descripcion = audienceViewModel.Descripcion,
                    Name = audienceViewModel.Name,
                    Restriction = audienceRestriction
                };
            }
            else
            {
                audience.Acronym = audienceViewModel.Acronym;
                audience.AudienceClassificationId = audienceViewModel.AudienceClassificationId;
                audience.Descripcion = audienceViewModel.Descripcion;
                audience.Name = audienceViewModel.Name;
                audience.Restriction = audienceRestriction;
            }

            return audience;
        }

        private RegisterAudienceClassificationViewModel ConvertAudienceToViewModel(AudienceClassification audience)
        {
            return new RegisterAudienceClassificationViewModel
            {
                Acronym = audience.Acronym,
                AudienceClassificationId = audience.AudienceClassificationId,
                Descripcion = audience.Descripcion,
                Name = audience.Name,
                Restriction = audience.Restriction.ToString()
            };
        }
    }
}
