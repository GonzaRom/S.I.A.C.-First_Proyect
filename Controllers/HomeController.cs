﻿using System.Web.Mvc;
using S.I.A.C.Filters;

namespace S.I.A.C.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(1)]
        public ActionResult CurrentTicket()
        {
            return RedirectToAction("CurrentTickets", "Ticket");
        }

        [AuthorizeUser(2)]
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