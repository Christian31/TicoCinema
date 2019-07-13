using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicoCinema.WebApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "La página de descripción del sitio.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "La página de contacto.";

            return View();
        }
    }
}