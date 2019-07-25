using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationUserManager _userManager;

        private const string directionParameter = "DIRECTION";
        private const string cacheDirectionKey = "param-DIR02";

        private Entities db = new Entities();

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(GetUserViewModels());
        }

        // GET: Users/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<RegisterUserViewModel> users = GetUserViewModels();
            RegisterUserViewModel user = users.FirstOrDefault(item => item.UserId == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            ViewBag.Provinces = GetProvinces();

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetCantonsByProvinceId(int provinceId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            List<Canton> cantons = GetCantons(provinceId);

            return Json(cantons, JsonRequestBehavior.AllowGet);
        }

        private List<Canton> GetCantons(int provinceId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            return provinces.FirstOrDefault(item => item.IdProvince == provinceId).Cantons;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult GetDistrictsByCantonId(int cantonId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            Canton canton = provinces.SelectMany(item => item.Cantons).ToList().
                FirstOrDefault(item => item.IdCanton == cantonId);

            return Json(canton.Districts, JsonRequestBehavior.AllowGet);
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterUserViewModel user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser appUser = new ApplicationUser
                {
                    UserName = user.Email,
                    Email = user.Email,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await UserManager.CreateAsync(appUser, WebConfigHelper.DefaultAdminPassword);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(appUser.Id, "Admin");

                    #region Extended User

                    User dbUser = new User()
                    {
                        FirstName = user.FirstName,
                        Birthdate = user.Birthdate,
                        Gender = (int)user.Gender,
                        LastName = user.LastName,
                        Canton = user.Canton,
                        Province = user.Province,
                        District = user.District,
                        UserId = new Guid(appUser.Id),
                        Details = user.Details,
                        CategoryPreferences = 0
                    };
                    db.User.Add(dbUser);
                    db.SaveChanges();

                    #endregion

                    return RedirectToAction("Index", "Users");
                }
                AddErrors(result);
            }

            ViewBag.Provinces = GetProvinces();
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Private Methods

        private List<RegisterUserViewModel> GetUserViewModels()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            IdentityRole role = roleManager.FindByName("Admin");
            List<ApplicationUser> appUsers = (from user in UserManager.Users
                                              where user.Roles.Any(r => r.RoleId == role.Id)
                                              select user).ToList();

            List<RegisterUserViewModel> users = new List<RegisterUserViewModel>();
            ApplicationUser appUser;
            foreach (var user in db.User.ToList())
            {
                appUser = appUsers.FirstOrDefault(item => new Guid(item.Id) == user.UserId);

                if (appUser != null)
                {
                    users.Add(new RegisterUserViewModel
                    {
                        UserId = user.UserId,
                        Birthdate = user.Birthdate,
                        Canton = user.Canton,
                        Details = user.Details,
                        District = user.District,
                        Email = appUser.Email,
                        FirstName = user.FirstName,
                        Gender = (Utils.Enums.Gender)user.Gender,
                        LastName = user.LastName,
                        Province = user.Province,
                        ProvinceName = GetProvince(user.Province).ProvinceName,
                        CantonName = GetCanton(user.Province, user.Canton).CantonName,
                        DistrictName = GetDistrict(user.Province, user.Canton, user.District).DistrictName
                    });
                }
            }

            return users;
        }

        private District GetDistrict(int provinceId, int cantonId, int districtId)
        {
            Canton canton = GetCanton(provinceId, cantonId);
            return canton.Districts.FirstOrDefault(district => district.IdDistrict == districtId);
        }

        private Canton GetCanton(int provinceId, int cantonId)
        {
            List<Canton> cantons = GetCantons(provinceId);
            return cantons.FirstOrDefault(canton => canton.IdCanton == cantonId);
        }

        private Province GetProvince(int provinceId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            return provinces.FirstOrDefault(province => province.IdProvince == provinceId);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private IEnumerable<SelectListItem> GetProvinces()
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            IEnumerable<SelectListItem> provinceListItems = (from item in provinces
                                                             select new SelectListItem
                                                             {
                                                                 Value = item.IdProvince.ToString(),
                                                                 Text = item.ProvinceName.ToString()
                                                             }).ToList();

            return provinceListItems;
        }

        private void LoadDirectionsParameter()
        {
            if (!HttpContext.KeyExistsOnCache(cacheDirectionKey))
            {
                var parameter = db.Parameter.Find(directionParameter);
                if (parameter != null)
                {
                    string paramValue = parameter.ParamValue;
                    List<Province> provinces = JsonConvert.DeserializeObject<List<Province>>(paramValue);
                    HttpContext.AddValuesToCache(cacheDirectionKey, provinces);
                }
            }
        }
        #endregion
    }
}