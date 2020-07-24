using S.I.A.C.Filters;
using System.Web.Mvc;

namespace S.I.A.C.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(2)]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            //Session["User"] = null;
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