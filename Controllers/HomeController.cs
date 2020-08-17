using System.Web.Mvc;
using S.I.A.C.Filters;

namespace S.I.A.C.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        ///     First Index view.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     Get List of current Tickets active.
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser(1)]
        public ActionResult CurrentTicket()
        {
            return RedirectToAction("CurrentTickets", "Ticket");
        }

        /// <summary>
        ///     Create a new ticket view.
        /// </summary>
        /// <returns></returns>
        [AuthorizeUser(2)]
        public ActionResult Ticket()
        {
            ViewBag.Message = "Crear nuevo ticket";
            return RedirectToAction("Ticket", "Ticket");
        }

        /// <summary>
        ///     Clear current User
        /// </summary>
        /// <returns></returns>
        public ActionResult SingOut()
        {
            ViewBag.Message = "Sing Out.";
            Session["User"] = null;
            return RedirectToAction("Login", "Login");
        }
    }
}