using System.Web.Mvc;
using S.I.A.C.Models;
using S.I.A.C.Service;

namespace S.I.A.C.Controllers
{
    public class LoginController : Controller
    {
        private PeopleQueriesService _peopleQueriesService;

        [HttpGet]
        public ActionResult Login()
        {
            var loginDataModel = new LoginDataModel();
            //Trying to add more security
            //var encryptedLoginId = Encrypt.GetSHA256(loginDataModel.ticketIdLocal.ToString());
            //ViewBag.LoginIdEncrypted = encryptedLoginId;
            return View(loginDataModel);
        }

        [HttpPost]
        [HandleError]
        public ActionResult Login(LoginDataModel loginDataModel)
        {
            if (!ModelState.IsValid) return View(loginDataModel);

            _peopleQueriesService = new PeopleQueriesService();
            var activeUser = _peopleQueriesService.SearchPeople(loginDataModel.email, loginDataModel.password);
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
    }
}