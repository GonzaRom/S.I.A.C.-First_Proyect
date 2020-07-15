using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using S.I.A.C.Filters;

namespace S.I.A.C.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(idOperation: 2)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //Session["User"] = null;
            return View();
        }

        [AuthorizeUser(idOperation: 6)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AuthorizeUser(idOperation: 2)]
        public ActionResult Ticket()
        {
            ViewBag.Message = "Crear nuevo ticket";
            return RedirectToAction("Ticket", "Ticket");
        }

        public ActionResult SingOut()
        {
            ViewBag.Message = "Sing Out.";
            Session["User"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}