using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using TicoCinema.WebApplication.Models;
using TicoCinema.WebApplication.Utils;
using TicoCinema.WebApplication.ViewModels;

namespace TicoCinema.WebApplication.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private Entities db = new Entities();

        private const string directionParameter = "DIRECTION";
        private const string cacheDirectionKey = "param-DIR01";

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
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

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Intento de inicio de sesión inválido.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            ViewBag.Provinces = GetProvinces();
            ViewBag.Cantons = GetCantonsByProvinceId(1);
            ViewBag.Districts = GetDistrictsByCantonId(101);

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser {
                    UserName = model.Email,
                    Email = model.Email,
                    EmailConfirmed = true,
                    LockoutEnabled = false
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Guest");

                    #region Extended User

                    User dbUser = new User()
                    {
                        FirstName = model.FirstName,
                        Birthdate = model.Birthdate,
                        Gender = (int)model.Gender,
                        LastName = model.LastName,
                        Canton = model.Canton,
                        Province = model.Province,
                        District = model.District,
                        UserId = new System.Guid(user.Id),
                        Details = model.Details,
                        CategoryPreferences = 0
                    };
                    db.User.Add(dbUser);
                    db.SaveChanges();

                    #endregion

                    await SignInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }
            
            return View(model);
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    return View("ForgotPasswordConfirmation");
                }
                
            }
            
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }

                db.Dispose();
                HttpContext.RemoveValuesFromCache(cacheDirectionKey);
            }

            base.Dispose(disposing);
        }

        #region Private Methods
        private IEnumerable<SelectListItem> GetProvinces()
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            IEnumerable<SelectListItem> provinceListItems = (from item in provinces  select new SelectListItem
            {
                Value = item.IdProvince.ToString(),
                Text = item.ProvinceName.ToString()
            }).ToList();

            return provinceListItems;
        }

        private IEnumerable<SelectListItem> GetCantonsByProvinceId(int provinceId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            List<Canton> cantons = provinces.FirstOrDefault(item => item.IdProvince == provinceId).Cantons;

            IEnumerable<SelectListItem> cantonListItems = (from item in cantons select new SelectListItem
            {
                Value = item.IdCanton.ToString(),
                Text = item.CantonName.ToString()
            }).ToList();

            return cantonListItems;
        }

        private IEnumerable<SelectListItem> GetDistrictsByCantonId(int cantonId)
        {
            LoadDirectionsParameter();
            List<Province> provinces = (List<Province>)HttpContext.GetValuesFromCache(cacheDirectionKey);
            Canton canton = provinces.SelectMany(item => item.Cantons).ToList().
                FirstOrDefault(item => item.IdCanton == cantonId);

            IEnumerable<SelectListItem> districtListItems = (from item in canton.Districts select new SelectListItem
            {
                Value = item.IdDistrict.ToString(),
                Text = item.DistrictName.ToString()
            }).ToList();

            return districtListItems;
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

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}