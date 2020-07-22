using System;
using System.Linq;
using System.Web.Mvc;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    public class LoginController : Controller
    {
        private readonly PeopleService _peopleService;

        public LoginController()
        {
            _peopleService = new PeopleService();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [HandleError]
        public ActionResult Login(LoginDataModel loginDataModel)
        {
            if (ModelState.IsValid)
            {
                var activeUser = _peopleService.SearchPeople(loginDataModel.email, loginDataModel.password);
                if (activeUser == null)
                {
                    var msg = "Error, usuario o password incorrecto !";
                    TempData["ErrorMessage"] = msg;
                    return RedirectToAction("Login", "Login");
                }

                Session["User"] = activeUser;
                //Acceso Correcto
                return RedirectToAction("Index", "Home");
            }

            return View(loginDataModel);
        }
    }
}