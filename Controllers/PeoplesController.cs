using S.I.A.C.Filters;
using S.I.A.C.Models;
using S.I.A.C.Service;
using System;
using System.Web.Mvc;
using S.I.A.C.Service.Implement;

namespace S.I.A.C.Controllers
{
    [HandleError]
    public class PeoplesController : Controller
    {
        private readonly PeopleCommandsService _peopleCommandsService;
        private readonly ViewUtilityServices _viewUtilityServices;

        /// <summary>
        /// Pseudo injection.
        /// </summary>
        public PeoplesController()
        {
            _peopleCommandsService = new PeopleCommandsService();
            _viewUtilityServices = new ViewUtilityServices();
        }

       
        //START CREATE**//
        [AuthorizeUser(1)]
        public ActionResult Create()
        {
            ViewBag.rols = _viewUtilityServices.GetListOfRols();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(1)]
        //remember to treat any data in an HTTP request as malicious until proven otherwise//
        public ActionResult Create(RegistrationViewModel registrationViewModel)
        {
            var isNewPeople = false;

            if (ModelState.IsValid)
            {
                isNewPeople = _peopleCommandsService.CreatePeople(registrationViewModel);
            }

            if (isNewPeople)
            {
                var msg = "Usuario creado exitosamente.";
                TempData["Successful"] = msg;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Falla al crear usuario.";
                return View(registrationViewModel);
            }
        }
        //**FINISH CREATE//

       
        // GET: Peoples/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Peoples/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}